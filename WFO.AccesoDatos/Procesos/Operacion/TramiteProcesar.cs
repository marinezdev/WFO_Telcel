﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using prop = WFO.Propiedades.Procesos.Operacion;
using promotoria = WFO.Propiedades.Procesos.Promotoria;

namespace WFO.AccesoDatos.Procesos.Operacion
{
    public class TramiteProcesar
    {
        ManejoDatos b = new ManejoDatos();

        public List<prop.TramiteProcesar> ObtenerTramite(int pIdTramite)
        {
            b.ExecuteCommandSP("Tramite_Selecionar_PorIdTramite");
            b.AddParameter("@IdTramite", pIdTramite, SqlDbType.Int);
            
            List<prop.TramiteProcesar> resultado = new List<prop.TramiteProcesar>();
            var reader = b.ExecuteReader();
            while (reader.Read())
            {
                prop.TramiteProcesar item = new prop.TramiteProcesar()
                {
                    IdTramite = Convert.ToInt32(reader["Id"].ToString()),
                    Folio = reader["Folio"].ToString(),
                    FechaRegistro = reader["FechaRegistro"].ToString(),
                    ////Moneda = reader["Moneda"].ToString(),
                    ////PrimaCotizacion = reader["PrimaCotizacion"].ToString(),
                    ////IdPromotoria = Convert.ToInt32(reader["IdPromotoria"].ToString()),
                    ////FechaSolicitud = reader["FechaSolicitud"].ToString(),
                    ////TipoPersona = Convert.ToInt32(reader["TipoPersona"].ToString()),
                    ////NumeroOrden = reader["NumeroOrden"].ToString(),
                    ////ContratanteNombre = reader["ContratanteNombre"].ToString(),
                    ////ContratanteApPaterno = reader["ContratanteApPaterno"].ToString(),
                    ////ContratanteApMaterno = reader["ContratanteApMaterno"].ToString(),
                    ////ContratanteSexo = reader["ContratanteSexo"].ToString(),
                    ////RFCContratante = reader["RFCContratante"].ToString(),
                    ////Nacionalidad = reader["Nacionalidad"].ToString(),
                    ////FechaConst = reader["FechaConst"].ToString(),
                    ////Contratante = reader["Contratante"].ToString(),
                    ////TitularContratante = reader["TitularContratante"].ToString(),
                    ////FNacimientoTitular = reader["TitularFechaNacimiento"].ToString(),
                    ////TitularNombre = reader["TitularNombre"].ToString(),
                    ////TitularApPat = reader["TitularApPat"].ToString(),
                    ////TitularApMat = reader["TitularApMat"].ToString(),
                    ////TitularNacionalidad = reader["TitularNacionalidad"].ToString(),
                    ////TitularSexo = reader["TitularSexo"].ToString(),
                    ////Producto = reader["Producto"].ToString(),
                    ////SubProducto = reader["SubProducto"].ToString(),
                    StatusTramite = reader["StatusTramite"].ToString(),
                };
                resultado.Add(item);
            }
            reader = null;
            b.ConnectionCloseToTransaction();
            return resultado;
        }

        public List<prop.TramiteProcesar> ObtenerTramite(int pIdUsuario, int pIdMesa, int pIdTramite, ref DataSet dsTramiteAsignado)
        {
            List<prop.TramiteProcesar> resultado = new List<prop.TramiteProcesar>();

            b.ExecuteCommandSP("WFOTramiteAsignar");
            b.AddParameter("@IdMesa", pIdMesa, SqlDbType.Int);
            b.AddParameter("@IdUsuario", pIdUsuario, SqlDbType.Int);
            b.AddParameter("@IdTramite", pIdTramite, SqlDbType.Int);
            dsTramiteAsignado = b.SelectExecuteFunctions();

            if (int.Parse(dsTramiteAsignado.Tables[0].Rows[0]["IdTramite"].ToString()) <= 0)
            {
                // Trámite no asignado
                prop.TramiteProcesar item = new prop.TramiteProcesar()
                {
                    IdTramite = -1
                };
                resultado.Add(item);
            }
            else 
            {
                prop.TramiteProcesar item = new prop.TramiteProcesar()
                {
                    IdTramite = int.Parse(dsTramiteAsignado.Tables[0].Rows[0]["IdTramite"].ToString()),
                    Folio = dsTramiteAsignado.Tables[0].Rows[0]["Folio"].ToString(),
                    FechaRegistro = dsTramiteAsignado.Tables[0].Rows[0]["FechaRegistro"].ToString(),
                    FechaTermino = dsTramiteAsignado.Tables[0].Rows[0]["FechaTermino"].ToString(),
                    IdStatus = int.Parse(dsTramiteAsignado.Tables[0].Rows[0]["Id Status"].ToString()),
                    StatusTramite = dsTramiteAsignado.Tables[0].Rows[0]["Status Trámite"].ToString(),
                    Proyecto = dsTramiteAsignado.Tables[0].Rows[0]["Proyecto"].ToString()
                };
                resultado.Add(item);
            }
            return resultado;
        }
        
        public List<prop.TramiteProcesado> PromotoriaAcepta(int IdTramite, bool StatusPoliza, int IdUsuario, string ObservacionPublica, string ObservacionPrivada)
        {
            b.ExecuteCommandSP("spWFOPromoRevisa");
            b.AddParameter("@IdTramite", IdTramite, SqlDbType.Int);
            b.AddParameter("@PolizaAceptada", StatusPoliza, SqlDbType.Bit);
            b.AddParameter("@IdUsuario", IdUsuario, SqlDbType.Int);
            b.AddParameter("@Observacion", ObservacionPublica, SqlDbType.NChar);
            
            List<prop.TramiteProcesado> resultado = new List<prop.TramiteProcesado>();
            var reader = b.ExecuteReader();
            while (reader.Read())
            {
                prop.TramiteProcesado item = new prop.TramiteProcesado()
                {
                    IdTramite = Convert.ToInt32(reader["IdTramite"].ToString()),
                    Accion = reader["Accion"].ToString(),
                };
                resultado.Add(item);
            }
            reader = null;
            b.ConnectionCloseToTransaction();
            return resultado;
        }

        public List<prop.TramiteProcesado> ReingresarTramite(int IdTramite, int IdUsuario, string ObservacionPublica, string ObservacionPrivada)
        {
            b.ExecuteCommandSP("spWFOTramiteReingresar");
            b.AddParameter("@IdTramite", IdTramite, SqlDbType.Int);
            b.AddParameter("@IdUsuario", IdUsuario, SqlDbType.Int);
            b.AddParameter("@ObservacionPub", ObservacionPublica, SqlDbType.NChar);
            b.AddParameter("@ObservacionPriv", ObservacionPrivada, SqlDbType.NChar);

            List<prop.TramiteProcesado> resultado = new List<prop.TramiteProcesado>();
            var reader = b.ExecuteReader();
            while (reader.Read())
            {
                prop.TramiteProcesado item = new prop.TramiteProcesado()
                {
                    IdTramite = Convert.ToInt32(reader["IdTramite"].ToString()),
                    Accion = reader["Accion"].ToString(),
                };
                resultado.Add(item);
            }
            reader = null;
            b.ConnectionCloseToTransaction();
            return resultado;
        }

        public List<prop.TramiteProcesado> ProcesarTramite(int IdTramite, int IdMesa, int IdUsuario, Funciones.VariablesGlobales.StatusMesa IdStatusMesa, string ObsPublica, string ObsPrivada, string MotivosRechazo)
        {
            int intStatusMesa = (int)IdStatusMesa;

            b.ExecuteCommandSP("WFOTramiteProcesar");
            b.AddParameter("@IdTramite", IdTramite, SqlDbType.Int);
            b.AddParameter("@IdMesa", IdMesa, SqlDbType.Int);
            b.AddParameter("@IdUsuario", IdUsuario, SqlDbType.Int);
            b.AddParameter("@IdStatusMesa", intStatusMesa, SqlDbType.Int);
            b.AddParameter("@ObservacionPub", ObsPublica, SqlDbType.VarChar);
            b.AddParameter("@ObservacionPriv", ObsPrivada, SqlDbType.VarChar);
            b.AddParameter("@MotivosRechazo", MotivosRechazo, SqlDbType.VarChar);
            
            List<prop.TramiteProcesado> resultado = new List<prop.TramiteProcesado>();
            var reader = b.ExecuteReader();
            while (reader.Read())
            {
                prop.TramiteProcesado item = new prop.TramiteProcesado()
                {
                    IdTramite = Convert.ToInt32(reader["IdTramite"].ToString()),
                    Accion = reader["Accion"].ToString(),
                };
                resultado.Add(item);
            }
            reader = null;
            b.ConnectionCloseToTransaction();
            return resultado;
        }

        public List<prop.TramiteProcesado> EnviarTramite(int IdTramite, int IdMesa, int IdMesaToSend, int IdUsuario, string observacionesPublicas, string observacionesPrivadas)
        {
            b.ExecuteCommandSP("spWFOTramiteEnviar");
            b.AddParameter("@IdTramite", IdTramite, SqlDbType.Int);
            b.AddParameter("@IdMesaEnvia", IdMesa, SqlDbType.Int);
            b.AddParameter("@IdMesaRecibe", IdMesaToSend, SqlDbType.Int);
            b.AddParameter("@IdUsuario", IdUsuario, SqlDbType.Int);
            b.AddParameter("@ObservacionPublica", observacionesPublicas, SqlDbType.VarChar);
            b.AddParameter("@ObservacionPrivada", observacionesPrivadas, SqlDbType.VarChar);
            
            List<prop.TramiteProcesado> resultado = new List<prop.TramiteProcesado>();
            var reader = b.ExecuteReader();
            while (reader.Read())
            {
                prop.TramiteProcesado item = new prop.TramiteProcesado()
                {
                    IdTramite = Convert.ToInt32(reader["IdTramite"].ToString()),
                    Accion = reader["Accion"].ToString(),
                };
                resultado.Add(item);
            }
            reader = null;
            b.ConnectionCloseToTransaction();
            return resultado;
        }

        public List<prop.RespuestaTramite> ActualizarTramite(int IdUsuario, int IdTramite, promotoria.TramiteN1 tramite)
        {
            b.ExecuteCommandSP("spWFOTramiteActualizar");
            b.AddParameter("@IdUsuario", IdUsuario, SqlDbType.Int);
            b.AddParameter("@IdTramite", IdTramite, SqlDbType.Int);
            b.AddParameter("@IdMoneda", tramite.IdMoneda, SqlDbType.Int);
            b.AddParameter("@IdTipoTramite", tramite.IdTipoTramite, SqlDbType.Int);
            b.AddParameter("@IdRiesgo", tramite.IdRiesgo, SqlDbType.Int);
            b.AddParameter("@HombreClave", tramite.HombreClave, SqlDbType.Int);
            b.AddParameter("@PrimaCotizacion", tramite.PrimaCotizacion, SqlDbType.Float);
            b.AddParameter("@SumaBasica", tramite.SumaBasica, SqlDbType.Float);
            b.AddParameter("@NumeroOrden", tramite.NumeroOrden, SqlDbType.NVarChar);
            b.AddParameter("@FechaSolicitud", string.Format("{0:yyyy/MM/dd}", DateTime.Parse(tramite.FechaSolicitud)), SqlDbType.DateTime);
            b.AddParameter("@TipoPersona", tramite.TipoPersona, SqlDbType.Int);
            b.AddParameter("@Nombre", tramite.Nombre, SqlDbType.NVarChar);
            b.AddParameter("@ApPaterno", tramite.ApPaterno, SqlDbType.NVarChar);
            b.AddParameter("@ApMaterno", tramite.ApMaterno, SqlDbType.NVarChar);
            b.AddParameter("@Sexo", tramite.Sexo, SqlDbType.NVarChar);
            b.AddParameter("@FechaNacimiento", string.Format("{0:yyyy/MM/dd}", DateTime.Parse(tramite.FechaNacimiento)), SqlDbType.DateTime);
            b.AddParameter("@RFC", tramite.RFC, SqlDbType.NVarChar);
            b.AddParameter("@IdNacionalidad", tramite.IdNacionalidad, SqlDbType.Int);
            b.AddParameter("@FechaConst", string.Format("{0:yyyy/MM/dd}", DateTime.Parse(tramite.FechaConst)), SqlDbType.DateTime);
            b.AddParameter("@TitularContratante", tramite.TitularContratante, SqlDbType.Int);
            b.AddParameter("@TitularNombre", tramite.TitularNombre, SqlDbType.NVarChar);
            b.AddParameter("@TitularApPat", tramite.TitularApPat, SqlDbType.NVarChar);
            b.AddParameter("@TitularApMat", tramite.TitularApMat, SqlDbType.NVarChar);
            b.AddParameter("@IdTitularNacionalidad", tramite.IdTitularNacionalidad, SqlDbType.Int);
            b.AddParameter("@TitularSexo", tramite.TitularSexo, SqlDbType.NVarChar);
            b.AddParameter("@TitularFechaNacimiento", string.Format("{0:yyyy/MM/dd}", DateTime.Parse(tramite.TitularFechaNacimiento)), SqlDbType.DateTime);
            b.AddParameter("@IdInstitucion", tramite.IdInstitucion, SqlDbType.Int);

            List<prop.RespuestaTramite> resultado = new List<prop.RespuestaTramite>();
            var reader = b.ExecuteReader();
            while (reader.Read())
            {
                prop.RespuestaTramite item = new prop.RespuestaTramite()
                {
                    Id = Convert.ToInt32(reader["Id"].ToString()),
                    Folio = reader["Folio"].ToString(),
                };
                resultado.Add(item);
            }
            reader = null;
            b.ConnectionCloseToTransaction();
            return resultado;
        }

        public List<prop.TramiteProcesado> ProcesarTramiteSeleccionCompleta(int IdTramite, int IdUsuario, int Chec1, int Chec2)
        {
            b.ExecuteCommandSP("spWFOSeleccionCompleta");
            b.AddParameter("@IdTramite", IdTramite, SqlDbType.Int);
            b.AddParameter("@IdUsuario", IdUsuario, SqlDbType.Int);
            b.AddParameter("@Chec1", Chec1, SqlDbType.Int);
            b.AddParameter("@Chec2", Chec2, SqlDbType.Int);

            List<prop.TramiteProcesado> resultado = new List<prop.TramiteProcesado>();
            var reader = b.ExecuteReader();
            while (reader.Read())
            {
                prop.TramiteProcesado item = new prop.TramiteProcesado()
                {
                    IdTramite = Convert.ToInt32(reader["IdTramite"].ToString()),
                    Accion = reader["Accion"].ToString(),
                };
                resultado.Add(item);
            }
            reader = null;
            b.ConnectionCloseToTransaction();
            return resultado;
        }

        public prop.TipoTramite ObtenerTipoTramite(int pIdTramite)
        {
            b.ExecuteCommandSP("Tramite_Consulta_TipoTramite");
            b.AddParameter("@IdTramite", pIdTramite, SqlDbType.Int);

            prop.TipoTramite resultado = new prop.TipoTramite();
            var reader = b.ExecuteReader();
            while (reader.Read())
            {
                resultado.Id = Convert.ToInt32(reader["Id"].ToString());
                resultado.Nombre = reader["Nombre"].ToString();
            }
            reader = null;
            b.ConnectionCloseToTransaction();
            return resultado;
        }

        public promotoria.cat_riesgos ObtenerRiesgoTramite(int pIdTramite)
        {
            b.ExecuteCommandSP("Tramite_Consulta_IdRiesgo");
            b.AddParameter("@IdTramite", pIdTramite, SqlDbType.Int);

            promotoria.cat_riesgos resultado = new promotoria.cat_riesgos();
            var reader = b.ExecuteReader();
            while (reader.Read())
            {
                resultado.Id = Convert.ToInt32(reader["IdRiesgo"].ToString());
            }
            reader = null;
            b.ConnectionCloseToTransaction();
            return resultado;
        }
    }
}