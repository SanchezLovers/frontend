using ClosedXML.Excel;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using SirgepPresentacion.ReferenciaDisco;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace SirgepPresentacion.Presentacion.Usuarios.Administrador
{
    public partial class CalendarioReservas : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["action"] == "getReservas")
            {
                CargarReservasJson();
                return;
            }

            if (!IsPostBack)
            {
                // Cargar reservas una sola vez al entrar a la página
                var reservas = new ReservaWSClient().listarReservas();
                Session["ReservasCalendario"] = reservas;

                // Habilitar los botones de exportar solo si hay datos
                btnExportarPDF.Enabled = reservas != null && reservas.Length > 0;
                btnExportarExcel.Enabled = reservas != null && reservas.Length > 0;
            }
        }

        private void CargarReservasJson()
        {
            Response.Clear();
            Response.ContentType = "application/json";

            var reservas = Session["ReservasCalendario"] as reserva[];
            if (reservas == null)
            {
                reservas = new ReservaWSClient().listarReservas();
                Session["ReservasCalendario"] = reservas;
            }

            var eventos = new List<object>();
            foreach (var r in reservas)
            {
                eventos.Add(new
                {
                    title = $"{r.espacio?.nombre} - {r.persona?.nombres} {r.persona?.primerApellido + ' ' + r.persona?.segundoApellido}",
                    start = r.fechaReserva.ToString("yyyy-MM-dd") + "T" + r.iniString,
                    end = r.fechaReserva.ToString("yyyy-MM-dd") + "T" + r.finString,
                    extendedProps = new
                    {
                        numReserva = r.numReserva,
                        espacio = r.espacio?.nombre,
                        fecha = r.fechaReserva.ToString("yyyy-MM-dd"),
                        horaInicio = r.iniString,
                        horaFin = r.finString,
                        nombres = r.persona?.nombres,
                        apellidos = r.persona?.primerApellido + " " + r.persona?.segundoApellido,
                        tipoDocumento = r.persona?.tipoDocumento.ToString(),
                        numDocumento = r.persona?.numDocumento
                    }
                });
            }


            var serializer = new JavaScriptSerializer();
            Response.Write(serializer.Serialize(eventos));
            Response.End();
        }

        protected void btnExportarPDF_Click(object sender, EventArgs e)
        {
            var reservas = Session["ReservasCalendario"] as reserva[];
            if (reservas == null || reservas.Length == 0)
            {
                ClientScript.RegisterStartupScript(GetType(), "alert", "alert('Primero debe cargar las reservas.');", true);
                return;
            }

            string rutaHtml = Server.MapPath("~/Resources/Pdfs/CalendarioReservasPdf.html");
            string paginaHTML_Texto = File.ReadAllText(rutaHtml, Encoding.UTF8);

            StringBuilder filas = new StringBuilder();
            foreach (var r in reservas)
            {
                filas.Append("<tr>");
                filas.Append($"<td>{r.numReserva}</td>");
                filas.Append($"<td>{r.fechaReserva:yyyy-MM-dd}</td>");
                filas.Append($"<td>{r.iniString}</td>");
                filas.Append($"<td>{r.finString}</td>");
                filas.Append($"<td>{r.espacio?.nombre}</td>");
                filas.Append($"<td>{r.persona?.nombres}</td>");
                filas.Append($"<td>{r.persona?.primerApellido + ' ' + r.persona?.segundoApellido}</td>");
                filas.Append($"<td>{r.persona?.tipoDocumento}</td>");
                filas.Append($"<td>{r.persona?.numDocumento}</td>");
                filas.Append("</tr>");
            }


            paginaHTML_Texto = paginaHTML_Texto.Replace("{FILAS_RESERVAS}", filas.ToString());

            string rutaFisicaLogo = Server.MapPath("~/Images/grl/Escudo_Región_Lima_recortado.PNG");
            paginaHTML_Texto = paginaHTML_Texto.Replace("{RUTA_LOGO}", "file:///" + rutaFisicaLogo.Replace("\\", "/"));

            using (MemoryStream ms = new MemoryStream())
            {
                Document pdfDoc = new Document(PageSize.A4, 30, 30, 30, 30);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, ms);
                pdfDoc.Open();
                using (StringReader stringReader = new StringReader(paginaHTML_Texto))
                {
                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, stringReader);
                }
                pdfDoc.Close();

                byte[] bytes = ms.ToArray();
                Response.Clear();
                Response.ContentType = "application/pdf";
                string nombreArchivo = $"Reporte_Reservas_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
                Response.AddHeader("Content-Disposition", $"attachment; filename={nombreArchivo}");
                Response.BinaryWrite(bytes);
                Response.Flush();
                Response.End();
            }
        }

        protected void btnExportarExcel_Click(object sender, EventArgs e)
        {
            var reservas = Session["ReservasCalendario"] as reserva[];
            if (reservas == null || reservas.Length == 0)
            {
                ClientScript.RegisterStartupScript(GetType(), "alert", "alert('Primero debe cargar las reservas.');", true);
                return;
            }

            using (var workbook = new XLWorkbook())
            {
                var ws = workbook.Worksheets.Add("Reservas");
                ws.Cell(1, 1).Value = "N° Reserva";
                ws.Cell(1, 2).Value = "Fecha";
                ws.Cell(1, 3).Value = "Hora Inicio";
                ws.Cell(1, 4).Value = "Hora Fin";
                ws.Cell(1, 5).Value = "Espacio";
                ws.Cell(1, 6).Value = "Nombres";
                ws.Cell(1, 7).Value = "Apellidos";
                ws.Cell(1, 8).Value = "Tipo Doc";
                ws.Cell(1, 9).Value = "N° Documento";


                int fila = 2;
                foreach (var r in reservas)
                {
                    ws.Cell(fila, 1).Value = r.numReserva;
                    ws.Cell(fila, 2).Value = r.fechaReserva.ToString("yyyy-MM-dd");
                    ws.Cell(fila, 3).Value = r.iniString;
                    ws.Cell(fila, 4).Value = r.finString;
                    ws.Cell(fila, 5).Value = r.espacio?.nombre;
                    ws.Cell(fila, 6).Value = r.persona?.nombres;
                    ws.Cell(fila, 7).Value = r.persona?.primerApellido + " " + r.persona?.segundoApellido;
                    ws.Cell(fila, 8).Value = r.persona?.tipoDocumento.ToString();
                    ws.Cell(fila, 9).Value = r.persona?.numDocumento;

                    fila++;
                }

                using (MemoryStream ms = new MemoryStream())
                {
                    workbook.SaveAs(ms);
                    byte[] bytes = ms.ToArray();
                    Response.Clear();
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    string nombreArchivo = $"Reporte_Reservas_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                    Response.AddHeader("Content-Disposition", $"attachment; filename={nombreArchivo}");
                    Response.BinaryWrite(bytes);
                    Response.Flush();
                    Response.End();
                }
            }
        }
    }
}