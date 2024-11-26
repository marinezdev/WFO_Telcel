using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using prop = WFO.Propiedades;

namespace WFO.Negocio.Sistema
{
    public class Menu
    {
        AccesoDatos.Sistema.Menu menu = new AccesoDatos.Sistema.Menu();

        public void Seleccionar_GridView(ref GridView gridview)
        {
            WFO.Funciones.LlenarControles.LlenarGridView<prop.Menu>(ref gridview, menu.Seleccionar());
        }

        public void SeleccionarPertenencia(ref DropDownList dropdownlist)
        {
            Funciones.LlenarControles.LlenarDropDownList<prop.Menu>(ref dropdownlist, menu.SeleccionarPertenencia(), "Descripcion", "IdMenu");
        }

        public void LlenarTreeView(ref TreeView treeview, int idrol)
        {
            Funciones.LlenarControles.LlenarTreeViewMenu(menu.SeleccionarAsignadosPorRol(idrol), null, ref treeview);
        }

        
        public prop.Menu SeleccionarPorId(int id)
        {
            return menu.SeleccionarPorId(id);
        }

        public List<prop.PermisosMenuRol> SeleccionarAsignadosPorRol(int idrol)
        {
            return menu.SeleccionarAsignadosPorRol(idrol);
        }

        private string getSubMenuHTML(DataTable dtMenu, int ClaveMenu)
        {
            // Procedimiento Optimizado 210322
            string resultado = "";

            // Obtenemos los Menús Principales
            DataTable MenuPrincipal = dtMenu.AsEnumerable()
                             .Where(r => r.Field<int>("PerteneceA") == ClaveMenu)
                             .CopyToDataTable();

            foreach (DataRow drMenu in MenuPrincipal.Rows)
            {
                int IdMenu = int.Parse(drMenu["IdMenu"].ToString());
                resultado += "<li><a href='" + drMenu["URL"].ToString() + "'><i class='fa " + drMenu["Icono"].ToString() + "'></i> " + drMenu["Descripcion"].ToString() + "</a>";

                try
                {
                    // Obtenemos el SubMenú
                    DataTable MenuSecundario = dtMenu.AsEnumerable()
                                 .Where(r => r.Field<int>("PerteneceA") == IdMenu)
                                 .CopyToDataTable();

                    if (MenuSecundario.Rows.Count > 0)
                    {
                        resultado += "<ul class='nav child_menu'>";
                        resultado += getSubMenuHTML(dtMenu, IdMenu);
                        resultado += "</ul>";
                    }
                }
                catch (Exception)
                {
                    // Ignoramos errores
                }
            }


            return resultado;
        }

        /// <summary>
        /// Menú que se construye dinámicamente por base de datos (4 nivels)
        /// </summary>
        /// <param name="dsMenu">DataSet con la estructura del menú a generar</param>
        /// <returns>cadena con el menú construído</returns>
        public string CrearMenuHTML(DataTable dtMenu)
        {
            // Procedimiento Optimizado 210322
            List<prop.Menu> mp = new List<prop.Menu>();
            //mp = menu.MenuDinamicoObtener(idrol);   no se utiliza ya...

            string menuConstruido = "<div id='sidebar-menu' class='main_menu_side hidden-print main_menu'><div class='menu_section'>" +
                     "<h3>Menú de navegación </h3>" +
                     "<ul class='nav side-menu'>";

            try
            {
                // Obtenemos los Menús Principales
                DataTable MenuPrincipal = dtMenu.AsEnumerable()
                                 .Where(r => r.Field<int>("PerteneceA") == 0)
                                 .CopyToDataTable();

                foreach (DataRow drMenu in MenuPrincipal.Rows)
                {
                    int IdMenu = int.Parse(drMenu["IdMenu"].ToString());
                    menuConstruido += "<li><a href='" + drMenu["URL"].ToString() + "'><i class='fa " + drMenu["Icono"].ToString() + "'></i> " + drMenu["Descripcion"].ToString() + "</a>";

                    // Obtenemos el SubMenú
                    DataTable MenuSecundario = dtMenu.AsEnumerable()
                                 .Where(r => r.Field<int>("PerteneceA") == IdMenu)
                                 .CopyToDataTable();

                    if (MenuSecundario.Rows.Count > 0)
                    {
                        menuConstruido += "<ul class='nav child_menu'>";
                        menuConstruido += getSubMenuHTML(dtMenu, IdMenu);
                        menuConstruido += "</ul>";
                    }
                }
            }
            catch (Exception)
            {
                // Ignoramos erroes
            }

            //foreach (var padre in mp)
            //{
            //    if (int.Parse(padre.PerteneceA.ToString()) == 0)
            //    {
            //        List<prop.Menu> Nivel1 = new List<prop.Menu>();
            //        if (padre.IdMenu == menu.SeleccionarHijos(idrol.ToString()))
            //            menuConstruido += "<li><a href='" + padre.URL + "'><i class='fa " + padre.icono + "'></i> " + padre.Descripcion + "<span class='fa fa-chevron-down'></span></a>";
            //        else
            //            menuConstruido += "<li><a href='" + padre.URL + "'><i class='fa " + padre.icono + "'></i> " + padre.Descripcion + "</a>";

            //        foreach (var hijo in mp)
            //        {
            //            if (int.Parse(hijo.PerteneceA.ToString()) == padre.IdMenu)
            //            {
            //                Nivel1.Add(hijo);
            //            }
            //        }

            //        if(Nivel1.Count > 0)
            //        {
            //            menuConstruido += "<ul class='nav child_menu'>";
            //            foreach (var Hijos in Nivel1)
            //            {
            //                menuConstruido += "<li><a href='" + Hijos.URL + "'>" + Hijos.Descripcion + "</a>";
            //                List<prop.Menu> Nivel2 = new List<prop.Menu>();
            //                foreach (var padre2 in mp)
            //                {
            //                    if (int.Parse(padre2.PerteneceA.ToString()) == Hijos.IdMenu)
            //                    {
            //                        Nivel2.Add(padre2);
            //                    }
            //                }

            //                if (Nivel2.Count > 0)
            //                {
            //                    menuConstruido += "<ul class='nav child_menu'>";
            //                    foreach (var Hijos2 in Nivel2)
            //                    {
            //                        menuConstruido += "<li><a href='" + Hijos2.URL + "'>" + Hijos2.Descripcion + "<span class='fa fa-chevron-down'></span></a>";
            //                    }
            //                    menuConstruido += "</ul>";
            //                }
            //            }
            //            menuConstruido += "</ul>";
            //        }
            //        menuConstruido += "</li>";
            //    }
            //}

            menuConstruido += "</ul>" +
                "</div>" +
            "</div>";

            return menuConstruido;
        }

        public int Agregar(string descripcion, string url, int pertenecea)
        {
            return menu.Agregar(descripcion, url, pertenecea);
        }

        public int Modificar(string descripcion, string url, int pertenecea, int idmenu)
        {
            return menu.Modificar(descripcion, url, pertenecea, idmenu);
        }


    }
}
