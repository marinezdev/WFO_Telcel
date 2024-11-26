using DevExpress.Web;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using prop = WFO.Propiedades.Procesos.Promotoria;

namespace WFO.Procesos.Promotoria
{
    public partial class TramiteNuevo1 : Utilerias.Comun
    {
        WFO.Negocio.Procesos.Promotoria.Archivos archivos = new Negocio.Procesos.Promotoria.Archivos();
        WFO.Negocio.Procesos.Promotoria.TramiteN1 tramiteN1 = new Negocio.Procesos.Promotoria.TramiteN1();

        public string ArchivoMaximo1 { get; set; }
        public string ExpedienteMaximo1 { get; set; }

        protected void Page_Init(object sender, EventArgs e)
        {
            ArchivoMaximo1 = System.Web.Configuration.WebConfigurationManager.AppSettings["ArchivoMaximo1"].ToString();
            ExpedienteMaximo1 = System.Web.Configuration.WebConfigurationManager.AppSettings["ExpedienteMaximo1"].ToString();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                manejo_sesion = (WFO.IU.ManejadorSesion)Session["Sesion"];
                lblDocumentosTipos.Text = "Archivos(*.PDF)";
                lblDocumentosTamaño.Text = "Tamaño máximo de archivo: " + ArchivoMaximo1.ToString() + " MB";
                ExpedienteTamaño.Text = "Tamaño máximo de expediente: " + ExpedienteMaximo1.ToString() + " MB";

                CargarProyectos(1);
            }
        }

        private void CargarProyectos(int IdProveedor)
        {
            List<prop.Proyectos> catalogoProyectos = tramiteN1.seleccionarPoryectos(IdProveedor);
            cboProyectos.DataSource = catalogoProyectos;
            cboProyectos.DataTextField = "Nombre";
            cboProyectos.DataValueField = "Id";
            cboProyectos.DataBind();
        }

        protected void btnSubirDocumento_Click(object sender, EventArgs e)
        {
            Mensajes.Text = "";
            manejo_sesion = (WFO.IU.ManejadorSesion)Session["Sesion"];
            LabRespuestaArchivosCarga.Text = "";
            List<prop.expediente> LstArchivosDocumento = new List<prop.expediente>();
            if (Session["documentos"] != null)
            {
                LstArchivosDocumento = (List<prop.expediente>)Session["documentos"];
            }

            if (fileUpDocumento.HasFile)
            {
                String fileExtension = System.IO.Path.GetExtension(fileUpDocumento.FileName).ToLower();

                if (".PDF".Contains(fileExtension.ToUpper()))
                {
                    bool archivoExiste = false;
                    List<prop.expediente> LstArchExpediente = new List<prop.expediente>();

                    if (Session["documentos"] != null)
                    {
                        LstArchExpediente = (List<prop.expediente>)Session["documentos"];

                        int contador = 0;
                        foreach (prop.expediente oArchivo in LstArchExpediente)
                        {
                            if (oArchivo.NmOriginal.ToUpper() == fileUpDocumento.FileName.ToUpper())
                            {
                                archivoExiste = true;
                            }
                            contador += 1;
                        }
                    }

                    if (archivoExiste)
                    {
                        LabRespuestaArchivosCarga.Text = "El archivo ya ha sido cargado...";
                    }
                    else
                    {
                        int fileSize = fileUpDocumento.PostedFile.ContentLength;
                        prop.expediente expedientes = new prop.expediente();

                        if (fileSize < int.Parse(System.Web.Configuration.WebConfigurationManager.AppSettings["ArchivoMaximoByte1"].ToString()))
                        {
                            double IdControlArchivo = double.Parse(DateTime.Now.ToString("yyMMddHHmmss") + manejo_sesion.Usuarios.IdUsuario.ToString("D3"));
                            string nombreArchivo = DateTime.Now.ToString("yyMMddHHmmss") + manejo_sesion.Usuarios.IdUsuario.ToString("D3"); //string nombreArchivo = IdControlArchivo.ToString().PadLeft(12, '0');
                            string directorioTemporal = Server.MapPath("~") + "\\DocsUp\\";

                            fileUpDocumento.PostedFile.SaveAs(directorioTemporal + nombreArchivo + fileExtension);
                            expedientes.Id_Archivo = IdControlArchivo;
                            expedientes.NmArchivo = nombreArchivo + fileExtension;
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
                            LabRespuestaArchivosCarga.Text = "Archivo Cargado";
                            //cargaPromotoria(manejo_sesion.Usuarios.IdUsuario);  // TODO: Verificar si esta funcionalidad deberá serguir disponible
                        }
                        else
                        {
                            // ###Pendiente: Hacer las alertas con la función de Rogelio
                            LabRespuestaArchivosCarga.Text = "El archivo excede el límite de " + ArchivoMaximo1 + "MB.";
                        }
                    }
                }
                else
                {
                    LabRespuestaArchivosCarga.Text = "El archivo no se encuentra en el formato adecuado.";
                }
            }
            else
            {
                LabRespuestaArchivosCarga.Text = "No a cargado ningun tipo de archivo.";
            }
        }

        protected void btnEliminaDocumento_Click(object sender, EventArgs e)
        {
            if (lstDocumentos.Items.Count > 0 && lstDocumentos.SelectedIndex > -1)
            {
                List<prop.expediente> LstArchExpediente = new List<prop.expediente>();
                List<prop.expediente> LstArchExpedienteTmp = new List<prop.expediente>();
                if (Session["documentos"] != null)
                {
                    LstArchExpediente = (List<prop.expediente>)Session["documentos"];
                }

                int contador = 0;
                foreach (prop.expediente oArchivo in LstArchExpediente)
                {
                    if (contador != lstDocumentos.SelectedIndex)
                    {
                        LstArchExpedienteTmp.Add(oArchivo);
                    }
                    else
                    {
                        string archivoDelete = LstArchExpediente[contador].NmArchivo.ToString();
                        string directorioTemporal = Server.MapPath("~") + "\\DocsUp\\";
                        File.Delete(directorioTemporal + archivoDelete);
                        LabRespuestaArchivosCarga.Text = "Archivo Eliminado";
                    }
                    contador += 1;
                }

                lstDocumentos.DataSource = LstArchExpedienteTmp;
                lstDocumentos.DataValueField = "Id";
                lstDocumentos.DataTextField = "NmOriginal";
                lstDocumentos.DataBind();
                Session["documentos"] = LstArchExpedienteTmp;
            }
        }

        protected void BtnContinuar_Click(object sender, EventArgs e)
        {
            Respuesta.Text = "";
            CrearTramite();
        }

        protected void TramiteTerminado(object sender, EventArgs e)
        {
            Session.Remove("documentos");
            Session.Remove("insumos");
            Session["documentos"] = null;
            Session["insumos"] = null;
            Response.Redirect("MisTramites.aspx");
        }


        protected void CrearTramite()
        {
            if (cboProyectos.SelectedValue.ToString() == "-1")
            {
                Mensajes.Text = "Debe seleccionar un proyecto.";
                return;
            }

            if (ValidaExpediente())
            {
                manejo_sesion = (WFO.IU.ManejadorSesion)Session["Sesion"];

                string nombreExpediente = "";
                if (string.IsNullOrEmpty(EvaluaDocumento(manejo_sesion.Usuarios.IdUsuario, ref nombreExpediente)))
                {

                    prop.TramiteNuevo1 NuevoTramite = new prop.TramiteNuevo1();
                    NuevoTramite.IdTipoTramite = 1;
                    //List<prop.promotoria_usuario> Promotoria_Usuarios = Catalogos.Promotoria_Usuarios(manejo_sesion.Usuarios.IdUsuario);
                    //NuevoTramite.IdPromotoria = Promotoria_Usuarios[0].Id;

                    NuevoTramite.IdUsuario = manejo_sesion.Usuarios.IdUsuario;
                    NuevoTramite.IdStatus = 1;
                    NuevoTramite.IdPrioridad = 5;
                    //NuevoTramite.FechaSolicitud = dtFechaSolicitud.Text;

                    //NuevoTramite.IdAgente = 0;
                    //if (!string.IsNullOrEmpty(txClaveAgente.Text.ToString()))
                    //{
                    //    List<prop.agente_promotoria_usuario> agente_Promotoria_Usuarios = Catalogos.Agente_Promotoria_Usuarios(manejo_sesion.Usuarios.IdUsuario, txClaveAgente.Text.ToString());
                    //    if (agente_Promotoria_Usuarios.Count != 0)
                    //    {
                    //        NuevoTramite.IdAgente = agente_Promotoria_Usuarios[0].Id;
                    //    }
                    //}

                    //NuevoTramite.Proyecto = cboProyectos.SelectedItem.Text.ToString();
                    NuevoTramite.Proyecto = cboProyectos.SelectedValue.ToString();
                    NuevoTramite.Expediente = nombreExpediente;

                    //NuevoTramite.IdSubProducto = Convert.ToInt32(LisSubproducto.SelectedValue);
                    ////NuevoTramite.IdRiesgo = Convert.ToInt32(LisRiesgos.SelectedValue);
                    //NuevoTramite.NumeroOrden = textNumeroOrden.Text;
                    //NuevoTramite.idRamo = 2;
                    //NuevoTramite.IdSisLegados = "";
                    //NuevoTramite.kwik = "";
                    //NuevoTramite.IdMoneda = Convert.ToInt32(cboMoneda.SelectedValue);

                    //// VARIABLES VACIAS
                    //NuevoTramite.TipoPersona = 0;
                    //NuevoTramite.Nombre = "";
                    //NuevoTramite.ApPaterno = "";
                    //NuevoTramite.ApMaterno = "";
                    //NuevoTramite.Sexo = "";
                    //NuevoTramite.FechaNacimiento = "1900-01-01";
                    //NuevoTramite.RFC = "";
                    //NuevoTramite.IdNacionalidad = 0;
                    //NuevoTramite.FechaConst = "1900-01-01";

                    //NuevoTramite.TitularNombre = "";
                    //NuevoTramite.TitularApPat = "";
                    //NuevoTramite.TitularApMat = "";
                    //NuevoTramite.IdTitularNacionalidad = 0;
                    //NuevoTramite.TitularSexo = "";
                    //NuevoTramite.TitularFechaNacimiento = "1900-01-01";
                    //NuevoTramite.TitularContratante = 0;
                    //NuevoTramite.PrimaCotizacion = 0;
                    //NuevoTramite.Observaciones = "";

                    //if (cboTipoContratante.SelectedValue.Equals("Fisica"))
                    //{
                    //    NuevoTramite.TipoPersona = 1;
                    //    NuevoTramite.Nombre = txNombre.Text;
                    //    NuevoTramite.ApPaterno = txApPat.Text;
                    //    NuevoTramite.ApMaterno = txApMat.Text;
                    //    NuevoTramite.Sexo = txSexo.SelectedValue.ToString();
                    //    NuevoTramite.FechaNacimiento = dtFechaNacimiento.Text;
                    //    NuevoTramite.RFC = txRfc.Text;
                    //    NuevoTramite.IdNacionalidad = Convert.ToInt32(txNacionalidad.Value);
                    //}
                    //else if (cboTipoContratante.SelectedValue.Equals("Moral"))
                    //{
                    //    NuevoTramite.TipoPersona = 2;
                    //    NuevoTramite.Nombre = txNomMoral.Text;
                    //    NuevoTramite.RFC = txRfcMoral.Text;
                    //    NuevoTramite.FechaConst = dtFechaConstitucion.Text;
                    //}

                    //if (CheckBox1.Checked.Equals(true))
                    //{
                    //    NuevoTramite.TitularContratante = 1;
                    //    NuevoTramite.TitularNombre = txTiNombre.Text;
                    //    NuevoTramite.TitularApPat = txTiApPat.Text;
                    //    NuevoTramite.TitularApMat = txTiApMat.Text;
                    //    NuevoTramite.TitularFechaNacimiento = dtFechaNacimientoTitular.Text;
                    //    NuevoTramite.IdTitularNacionalidad = Convert.ToInt32(txNacionalidad.Value);
                    //    NuevoTramite.TitularSexo = txtSexoM.SelectedValue.ToString();
                    //}

                    //if (HombresClave.Checked.Equals(true))
                    //{
                    //    NuevoTramite.HombreClave = 1;
                    //}
                    //else
                    //{
                    //    NuevoTramite.HombreClave = 0;
                    //}


                    //NuevoTramite.PrimaCotizacion = Convert.ToDouble(txtPrimaTotalGMM.Text);
                    //NuevoTramite.SumaBasica = Convert.ToDouble(txtSumaAseguradaBasica.Text);
                    //NuevoTramite.Observaciones = txObervaciones.Text;

                    // REALIZA REGISTRO DEL NUEVO TRAMITE
                    List<prop.RespuestaNuevoTramiteN1> respuestaNuevoTramiteN1s = tramiteN1.NuevoTramiteN1(NuevoTramite);

                    if (respuestaNuevoTramiteN1s[0].Folio == "KO")
                    {
                        Respuesta.Text = "No registrado";
                    }
                    else
                    {
                        string script = "";
                        script = "$('#myModal').modal({backdrop: 'static', keyboard: false});";
                        ScriptManager.RegisterStartupScript(this, GetType(), "ServerControlScript", script, true);
                        LabelFolio.Text = respuestaNuevoTramiteN1s[0].Folio.ToString();
                    }
                }
                else
                {
                    string script = "";
                    script = "ErrorArchvios();";
                    ScriptManager.RegisterStartupScript(this, GetType(), "ServerControlScript", script, true);
                }
            }
            else
            {
                string script = "";
                script = "FormularioIncompleto();";
                ScriptManager.RegisterStartupScript(this, GetType(), "ServerControlScript", script, true);

                Mensajes.Text = "No se han subido archivos al expediente";
            }
        }

        private string registraDocumentosExpediente(int pIdTramite, int TipoTramite)
        {
            string msgError = "";
            string strRutaServidor = "";
            string strArchivoOrigen = "";
            try
            {
                strRutaServidor = Server.MapPath("~") + "\\DocsUp\\";

                List<prop.expediente> LstExpediente = new List<prop.expediente>();

                /* COMPRUEBA LA LISTA APÁRTIR DE LA SESION */
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
            }

            catch (Exception ex) { msgError = ex.Message; }
            return "";
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

        private string EvaluaDocumento(int IdUsuario, ref string ExpedienteFinal)
        {
            string msgError = "";
            string strRutaServidor = "";
            string strArchivoOrigen = "";

            try
            {
                strRutaServidor = Server.MapPath("~") + "\\DocsUp\\";
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
                        if (obtenerNumeroPaginas(strArchivoOrigen) > 0)
                        {
                            lstArchivos.Add(strArchivoOrigen);
                        }
                        else
                        {
                            lstArchivos.Clear();
                            msgError = "No se pudó leer el archivo " + oDocumento.NmOriginal.ToString() + ". Puede encontrarse dañado. Por favor verificarlo";
                        }

                    }
                }

                // Generamos nombre de archivo unico en sistema.
                bool expediente = false;
                string nombreArchivoFusion = DateTime.Now.ToString("yyMMddHHmmss") + IdUsuario.ToString("D3") + "01" + ".pdf";
                string directorioTemporal = Server.MapPath("~") + "\\Expedientes\\";
                while (expediente == false)
                {
                    if (!File.Exists(directorioTemporal + nombreArchivoFusion))
                    {
                        expediente = true;
                        ExpedienteFinal = nombreArchivoFusion;
                    }
                }
                msgError = Funciones.ManejoArchivos.Fusiona(lstArchivos, directorioTemporal + nombreArchivoFusion);
            }
            catch (Exception ex)
            {
                msgError = ex.Message;
            }
            return msgError;
        }

        private int obtenerNumeroPaginas(string archivo)
        {
            PdfReader pdfReader = new PdfReader(archivo);
            int numberOfPages = pdfReader.NumberOfPages;
            return numberOfPages;
        }

    }
}