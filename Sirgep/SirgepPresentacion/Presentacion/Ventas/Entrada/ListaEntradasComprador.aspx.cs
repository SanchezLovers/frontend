using iTextSharp.text.pdf.codec.wmf;
using SirgepPresentacion.ReferenciaDisco;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.ServiceModel;
using System.Text;
using System.Web.UI.WebControls;

namespace SirgepPresentacion.Presentacion.Ventas.Entrada
{
    public partial class ListaEntradasComprador : System.Web.UI.Page
    {
        private EntradaWSClient entradaWS;
        private List<detalleEntradaDTO> listaDetalleEntradas
        {
            get => ViewState["listaDetalleEntradas"] as List<detalleEntradaDTO> ?? new List<detalleEntradaDTO>();
            set => ViewState["listaDetalleEntradas"] = value;
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            entradaWS = new EntradaWSClient();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                entradaWS = new EntradaWSClient();
                CargarDatos(null, null, null);
            }
        }
        protected void GvListaEntradasComprador_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GvListaEntradasComprador.PageIndex = e.NewPageIndex;
            CargarDatos(null,null,null);
            //GvListaEntradasComprador.DataBind();
        }
        protected void CargarDatos(string fechaInicio, string fechaFin, string estado)
        {
            int idComprador = 40;
            //int idComprador = int.Parse(Session["idUsuario"].ToString());
            detalleEntradaDTO[] listaDetalleEntradas = null;
            try
            {
                listaDetalleEntradas = entradaWS.listarDetalleEntradasFiltradaPorComprador(idComprador, fechaInicio, fechaFin, estado);
            }
            catch (FaultException ex)
            {
                listaDetalleEntradas = new detalleEntradaDTO[0]; // Para evitar que el GridView falle
            }
            finally
            {
                GvListaEntradasComprador.DataSource = listaDetalleEntradas;
                GvListaEntradasComprador.DataBind();
            }
        }
        protected void btnDescargar_Click(object sender, EventArgs e)
        {
            int idComprador = int.Parse(Session["idUsuario"].ToString());
            //int idComprador = 3;
            entradaWS.crearLibroExcelEntradas(idComprador);
        }
        protected void BtnAbrir_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            string idConstancia = btn.CommandArgument;
            Response.Redirect("/Presentacion/Ventas/Entrada/ConstanciaEntrada.aspx?idConstancia=" + idConstancia);
        }
        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            DateTime? fechaInicio = DateTime.TryParse(txtFechaInicio.Text, out var fi) ? fi : (DateTime?)null;
            DateTime? fechaFin = DateTime.TryParse(txtFechaFin.Text, out var ff) ? ff : (DateTime?)null;

            // Validar que la fecha de inicio no sea mayor que la fecha de fin
            if (fechaInicio != null && fechaFin != null && fechaInicio > fechaFin)
            {
                lblMensaje.Text = "La fecha de inicio no puede ser mayor que la fecha de fin.";
                txtFechaInicio.Text = txtFechaFin.Text = "";
                chkVigentes.Checked = chkFinalizadas.Checked = chkCanceladas.Checked = false;
                return;
            }

            // Obtener estados seleccionados
            string estado=null;
            if (chkVigentes.Checked) estado="Vigentes";
            else if (chkFinalizadas.Checked) estado="Finalizadas";
            else if (chkCanceladas.Checked) estado="Canceladas";

            // Llamar a método de carga
            CargarDatos(fechaInicio?.ToString("yyyy-MM-dd"), fechaFin?.ToString("yyyy-MM-dd"), estado);

            // Limpiar filtros
            txtFechaInicio.Text = txtFechaFin.Text = "";
            chkVigentes.Checked = chkFinalizadas.Checked = chkCanceladas.Checked = false;

            // Construir mensaje
            StringBuilder mensaje = new StringBuilder("Entradas filtradas");

            if (fechaInicio != null && fechaFin != null)
            {
                mensaje.Append($" entre {fechaInicio.Value:dd/MM/yyyy} y {fechaFin.Value:dd/MM/yyyy}");
            }
            else if (fechaInicio != null)
            {
                mensaje.Append($" desde {fechaInicio.Value:dd/MM/yyyy}");
            }
            else if (fechaFin != null)
            {
                mensaje.Append($" hasta {fechaFin.Value:dd/MM/yyyy}");
            }

            if (estado!=null)
            {
                mensaje.Append(" con estado " + estado);
            }

            lblMensaje.Text = mensaje.ToString();
        }
    }
}