using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using prop = WFO.Propiedades.Procesos.Promotoria;

namespace WFO.Procesos.Promotoria
{
    public partial class ConsultaTramite : Utilerias.Comun
    {
        WFO.Negocio.Procesos.Promotoria.Archivos archivos = new Negocio.Procesos.Promotoria.Archivos();
        WFO.Negocio.Procesos.Promotoria.TramitesPromotoria tramitesPromotoria = new Negocio.Procesos.Promotoria.TramitesPromotoria();
        WFO.Negocio.Procesos.Operacion.TramiteProcesar Tramites = new WFO.Negocio.Procesos.Operacion.TramiteProcesar();
        WFO.Negocio.Procesos.Promotoria.Bitacora bitacora = new Negocio.Procesos.Promotoria.Bitacora();

        protected void Page_Init(object sender, EventArgs e)
        {
            manejo_sesion = (WFO.IU.ManejadorSesion)Session["Sesion"];
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int IdTramite = Convert.ToInt32(Request.Params["Id"].ToString());
                DataSet dsTramite = null;

                // manejo_sesion = (WFO.IU.ManejadorSesion)Session["Sesion"];
                List<prop.TramitesPromotoria> tramitesPromotorias = tramitesPromotoria.ConsultaTramitesPromotoria(manejo_sesion.Usuarios.IdUsuario, IdTramite, ref dsTramite);

                PintaInformacionTramite(tramitesPromotorias, dsTramite);
                //MuestraPDF(IdTramite, tramitesPromotorias[0].IdTipoTramite);
                MuestraPDF(IdTramite, dsTramite.Tables[1].Rows[0]["NmArchivo"].ToString()); // TODO: ### Pendiente de Revisión
                MuestraBitacora(IdTramite, dsTramite);
            }
        }

        private void PintaInformacionTramite(List<prop.TramitesPromotoria> Tramite, DataSet dsTramite)
        {
            foreach (prop.TramitesPromotoria DatosTramite in Tramite)
            {
                //LabelNombre.Text = DatosTramite.Contratante;
                //LabelRFC.Text = DatosTramite.RFC;
                LabelEstatusTramite.Text = DatosTramite.Estatus.ToUpper();
                LabelFolio.Text = DatosTramite.FolioCompuesto;
                //LabelFlujo.Text = DatosTramite.Operacion;
                LabelFechaRegistro.Text = DatosTramite.FechaRegistro.ToString();

                // LA INFORMACIÓN MOSTRADA ES APARTIR DEL ESTATUS DEL TRAMITE
                MuestraTramitePorEstatus(DatosTramite.Estatus, DatosTramite.Id, dsTramite);
            }
        }

        private void MuestraPDF(int Id, string Expediente)
        {
            string strDoctoWeb = "";
            string strDoctoServer = "";

            strDoctoWeb = "..\\..\\Expedientes\\" + Expediente;
            strDoctoServer = Server.MapPath("~") + "\\Expedientes\\" + Expediente;
            if (!File.Exists(strDoctoServer))
            {
                // AGREGAR ARCHIVO NO ENCONTRADO
                strDoctoWeb = "..\\..\\ArchivosDefinitivos\\404.pdf";
            }
            ltMuestraPdf.Text = "<iframe id='pdfviewer' src='" + strDoctoWeb + "#toolbar=0&navpanes=0' style='width:100%; height:450px' style='border: none;pointer-events:none;'></iframe>";
        }

        private void MuestraBitacora(int Id, DataSet dsTramite)
        {
            rptBitacora.DataSource = dsTramite.Tables[3];
            rptBitacora.DataBind();
        }

        private void MuestraTramitePorEstatus(string Estatus, int IdTramite, DataSet dsTramite)
        {


            switch (Estatus)
            {
                case "Suspendido":
                    // Ocultamos el panel de observaciones... que se consulta en bitácora
                    //ObservacionesCartaSuspendido.Visible = true;
                    AnexoArchivos.Visible = true;
                    RegistrarSuspendido.Visible = true;
                    LabelObservacionesSuspendido.Text = dsTramite.Tables[2].Rows[0]["Observación Pública"].ToString();
                    //////MuestraCartaSuspendido(IdTramite);
                    break;

                case "Ejecucion":
                    //ObservacionesCartaEjecucion.Visible = true;
                    //MuestraCartaEjecuacion(IdTramite);
                    break;

                case "Rechazo":
                    //ObservacionesCartaRechazo.Visible = true;
                    //MuestraCartaRechazo(IdTramite);
                    break;

                case "Cancelado":
                    //ObservacionesCartaCancelado.Visible = true;
                    //MuestraCartaCancelado(IdTramite);
                    break;

                default:
                    break;
            }
        }
        
        protected void btnSubirDocumento_Click(object sender, EventArgs e)
        {
            LabRespuestaArchivosCarga.Text = "";
            /* LISTA LOS ARCHIVOS DEL DOCUMENTO */
            List<prop.expediente> LstArchivosDocumento = new List<prop.expediente>();
            // SI EXISTE LA VARIABLE DE SESION LLENA LA LISTA
            if (Session["documentos"] != null)
            {
                LstArchivosDocumento = (List<prop.expediente>)Session["documentos"];
            }

            if (fileUpDocumento.HasFile)
            {
                String fileExtension = System.IO.Path.GetExtension(fileUpDocumento.FileName).ToLower();
                string fileExtension2 = "";
                if (".pdf".Contains(fileExtension) ^ ".jpg".Contains(fileExtension) ^ ".png".Contains(fileExtension))
                {
                    int fileSize = fileUpDocumento.PostedFile.ContentLength;
                    prop.expediente expedientes = new prop.expediente();

                    if (fileSize < 41943040)
                    {
                        List<prop.control_archivos> control_Archivos = archivos.ControlArchivoNuevoID();
                        int IdControlArchivo = control_Archivos[0].Id;
                        string nombreArchivo = IdControlArchivo.ToString().PadLeft(12, '0');
                        string directorioTemporal = Server.MapPath("~") + "\\DocsUp\\";

                        fileUpDocumento.PostedFile.SaveAs(directorioTemporal + nombreArchivo + fileExtension);

                        if (!fileExtension.Equals(".pdf"))
                        {
                            if (Funciones.ManejoArchivos.ConviertePDF(directorioTemporal + nombreArchivo + fileExtension, directorioTemporal + nombreArchivo + ".pdf"))
                            {
                                fileExtension2 = ".pdf";
                            }
                        }

                        fileExtension2 = ".pdf";

                        bool archivoEnPdf = false;
                        if (!fileExtension2.Equals(".pdf"))
                        {
                            archivoEnPdf = false;
                        }
                        else
                        {
                            nombreArchivo = nombreArchivo + fileExtension2;
                            archivoEnPdf = true;
                        }

                        if (archivoEnPdf)
                        {
                            expedientes.Id_Archivo = IdControlArchivo;
                            expedientes.NmArchivo = nombreArchivo;
                            expedientes.NmOriginal = fileUpDocumento.FileName;
                            expedientes.Activo = 1;
                            expedientes.Fusion = 0;
                            expedientes.Descripcion = "";

                            LstArchivosDocumento.Add(expedientes);
                            lstDocumentos.DataSource = LstArchivosDocumento;
                            lstDocumentos.DataValueField = "Id_Archivo";
                            lstDocumentos.DataTextField = "NmOriginal";
                            lstDocumentos.DataBind();

                            Session["documentos"] = LstArchivosDocumento;
                            LabRespuestaArchivosCarga.Text = "Cargado";
                        }
                        else { LabRespuestaArchivosCarga.Text = "El archivo no se puede convertir a PDF."; }
                    }
                    else
                    {
                        LabRespuestaArchivosCarga.Text = "El archivo excede el límite de 40MB.";
                    }
                }
                else
                {
                    LabRespuestaArchivosCarga.Text = "El archivo no es un PDF o JPG.";
                }
            }
            else
            {
                LabRespuestaArchivosCarga.Text = "No a cargado ningun tipo de archivo.";
            }
        }

        protected void btnSubirInsumo_Click(object sender, EventArgs e)
        {
            //List<prop.insumos> LstArchivosInsumo = new List<prop.insumos>();
            //if (Session["insumos"] != null) { LstArchivosInsumo = (List<prop.insumos>)Session["insumos"]; }
            //if (fileUpInsumo.HasFile)
            //{
            //    String fileExtension = System.IO.Path.GetExtension(fileUpInsumo.FileName).ToLower();
            //    prop.insumos oInsumo = new prop.insumos();
            //    int fileSize = fileUpInsumo.PostedFile.ContentLength;
            //    if (fileSize < 41943040)
            //    {

            //        List<prop.control_archivos> control_Archivos = archivos.ControlArchivoNuevoID();
            //        int IdArchivo = control_Archivos[0].Id;
            //        string nombreArchivo = IdArchivo.ToString().PadLeft(12, '0') + fileExtension;
            //        string directorioTemporal = Server.MapPath("~") + "\\DocsInsumos\\";
            //        fileUpInsumo.PostedFile.SaveAs(directorioTemporal + nombreArchivo);

            //        oInsumo.Id_Archivo = IdArchivo;
            //        oInsumo.NmArchivo = nombreArchivo;
            //        oInsumo.NmOriginal = fileUpInsumo.FileName;
            //        oInsumo.Activo = 1;
            //        oInsumo.Descripcion = "";
            //        oInsumo.RutaTemporal = directorioTemporal;

            //        LstArchivosInsumo.Add(oInsumo);
            //        lstInsumos.DataSource = LstArchivosInsumo;
            //        lstInsumos.DataValueField = "Id";
            //        lstInsumos.DataTextField = "NmOriginal";
            //        lstInsumos.DataBind();

            //        Session["insumos"] = LstArchivosInsumo;
            //        MensajeInsumos.Text = "Cargado";
            //    }
            //    else
            //    {
            //        MensajeInsumos.Text = "El archivo excede el límite de 40MB.";
            //    }
            //}
            //else
            //{
            //    MensajeInsumos.Text = "No has seleccionado un archivo";
            //}
        }

        protected void btnEliminaDocumento_Click(object sender, EventArgs e)
        {
            if (lstDocumentos.Items.Count > 0 && lstDocumentos.SelectedIndex > -1)
            {
                List<prop.expediente> LstArchExpediente = new List<prop.expediente>();
                List<prop.expediente> LstArchExpedienteTmp = new List<prop.expediente>();
                if (Session["documentos"] != null) { LstArchExpediente = (List<prop.expediente>)Session["documentos"]; }
                int contador = 0;
                foreach (prop.expediente oArchivo in LstArchExpediente)
                {
                    if (contador != lstDocumentos.SelectedIndex) { LstArchExpedienteTmp.Add(oArchivo); }
                    contador += 1;
                }
                lstDocumentos.DataSource = LstArchExpedienteTmp;
                lstDocumentos.DataValueField = "Id";
                lstDocumentos.DataTextField = "NmOriginal";
                lstDocumentos.DataBind();
                Session["documentos"] = LstArchExpedienteTmp;
            }
        }

        protected void btnEliminaInsumo_Click(object sender, EventArgs e)
        {
            //if (lstInsumos.Items.Count > 0 && lstInsumos.SelectedIndex > -1)
            //{
            //    List<prop.insumos> LstArchInsumos = new List<prop.insumos>();
            //    List<prop.insumos> LstArchInsumosTmp = new List<prop.insumos>();
            //    if (Session["insumos"] != null) { LstArchInsumos = (List<prop.insumos>)Session["insumos"]; }
            //    int contador = 0;
            //    foreach (prop.insumos oInsumo in LstArchInsumos)
            //    {
            //        if (contador != lstInsumos.SelectedIndex) { LstArchInsumosTmp.Add(oInsumo); }
            //        contador += 1;
            //    }
            //    lstInsumos.DataSource = LstArchInsumosTmp;
            //    lstInsumos.DataValueField = "Id";
            //    lstInsumos.DataTextField = "NmOriginal";
            //    lstInsumos.DataBind();
            //    Session["insumos"] = LstArchInsumosTmp;
            //}
        }

        protected void CheckBox_Habilita_Insumos(object sender, EventArgs e)
        {
            //if (CheckBoxInsumos.Checked.Equals(true))
            //{
            //    PanelInsumos.Visible = true;
            //}
            //else
            //{
            //    PanelInsumos.Visible = false;
            //}
        }
        
        protected void BtnContinuarSuspendido_Click(object sender, EventArgs e)
        {
            if (ValidaExpediente())
            {
                // VARIABLES
                int IdTramite = Convert.ToInt32(Request.Params["Id"].ToString());
                int TipoTramite = 0;

                string Observaciones = txObervacionesSuspendido.Text;
                string script = "";

                // REGISTRO DE ARCHIVOS - COLOCAR DESPUES DE ACTUALIZAR EL TRAMITE
                if (Session["documentos"] != null) { string resultadoExpediente = registraDocumentosExpediente(IdTramite, TipoTramite); }
                if (Session["insumos"] != null) { string resultadoInsumo = registraDocumentosInsumos(IdTramite, TipoTramite); }

                List<Propiedades.Procesos.Operacion.TramiteProcesado> objResultado = Tramites.ReingresarTramite(IdTramite, manejo_sesion.Usuarios.IdUsuario, Observaciones, "");
                if (objResultado[0].IdTramite > 0)
                {
                    // MENSAJE DE REGISTRO EN CASO DE SER EXITOSO
                    LabelActualizacionTramite.Text = "Actualización exitosa";
                    script = "$('#myModal').modal({backdrop: 'static', keyboard: false});";
                    ScriptManager.RegisterStartupScript(this, GetType(), "ServerControlScript", script, true);
                }
                else
                {
                    // MENSAJE DE REGISTRO EN CASO DE SER EXITOSO
                    LabelActualizacionTramite.Text = "No se pudo reingresar el trámite. Por favor contacte a su Administrador.";
                    script = "$('#myModal').modal({backdrop: 'static', keyboard: false});";
                    ScriptManager.RegisterStartupScript(this, GetType(), "ServerControlScript", script, true);
                }
            }
            else
            {
                MensajeSuspendido.Text = "No se han subido archivos al expediente";
            }
        }

        private bool ValidaExpediente()
        {
            bool respuesta = false;
            List<prop.expediente> LstExpediente = new List<prop.expediente>();

            if (Session["documentos"] != null)
                LstExpediente = (List<prop.expediente>)Session["documentos"];

            if (LstExpediente.Count > 0)
            {
                respuesta = true;
            }
            return respuesta;
        }

        private string registraDocumentosExpediente(int pIdTramite, int TipoTramite)
        {
            string msgError = "";
            string strRutaServidor = "";
            string strArchivoOrigen = "";

            string directorioTemporal = Server.MapPath("~") + "\\DocsUp\\";

            List<prop.expediente> LstExpediente = new List<prop.expediente>();
            if (Session["documentos"] != null)
            {
                LstExpediente = (List<prop.expediente>)Session["documentos"];
            }

            List<string> lstArchivos = new List<string>();
            foreach (prop.expediente oDocumento in LstExpediente)
            {
                strArchivoOrigen = Server.MapPath("~") + "\\DocsUp\\" + oDocumento.NmArchivo;
                if (File.Exists(strArchivoOrigen))
                {
                    archivos.Agregar_Expedientes_Tramite(TipoTramite, pIdTramite, oDocumento.Id_Archivo, oDocumento.NmArchivo, oDocumento.NmOriginal, oDocumento.Activo, oDocumento.Fusion, oDocumento.Descripcion);
                    lstArchivos.Add(strArchivoOrigen);
                }
            }

            // CONSULTA ID DEL EXPEDIENTE FUCIONADO
            List<prop.expediente> expedientes = null; //  archivos.ConsultaExpediente(pIdTramite, TipoTramite);
            string ArchFusionAnt = "";
            int Id_Expediente = 0;
            if (expedientes.Count > 0)
            {
                ArchFusionAnt = directorioTemporal + expedientes[0].NmArchivo;
                Id_Expediente = expedientes[0].Id;
            }
            
            List<prop.control_archivos> control_Archivos = archivos.ControlArchivoNuevoID();
            int IdControlArchivo = control_Archivos[0].Id;
            string nombreFusion = directorioTemporal + IdControlArchivo.ToString().PadLeft(12, '0') + ".pdf";
            if (File.Exists(nombreFusion))
            {
                File.Delete(nombreFusion);
            }

            manejo_sesion = (WFO.IU.ManejadorSesion)Session["Sesion"];
            string nmSeparador = directorioTemporal + manejo_sesion.Usuarios.IdUsuario + ".pdf";
            string nmLogo = Server.MapPath("~\\Imagenes") + @"\logo_sep.png";

            
            msgError = WFO.Funciones.ManejoArchivos.Adiciona(lstArchivos, ArchFusionAnt, nombreFusion, manejo_sesion.Usuarios.Nombre, nmSeparador, nmLogo);

            if (string.IsNullOrEmpty(msgError))
            {
                archivos.ModificarExpedienteFusion(Id_Expediente);
                archivos.Agregar_Expedientes_Tramite(TipoTramite, pIdTramite, IdControlArchivo, IdControlArchivo.ToString().PadLeft(12, '0') + ".pdf","Archivo Fucion Agragacion",1,1,"");
                msgError = "";
            }
            else
            {
                MensajeSuspendido.Text = msgError;
            }

            return "";
        }

        private string registraDocumentosInsumos(int pIdTramite, int TipoTramite)
        {
            List<prop.insumos> LstArchivosInsumo = new List<prop.insumos>();
            if (Session["insumos"] != null) { LstArchivosInsumo = (List<prop.insumos>)Session["insumos"]; }

            string strArchivoOrigen = "";

            foreach (prop.insumos oDocumento in LstArchivosInsumo)
            {
                strArchivoOrigen = Server.MapPath("~") + "\\DocsInsumos\\" + oDocumento.NmArchivo;

                if (File.Exists(strArchivoOrigen))
                {
                    archivos.Agregar_Insumo_Tramite(TipoTramite, pIdTramite, oDocumento.Id_Archivo, oDocumento.NmArchivo, oDocumento.NmOriginal, oDocumento.Activo, oDocumento.Descripcion);
                }
            }
            return "";
        }

        protected void TramiteTerminado(object sender, EventArgs e)
        {
            Session.Remove("documentos");
            Session.Remove("insumos");
            Session["documentos"] = null;
            Session["insumos"] = null;
            Response.Redirect("MisTramites.aspx");
        }

        #region EVENTOS CARTAS
        private void MuestraCartaRechazo(int IdTramite)
        {
            LabelTipoCarta.Text = "Carta rechazo";
            ltMuestraCarta.Text = "<iframe src='Cartas\\CartaRechazo.aspx?Id=" + IdTramite + "' style='width:100%; height:450px' style='border: none;'></iframe>";
        }

        private void MuestraCartaSuspendido(int IdTramite)
        {
            LabelTipoCarta.Text = "Carta suspendido";
            ltMuestraCarta.Text = "<iframe src='Cartas\\CartaSuspendido.aspx?Id=" + IdTramite + "' style='width:100%; height:450px' style='border: none;'></iframe>";
        }

        private void MuestraCartaEjecuacion(int IdTramite)
        {
            LabelTipoCarta.Text = "Carta ejecución";
            ltMuestraCarta.Text = "<iframe src='Cartas\\CartaEjecucion.aspx?Id=" + IdTramite + "' style='width:100%; height:450px' style='border: none;'></iframe>";
        }

        private void MuestraCartaHold(int IdTramite)
        {
            LabelTipoCarta.Text = "Carta hold";
            ltMuestraCarta.Text = "<iframe src='Cartas\\CartaHold.aspx?Id=" + IdTramite + "' style='width:100%; height:450px' style='border: none;'></iframe>";
        }

        private void MuestraCartaPCI(int IdTramite)
        {
            LabelTipoCarta.Text = "Carta PCI";
            ltMuestraCarta.Text = "<iframe src='Cartas\\CartaPCI.aspx?Id=" + IdTramite + "' style='width:100%; height:450px' style='border: none;'></iframe>";
        }
        private void MuestraCartaCancelado(int IdTramite)
        {
            LabelTipoCarta.Text = "Carta Cancelado";
            ltMuestraCarta.Text = "<iframe src='Cartas\\CartePDFCancelar.aspx?Id=" + IdTramite + "' style='width:100%; height:450px' style='border: none;'></iframe>";
        }
        #endregion
    }
}