using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using prop = WFO.Propiedades.Procesos.Operacion;
using DevExpress.Web.ASPxTreeList;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace WFO.Procesos.Operador
{
    public partial class TramiteProcesar : Utilerias.Comun
    {
        WFO.Negocio.Procesos.Operacion.TramiteProcesar Tramites = new WFO.Negocio.Procesos.Operacion.TramiteProcesar();
        WFO.Negocio.Procesos.Operacion.Mesas mesas = new Negocio.Procesos.Operacion.Mesas();
        WFO.Negocio.Procesos.Promotoria.Archivos archivos = new Negocio.Procesos.Promotoria.Archivos();
        WFO.Negocio.Procesos.Promotoria.Catalogos Catalogos = new Negocio.Procesos.Promotoria.Catalogos();
        WFO.Negocio.Procesos.Operacion.Bitacora bitacora = new Negocio.Procesos.Operacion.Bitacora();
        WFO.Negocio.Procesos.Operacion.MotivosSuspension _MotivosHold = new Negocio.Procesos.Operacion.MotivosSuspension();
        WFO.Negocio.Procesos.Operacion.CapturaMasiva.Catalogos capturaMasiva = new Negocio.Procesos.Operacion.CapturaMasiva.Catalogos();
        WFO.Negocio.Procesos.Operacion.CapturaMasiva.Asegurados asegurados = new Negocio.Procesos.Operacion.CapturaMasiva.Asegurados();
        WFO.Negocio.Procesos.Operacion.CapturaMasiva.CoAsegurados coAsegurados = new Negocio.Procesos.Operacion.CapturaMasiva.CoAsegurados();
        WFO.Negocio.Procesos.Operacion.CapturaMasiva.AgentesDXN agentesDXN = new Negocio.Procesos.Operacion.CapturaMasiva.AgentesDXN();
        WFO.Negocio.Procesos.Operacion.CapturaMasiva.Tarjetas tarjetas = new Negocio.Procesos.Operacion.CapturaMasiva.Tarjetas();
        WFO.Negocio.Procesos.Operacion.CapturaMasiva.Asegurado_direciones asegurado_Direciones = new Negocio.Procesos.Operacion.CapturaMasiva.Asegurado_direciones();
        WFO.Negocio.Procesos.Operacion.PermisosMesaControles permisosMesa = new Negocio.Procesos.Operacion.PermisosMesaControles();
        WFO.Negocio.Procesos.Operacion.Indicador_StatusMesas indicador = new Negocio.Procesos.Operacion.Indicador_StatusMesas();
        WFO.Negocio.Procesos.Operacion.Cat_CheckBox_Mesa cat_CheckBox = new Negocio.Procesos.Operacion.Cat_CheckBox_Mesa();

        List<prop.TramiteProcesar> Tramite_a_Procesar;

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(Request.QueryString["IdMesa"]))
            {
                manejo_sesion = (WFO.IU.ManejadorSesion)Session["Sesion"];

                //string objeto = Request.QueryString["objeto"].ToString();
                //string[] parametros = Seguridad.Cifrado.Desencriptar(objeto).Split(',');

                string pIdMesa = Request.QueryString["IdMesa"].ToString();
                string pIdTramite = Request.QueryString["IdTramite"].ToString();

                //hfIdMesa.Value = parametros[0].ToString();
                //hfIdTramite.Value = parametros[1].ToString();

                hfIdMesa.Value = pIdMesa;
                hfIdTramite.Value = pIdTramite;
            }
            else
            {
                Response.Redirect("Default.aspx", true);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            MotivosSuspension();
            if (int.Parse(hfIdMesa.Value) > 0)
            {
                if (!IsPostBack)
                {
                    PostBack(int.Parse(hfIdMesa.Value), manejo_sesion.Usuarios.IdUsuario);
                }
            }
            else
            {
                Response.Redirect("Default.aspx", true);
            }
        }

        private void MotivosSuspension()
        {
            List<prop.MotivosSuspension> lsMotivosSuspension = _MotivosHold.SelecionarMotivos(int.Parse(hfIdMesa.Value));

            treeListSuspender.ClearNodes();
            treeListSuspender.DataSource = lsMotivosSuspension.Where(MotivoSuspension => lsMotivosSuspension.FirstOrDefault(valor => MotivoSuspension.IdTramiteTipoRechazo == 1) != null);      // SELECT * FROM cat_Tramite_RechazosTipos;
            treeListSuspender.DataBind();
            treeListSuspender.ExpandToLevel(3);

            treeListRechazar.ClearNodes();
            treeListRechazar.DataSource = lsMotivosSuspension.Where(MotivoSuspension => lsMotivosSuspension.FirstOrDefault(valor => MotivoSuspension.IdTramiteTipoRechazo == 1) != null);      // SELECT * FROM cat_Tramite_RechazosTipos;
            treeListRechazar.DataBind();
            treeListRechazar.ExpandToLevel(3);
        }


        private void PostBack(int pIdMesa, int IdUsuario)
        {
            // Permisos de Botones
            //btnHold.OnClientClick = "ShowHold(); return false;";
            btnSuspender.OnClientClick = "ShowSuspender(); return false;";
            btnRechazar.OnClientClick = "ShowRechazar(); return false;";
            //btnPausa.OnClientClick = "ShowPausa(); return false;";
            //btnCancelacion.OnClientClick = "ShowCancelar(); return false;";

            PintaMesa(pIdMesa, IdUsuario);
            VerificaTramiteDisponible(pIdMesa);
        }

        private void PintaMesa(int pIdMesa, int IdUsuario)
        {
            List<prop.Mesa> mesa = mesas.SelecionarMesasUsuarioMesa(IdUsuario, pIdMesa);

            if (mesa.Count > 0)
            {
                LabelNombreMesa.ForeColor = System.Drawing.Color.FromName(mesa[0].Color);
                LabelNombreMesa.Text = "Proceso - " + mesa[0].nombre;
                hfNombreMesa.Value = mesa[0].nombre;

                prop.PermisosMesaControles permisosMesaControles = new prop.PermisosMesaControles();
                btnAceptar.Visible = Convert.ToBoolean((permisosMesaControles = permisosMesa.PermisosMesaControles_Selecionar(pIdMesa, "btnAceptar")).Activo);
                //btnPCI.Visible = Convert.ToBoolean((permisosMesaControles = permisosMesa.PermisosMesaControles_Selecionar(pIdMesa, "btnPCI")).Activo);
                //btnHold.Visible = Convert.ToBoolean((permisosMesaControles = permisosMesa.PermisosMesaControles_Selecionar(pIdMesa, "btnHold")).Activo);
                btnPausa.Visible = Convert.ToBoolean((permisosMesaControles = permisosMesa.PermisosMesaControles_Selecionar(pIdMesa, "btnPausa")).Activo);
                btnDetener.Visible = Convert.ToBoolean((permisosMesaControles = permisosMesa.PermisosMesaControles_Selecionar(pIdMesa, "btnDetener")).Activo);
                btnRechazar.Visible = Convert.ToBoolean((permisosMesaControles = permisosMesa.PermisosMesaControles_Selecionar(pIdMesa, "btnRechazar")).Activo);
                //btnEnviaMesa.Visible = Convert.ToBoolean((permisosMesaControles = permisosMesa.PermisosMesaControles_Selecionar(pIdMesa, "btnEnviaMesa")).Activo);
                btnSuspender.Visible = Convert.ToBoolean((permisosMesaControles = permisosMesa.PermisosMesaControles_Selecionar(pIdMesa, "btnSuspender")).Activo);
                //btnSelccionCompleta.Visible = Convert.ToBoolean((permisosMesaControles = permisosMesa.PermisosMesaControles_Selecionar(pIdMesa, "btnSelccionCompleta")).Activo);
                //btnCancelacion.Visible = Convert.ToBoolean((permisosMesaControles = permisosMesa.PermisosMesaControles_Selecionar(pIdMesa, "btnCancelacion")).Activo);
                //PanelObservacionesPubicas.Visible = Convert.ToBoolean((permisosMesaControles = permisosMesa.PermisosMesaControles_Selecionar(pIdMesa, "PanelObservacionesPubicas")).Activo);
            }
            else
            {
                Response.Redirect("Default.aspx");
            }
        }

        private void VerificaTramiteDisponible(int pIdMesa)
        {
            DataSet dsTramiteAsignado = null;
            Tramite_a_Procesar = Tramites.ObtenerTramite(manejo_sesion.Usuarios.IdUsuario, pIdMesa, int.Parse(hfIdTramite.Value), ref dsTramiteAsignado);
            if (Tramite_a_Procesar.Count > 0)
            {
                for (int i = 0; i < Tramite_a_Procesar.Count; i++)
                {
                    // COLOCA EL ID DEL TRAMITE PARA SER UTILIZADO DESPUES
                    hfIdTramite.Value = Tramite_a_Procesar[i].IdTramite.ToString();
                    ////hfTipoTramite.Value = Tramite_a_Procesar[i].IdTipoTramite.ToString();
                    ////LabelFlujo.Text = Tramites.ObtenerTipoTramite(Tramite_a_Procesar[i].IdTramite).Nombre;

                    if (Int32.Parse(hfIdTramite.Value) <= 0)
                    {
                        Response.Redirect("Default.aspx?msj=1", true);
                    }
                    else
                    {
                        LabelFolio.Text = Tramite_a_Procesar[i].Folio;
                        InfoFechaRegistro.Text = Tramite_a_Procesar[i].FechaRegistro;
                        ////LabelNumeroPoliza.Text = Tramite_a_Procesar[i].IdSisLegados;
                        ////LabelProducto.Text = Tramite_a_Procesar[i].Producto;
                        ////LabelSubProducto.Text = Tramite_a_Procesar[i].SubProducto;

                        CargarPFD(dsTramiteAsignado.Tables[1]);
                        CargaBitacoraPublica(dsTramiteAsignado.Tables[2]);
                        CargaBitacoraPrivada(dsTramiteAsignado.Tables[2]);
                    }
                }
            }
            else
            {
                mensajes.MostrarMensaje(this, "No hay trámites disponibles...", "Default.aspx");
            }
        }

        private void CargaBitacoraPrivada(DataTable dtBitacora)
        {
            ////List<promotoria.bitacora> bitacoras = bitacora.ConsultaBitacoraPrivada(Id);
            rptBitacoraPrivada.DataSource = dtBitacora;
            rptBitacoraPrivada.DataBind();
        }

        private void CargaBitacoraPublica(DataTable dtBitacora)
        {
            ////List<promotoria.bitacora> bitacoras = bitacora.ConsultaBitacoraPublica(Id);
            rptBitacoraPublica.DataSource = dtBitacora;
            rptBitacoraPublica.DataBind();
        }

        private void CargarPFD(DataTable dtExpediente)
        {
            ////int TipoTramite = int.Parse(hfTipoTramite.Value.ToString());

            string strDoctoWeb = "";
            string strDoctoServer = "";

            strDoctoWeb = "..\\..\\ArchivosDefinitivos\\404.pdf";

            if (dtExpediente.Rows.Count > 0)
            {
                strDoctoWeb = "..\\..\\Expedientes\\" + dtExpediente.Rows[0]["NmArchivo"].ToString();
                strDoctoServer = Server.MapPath("~") + "\\Expedientes\\" + dtExpediente.Rows[0]["NmArchivo"].ToString();

                if (!File.Exists(strDoctoServer))
                {
                    // AGREGAR ARCHIVO NO ENCONTRADO
                    strDoctoWeb = "..\\..\\ArchivosDefinitivos\\404.pdf";
                }

                ltMuestraPdf.Text = "";
                ltMuestraPdf.Text = "<iframe src='" + strDoctoWeb + "' style='width:100%; height:540px' style='border: none;'></iframe>";

                hfExpediente.Value = strDoctoServer;
            }
        }

        protected void btnAceptarValida_Click(object sender, EventArgs e)
        {
            string NombreMesa = hfNombreMesa.Value;
            prop.PermisosMesaControles permisosMesaControles = new prop.PermisosMesaControles();

            string script = "";
            script = "$('#ContinuarTramite').modal('show');";
            ScriptManager.RegisterStartupScript(this, GetType(), "ServerControlScript", script, true);
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            Funciones.VariablesGlobales.StatusMesa IdStatusMesaProcesado = Funciones.VariablesGlobales.StatusMesa.Procesado;
            int IdTramite = Int32.Parse(hfIdTramite.Value);
            prop.PermisosMesaControles permisosMesaControles = new prop.PermisosMesaControles();

            List<Propiedades.Procesos.Operacion.TramiteProcesado> objResultado = Tramites.ProcesarTramite(IdTramite, int.Parse(hfIdMesa.Value), manejo_sesion.Usuarios.IdUsuario, IdStatusMesaProcesado, "", txtObservacionesPrivadas.Text, "");
            if (objResultado[0].IdTramite > 0)
            {
                //int TipoTramite = int.Parse(hfTipoTramite.Value.ToString());
                //// REGISTRO DE ARCHIVOS - COLOCAR DESPUES DE ACTUALIZAR EL TRAMITE
                //if (Session["documentos"] != null) { string resultadoExpediente = registraDocumentosExpediente(IdTramite, TipoTramite); }
                //if (Session["insumos"] != null) { string resultadoInsumo = registraDocumentosInsumos(IdTramite, TipoTramite); }
                //// REGISTRA EL CHECKLIST
                //RegistraCheckBoxList();

                // Verificmaos el tipo de mesa para el procesamiento...
                switch (hfNombreMesa.Value.ToString().ToUpper())
                {
                    case "REVISIÓN":
                        agregarFirmaDigital("TxjMHLo4Pv4QNhNxuMWwcQ==", 850, 125, "firma1.png", 1, 100f, 75f);
                        break;

                    case "VOBO 1":
                        agregarFirmaDigital("TxjMHLo4Pv4QNhNxuMWwcQ==", 130, 265, "autoriza_yellow.png", 2, 50f, 50f);
                        break;

                    case "VOBO 2":
                        agregarFirmaDigital("TxjMHLo4Pv4QNhNxuMWwcQ==", 850, 265, "autoriza_green.png", 3, 50f, 50f);
                        break;

                    case "VOBO 3":
                        agregarFirmaDigital("TxjMHLo4Pv4QNhNxuMWwcQ==", 130, 125, "autoriza_blue.png", 4, 50f, 50f);
                        break;
                }

                

                if (objResultado[0].Accion == "KO")
                {
                    int IdMesa = int.Parse(hfIdMesa.Value.ToString());
                    log.Agregar("Error de aceptación de trámite ID = " + IdTramite + ", en mesa IDMESA = " + IdMesa + " , respuesta de store procedure spWFOTramiteProcesar: " + objResultado[0].Accion);
                }
                TramiteTerminado();


                //Response.Redirect("TramiteProcesar.aspx?IdMesa=" + hfIdMesa.Value.ToString(), true);
                Response.Redirect("TramiteProcesar.aspx?IdMesa=" + hfIdMesa.Value.ToString() + "&IdTramite=0", true);

                //string parameters = "";
                //parameters = hfIdMesa.Value.ToString() + ";0";
                //parameters = Seguridad.Cifrado.Encriptar(parameters);
                //Response.Redirect("TramiteProcesar.aspx?objeto=" + parameters + "",true);
            }
        }

        private void agregarFirmaDigital(string usuario, int X1, int Y1, string imagen, int consecutivo, float ancho, float alto)
        {
            string archivoFirmado= hfExpediente.Value.ToString().Replace(".pdf", "_fase" + consecutivo + ".pdf");

            if (File.Exists(archivoFirmado))
                File.Delete(archivoFirmado);


            using (Stream inputPdfStream = new FileStream(hfExpediente.Value.ToString(), FileMode.Open, FileAccess.Read, FileShare.Read))
            using (Stream inputImageStream = new FileStream("C:\\RMF\\ASAE\\Proyectos\\GIT\\WFO_Telcel\\WFO\\Imagenes\\" + imagen, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (Stream outputPdfStream = new FileStream(archivoFirmado, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                var reader = new PdfReader(inputPdfStream);
                var stamper = new PdfStamper(reader, outputPdfStream);
                var pdfContentByte = stamper.GetOverContent(1);

                iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(inputImageStream);
                image.ScaleAbsolute(ancho, alto);
                image.SetAbsolutePosition(X1, Y1);
                pdfContentByte.AddImage(image);


                // select the font properties
                BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                pdfContentByte.SetColorFill(BaseColor.GRAY);
                pdfContentByte.SetFontAndSize(bf, 12);

                // write the text in the pdf content
                pdfContentByte.BeginText();
                string text = usuario;
                // put the alignment and coordinates here
                pdfContentByte.ShowTextAligned(Element.ALIGN_LEFT, text, 0, 0, 0);
                pdfContentByte.EndText();

                stamper.Close();
            }

            File.Delete(hfExpediente.Value.ToString());
            File.Copy(archivoFirmado, hfExpediente.Value.ToString(), true);
            File.Delete(archivoFirmado);
        }

        protected void btnAplicarSuspender_Click(object sender, EventArgs e)
        {
            var nodos = treeListSuspender.GetSelectedNodes();
            int IdMotivoRechazo = 0;
            string MotivosRechazos = "";

            if (nodos.Count > 0)
            {
                if (txtObservacionesPublicasSuspender.Text.Length > 0)
                {
                    MotivosRechazos = "";
                    foreach (TreeListNode node in nodos)
                    {
                        IdMotivoRechazo = Convert.ToInt32(node.GetValue("id"));
                        if (MotivosRechazos.Length > 0)
                            MotivosRechazos += ",";
                        MotivosRechazos += IdMotivoRechazo.ToString();
                    }

                    treeListSuspender.UnselectAll();

                    Funciones.VariablesGlobales.StatusMesa IdStatusMesa = Funciones.VariablesGlobales.StatusMesa.Suspendido;
                    int IdTramite = Int32.Parse(hfIdTramite.Value);
                    int TipoTramite = int.Parse(hfTipoTramite.Value.ToString());

                    List<Propiedades.Procesos.Operacion.TramiteProcesado> objResultado = Tramites.ProcesarTramite(IdTramite, int.Parse(hfIdMesa.Value), manejo_sesion.Usuarios.IdUsuario, IdStatusMesa, txtObservacionesPublicasSuspender.Text.Trim(), txtObservacionesPrivadas.Text, MotivosRechazos);
                    if (!Page.ClientScript.IsStartupScriptRegistered(this.GetType(), "PopupScript"))
                    {
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "PopupScript", "LimpiarForm();", true);
                    }

                    //GeneraCartaEstatusTramite(IdTramite, "Suspendido", TipoTramite);

                    if (objResultado[0].IdTramite > 0)
                    {
                        //// REGISTRO DE ARCHIVOS - COLOCAR DESPUES DE ACTUALIZAR EL TRAMITE
                        //if (Session["documentos"] != null) { string resultadoExpediente = registraDocumentosExpediente(IdTramite, TipoTramite); }
                        //if (Session["insumos"] != null) { string resultadoInsumo = registraDocumentosInsumos(IdTramite, TipoTramite); }

                        TituloModal.Text = "Trámite suspendido";
                        MensajeModal.Text = "El trámite se suspendió...";

                        string script = "";
                        script = "pnlPopMotivosSuspender.Hide(); $('#myModal').modal({backdrop: 'static', keyboard: false}); ";
                        ScriptManager.RegisterStartupScript(this, GetType(), "ServerControlScript", script, true);

                        // Response.Redirect("TramiteProcesar.aspx?IdMesa=" + hfIdMesa.Value.ToString(), true);
                    }
                }
                else
                {
                    string script = "";
                    script = "Alerta('Observaciones Públicas', 'Debe establecer las observaciones públicas.');";
                    ScriptManager.RegisterStartupScript(this, GetType(), "ServerControlScript", script, true);
                }
            }
            else
            {
                string script = "";
                script = "Alerta('Motivos Suspensión', 'No ha seleccionado ningún motivo de suspensión');";
                ScriptManager.RegisterStartupScript(this, GetType(), "ServerControlScript", script, true);
            }
        }

        protected void btnAplicarRechazar_Click(object sender, EventArgs e)
        {
            List<prop.MotivosSuspension> lsMotivosSuspension = _MotivosHold.SelecionarMotivos(int.Parse(hfIdMesa.Value.ToString()));
            lsMotivosSuspension.Where(MotivoSuspension => lsMotivosSuspension.FirstOrDefault(valor => MotivoSuspension.IdTramiteTipoRechazo == 1) != null);      // SELECT * FROM cat_Tramite_RechazosTipos;

            var nodos = treeListRechazar.GetSelectedNodes();
            int IdMotivoRechazo = 0;
            string MotivosRechazos = "";

            if (lsMotivosSuspension.Count > 0)
            {
                if (nodos.Count > 0)
                {
                    if (txtObservacionesPublicasRechazara.Text.Length > 0)
                    {
                        MotivosRechazos = "";
                        foreach (TreeListNode node in nodos)
                        {
                            IdMotivoRechazo = Convert.ToInt32(node.GetValue("id"));
                            if (MotivosRechazos.Length > 0)
                                MotivosRechazos += ",";
                            MotivosRechazos += IdMotivoRechazo.ToString();
                        }

                        treeListRechazar.UnselectAll();
                        Rechazo(MotivosRechazos);
                    }
                    else
                    {
                        string script = "";
                        script = "Alerta('Observaciones Públicas', 'Favor de agregar las obseravaciones públicas');";
                        ScriptManager.RegisterStartupScript(this, GetType(), "ServerControlScript", script, true);
                    }
                }
                else
                {
                    string script = "";
                    script = "Alerta('Motivos de Rechazo','Favor de seleccionar los motivos del rechazo');";
                    ScriptManager.RegisterStartupScript(this, GetType(), "ServerControlScript", script, true);
                }
            }
            else
            {
                Rechazo(MotivosRechazos);
            }
        }

        protected void Rechazo(string MotivosRechazos)
        {
            if (txtObservacionesPublicasRechazara.Text.Length > 0)
            {
                Funciones.VariablesGlobales.StatusMesa IdStatusMesa = Funciones.VariablesGlobales.StatusMesa.Rechazo;
                int IdTramite = Int32.Parse(hfIdTramite.Value);
                int TipoTramite = int.Parse(hfTipoTramite.Value);

                List<Propiedades.Procesos.Operacion.TramiteProcesado> objResultado = Tramites.ProcesarTramite(IdTramite, int.Parse(hfIdMesa.Value), manejo_sesion.Usuarios.IdUsuario, IdStatusMesa, txtObservacionesPublicasRechazara.Text.Trim(), txtObservacionesPrivadas.Text, MotivosRechazos);
                if (!Page.ClientScript.IsStartupScriptRegistered(this.GetType(), "PopupScript"))
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "PopupScript", "LimpiarForm();", true);
                }

                //GeneraCartaEstatusTramite(IdTramite, "Rechazo", TipoTramite);

                if (objResultado[0].IdTramite > 0)
                {
                    // REGISTRO DE ARCHIVOS - COLOCAR DESPUES DE ACTUALIZAR EL TRAMITE
                    //if (Session["documentos"] != null) { string resultadoExpediente = registraDocumentosExpediente(IdTramite, TipoTramite); }
                    //if (Session["insumos"] != null) { string resultadoInsumo = registraDocumentosInsumos(IdTramite, TipoTramite); }

                    TituloModal.Text = "Operación realizada";
                    MensajeModal.Text = "Trámite rechazado";
                    TramiteTerminado();


                    string script = "";
                    script = "pnlPopMotivosRechazar.Hide(); $('#myModal').modal({backdrop: 'static', keyboard: false});";
                    ScriptManager.RegisterStartupScript(this, GetType(), "ServerControlScript", script, true);

                    //Response.Redirect("TramiteProcesar.aspx?IdMesa=" + hfIdMesa.Value.ToString(), true);
                }
            }
            else
            {
                string script = "";
                script = "Alerta('Observaciones Públicas','Por favor agregue observaciones públicas');";
                ScriptManager.RegisterStartupScript(this, GetType(), "ServerControlScript", script, true);
            }
        }

        protected void TramiteActualizado_Click(object sender, EventArgs e)
        {
            TramiteTerminado();
            //Response.Redirect("TramiteProcesar.aspx?IdMesa=" + hfIdMesa.Value.ToString(), true);
            //Page.Response.Redirect(Page.Request.Url.ToString(), true);

            //string parameters = "";
            //parameters = hfIdMesa.Value.ToString() + "|0";
            //parameters = Seguridad.Cifrado.Encriptar(parameters);
            //Response.Redirect("TramiteProcesar.aspx?objeto=" + parameters + "", true);

            string parameters = "";
            parameters = "IdMesa=" + hfIdMesa.Value.ToString() + "&IdTramite=0";
            Response.Redirect("TramiteProcesar.aspx?" + parameters, true);
        }

        protected void TramiteTerminado()
        {
            Session.Remove("documentos");
            Session.Remove("insumos");
            Session["documentos"] = null;
            Session["insumos"] = null;
        }

        private void SetNodeSelectionSettings(ref ASPxTreeList Motivos)
        {
            TreeListNodeIterator iterator = Motivos.CreateNodeIterator();
            TreeListNode node;
            while (true)
            {
                node = iterator.GetNext();
                if (node == null) break;
            }
        }

        protected void treeList_DataBoundSuspender(object sender, EventArgs e)
        {
            SetNodeSelectionSettings(ref treeListSuspender);
        }

        protected void treeList_CustomDataCallbackSuspender(object sender, DevExpress.Web.ASPxTreeList.TreeListCustomDataCallbackEventArgs e)
        {
            // e.Result = treeListSuspender.SelectionCount.ToString();
        }

        protected void pnlCallbackMotSuspender_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
        }

        protected void treeList_DataBoundRechazar(object sender, EventArgs e)
        {
            SetNodeSelectionSettings(ref treeListSuspender);
        }
        protected void treeList_CustomDataCallbackRechazar(object sender, DevExpress.Web.ASPxTreeList.TreeListCustomDataCallbackEventArgs e)
        {
            // e.Result = treeListSuspender.SelectionCount.ToString();
        }
        protected void pnlCallbackMotRechazar_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
        }
        protected void treeList_CustomDataCallbackRechazo(object sender, DevExpress.Web.ASPxTreeList.TreeListCustomDataCallbackEventArgs e)
        {
            // e.Result = treeListSuspender.SelectionCount.ToString();
        }
        protected void treeList_DataBoundRechazo(object sender, EventArgs e)
        {
            SetNodeSelectionSettings(ref treeListRechazar);
        }
    }
}