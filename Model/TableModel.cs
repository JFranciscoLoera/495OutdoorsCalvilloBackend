namespace Backend_P13C.Model
{
    public class SubCategoriaModel
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string imagen { get; set; }
    }

    public class LineData
    {
        public int CD { get; set; } // Asumiendo que CD es un entero. Cambia el tipo si es necesario.
        public string Name { get; set; }
        public string Descrip { get; set; }
        public string SHORTDESC { get; set; }
        public int Stdat { get; set; }
        public string Ultest { get; set; }
        public string NameCve { get; set; }
    }

    public class LossesData
    {
        public string L_NAME { get; set; }
        public string SHIFT_NAME { get; set; }
        public string EV_T { get; set; }
        public string EV_E { get; set; }
        public string O_NM { get; set; }
        public string R_NM { get; set; }
        public string S_NM { get; set; }
        public string COMM { get; set; }
        public decimal S_TIM { get; set; }
        public DateTime PROD_YMD { get; set; }
        public int SHIFT_CD { get; set; }
        public decimal S_TIM_S { get; set; }
        public DateTime EVENT_DATE { get; set; }
        public string PRG_NO { get; set; }
        public string STEP_NO { get; set; }
        public string LINE_CD { get; set; }
        public string RESP { get; set; }
        public int CD { get; set; }
        public string DESC_FALLA { get; set; }
        public string STATUS_L { get; set; }
        public string FAILURE_CD { get; set; }
        public string AFFECTATION_CD { get; set; }
        public string SIX_CD { get; set; }
        public string MACHINE_CD { get; set; }
        public string MACHINE_DESC { get; set; }
        public string SIX_DESC { get; set; }
        public string AFFECTATION_DESC { get; set; }
        public string RESPONSABLE { get; set; }
        public string TYPE_LOSSES { get; set; }
    }

    public class LossesOccurrences
    {
        public string Loss_Label { get; set; }
        public int Occurrences { get; set; }
    }

    public class DailyLoss
    {
        public DateTime LossDate { get; set; }
        public decimal DailyLosses { get; set; }
    }

    public class LossesResponse
    {
        public List<LossesData> TableData { get; set; } = new List<LossesData>();
        public List<string> LossesLabel { get; set; } = new List<string>();
        public List<int> LossesLabelTime { get; set; } = new List<int>();
        public List<LossesOccurrences> LossesOccurrences { get; set; } = new List<LossesOccurrences>();
        public List<DailyLoss> DailyLosses { get; set; } = new List<DailyLoss>();
    }

    public class TablaOeeResult
    {
        public string SHIFT_CD { get; set; }
        public string LINE_NAME { get; set; }
        public string STATION_NAME { get; set; }
        public decimal HOUR_JPH { get; set; }
        public int COUNT_ST { get; set; }
        public decimal AVG_TC { get; set; }
        public string MODEL { get; set; }
        public DateTime DATE_ST { get; set; }
        public DateTime UPDATETIME { get; set; }
        public int ID { get; set; }
    }

    public class OeeStationResult
    {
        public string STATION_NAME { get; set; }
        public int Total_Count { get; set; }
        public decimal Average_TC { get; set; }
        public decimal Min_Hour { get; set; }
        public decimal Max_Hour { get; set; }
    }

    public class OeeModelResult
    {
        public string MODEL { get; set; }
        public int Total_Pieces { get; set; }
    }

    public class OeeResults
    {
        public List<TablaOeeResult> TablaOeeResults { get; set; }
        public List<OeeStationResult> OeeStationResults { get; set; }
        public List<OeeModelResult> OeeModelResults { get; set; }
    }

    public class TableDowtimeLosses
    {
        public string L_NAME { get; set; }            // Nombre de la línea
        public string SHIFT_NAME { get; set; }         // Nombre del turno
        public decimal EV_T { get; set; }              // Tiempo efectivo (por definir)
        public decimal EV_E { get; set; }              // Eficiencia efectiva (por definir)
        public string O_NM { get; set; }               // Nombre del evento
        public string R_NM { get; set; }               // Nombre del responsable
        public string S_NM { get; set; }               // Nombre del estado
        public string COMM { get; set; }                // Comentario
        public decimal S_TIM { get; set; }             // Tiempo en minutos
        public DateTime PROD_YMD { get; set; }         // Fecha de producción
        public string SHIFT_CD { get; set; }           // Código del turno
        public decimal S_TIM_S { get; set; }           // Tiempo de caída en segundos
        public DateTime EVENT_DATE { get; set; }       // Fecha del evento
        public string PRG_NO { get; set; }             // Número del programa
        public string STEP_NO { get; set; }            // Número de paso
        public string LINE_CD { get; set; }            // Código de línea
        public string RESP { get; set; }                // Responsable
        public string CD { get; set; }                  // Código
        public string DESC_FALLA { get; set; }          // Descripción de la falla
        public string STATUS_L { get; set; }            // Estado de la línea
        public string FAILURE_CD { get; set; }          // Código de falla
        public string AFFECTATION_CD { get; set; }     // Código de afectación
        public string SIX_CD { get; set; }              // Código SIX
        public string MACHINE_CD { get; set; }          // Código de máquina
        public string MACHINE_DESC { get; set; }        // Descripción de la máquina
        public string SIX_DESC { get; set; }            // Descripción SIX
        public string AFFECTATION_DESC { get; set; }    // Descripción de afectación
        public string RESPONSABLE { get; set; }         // Responsable
        public string TYPE_LOSSES { get; set; }         // Tipo de pérdidas
    }


    public class AggregatedLossesResult
    {
        public string O_NM { get; set; }                // Nombre del evento
        public decimal Total_S_TIM_S { get; set; }      // Suma de S_TIM_S
        public int Total_Registros { get; set; }         // Total de registros
    }

    public class DownTimeResult
    {
        public List<TableDowtimeLosses> TableResults { get; set; }                
        public List<AggregatedLossesResult> AggregatedLosses { get; set; } 
    }

    public class TableTC
    {
        public string Shift_CD { get; set; }
        public string Line_Name { get; set; }
        public string Station_Name { get; set; }
        public decimal Hour_JPH { get; set; }
        public int Count_ST { get; set; }
        public decimal Avg_TC { get; set; }
        public string Model { get; set; }
        public DateTime Date_ST { get; set; }
        public DateTime UpdateTime { get; set; }
    }

    public class ChartTC
    {
        public string Station_Name { get; set; }
        public decimal Average_TC { get; set; }
    }

    public class TCResult
    {
        public List<TableTC> TableTC { get; set; }
        public List<ChartTC> ChartTC { get; set; }

        public TCResult()
        {
            TableTC = new List<TableTC>();
            ChartTC = new List<ChartTC>();
        }
    }

    public class TPTable
    {
        public DateTime EVENT_DATE { get; set; }                // Fecha del evento
        public decimal TOTAL_S_TIM_S { get; set; }              // Suma de S_TIM_S
        public string L_NAME { get; set; }                       // Nombre de la línea
        public string SHIFT_NAME { get; set; }                   // Nombre del turno
        public decimal TOTAL_HOURS_S { get; set; }              // Horas totales en segundos (43,200 segundos)
        public decimal LUNCH_TIME_S { get; set; }                // Tiempo de almuerzo en segundos (1,800 segundos)
        public decimal SCHEDULED_DOWNTIME_S { get; set; }        // Tiempo de inactividad programada en segundos (900 segundos)
        public decimal PRODUCTIVE_TIME_S { get; set; }           // Tiempo productivo calculado
    }

    public class TPResult
    {
        public List<TPTable> TableTP { get; set; }

        public TPResult()
        {
            TableTP = new List<TPTable>();
        }
    }



}
