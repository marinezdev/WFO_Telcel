using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using prop = WFO.Propiedades.Procesos.Operacion;

namespace WFO.Procesos.Operador
{
    public partial class Default : Utilerias.Comun
    {
        WFO.Negocio.Procesos.Operacion.Mesas mesas = new Negocio.Procesos.Operacion.Mesas();
        WFO.Negocio.Procesos.Operacion.UsuariosFlujo usuariosFlujo = new Negocio.Procesos.Operacion.UsuariosFlujo();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    manejo_sesion = (WFO.IU.ManejadorSesion)Session["Sesion"];
                    Session["TramitesAutomaticos"] = true;

                    if (!String.IsNullOrEmpty(Request.QueryString["msj"]))
                    {
                        if (Request.QueryString["msj"].ToString() == "1")
                        {
                            mensajes.MostrarMensaje(this, "No hay trámites disponibles...");
                        }
                    }

                    CargaFlujos(manejo_sesion.Usuarios.IdUsuario);
                }
            }
            catch (Exception ex)
            {
                log.Agregar(ex.Message + " // " + ex.Source );
            }
        }

        protected void CargaFlujos(int Id)
        {
            List<prop.UsuariosFlujo> Flujos = usuariosFlujo.SelecionarFlujo(Id);
            cbFlujos.DataSource = Flujos;
            cbFlujos.DataBind();
            cbFlujos.DataTextField = "Nombre";
            cbFlujos.DataValueField = "Id";
            cbFlujos.DataBind();
        }

        protected void CargaFlujos_SelectedIndexChanged(object sender, EventArgs e)
        {
            int IdFlujo = Convert.ToInt32(cbFlujos.SelectedValue.ToString());
            manejo_sesion = (WFO.IU.ManejadorSesion)Session["Sesion"];
            PintaMesas(manejo_sesion.Usuarios.IdUsuario, IdFlujo);
        }

        protected void PintaMesas(int Id, int IdFlujo)
        {
            List<prop.Mesa> MesasUsaurio = mesas.SelecionarMesas(Id, IdFlujo);
            string parameters = "";

            string MesaUsuario = "";
            for (int i=0; i< MesasUsaurio.Count;i++)
            {
                //parameters = MesasUsaurio[i].Id + ",0" ;
                //parameters = Seguridad.Cifrado.Encriptar(parameters);

                parameters = "IdMesa=" + MesasUsaurio[i].Id.ToString() + "&IdTramite=0";

                MesaUsuario += "<div class='control-label col-md-4 col-sm-4 col-xs-6'>" +
                            "<div class='x_panel text-center'>" +
                                "<a href='TramiteProcesar.aspx?" + parameters + "'>" +
                                    "<img src='" + MesasUsaurio[i].icono + "'/>" +
                                    "<div class='form-group text-center'>" +
                                        "<hr />" +
                                        "<h2><small style='color:" + MesasUsaurio[i].Color + ";'>"+ MesasUsaurio[i].nombre +"</small></h2>" +
                                    "</div>" +
                                "</a>" +
                            "</div>" +
                         "</div>";
            }

            MesasLiteral.Text = MesaUsuario;
        }
    }
}