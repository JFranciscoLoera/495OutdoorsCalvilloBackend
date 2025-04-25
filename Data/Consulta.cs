using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Backend_P13C.Model;
using Dapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ZXing;
using ZXing.Common;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.Fonts;

namespace Backend_P13C.Data
{
    public class Consulta
    {
        private readonly string _connectionString;

        public Consulta(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public List<SubCategoriaModel> ObtenerSubCategorias(int? id_categoria = null)
        {
            using (IDbConnection dbConnection = new SqlConnection(_connectionString))
            {
                dbConnection.Open();
                var parameters = new DynamicParameters();
                parameters.Add("@id_categoria", id_categoria);

                return dbConnection.Query<SubCategoriaModel>(
                    "SP_GetSubcategorias",
                    parameters,
                    commandType: CommandType.StoredProcedure
                ).ToList();
            }
        }

        public JObject fnInsertarProducto(JObject objProducto)
        {
            try
            {
                using (IDbConnection dbConnection = new SqlConnection(_connectionString))
                {
                    dbConnection.Open();

                    string jsonInput = JsonConvert.SerializeObject(objProducto);
                    Console.WriteLine("[DEBUG] JSON de entrada: " + jsonInput);

                    var parameters = new DynamicParameters();
                    parameters.Add("@objProducto", jsonInput, DbType.String, size: -1);

                    string rawResult = dbConnection.ExecuteScalar<string>(
                        "SP_Insertar_Producto",
                        parameters,
                        commandType: CommandType.StoredProcedure);

                    Console.WriteLine("[DEBUG] Respuesta cruda del SP: " + (rawResult ?? "NULL"));

                    if (string.IsNullOrWhiteSpace(rawResult))
                    {
                        return new JObject { ["status"] = "empty_response" };
                    }

                    var result = JObject.Parse(rawResult);
                    
                    if (result["status"]?.ToString() == "success")
                    {
                        int id = (int)result["id"];
                        string codigoProducto = result["codigo_producto"]?.ToString();
                        decimal precio = (decimal)result["precio"];
                        string barcodePath = GenerarYGuardarCodigoBarras(codigoProducto, id, precio);
                        ActualizarRutaBarcode(id, barcodePath);
                        result["barcode_path"] = barcodePath;
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[ERROR] " + ex.ToString());
                return new JObject {
                    ["error"] = "unexpected_error",
                    ["message"] = ex.Message
                };
            }
        }

        private string GenerarYGuardarCodigoBarras(string codigoProducto, int id, decimal precio)
        {
            try
            {
                string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string barcodeFolder = Path.Combine(documentsPath, "CodigosBarras_Productos");
                Directory.CreateDirectory(barcodeFolder);

                string cleanCode = LimpiarNombreArchivo(codigoProducto);
                string fileName = $"Barcode_{cleanCode}.png";
                string fullPath = Path.Combine(barcodeFolder, fileName);

                // 1. Generar el código de barras
                var barcodeWriter = new BarcodeWriterPixelData
                {
                    Format = BarcodeFormat.CODE_128,
                    Options = new EncodingOptions
                    {
                        Height = 120,
                        Width = 350,
                        Margin = 15,
                        PureBarcode = true
                    }
                };

                var pixelData = barcodeWriter.Write(codigoProducto);

                // 2. Crear imagen con espacio para texto
                int textHeight = 80; // Más espacio para texto
                using (var image = new Image<Rgba32>(pixelData.Width, pixelData.Height + textHeight))
                {
                    // Fondo blanco
                    image.Mutate(x => x.BackgroundColor(Color.White));

                    // Dibujar código de barras
                    for (int y = 0; y < pixelData.Height; y++)
                    {
                        for (int x = 0; x < pixelData.Width; x++)
                        {
                            int index = (y * pixelData.Width + x) * 4;
                            image[x, y] = new Rgba32(
                                pixelData.Pixels[index],
                                pixelData.Pixels[index + 1],
                                pixelData.Pixels[index + 2],
                                pixelData.Pixels[index + 3]);
                        }
                    }

                    // 3. Configurar fuente
                    var fontCollection = new FontCollection();
                    var fontFamily = fontCollection.AddSystemFonts().Families.First();
                    var font = fontFamily.CreateFont(16, FontStyle.Bold);

                    // 4. Dibujar texto (aproximación centrada)
                    var textBrush = Brushes.Solid(Color.Black);
                    var pen = Pens.Solid(Color.Black, 1f);

                    // Aproximación de centrado para código de producto
                    float codeTextX = (image.Width - (codigoProducto.Length * 11)) / 2; // Estimación aproximada
                    float codeTextY = pixelData.Height + 15;

                    image.Mutate(x => x.DrawText(
                        codigoProducto,
                        font,
                        textBrush,
                        new PointF(codeTextX, codeTextY)));

                    // Aproximación de centrado para precio
                    var priceText = $"${precio.ToString("0.00")}";
                    float priceTextX = (image.Width - (priceText.Length * 9)) / 2; // Estimación aproximada
                    float priceTextY = pixelData.Height + 45;

                    image.Mutate(x => x.DrawText(
                        priceText,
                        font,
                        textBrush,
                        new PointF(priceTextX, priceTextY)));

                    // 5. Guardar imagen
                    image.Save(fullPath);
                }

                return fullPath;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[BARCODE ERROR] {ex}");
                return "error_generating_barcode";
            }
        }

        private string LimpiarNombreArchivo(string input)
        {
            var invalidChars = Path.GetInvalidFileNameChars();
            return new string(input
                .Where(ch => !invalidChars.Contains(ch))
                .ToArray())
                .Replace(" ", "_")
                .Trim();
        }

        private void ActualizarRutaBarcode(int id, string barcodePath)
        {
            try
            {
                using (IDbConnection dbConnection = new SqlConnection(_connectionString))
                {
                    dbConnection.Execute(
                        "UPDATE INVENTARIO_Curricanes_Pesca SET qr = @barcodePath WHERE id = @id",
                        new { barcodePath, id });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[UPDATE BARCODE ERROR] " + ex.ToString());
            }
        }

        public List<LineData> ObtenerCatalogoPerdidas()
        {
            using (IDbConnection dbConnection = new SqlConnection(_connectionString))
            {
                dbConnection.Open();
                var parameters = new DynamicParameters();

                return dbConnection.Query<LineData>(
                    "GetLossDescriptions",
                    parameters,
                    commandType: CommandType.StoredProcedure
                ).ToList();
            }
        }
    }
}