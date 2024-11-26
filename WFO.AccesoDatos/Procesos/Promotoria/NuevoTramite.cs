using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using prop = WFO.Propiedades.Procesos.Promotoria;

namespace WFO.AccesoDatos.Procesos.Promotoria
{
    public class NuevoTramite
    {
        ManejoDatos b = new ManejoDatos();

        //public List<prop.RespuestaNuevoTramiteN1> NuevoTramiteN1(int IdTipoTramite, int IdPromotoria, int IdUsuario, int IdStatus, int idPrioridad, string FechaSolicitud, int IdAgente, string NumeroOrden, int idRamo, string IdSisLegados, string kwik, int IdMoneda, int TipoPersona, string Nombre, string ApPaterno, string ApMaterno, string Sexo, string FechaNacimiento, string RFC, string FechaConst, int IdNacionalidad, string TitularNombre, string TitularApPat, string TitularApMat, int IdTitularNacionalidad, string TitularSexo, string TitularFechaNacimiento, double PrimaCotizacion, int TitularContratante, string Observaciones, int IdProducto, int IdSubProducto)
        public List<prop.RespuestaNuevoTramiteN1> NuevoTramiteN1(prop.TramiteNuevo1 tramiteN1)
        {
            b.ExecuteCommandSP("TramiteNuevo");
            b.AddParameter("@IdTipoTramite", tramiteN1.IdTipoTramite, SqlDbType.Int);
            b.AddParameter("@IdUsuario", tramiteN1.IdUsuario, SqlDbType.Int);
            b.AddParameter("@IdStatus", tramiteN1.IdStatus, SqlDbType.Int);
            b.AddParameter("@IdPrioridad", tramiteN1.IdPrioridad, SqlDbType.Int);
            b.AddParameter("@Proyecto", tramiteN1.Proyecto, SqlDbType.NVarChar);
            b.AddParameter("@Expediente", tramiteN1.Expediente, SqlDbType.NVarChar);

            List<prop.RespuestaNuevoTramiteN1> resultado = new List<prop.RespuestaNuevoTramiteN1>();
            var reader = b.ExecuteReader();
            while (reader.Read())
            {
                prop.RespuestaNuevoTramiteN1 item = new prop.RespuestaNuevoTramiteN1()
                {
                    Id = Funciones.Numeros.ConvertirTextoANumeroEntero(reader["Id"].ToString()),
                    Folio = reader["Folio"].ToString()
                };
                resultado.Add(item);
            }
            reader = null;
            b.ConnectionCloseToTransaction();
            return resultado;
        }

        public List<prop.Proyectos> seleccionarPoryectos(int IdProveedor)
        {
            b.ExecuteCommandSP("Proyectos_Get");
            b.AddParameter("@IdProveedor", IdProveedor, SqlDbType.Int);

            List<prop.Proyectos> resultado = new List<prop.Proyectos>();
            var reader = b.ExecuteReader();
            while (reader.Read())
            {
                prop.Proyectos item = new prop.Proyectos()
                {
                    Id = Funciones.Numeros.ConvertirTextoANumeroEntero(reader["Id"].ToString()),
                    IdProveedor = Funciones.Numeros.ConvertirTextoANumeroEntero(reader["IdProveedor"].ToString()),
                    Nombre = reader["Nombre"].ToString()
                };
                resultado.Add(item);
            }
            reader = null;
            b.ConnectionCloseToTransaction();
            return resultado;
        }

    }
}
