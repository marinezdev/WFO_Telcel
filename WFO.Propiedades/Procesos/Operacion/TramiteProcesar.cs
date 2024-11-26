using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFO.Propiedades.Procesos.Operacion
{
    public class TramiteProcesado
    {
        public int IdTramite { get; set; }
        public string Accion { get; set; }
    }

    public class TramiteProcesar
    {
        public int IdTramite { get; set; }
        public string Folio { get; set; }
        public string FechaRegistro { get; set; }
        public string FechaTermino { get; set; }
        public int IdStatus { get; set; }
        public string StatusTramite { get; set; }
        public string Proyecto { get; set; }
    }
}
