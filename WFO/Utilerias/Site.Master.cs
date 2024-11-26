using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WFO
{
    public partial class SiteMaster : MasterPage
    {
        WFO.IU.ManejadorSesion manejo_sesion = new WFO.IU.ManejadorSesion();

        protected void Page_Load(object sender, EventArgs e)
        {
           if (Session["Sesion"] == null)
                Response.Redirect("..\\.");

            manejo_sesion = (WFO.IU.ManejadorSesion)Session["Sesion"];
            LblNombreUsuario.Text = manejo_sesion.Usuarios.Nombre;
            LblTextNombreUsuario.Text = manejo_sesion.Usuarios.Nombre;

            // TODO: Realizar funcionalidad para realizar el indicado de cambio de contraseña. para esta funcionalidad realizar la utilización de notificaciones PUSH
        }

        protected void BtnSalirSistema_Click(object sender, EventArgs e)
        {
            //////WFO.Negocio.Sistema.Usuarios sisusr = new WFO.Negocio.Sistema.Usuarios();
            //////sisusr.RegistroLog(Session["IdSesion"].ToString(), Session["idusuario"].ToString(), Session["Inicio"].ToString(), 0);
            //////sisusr.ActualizarDesconectarSesion(manejo_sesion.Usuarios.IdUsuario, 0);

            // VALIDA LA VARIABLE DE SESION PARA REGISTRAR SU SALIDA
            if (manejo_sesion != null)
            {
                Response.Redirect("/Defautlt.aspx");
            }
            else
            {
                Response.Redirect("/Defautlt.aspx");
            }

        }

        protected void BtnSalirAplicacion_Click(object sender, EventArgs e)
        {
            WFO.Negocio.Sistema.Usuarios sisusr = new WFO.Negocio.Sistema.Usuarios();
            sisusr.RegistroLog(Session["IdSesion"].ToString(), Session["idusuario"].ToString(), Session["Inicio"].ToString(), 0);
            sisusr.ActualizarDesconectarSesion(manejo_sesion.Usuarios.IdUsuario, 0);

            Session.RemoveAll();
            //Eliminar una sola
            Session.Remove("Sesion");
            Session["Sesion"] = null;
            Session.Remove("idusuario");
            Session["idusuario"] = null;
            Session.Remove("IdSesion");
            Session["IdSesion"] = null;
            Session.Remove("Inicio");
            Session["Inicio"] = null;
            //Eliminar todas las variables
            Session.Contents.RemoveAll();
            Session.Clear();
            System.Web.Security.FormsAuthentication.SignOut();

            // VALIDA LA VARIABLE DE SESION PARA REGISTRAR SU SALIDA
            if (manejo_sesion != null)
            {
                Response.Redirect("/Defautlt.aspx");
            }
            else
            {
                Response.Redirect("/Defautlt.aspx");
            }
        }
    }
}