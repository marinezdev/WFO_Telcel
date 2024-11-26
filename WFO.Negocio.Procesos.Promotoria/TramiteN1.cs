﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using prop = WFO.Propiedades.Procesos.Promotoria;

namespace WFO.Negocio.Procesos.Promotoria
{
    public class TramiteN1
    {
        AccesoDatos.Procesos.Promotoria.NuevoTramite nuevoTramite = new AccesoDatos.Procesos.Promotoria.NuevoTramite();

        public List<prop.RespuestaNuevoTramiteN1> NuevoTramiteN1(prop.TramiteNuevo1 tramiteN1)
        {
            return nuevoTramite.NuevoTramiteN1(tramiteN1);
        }

        public List<prop.Proyectos> seleccionarPoryectos(int IdProveedor)
        {
            return nuevoTramite.seleccionarPoryectos(IdProveedor);
        }

    }
}
