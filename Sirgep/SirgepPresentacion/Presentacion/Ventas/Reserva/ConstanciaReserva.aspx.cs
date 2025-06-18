using System;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using SirgepPresentacion.ReferenciaDisco;
using System.Text.RegularExpressions;

namespace SirgepPresentacion.Presentacion.Ventas.Reserva
{
    public partial class ConstanciaReserva : System.Web.UI.Page
    {
        private ReservaWSClient reservaWS;
        //private CompradorWSClient compradorWS;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                reservaWS = new ReservaWSClient();
                //int numReserva = 1;
                int numReserva = int.Parse(Request.QueryString["numReserva"]);
                CargarDatosEnPantalla(numReserva);
            }
        }
        protected void CargarDatosEnPantalla(int numReserva)
        {
            reserva reservaDomain = reservaWS.buscarReserva(numReserva);
            comprador compradorDomain = reservaWS.buscarCompradorDeReserva(reservaDomain.persona.idPersona);
            espacio espacioDomain = reservaWS.buscarEspacioDeReserva(reservaDomain.espacio.idEspacio);
            distrito distritoDomain = reservaWS.buscarDistritoDeReserva(espacioDomain.distrito.idDistrito);
            // Datos del espacio
            lblEspacio.Text = espacioDomain.nombre;
            lblTipoEspacio.Text = espacioDomain.tipoEspacio.ToString();
            lblSuperficie.Text = espacioDomain.superficie.ToString() + " m²";
            lblUbicacion.Text = espacioDomain.ubicacion;
            lblDistrito.Text = distritoDomain.nombre;
            // Datos de la Reserva
            lblNumReserva.Text = reservaDomain.numReserva.ToString();
            lblFechaReserva.Text = reservaDomain.fecha.ToString("dd/MM/yyyy");
            lblHoraInicio.Text = reservaDomain.horarioIni.ToString();
            lblHoraFin.Text = reservaDomain.horarioFin.ToString();
            //Datos del comprador
            lblNombres.Text = compradorDomain.nombres;
            lblApellidos.Text = compradorDomain.primerApellido + " " + compradorDomain.segundoApellido;
            lblTipoDocumento.Text = compradorDomain.tipoDocumento.ToString();
            lblTNumDocumento.Text = compradorDomain.numDocumento.ToString();
            lblCorreo.Text = compradorDomain.correo;
            // Datos de la constancia del pago
            lblFechaConstancia.Text = DateTime.Today.ToString("dd/MM/yyyy");
            lblMetodoPago.Text = reservaDomain.metodoPago.ToString();
            lblDetallePago.Text = reservaDomain.detallePago;
            //lblPrecio.Text = eventoDomain.precioEntrada.ToString("C2");
            lblTotal.Text = "S/. " + reservaDomain.total.ToString();
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            // Mostrar el modal de feedback
            string script = "mostrarModalFeedback();";
            ClientScript.RegisterStartupScript(this.GetType(), "mostrarModalFeedback", script, true);
        }

        protected void btnDescargar_Click(object sender, EventArgs e)
        {
            string rutaHtml = Server.MapPath("~/Resources/Pdfs/ReservaPdf.html");
            string paginaHTML_Texto = File.ReadAllText(rutaHtml);
            CargarDatosEnPdf(ref paginaHTML_Texto);
            using (MemoryStream ms = new MemoryStream())
            {
                Document pdfDoc = new Document(PageSize.A4, 50, 50, 50, 50);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, ms);
                pdfDoc.Open();
                pdfDoc.Add(new Phrase(""));
                // Ruta física y reemplazo del logo
                string rutaFisicaLogo = Server.MapPath("~/Images/grl/Escudo_Región_Lima_recortado.PNG");
                string rutaLogoFormatoFile = "file:///" + rutaFisicaLogo.Replace("\\", "/");
                paginaHTML_Texto = paginaHTML_Texto.Replace("{RUTA_LOGO}", rutaLogoFormatoFile);
                using (StringReader stringReader = new StringReader(paginaHTML_Texto))
                {
                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, stringReader);
                }
                pdfDoc.Close();
                // Envía el PDF al navegador
                byte[] bytes = ms.ToArray();
                Response.Clear();
                Response.ContentType = "application/pdf";
                string fechaFormato = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string nombresLimpios = Regex.Replace(lblNombres.Text, @"[^A-Za-z0-9]", "_");
                string nombreArchivo = $"Constancia_Reserva_{lblNumReserva.Text}_{nombresLimpios}_{fechaFormato}.pdf";
                Response.AddHeader("Content-Disposition", $"attachment; filename={nombreArchivo}");
                Response.OutputStream.Write(bytes, 0, bytes.Length);
                Response.Flush();
                Response.End();
            }
        }
        protected void CargarDatosEnPdf(ref string paginaHTML_Texto)
        {
            paginaHTML_Texto = paginaHTML_Texto.Replace("{NUMERO_RESERVA}", lblNumReserva.Text);
            // Datos del Espacio
            paginaHTML_Texto = paginaHTML_Texto.Replace("{NOMBRE_ESPACIO}", lblEspacio.Text);
            paginaHTML_Texto = paginaHTML_Texto.Replace("{TIPO_ESPACIO}", lblTipoEspacio.Text);
            paginaHTML_Texto = paginaHTML_Texto.Replace("{SUPERFICIE}", lblSuperficie.Text);
            paginaHTML_Texto = paginaHTML_Texto.Replace("{UBICACION}", lblUbicacion.Text);
            paginaHTML_Texto = paginaHTML_Texto.Replace("{DISTRITO}", lblDistrito.Text);
            // Datos de la Reserva
            paginaHTML_Texto = paginaHTML_Texto.Replace("{FECHA_RESERVA}", lblFechaReserva.Text);
            paginaHTML_Texto = paginaHTML_Texto.Replace("{HORA_INICIO}", lblHoraInicio.Text);
            paginaHTML_Texto = paginaHTML_Texto.Replace("{HORA_FIN}", lblHoraFin.Text);
            // Datos del comprador
            paginaHTML_Texto = paginaHTML_Texto.Replace("{NOMBRES}", lblNombres.Text);
            paginaHTML_Texto = paginaHTML_Texto.Replace("{APELLIDOS}", lblApellidos.Text);
            paginaHTML_Texto = paginaHTML_Texto.Replace("{TIPO_DOCUMENTO}", lblTipoDocumento.Text);
            paginaHTML_Texto = paginaHTML_Texto.Replace("{NUM_DOCUMENTO}", lblTNumDocumento.Text);
            paginaHTML_Texto = paginaHTML_Texto.Replace("{CORREO}", lblCorreo.Text);
            // Datos de la constancia de pago
            paginaHTML_Texto = paginaHTML_Texto.Replace("{FECHA_CONSTANCIA}", lblFechaConstancia.Text);
            paginaHTML_Texto = paginaHTML_Texto.Replace("{METODO_PAGO}", lblMetodoPago.Text);
            paginaHTML_Texto = paginaHTML_Texto.Replace("{TOTAL}", lblTotal.Text);
            paginaHTML_Texto = paginaHTML_Texto.Replace("{DETALLE_PAGO}", lblDetallePago.Text);
            //paginaHTML_Texto = paginaHTML_Texto.Replace("@FECHA", DateTime.Now.ToString("dd/MM/yyyy"));
        }
    }
}