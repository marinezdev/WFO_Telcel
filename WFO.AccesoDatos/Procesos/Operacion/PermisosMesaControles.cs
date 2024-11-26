using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using prop = WFO.Propiedades.Procesos.Operacion;

namespace WFO.AccesoDatos.Procesos.Operacion
{
    public class PermisosMesaControles
    {
        ManejoDatos b = new ManejoDatos();

        public prop.PermisosMesaControles PermisosMesaControles_Selecionar(int IdMesa, string NombreControl)
        {
            prop.PermisosMesaControles resultado = new prop.PermisosMesaControles();
            resultado.Activo = 1;

            // TODO: ### Pendiente: Actualizar procedimiento en base de datos y hacerlo correctamente.
            ////b.ExecuteCommandSP("PermisosMesaControles_Selecionar_PorIdMesa_Control");
            ////b.AddParameter("@IdMesa", IdMesa, SqlDbType.Int);
            ////b.AddParameter("@NombreControl", NombreControl, SqlDbType.NVarChar);

            ////var reader = b.ExecuteReader();
            ////while (reader.Read())
            ////{
            ////    resultado.Activo = Convert.ToInt32(reader["Activo"].ToString());
            ////}
            ////reader = null;
            ////b.ConnectionCloseToTransaction();
            return resultado;
        }

    }
}
