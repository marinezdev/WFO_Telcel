using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFO.Funciones
{
    public class VariablesGlobales
    {
        public enum StatusTramite
        {
            Registro = 1,
	        Proceso = 2,
	        Suspendido = 3, 
	        Ejecucion = 4,
	        Rechazo = 5,
	        Cancelado = 6,
        }

        public enum StatusMesa
        {
            Registro = 1,
            Atrapado = 2,
            Suspendido = 3,
            RevisionSuspencion = 4,
            ReingresoSuspencion = 5,
            Pausa = 6,
            Procesado = 7,
            Rechazo = 8,
            Cancelado = 9
        }
    }
}
