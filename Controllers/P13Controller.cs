using Backend_P13C.Data;
using Microsoft.AspNetCore.Mvc;
using Backend_P13C.Model;
using Newtonsoft.Json.Linq;

namespace MyApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class P13Controller : ControllerBase
    {
        private readonly Consulta _objConsulta;

        // Inyecta Consulta en el constructor
        public P13Controller(Consulta consulta)
        {
            _objConsulta = consulta;
        }

        // GET: api/p13/getMetalLineFailureData
        [HttpGet("getSubCategorias")]
        public IActionResult GetSubCategorias([FromQuery] int? id_categoria = null)
        {
            var data = _objConsulta.ObtenerSubCategorias(id_categoria);
            return Ok(data);
        }
        
        [HttpPost("PostInsertarProducto")]
        public IActionResult InsertarProducto([FromBody] JObject objProducto)
        {
        try
        {
            if (objProducto == null || !objProducto.HasValues)
            {
                return BadRequest("JSON inválido o vacío");
            }

            var data = _objConsulta.fnInsertarProducto(objProducto);
            return Ok(data);

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

    }
}
