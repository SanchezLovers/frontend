
﻿using System;
using System.IO;

using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using SirgepPresentacion.ReferenciaDisco;


using System.Text;
using System.Text.RegularExpressions;




namespace SirgepPresentacion.Presentacion.Ventas.Entrada
{
    public partial class ConstanciaEntrada : System.Web.UI.Page
    {
        private EntradaWSClient entradaWS;
        //private CompradorWSClient compradorWS;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                entradaWS = new EntradaWSClient();
                //int idEntrada = 1;
                //int idEntrada = int.Parse((sender as Button).CommandArgument);
                int numEntrada = int.Parse(Request.QueryString["idEntrada"]);
                CargarDatosEnPantalla(numEntrada);
            }
        }
        protected void CargarDatosEnPantalla(int numEntrada)
        {
            entrada entradaDomain = entradaWS.buscarEntrada(numEntrada);
            comprador compradorDomain = entradaWS.buscarCompradorDeEntrada(entradaDomain.persona.idPersona);
            funcion funcionDomain = entradaWS.buscarFuncionDeEntrada(entradaDomain.funcion.idFuncion);
            evento eventoDomain = entradaWS.buscarEventoDeEntrada(funcionDomain.evento.idEvento);
            distrito distritoDomain = entradaWS.buscarDistritoDeEntrada(eventoDomain.distrito.idDistrito);
            // Datos de la entrada
            lblNumEntrada.Text = entradaDomain.numEntrada.ToString();
            // Datos del evento
            lblEvento.Text = eventoDomain.nombre;
            lblUbicacion.Text = eventoDomain.ubicacion;
            lblReferencias.Text = eventoDomain.referencia;
            lblDistrito.Text = distritoDomain.nombre;
            // Datos de la funcion
            lblFechaFuncion.Text = funcionDomain.fecha.ToString();
            lblHoraInicio.Text = funcionDomain.horaInicio.ToString();
            lblHoraFin.Text = funcionDomain.horaFin.ToString();
            //Datos del comprador
            lblNombres.Text = compradorDomain.nombres;
            lblApellidos.Text = compradorDomain.primerApellido + " " + compradorDomain.segundoApellido;
            lblTipoDocumento.Text = compradorDomain.tipoDocumento.ToString();
            lblTNumDocumento.Text = compradorDomain.numDocumento.ToString();
            lblCorreo.Text = compradorDomain.correo;
            // Datos de la constancia del pago
            lblFechaConstancia.Text = DateTime.Today.ToString("dd/MM/yyyy");
            lblMetodoPago.Text = entradaDomain.metodoPago.ToString();
            lblDetallePago.Text = entradaDomain.detallePago.ToString();
            //lblPrecio.Text = eventoDomain.precioEntrada.ToString("C2");
            lblTotal.Text = "S/. " + entradaDomain.total.ToString();
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            Session["tipoServicio"] = "entrada";
            string script = "setTimeout(function(){ mostrarModalFeedback(); }, 300);";
            ClientScript.RegisterStartupScript(this.GetType(), "mostrarModalFeedback", script, true);
        }

        protected void btnDescargar_Click(object sender, EventArgs e)
        {
            string rutaHtml = Server.MapPath("~/Resources/Pdfs/EntradaPdf.html");
            string paginaHTML_Texto = File.ReadAllText(rutaHtml, Encoding.UTF8);
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
                string nombreArchivo = $"Constancia_Entrada_{lblNumEntrada.Text}_{nombresLimpios}_{fechaFormato}.pdf";
                Response.AddHeader("Content-Disposition", $"attachment; filename={nombreArchivo}");
                Response.OutputStream.Write(bytes, 0, bytes.Length);
                Response.Flush();
                Response.End();
            }
        }
        protected void CargarDatosEnPdf(ref string paginaHTML_Texto)
        {
            paginaHTML_Texto = paginaHTML_Texto.Replace("{NUMERO_ENTRADA}", lblNumEntrada.Text);
            // Datos de la entrada
            paginaHTML_Texto = paginaHTML_Texto.Replace("{NOMBRE_EVENTO}", lblEvento.Text);
            paginaHTML_Texto = paginaHTML_Texto.Replace("{UBICACION}", lblUbicacion.Text);
            paginaHTML_Texto = paginaHTML_Texto.Replace("{DISTRITO}", lblDistrito.Text);
            paginaHTML_Texto = paginaHTML_Texto.Replace("{REFERENCIAS}", lblReferencias.Text);
            // Datos del evento
            paginaHTML_Texto = paginaHTML_Texto.Replace("{FECHA_FUNCION}", lblFechaFuncion.Text);
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