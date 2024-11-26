﻿using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using static iTextSharp.text.Font;
using prop = WFO.Propiedades.Procesos.Promotoria;

namespace WFO.Negocio.Procesos.Promotoria
{
    public class Cartas
    {
        AccesoDatos.Procesos.Promotoria.Carta carta = new AccesoDatos.Procesos.Promotoria.Carta();

        public List<prop.carta> Consulta_Carta(int Id, string Nombre, int Rechazo)
        {
            return carta.Consulta_Carta(Id, Nombre, Rechazo);
        }

        public List<prop.carta> Consulta_Carta_PCI(int Id)
        {
            return carta.Consulta_Carta_PCI(Id);
        }

        public List<prop.motivosRechazo> Consulta_MotivosRechazo(int Id)
        {
            return carta.Consulta_Motivos_Rechazo(Id);
        }

        public List<prop.bitacora> Consulta_Observaciones_Bitacora(int Id)
        {
            return carta.Consulta_Observaciones_Bitacora(Id);
        }

        public void CartaSuspendidoPDF(int IdTramite, HttpResponse Response, int CreaPDF, int IdUsuario)
        {
            List<prop.carta> cartas = carta.Consulta_Carta(IdTramite, "Suspendido", 1);

            if (cartas.Count > 0)
            {
                foreach (prop.carta DatosCarta in cartas)
                {
                    MuestraSuspendidoPDF(DatosCarta, Response, 1, IdUsuario);
                }
            }
            else
            {
                NoExistente(Response);
            }
        }

        private void MuestraSuspendidoPDF(prop.carta CartaDatos, HttpResponse Response, int CreaPDF, int Credencial)
        {
            CultureInfo culture = new CultureInfo("es-MX");
            // Medidas de la hoja A4
            var document = new Document(PageSize.A4, 30, 30, 50, 50);
            document.SetMargins(30, 30, 80, 50);
            var output = (dynamic)null;

            // VALIDAR SI ES CREADO O ALMACENADO
            if (CreaPDF == 1)
            {
                //string directorioTemporal = Server.MapPath("~") + "\\Cartas\\";
                string directorioTemporal = HttpContext.Current.Server.MapPath("~") + "\\Cartas\\";
                output = new FileStream(directorioTemporal + "CartaSuspendido_" + Credencial + "_" + CartaDatos.FolioCompuesto + ".pdf", FileMode.Create);

            }
            else
            {
                output = new MemoryStream();
            }
            var writer = PdfWriter.GetInstance(document, output);

            PdfPCell cell = null;

            // FORMATOS DE TEXTOS 
            Font Text = new Font(FontFamily.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK);
            Font TextFooter = new Font(FontFamily.HELVETICA, 7, Font.NORMAL, BaseColor.BLACK);
            Font Negrita = new Font(FontFamily.HELVETICA, 10, Font.BOLD, BaseColor.BLACK);

            // COLOCA ENCABEZADO Y PIE DE PAGINA
            writer.PageEvent = new PDFFooter();

            // RECHAZOS 
            List<prop.motivosRechazo> motivos = carta.Consulta_Motivos_Rechazo(CartaDatos.Id);
            // OBSERVACIONES 
            List<prop.bitacora> ObservacionesBitacora = carta.Consulta_Observaciones_Bitacora(CartaDatos.Id);


            // DEFINICACION DE VARIABLES UTILIZADAS EN PLANTILLA
            string Fecha = "Ciudad de México, " + CartaDatos.FechaTermino.ToString(culture.DateTimeFormat.LongDatePattern, culture);
            string TipoTramite = CartaDatos.TipoTramite;
            string Ramo = CartaDatos.Operacion;
            string Folio = CartaDatos.FolioCompuesto;
            string Titular = "-";

            if (CartaDatos.Titular.Length > 5)
            {
                Titular = CartaDatos.Titular;
            }
            else
            {
                Titular = CartaDatos.Contratante;
            }

            string Poliza = CartaDatos.IdSisLegados;
            string InicioVigencia = CartaDatos.FechaTermino.ToString(culture.DateTimeFormat.LongDatePattern, culture);
            string Productos = CartaDatos.Producto;
            string Agente = CartaDatos.Agente;
            string Promotoria = CartaDatos.Promotoria;

            document.Open();

            PdfPTable table = new PdfPTable(10);
            table.WidthPercentage = 100;
            table.SpacingBefore = 20;

            // ESPACIO EN BLANCO
            cell = new PdfPCell(new Phrase("", Text));
            cell.Colspan = 10;
            cell.FixedHeight = 15f;
            cell.MinimumHeight = 20F;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = 0;
            table.AddCell(cell);
            cell = null;

            // FECHA 
            cell = new PdfPCell(new Phrase(Fecha, Text));
            cell.Colspan = 10;
            cell.FixedHeight = 50f;
            cell.MinimumHeight = 30f;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell.Border = 0;
            table.AddCell(cell);
            cell = null;

            // TIPO TRAMITE
            cell = new PdfPCell(new Phrase("Tipo de Trámite: ", Negrita));
            cell.Colspan = 2;
            cell.FixedHeight = 15f;
            cell.MinimumHeight = 15f;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = 0;
            table.AddCell(cell);
            cell = null;

            cell = new PdfPCell(new Phrase(TipoTramite, Text));
            cell.Colspan = 8;
            cell.FixedHeight = 15f;
            cell.MinimumHeight = 15f;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = 0;
            table.AddCell(cell);
            cell = null;

            // RAMO
            cell = new PdfPCell(new Phrase("Ramo: ", Negrita));
            cell.Colspan = 2;
            cell.FixedHeight = 15f;
            cell.MinimumHeight = 15f;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = 0;
            table.AddCell(cell);
            cell = null;

            cell = new PdfPCell(new Phrase(Ramo, Text));
            cell.Colspan = 8;
            cell.FixedHeight = 15f;
            cell.MinimumHeight = 15f;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = 0;
            table.AddCell(cell);
            cell = null;

            // FOLIO TRAMITE 
            cell = new PdfPCell(new Phrase("Póliza Nueva", Negrita));
            cell.Colspan = 2;
            cell.FixedHeight = 15f;
            cell.MinimumHeight = 15f;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = 0;
            table.AddCell(cell);
            cell = null;
            /*
            cell = new PdfPCell(new Phrase(Folio, Negrita));
            cell.Colspan = 10;
            cell.FixedHeight = 15f;
            cell.MinimumHeight = 15f;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = 0;
            table.AddCell(cell);
            cell = null;
            */
            // ESPACIO EN BLANCO
            cell = new PdfPCell(new Phrase("", Text));
            cell.Colspan = 10;
            cell.FixedHeight = 15f;
            cell.MinimumHeight = 40F;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = 0;
            table.AddCell(cell);
            cell = null;

            // NOMBRE DEL TITULAR
            cell = new PdfPCell(new Phrase("Estimado (a):  " + Titular, Text));
            cell.Colspan = 10;
            cell.FixedHeight = 15f;
            cell.MinimumHeight = 35f;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = 0;
            table.AddCell(cell);
            cell = null;

            cell = new PdfPCell(new Phrase("En respuesta a tu solicitud de contratación de tu seguro te informamos que para poder continuar con el análisis requerimos que nos compartas la siguiente información: ", Text));
            cell.Colspan = 10;
            cell.FixedHeight = 30f;
            cell.MinimumHeight = 30f;
            cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            cell.Border = 0;
            table.AddCell(cell);
            cell = null;

            // EN LISTA LOS MOTIVOS DE RECHAZO
            if (motivos.Count > 0)
            {
                foreach (prop.motivosRechazo motivosRechazo in motivos)
                {
                    cell = new PdfPCell(new Phrase("   *   " + motivosRechazo.MotivoRechazo, Text));
                    cell.Colspan = 10;
                    cell.FixedHeight = 20f;
                    cell.MinimumHeight = 20f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 0;
                    table.AddCell(cell);
                    cell = null;
                }
            }
            else
            {

                cell = new PdfPCell(new Phrase("   *   NO SE ENCONTRARON MOTIVOS ", Text));
                cell.Colspan = 10;
                cell.FixedHeight = 20f;
                cell.MinimumHeight = 20f;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = 0;
                table.AddCell(cell);
                cell = null;
            }

            // ESPACIO EN BLANCO
            cell = new PdfPCell(new Phrase("", Text));
            cell.Colspan = 10;
            cell.FixedHeight = 15f;
            cell.MinimumHeight = 20F;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = 0;
            table.AddCell(cell);
            cell = null;

            // OBSERVACIONES
            cell = new PdfPCell(new Phrase("OBSERVACIONES.", Negrita));
            cell.Colspan = 10;
            cell.FixedHeight = 15f;
            cell.MinimumHeight = 15f;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = 0;
            table.AddCell(cell);
            cell = null;

            // EN LISTA LAS OBSERVACIONES DE BITACORA
            if (ObservacionesBitacora.Count > 0)
            {
                foreach (prop.bitacora observaciones in ObservacionesBitacora)
                {

                    // MOTIVOS DE HOLD OBSERVACIONES 
                    cell = new PdfPCell(new Phrase("   >   " + observaciones.Observacion, Text));
                    cell.Colspan = 10;
                    cell.FixedHeight = 20f;
                    cell.MinimumHeight = 20f;
                    cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                    cell.Border = 0;
                    table.AddCell(cell);
                    cell = null;
                }
            }
            else
            {
                // MOTIVOS DE HOLD OBSERVACIONES 
                cell = new PdfPCell(new Phrase(" > ", Text));
                cell.Colspan = 10;
                cell.FixedHeight = 20f;
                cell.MinimumHeight = 20f;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = 0;
                table.AddCell(cell);
                cell = null;
            }

            // ESPACIO EN BLANCO
            cell = new PdfPCell(new Phrase("", Text));
            cell.Colspan = 10;
            cell.FixedHeight = 15f;
            cell.MinimumHeight = 20F;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = 0;
            table.AddCell(cell);
            cell = null;

            cell = new PdfPCell(new Phrase("Te pedimos hacernos llegar la información y/o respuesta a través de tu agente de seguros, en un máximo de 15 días hábiles, para evitar la cancelación de este trámite.", Text));
            cell.Colspan = 10;
            cell.FixedHeight = 30f;
            cell.MinimumHeight = 30f;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = 0;
            table.AddCell(cell);
            cell = null;

            // ESPACIO EN BLANCO
            cell = new PdfPCell(new Phrase("", Text));
            cell.Colspan = 10;
            cell.FixedHeight = 15f;
            cell.MinimumHeight = 20F;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = 0;
            table.AddCell(cell);
            cell = null;

            cell = new PdfPCell(new Phrase("Sin más por el momento, nos reiteramos a tus órdenes.", Text));
            cell.Colspan = 10;
            cell.FixedHeight = 15f;
            cell.MinimumHeight = 15f;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = 0;
            table.AddCell(cell);
            cell = null;

            // ESPACIO EN BLANCO
            cell = new PdfPCell(new Phrase("", Text));
            cell.Colspan = 10;
            cell.FixedHeight = 15f;
            cell.MinimumHeight = 20F;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = 0;
            table.AddCell(cell);
            cell = null;

            cell = new PdfPCell(new Phrase("Atentamente.", Text));
            cell.Colspan = 10;
            cell.FixedHeight = 15f;
            cell.MinimumHeight = 15f;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = 0;
            table.AddCell(cell);
            cell = null;

            cell = new PdfPCell(new Phrase("METLIFE, MÉXICO S.A.", Text));
            cell.Colspan = 10;
            cell.FixedHeight = 15f;
            cell.MinimumHeight = 15f;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = 0;
            table.AddCell(cell);
            cell = null;

            string directorioImagen = HttpContext.Current.Server.MapPath("~") + "\\Imagenes\\";

            iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(directorioImagen + "firma.png");
            PdfPCell imageCell = new PdfPCell(jpg);
            imageCell.Colspan = 10; // either 1 if you need to insert one cell
            imageCell.Border = 0;
            imageCell.MinimumHeight = 125f;
            imageCell.FixedHeight = 125f;
            imageCell.HorizontalAlignment = Element.ALIGN_LEFT;
            table.AddCell(imageCell);
            imageCell = null;

            // ESPACIO EN BLANCO
            cell = new PdfPCell(new Phrase("", Text));
            cell.Colspan = 10;
            cell.FixedHeight = 15f;
            cell.MinimumHeight = 35F;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = 0;
            table.AddCell(cell);
            cell = null;

            cell = new PdfPCell(new Phrase("Agente: ", Text));
            cell.Colspan = 2;
            cell.FixedHeight = 15f;
            cell.MinimumHeight = 15f;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = 0;
            table.AddCell(cell);
            cell = null;

            // AGENTE 
            cell = new PdfPCell(new Phrase(Agente, Negrita));
            cell.Colspan = 8;
            cell.FixedHeight = 15f;
            cell.MinimumHeight = 15f;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = 0;
            table.AddCell(cell);
            cell = null;

            cell = new PdfPCell(new Phrase("Promotoría: ", Text));
            cell.Colspan = 2;
            cell.FixedHeight = 15f;
            cell.MinimumHeight = 15f;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = 0;
            table.AddCell(cell);
            cell = null;

            // PROMOTORIA
            cell = new PdfPCell(new Phrase(Promotoria, Negrita));
            cell.Colspan = 8;
            cell.FixedHeight = 15f;
            cell.MinimumHeight = 15f;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = 0;
            table.AddCell(cell);
            cell = null;



            document.Add(table);
            document.Close();

            //Response.ContentType = "application/pdf";
            //Response.AddHeader("Content-Disposition", "");
            //Response.BinaryWrite(output.ToArray());

        }
        
        public void CartaRechazoPDF(int IdTramite, HttpResponse Response, int CreaPDF, int IdUsuario)
        {
            List<prop.carta> cartas = carta.Consulta_Carta(IdTramite, "Rechazo", 0);

            if (cartas.Count > 0)
            {
                foreach (prop.carta DatosCarta in cartas)
                {
                    MuestraRechazoPDF(DatosCarta, Response, 1, IdUsuario);
                }
            }
            else
            {
                NoExistente(Response);
            }
        }

        private void MuestraRechazoPDF(prop.carta CartaDatos, HttpResponse Response, int CreaPDF, int Credencial)
        {
            CultureInfo culture = new CultureInfo("es-MX");
            // Medidas de la hoja A4
            var document = new Document(PageSize.A4, 30, 30, 50, 50);
            document.SetMargins(30, 30, 80, 50);
            var output = (dynamic)null;

            // VALIDAR SI ES CREADO O ALMACENADO
            if (CreaPDF == 1)
            {
                //string directorioTemporal = Server.MapPath("~") + "\\Cartas\\";
                string directorioTemporal = HttpContext.Current.Server.MapPath("~") + "\\Cartas\\";
                output = new FileStream(directorioTemporal + "CartaRechazo_" + Credencial + "_" + CartaDatos.FolioCompuesto + ".pdf", FileMode.Create);
            }
            else
            {
                output = new MemoryStream();
            }

            var writer = PdfWriter.GetInstance(document, output);

            PdfPCell cell = null;

            // FORMATOS DE TEXTOS 
            Font Text = new Font(FontFamily.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK);
            Font TextFooter = new Font(FontFamily.HELVETICA, 7, Font.NORMAL, BaseColor.BLACK);
            Font Negrita = new Font(FontFamily.HELVETICA, 10, Font.BOLD, BaseColor.BLACK);

            // COLOCA ENCABEZADO Y PIE DE PAGINA
            writer.PageEvent = new PDFFooter();

            // RECHAZOS 
            List<prop.motivosRechazo> motivos = new List<prop.motivosRechazo>();
            // OBSERVACIONES 
            List<prop.bitacora> ObservacionesBitacora = new List<prop.bitacora>();

            if (CartaDatos.TipoTramite == "EMISIÓN NC")
            {
                motivos = carta.Consulta_Motivos_Rechazo(CartaDatos.Id);
                ObservacionesBitacora = carta.Consulta_Observaciones_Bitacora(CartaDatos.Id);
            }
            
            // DEFINICACION DE VARIABLES UTILIZADAS EN PLANTILLA
            string Fecha = "Ciudad de México, " + CartaDatos.FechaTermino.ToString(culture.DateTimeFormat.LongDatePattern, culture);
            string TipoTramite = CartaDatos.TipoTramite;
            string Ramo = CartaDatos.Operacion;
            string Folio = CartaDatos.FolioCompuesto;
            string Titular = "-";
            string FechaRegistro = CartaDatos.FechaTermino.ToString();

            if (CartaDatos.Titular.Length > 5)
            {
                Titular = CartaDatos.Titular;
            }
            else
            {
                Titular = CartaDatos.Contratante;
            }

            string Poliza = CartaDatos.IdSisLegados;
            string InicioVigencia = CartaDatos.FechaTermino.ToString(culture.DateTimeFormat.LongDatePattern, culture);

            string Productos = CartaDatos.Producto;
            string Agente = CartaDatos.Agente;
            string Promotoria = CartaDatos.Promotoria;

            document.Open();

            PdfPTable table = new PdfPTable(10);
            table.WidthPercentage = 100;
            table.SpacingBefore = 20;

            // ESPACIO EN BLANCO
            cell = new PdfPCell(new Phrase("", Text));
            cell.Colspan = 10;
            cell.FixedHeight = 15f;
            cell.MinimumHeight = 20F;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = 0;
            table.AddCell(cell);
            cell = null;

            // FECHA 
            cell = new PdfPCell(new Phrase(Fecha, Text));
            cell.Colspan = 10;
            cell.FixedHeight = 50f;
            cell.MinimumHeight = 30f;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell.Border = 0;
            table.AddCell(cell);
            cell = null;

            // TIPO TRAMITE
            cell = new PdfPCell(new Phrase("Tipo de Trámite: ", Text));
            cell.Colspan = 2;
            cell.FixedHeight = 15f;
            cell.MinimumHeight = 15f;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = 0;
            table.AddCell(cell);
            cell = null;

            cell = new PdfPCell(new Phrase(TipoTramite, Negrita));
            cell.Colspan = 8;
            cell.FixedHeight = 15f;
            cell.MinimumHeight = 15f;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = 0;
            table.AddCell(cell);
            cell = null;

            // RAMO
            cell = new PdfPCell(new Phrase("Ramo: ", Text));
            cell.Colspan = 2;
            cell.FixedHeight = 15f;
            cell.MinimumHeight = 15f;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = 0;
            table.AddCell(cell);
            cell = null;

            cell = new PdfPCell(new Phrase(Ramo, Negrita));
            cell.Colspan = 8;
            cell.FixedHeight = 15f;
            cell.MinimumHeight = 15f;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = 0;
            table.AddCell(cell);
            cell = null;

            // FOLIO TRAMITE 
            cell = new PdfPCell(new Phrase("Ref. Solicitud No.: ", Text));
            cell.Colspan = 2;
            cell.FixedHeight = 15f;
            cell.MinimumHeight = 15f;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = 0;
            table.AddCell(cell);
            cell = null;

            cell = new PdfPCell(new Phrase(Folio, Negrita));
            cell.Colspan = 10;
            cell.FixedHeight = 15f;
            cell.MinimumHeight = 15f;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = 0;
            table.AddCell(cell);
            cell = null;

            // NOMBRE DEL TITULAR
            cell = new PdfPCell(new Phrase("Estimado(a): " + Titular, Text));
            cell.Colspan = 10;
            cell.FixedHeight = 15f;
            cell.MinimumHeight = 30f;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = 0;
            table.AddCell(cell);
            cell = null;

            cell = new PdfPCell(new Phrase("En relación a la solicitud de contratación del seguro de " + Productos + " que nos hiciste llegar el día " + FechaRegistro.Substring(0, 10) + " lamentamos informarte  que después de revisar con detenimiento tu caso, no será posible proceder con la contratación de este plan con MetLife.", Text));
            cell.Colspan = 10;
            cell.FixedHeight = 30f;
            cell.MinimumHeight = 30f;
            cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            cell.Border = 0;
            table.AddCell(cell);
            cell = null;


            // ESPACIO EN BLANCO
            cell = new PdfPCell(new Phrase("", Text));
            cell.Colspan = 10;
            cell.FixedHeight = 15f;
            cell.MinimumHeight = 20F;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = 0;
            table.AddCell(cell);
            cell = null;


            // EN LISTA LOS MOTIVOS DE RECHAZO
            if (motivos.Count > 0)
            {
                // OBSERVACIONES
                cell = new PdfPCell(new Phrase("MOTIVOS DE RECHAZO.", Negrita));
                cell.Colspan = 10;
                cell.FixedHeight = 15f;
                cell.MinimumHeight = 15f;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = 0;
                table.AddCell(cell);
                cell = null;

                foreach (prop.motivosRechazo motivosRechazo in motivos)
                {
                    cell = new PdfPCell(new Phrase("   *   " + motivosRechazo.MotivoRechazo, Text));
                    cell.Colspan = 10;
                    cell.FixedHeight = 20f;
                    cell.MinimumHeight = 20f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 0;
                    table.AddCell(cell);
                    cell = null;
                }

                // ESPACIO EN BLANCO
                cell = new PdfPCell(new Phrase("", Text));
                cell.Colspan = 10;
                cell.FixedHeight = 15f;
                cell.MinimumHeight = 20F;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = 0;
                table.AddCell(cell);
                cell = null;
            }

            cell = new PdfPCell(new Phrase("Sin más por el momento, nos reiteramos a tus órdenes.", Text));
            cell.Colspan = 10;
            cell.FixedHeight = 15f;
            cell.MinimumHeight = 15f;
            cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            cell.Border = 0;
            table.AddCell(cell);
            cell = null;

            /*
            cell = new PdfPCell(new Phrase("De manera sincera agradecemos haber considero a nuestra empresa,  esperando poder servirte en un futuro.", Text));
            cell.Colspan = 10;
            cell.FixedHeight = 15f;
            cell.MinimumHeight = 15f;
            cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            cell.Border = 0;
            table.AddCell(cell);
            cell = null;
            */

            // EN LISTA LAS OBSERVACIONES DE BITACORA
            if (ObservacionesBitacora.Count > 0)
            {
                // ESPACIO EN BLANCO
                cell = new PdfPCell(new Phrase("", Text));
                cell.Colspan = 10;
                cell.FixedHeight = 15f;
                cell.MinimumHeight = 20F;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = 0;
                table.AddCell(cell);
                cell = null;

                // OBSERVACIONES
                cell = new PdfPCell(new Phrase("OBSERVACIONES.", Negrita));
                cell.Colspan = 10;
                cell.FixedHeight = 15f;
                cell.MinimumHeight = 15f;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = 0;
                table.AddCell(cell);
                cell = null;

                foreach (prop.bitacora observaciones in ObservacionesBitacora)
                {

                    // MOTIVOS DE HOLD OBSERVACIONES 
                    cell = new PdfPCell(new Phrase("   >   " + observaciones.Observacion, Text));
                    cell.Colspan = 10;
                    cell.FixedHeight = 20f;
                    cell.MinimumHeight = 20f;
                    cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                    cell.Border = 0;
                    table.AddCell(cell);
                    cell = null;
                }
            }

            // ESPACIO EN BLANCO
            cell = new PdfPCell(new Phrase("", Text));
            cell.Colspan = 10;
            cell.FixedHeight = 15f;
            cell.MinimumHeight = 20F;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = 0;
            table.AddCell(cell);
            cell = null;

            cell = new PdfPCell(new Phrase("Atentamente.", Text));
            cell.Colspan = 10;
            cell.FixedHeight = 15f;
            cell.MinimumHeight = 15f;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = 0;
            table.AddCell(cell);
            cell = null;
            /*
            cell = new PdfPCell(new Phrase("METLIFE, MÉXICO S.A.", Text));
            cell.Colspan = 10;
            cell.FixedHeight = 15f;
            cell.MinimumHeight = 15f;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = 0;
            table.AddCell(cell);
            cell = null;
            */

            string directorioImagen = HttpContext.Current.Server.MapPath("~") + "\\Imagenes\\";

            iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(directorioImagen + "firma.png");
            PdfPCell imageCell = new PdfPCell(jpg);
            imageCell.Colspan = 10; // either 1 if you need to insert one cell
            imageCell.Border = 0;
            imageCell.MinimumHeight = 125f;
            imageCell.FixedHeight = 125f;
            imageCell.HorizontalAlignment = Element.ALIGN_LEFT;
            table.AddCell(imageCell);
            imageCell = null;

            // ESPACIO EN BLANCO
            cell = new PdfPCell(new Phrase("", Text));
            cell.Colspan = 10;
            cell.FixedHeight = 15f;
            cell.MinimumHeight = 35F;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = 0;
            table.AddCell(cell);
            cell = null;

            cell = new PdfPCell(new Phrase("Agente: ", Text));
            cell.Colspan = 2;
            cell.FixedHeight = 15f;
            cell.MinimumHeight = 15f;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = 0;
            table.AddCell(cell);
            cell = null;

            // AGENTE 
            cell = new PdfPCell(new Phrase(Agente, Negrita));
            cell.Colspan = 8;
            cell.FixedHeight = 15f;
            cell.MinimumHeight = 15f;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = 0;
            table.AddCell(cell);
            cell = null;

            cell = new PdfPCell(new Phrase("Promotoría: ", Text));
            cell.Colspan = 2;
            cell.FixedHeight = 15f;
            cell.MinimumHeight = 15f;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = 0;
            table.AddCell(cell);
            cell = null;

            // PROMOTORIA
            cell = new PdfPCell(new Phrase(Promotoria, Negrita));
            cell.Colspan = 10;
            cell.FixedHeight = 15f;
            cell.MinimumHeight = 15f;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = 0;
            table.AddCell(cell);
            cell = null;



            document.Add(table);
            document.Close();

            //Response.ContentType = "application/pdf";
            //Response.AddHeader("Content-Disposition", "");
            //Response.BinaryWrite(output.ToArray());

        }
        
        public void CartaHoldPDF(int IdTramite, HttpResponse Response, int CreaPDF, int IdUsuario)
        {
            List<prop.carta> cartas = carta.Consulta_Carta(IdTramite, "Hold", 1);
            
            if (cartas.Count > 0)
            {
                foreach (prop.carta DatosCarta in cartas)
                {
                    MuestraHoldPDF(DatosCarta, Response, 1, IdUsuario);
                }
            }
            else
            {
                NoExistente(Response);
            }
        }
        
        private void MuestraHoldPDF(prop.carta CartaDatos, HttpResponse Response, int CreaPDF, int Credencial)
        {
            CultureInfo culture = new CultureInfo("es-MX");
            // Medidas de la hoja A4
            var document = new Document(PageSize.A4, 30, 30, 50, 50);
            document.SetMargins(30, 30, 80, 50);

            var output = (dynamic)null;

            // VALIDAR SI ES CREADO O ALMACENADO
            if (CreaPDF == 1)
            {
                //string directorioTemporal = Server.MapPath("~") + "\\Cartas\\";
                string directorioTemporal = HttpContext.Current.Server.MapPath("~") + "\\Cartas\\";
                output = new FileStream(directorioTemporal + "CartaHold_" + Credencial + "_" + CartaDatos.FolioCompuesto + ".pdf", FileMode.Create);

            }
            else
            {
                output = new MemoryStream();
            }

            var writer = PdfWriter.GetInstance(document, output);

            PdfPCell cell = null;

            // FORMATOS DE TEXTOS 
            Font Text = new Font(FontFamily.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK);
            Font TextFooter = new Font(FontFamily.HELVETICA, 7, Font.NORMAL, BaseColor.BLACK);
            Font Negrita = new Font(FontFamily.HELVETICA, 10, Font.BOLD, BaseColor.BLACK);

            // COLOCA ENCABEZADO Y PIE DE PAGINA
            writer.PageEvent = new PDFFooter();

            // RECHAZOS 
            List<prop.motivosRechazo> motivos = carta.Consulta_Motivos_Rechazo(CartaDatos.Id);
            // OBSERVACIONES 
            List<prop.bitacora> ObservacionesBitacora = carta.Consulta_Observaciones_Bitacora(CartaDatos.Id);


            // DEFINICACION DE VARIABLES UTILIZADAS EN PLANTILLA
            string Fecha = "Ciudad de México, " + CartaDatos.FechaTermino.ToString(culture.DateTimeFormat.LongDatePattern, culture);
            string TipoTramite = CartaDatos.TipoTramite;
            string Ramo = CartaDatos.Operacion;
            string Folio = CartaDatos.FolioCompuesto;
            string Titular = "-";

            if (CartaDatos.Titular.Length > 5)
            {
                Titular = CartaDatos.Titular;
            }
            else
            {
                Titular = CartaDatos.Contratante;
            }

            string Poliza = CartaDatos.IdSisLegados;
            string InicioVigencia = CartaDatos.FechaTermino.ToString(culture.DateTimeFormat.LongDatePattern, culture);
            string Productos = CartaDatos.Producto;
            string Agente = CartaDatos.Agente;
            string Promotoria = CartaDatos.Promotoria;

            document.Open();

            PdfPTable table = new PdfPTable(10);
            table.WidthPercentage = 100;
            table.SpacingBefore = 20;

            // ESPACIO EN BLANCO
            cell = new PdfPCell(new Phrase("", Text));
            cell.Colspan = 10;
            cell.FixedHeight = 15f;
            cell.MinimumHeight = 20F;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = 0;
            table.AddCell(cell);
            cell = null;

            // FECHA 
            cell = new PdfPCell(new Phrase(Fecha, Text));
            cell.Colspan = 10;
            cell.FixedHeight = 50f;
            cell.MinimumHeight = 30f;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell.Border = 0;
            table.AddCell(cell);
            cell = null;

            // TIPO TRAMITE
            cell = new PdfPCell(new Phrase("Tipo de Trámite: ", Negrita));
            cell.Colspan = 2;
            cell.FixedHeight = 15f;
            cell.MinimumHeight = 15f;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = 0;
            table.AddCell(cell);
            cell = null;

            cell = new PdfPCell(new Phrase(TipoTramite, Text));
            cell.Colspan = 8;
            cell.FixedHeight = 15f;
            cell.MinimumHeight = 15f;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = 0;
            table.AddCell(cell);
            cell = null;

            // RAMO
            cell = new PdfPCell(new Phrase("Ramo: ", Negrita));
            cell.Colspan = 2;
            cell.FixedHeight = 15f;
            cell.MinimumHeight = 15f;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = 0;
            table.AddCell(cell);
            cell = null;

            cell = new PdfPCell(new Phrase(Ramo, Text));
            cell.Colspan = 8;
            cell.FixedHeight = 15f;
            cell.MinimumHeight = 15f;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = 0;
            table.AddCell(cell);
            cell = null;

            // FOLIO TRAMITE 
            cell = new PdfPCell(new Phrase("Póliza Nueva", Negrita));
            cell.Colspan = 2;
            cell.FixedHeight = 15f;
            cell.MinimumHeight = 15f;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = 0;
            table.AddCell(cell);
            cell = null;
            /*
            cell = new PdfPCell(new Phrase(Folio, Negrita));
            cell.Colspan = 10;
            cell.FixedHeight = 15f;
            cell.MinimumHeight = 15f;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = 0;
            table.AddCell(cell);
            cell = null;
            */
            // ESPACIO EN BLANCO
            cell = new PdfPCell(new Phrase("", Text));
            cell.Colspan = 10;
            cell.FixedHeight = 15f;
            cell.MinimumHeight = 40F;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = 0;
            table.AddCell(cell);
            cell = null;

            // NOMBRE DEL TITULAR
            cell = new PdfPCell(new Phrase("Estimado (a):  " + Titular, Text));
            cell.Colspan = 10;
            cell.FixedHeight = 15f;
            cell.MinimumHeight = 35f;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = 0;
            table.AddCell(cell);
            cell = null;

            cell = new PdfPCell(new Phrase("En respuesta a tu solicitud de contratación de tu seguro te informamos que para poder continuar con el análisis requerimos que nos compartas la siguiente información: ", Text));
            cell.Colspan = 10;
            cell.FixedHeight = 30f;
            cell.MinimumHeight = 30f;
            cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            cell.Border = 0;
            table.AddCell(cell);
            cell = null;

            // EN LISTA LOS MOTIVOS DE RECHAZO
            if (motivos.Count > 0)
            {
                foreach (prop.motivosRechazo motivosRechazo in motivos)
                {
                    cell = new PdfPCell(new Phrase("   *   " + motivosRechazo.MotivoRechazo, Text));
                    cell.Colspan = 10;
                    cell.FixedHeight = 20f;
                    cell.MinimumHeight = 20f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 0;
                    table.AddCell(cell);
                    cell = null;
                }
            }
            else
            {

                cell = new PdfPCell(new Phrase("   *   NO SE ENCONTRARON MOTIVOS ", Text));
                cell.Colspan = 10;
                cell.FixedHeight = 20f;
                cell.MinimumHeight = 20f;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = 0;
                table.AddCell(cell);
                cell = null;
            }

            // ESPACIO EN BLANCO
            cell = new PdfPCell(new Phrase("", Text));
            cell.Colspan = 10;
            cell.FixedHeight = 15f;
            cell.MinimumHeight = 20F;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = 0;
            table.AddCell(cell);
            cell = null;

            // OBSERVACIONES
            cell = new PdfPCell(new Phrase("OBSERVACIONES.", Negrita));
            cell.Colspan = 10;
            cell.FixedHeight = 15f;
            cell.MinimumHeight = 15f;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = 0;
            table.AddCell(cell);
            cell = null;

            // EN LISTA LAS OBSERVACIONES DE BITACORA
            if (ObservacionesBitacora.Count > 0)
            {
                foreach (prop.bitacora observaciones in ObservacionesBitacora)
                {

                    // MOTIVOS DE HOLD OBSERVACIONES 
                    cell = new PdfPCell(new Phrase("   >   " + observaciones.Observacion, Text));
                    cell.Colspan = 10;
                    cell.FixedHeight = 20f;
                    cell.MinimumHeight = 20f;
                    cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                    cell.Border = 0;
                    table.AddCell(cell);
                    cell = null;
                }
            }
            else
            {
                // MOTIVOS DE HOLD OBSERVACIONES 
                cell = new PdfPCell(new Phrase(" > ", Text));
                cell.Colspan = 10;
                cell.FixedHeight = 20f;
                cell.MinimumHeight = 20f;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = 0;
                table.AddCell(cell);
                cell = null;
            }

            // ESPACIO EN BLANCO
            cell = new PdfPCell(new Phrase("", Text));
            cell.Colspan = 10;
            cell.FixedHeight = 15f;
            cell.MinimumHeight = 20F;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = 0;
            table.AddCell(cell);
            cell = null;

            cell = new PdfPCell(new Phrase("Te pedimos hacernos llegar la información y/o respuesta a través de tu agente de seguros, en un máximo de 15 días hábiles, para evitar la cancelación de este trámite.", Text));
            cell.Colspan = 10;
            cell.FixedHeight = 30f;
            cell.MinimumHeight = 30f;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = 0;
            table.AddCell(cell);
            cell = null;

            // ESPACIO EN BLANCO
            cell = new PdfPCell(new Phrase("", Text));
            cell.Colspan = 10;
            cell.FixedHeight = 15f;
            cell.MinimumHeight = 20F;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = 0;
            table.AddCell(cell);
            cell = null;

            cell = new PdfPCell(new Phrase("Sin más por el momento, nos reiteramos a tus órdenes.", Text));
            cell.Colspan = 10;
            cell.FixedHeight = 15f;
            cell.MinimumHeight = 15f;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = 0;
            table.AddCell(cell);
            cell = null;

            // ESPACIO EN BLANCO
            cell = new PdfPCell(new Phrase("", Text));
            cell.Colspan = 10;
            cell.FixedHeight = 15f;
            cell.MinimumHeight = 20F;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = 0;
            table.AddCell(cell);
            cell = null;

            cell = new PdfPCell(new Phrase("Atentamente.", Text));
            cell.Colspan = 10;
            cell.FixedHeight = 15f;
            cell.MinimumHeight = 15f;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = 0;
            table.AddCell(cell);
            cell = null;

            cell = new PdfPCell(new Phrase("METLIFE, MÉXICO S.A.", Text));
            cell.Colspan = 10;
            cell.FixedHeight = 15f;
            cell.MinimumHeight = 15f;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = 0;
            table.AddCell(cell);
            cell = null;

            string directorioImagen = HttpContext.Current.Server.MapPath("~") + "\\Imagenes\\";

            iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(directorioImagen + "firma.png");
            PdfPCell imageCell = new PdfPCell(jpg);
            imageCell.Colspan = 10; // either 1 if you need to insert one cell
            imageCell.Border = 0;
            imageCell.MinimumHeight = 125f;
            imageCell.FixedHeight = 125f;
            imageCell.HorizontalAlignment = Element.ALIGN_LEFT;
            table.AddCell(imageCell);
            imageCell = null;

            // ESPACIO EN BLANCO
            cell = new PdfPCell(new Phrase("", Text));
            cell.Colspan = 10;
            cell.FixedHeight = 15f;
            cell.MinimumHeight = 35F;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = 0;
            table.AddCell(cell);
            cell = null;

            cell = new PdfPCell(new Phrase("Agente: ", Text));
            cell.Colspan = 2;
            cell.FixedHeight = 15f;
            cell.MinimumHeight = 15f;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = 0;
            table.AddCell(cell);
            cell = null;

            // AGENTE 
            cell = new PdfPCell(new Phrase(Agente, Negrita));
            cell.Colspan = 8;
            cell.FixedHeight = 15f;
            cell.MinimumHeight = 15f;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = 0;
            table.AddCell(cell);
            cell = null;

            cell = new PdfPCell(new Phrase("Promotoría: ", Text));
            cell.Colspan = 2;
            cell.FixedHeight = 15f;
            cell.MinimumHeight = 15f;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = 0;
            table.AddCell(cell);
            cell = null;

            // PROMOTORIA
            cell = new PdfPCell(new Phrase(Promotoria, Negrita));
            cell.Colspan = 8;
            cell.FixedHeight = 15f;
            cell.MinimumHeight = 15f;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = 0;
            table.AddCell(cell);
            cell = null;



            document.Add(table);
            document.Close();

            //Response.ContentType = "application/pdf";
            //Response.AddHeader("Content-Disposition", "");
            //Response.BinaryWrite(output.ToArray());
            
        }

        private void NoExistente(HttpResponse Response)
        {
            // Medidas de la hoja A4
            var document = new Document(PageSize.A4, 30, 30, 50, 50);
            document.SetMargins(30, 30, 80, 50);
            var output = new MemoryStream();
            var writer = PdfWriter.GetInstance(document, output);

            PdfPCell cell = null;
            Font Text = new Font(FontFamily.HELVETICA, 30, Font.BOLD, BaseColor.BLACK);

            writer.PageEvent = new PDFFooter();

            document.Open();


            PdfPTable table = new PdfPTable(10);
            table.WidthPercentage = 100;
            table.SpacingBefore = 100F;


            cell = new PdfPCell(new Phrase("\n\n\n\n\n\n\n\n DOCUMENTO NO ENCONTRADO, FAVOR DE COMUNICARTE A SOPORTE PARA VERIFICAR LA EXISTENCIA DEL ARCHIVO", Text));
            cell.Colspan = 10;
            cell.FixedHeight = 15f;
            cell.MinimumHeight = 15f;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.Border = 0;
            table.AddCell(cell);
            cell = null;

            document.Add(table);
            document.Close();

            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Disposition", "");
            Response.BinaryWrite(output.ToArray());

        }
    }

    public class PDFFooter : PdfPageEventHelper
    {
        // write on top of document
        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            base.OnOpenDocument(writer, document);
        }

        // write on start of each page
        public override void OnStartPage(PdfWriter writer, Document document)
        {
            base.OnStartPage(writer, document);
            PdfPTable tabFot = new PdfPTable(new float[] { 1F });
            tabFot.TotalWidth = 540F;

            iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance("c:\\logo_grap.jpg");
            PdfPCell imageCell = new PdfPCell(jpg);
            imageCell.Colspan = 10; // either 1 if you need to insert one cell
            imageCell.Border = 0;
            imageCell.MinimumHeight = 50f;
            imageCell.FixedHeight = 50f;
            imageCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            tabFot.AddCell(imageCell);
            imageCell = null;
            tabFot.WriteSelectedRows(0, -1, 26, document.Top + 50, writer.DirectContent);
        }

        // write on end of each page
        public override void OnEndPage(PdfWriter writer, Document document)
        {
            base.OnEndPage(writer, document);
            PdfPTable tabFot = new PdfPTable(new float[] { 1F });
            Font TextFooter = new Font(FontFamily.HELVETICA, 7, Font.NORMAL, BaseColor.BLACK);
            PdfPCell cell;
            tabFot.TotalWidth = 540F;

            //cell = new PdfPCell(new Phrase("MetLife México, S.A., Blvd. Manuel Ávila Camacho No.32, pisos SKL, 14 a 20 y PH. Col Lomas de Chapultepec, CP. 11000, Delegación Miguel Hidalgo, Ciudad de México, Tel. 5328-7000 ó lada sin costo 01-800-00 METLIFE", TextFooter));
            //cell = new PdfPCell(new Phrase("MetLife México, S.A., Av. de los Insurgentes Sur 1457, Insurgentes Mixcoac, 03920 Ciudad de México, CDMX, Tel. 5328-7000 ó lada sin costo 01-800-00 METLIFE", TextFooter));
            cell = new PdfPCell(new Phrase("MetLife México, S.A., Avenida Insurgentes Sur No. 1457 pisos 7 al 14, Colonia Insurgentes Mixcoac, Alcaldía Benito Juárez, C.P. 03920, en la Ciudad de México.Tel. 5328 - 7000 ó lada sin costo 01 - 800 - 00 METLIFE(638 - 5433)", TextFooter));
            cell.Colspan = 10;
            cell.FixedHeight = 15f;
            cell.MinimumHeight = 15f;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.Border = 0;
            tabFot.AddCell(cell);
            cell = null;

            tabFot.WriteSelectedRows(0, -1, 30, document.Bottom, writer.DirectContent);
        }

        //write on close of document
        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            base.OnCloseDocument(writer, document);
        }
    }
}
