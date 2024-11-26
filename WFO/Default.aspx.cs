using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using prop = WFO.Propiedades;

namespace WFO
{
    public partial class Default : WFO.Utilerias.Comun
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session.RemoveAll();
            Session.Remove("Sesion");
            Session["Sesion"] = null;
            Session.Remove("idusuario");
            Session["idusuario"] = null;
            Session.Remove("IdSesion");
            Session["IdSesion"] = null;
            Session.Remove("Inicio");
            Session["Inicio"] = null;
            Session.Contents.RemoveAll();

#if DEBUG
            txUsuario.Text = "ruben.marinez";
            txClave.Text = "ASAE2021";
#else
#endif
        }

        protected string getIP()
        {
            string strIP = "";

            try
            {
                strIP = Dns.GetHostEntry(Dns.GetHostName()).AddressList[2].ToString();
            }
            catch (Exception ex)
            {
                strIP = "<< no asignada >>";
            }

            return strIP;
        }


        protected void LoginButton_Click(object sender, EventArgs e)
        {
            DataSet UsuarioAcceso = null;
            string localIP = getIP();
            lnkRecuperarClave.Visible = false;

            UsuarioAcceso = sisUsrs.SistemaAcceso(txUsuario.Text, txClave.Text);

            if (UsuarioAcceso.Tables[0].Rows[0]["Acceso"].ToString() == "1")
            {
                manejo_sesion.Inicializar();
                Propiedades.Configuracion conf = new Propiedades.Configuracion();

                // Obtenemos la informacion del usuario
                prop.Usuarios item = new prop.Usuarios()
                {
                    IdUsuario = int.Parse(UsuarioAcceso.Tables[1].Rows[0]["IdUsuario"].ToString()),
                    Clave = UsuarioAcceso.Tables[1].Rows[0]["Clave"].ToString(),
                    FechaRegistro = UsuarioAcceso.Tables[1].Rows[0]["FechaRegistro"].ToString(),
                    Nombre = UsuarioAcceso.Tables[1].Rows[0]["Nombre"].ToString(),
                    RolNombre = UsuarioAcceso.Tables[1].Rows[0]["Rol Nombre"].ToString(),
                    Activo = bool.Parse(UsuarioAcceso.Tables[1].Rows[0]["Activo"].ToString())
                };

                manejo_sesion.Usuarios = item;                          // sisUsrs.SeleccionarDetalle(txUsuario.Text, txClave.Text);
                manejo_sesion.DiasAvisoCambioContraseña = 5;            // sisConfig.SeleccionarPorId(3).Valor;         // TODO: ### Pendiente:  esta información se debe de encontrar en alguna configuración 
                manejo_sesion.Menu = sisMenu.CrearMenuHTML(UsuarioAcceso.Tables[2]);

                // ACCESO DIRECTO
                Session["Sesion"] = manejo_sesion;
                Session["idusuario"] = manejo_sesion.Usuarios.IdUsuario;
                Session["IdSesion"] = HttpContext.Current.Session.SessionID;
                Session["Inicio"] = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

                // sisUsrs.RegistroLog(Session["IdSesion"].ToString(), Session["idusuario"].ToString(), Session["Inicio"].ToString(), 1);  // TODO: Revisar si se requiere dicha funcionalidad...
                Response.Redirect(UsuarioAcceso.Tables[1].Rows[0]["Página Inicial"].ToString(), false);
            }
            else
            {
                log.Agregar(txUsuario.Text.Trim() + " ha intentado ingresar al sistema, ha equivocado su clave o intenta accesar sin autorización. IP: " + localIP);
                LblMensajes.Text = "No se pudo accesar al sistema. Favor de verificar nombre de usuario y/o contraseña.";
                lnkRecuperarClave.Visible = true;
            }
        }
    }
}