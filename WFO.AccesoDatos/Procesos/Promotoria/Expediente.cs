using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using prop = WFO.Propiedades.Procesos.Promotoria;

namespace WFO.AccesoDatos.Procesos.Promotoria
{
    public class Expediente
    {
        ManejoDatos b = new ManejoDatos();

        public int Agregar(int TipoTramite, int Id_Tramite, double Id_Archivo, string NmArchivo, string NmOriginal, int Activo, int Fusion, string Descripcion)
        {
            b.ExecuteCommandSP("Expediente_Agregar");
            b.AddParameter("@IdTramite", Id_Tramite, SqlDbType.Int);
            b.AddParameter("@NmArchivo", NmArchivo, SqlDbType.NVarChar);
            b.AddParameter("@NmOriginal", NmOriginal, SqlDbType.NVarChar);
            b.AddParameter("@Activo", Activo, SqlDbType.Int);
            return b.InsertUpdateDeleteWithTransaction();
        }

        public int ModificarExpedienteFusion(int Id)
        {
            b.ExecuteCommandSP("Expediente_Modificar");
            b.AddParameter("@Id", Id, SqlDbType.Int);
            return b.InsertUpdateDeleteWithTransaction();
        }

        public List<prop.expediente> ConsultaExpediente(int IdTramite)
        {
            b.ExecuteCommandSP("Expediente_Get");
            b.AddParameter("@IdTramite", IdTramite, SqlDbType.Int);
            List<prop.expediente> resultado = new List<prop.expediente>();
            var reader = b.ExecuteReader();
            while (reader.Read())
            {
                prop.expediente item = new prop.expediente()
                {
                    Id = Funciones.Numeros.ConvertirTextoANumeroEntero(reader["Id"].ToString()),
                    Id_Tramite = Funciones.Numeros.ConvertirTextoANumeroEntero(reader["Id_Tramite"].ToString()),
                    Id_Archivo = Funciones.Numeros.ConvertirTextoANumeroEntero(reader["Id_Archivo"].ToString()),
                    NmArchivo = reader["NmArchivo"].ToString(),
                    NmOriginal = reader["NmOriginal"].ToString()
                };
                resultado.Add(item);
            }
            reader = null;
            b.ConnectionCloseToTransaction();
            return resultado;
        }


        public List<prop.expediente> Expediente_Consultar_PorIdTramite(int Id)
        {
            b.ExecuteCommandSP("Expediente_Consultar_PorIdTramite");
            b.AddParameter("@IdTramite", Id, SqlDbType.Int);
            List<prop.expediente> resultado = new List<prop.expediente>();
            var reader = b.ExecuteReader();
            while (reader.Read())
            {
                prop.expediente item = new prop.expediente()
                {
                    Id = Funciones.Numeros.ConvertirTextoANumeroEntero(reader["IdExpediente"].ToString()),
                    Id_Tramite = Funciones.Numeros.ConvertirTextoANumeroEntero(reader["IdTramite"].ToString()),
                    NmArchivo = reader["NmArchivo"].ToString(),
                    Fecha_Registro = Convert.ToDateTime(reader["Fecha_Registro"].ToString()),
                    FusionTexto =  reader["Fusion"].ToString()
                };
                resultado.Add(item);
            }
            reader = null;
            b.ConnectionCloseToTransaction();
            return resultado;
        }

        public prop.expediente Asegurados_Selecionar_PorIdTramite(int IdExpediente , int IdTramite)
        {
            b.ExecuteCommandSP("Expediente_Consultar_PorId");
            b.AddParameter("@IdExpediente", IdExpediente, SqlDbType.Int);
            b.AddParameter("@IdTramite", IdTramite, SqlDbType.Int);
            prop.expediente resultado = new prop.expediente();
            var reader = b.ExecuteReader();
            while (reader.Read())
            {
                resultado.Id = Convert.ToInt32(reader["IdExpediente"].ToString());
                resultado.NmArchivo = reader["NmArchivo"].ToString();
            }
            reader = null;
            b.ConnectionCloseToTransaction();
            return resultado;
        }
    }
}
