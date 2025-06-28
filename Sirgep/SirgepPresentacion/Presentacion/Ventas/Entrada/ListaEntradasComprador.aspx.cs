using iTextSharp.text.pdf;
using iTextSharp.text.pdf.codec.wmf;
using SirgepPresentacion.ReferenciaDisco;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.ServiceModel;
using System.Text;
using System.Web.UI;
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
            CargarDatos(null, null, null);
            //GvListaEntradasComprador.DataBind();
        }
        protected void CargarDatos(string fechaInicio, string fechaFin, string estado)
        {
            int idComprador = 40;
            //int idComprador = int.Parse(Session["idUsuario"].ToString());
            detalleEntradaDTO[] listaDetalleEntradas = entradaWS.listarPorComprador(idComprador, fechaInicio, fechaFin, estado);
            if (listaDetalleEntradas==null)
            {
                listaDetalleEntradas = new detalleEntradaDTO[0];
            }
            GvListaEntradasComprador.DataSource = listaDetalleEntradas;
            GvListaEntradasComprador.DataBind();
        }
        protected void btnDescargar_Click(object sender, EventArgs e)
        {
            //int idComprador = int.Parse(Session["idUsuario"].ToString());
            int idComprador = 40;
            DateTime? fechaInicio, fechaFin;
            string estado = null;
            ObtenerFiltros(out fechaInicio, out fechaFin, out estado);
            bool resultado=entradaWS.crearLibroExcelEntradas(idComprador, fechaInicio?.ToString("yyyy-MM-dd"), fechaFin?.ToString("yyyy-MM-dd"), estado);
            if (resultado)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "mostrarModal", $@"setTimeout(function() {{
                    mostrarModalExito('Descarga exitosa', 'La lista de entradas fue descargada correctamente.');}}, 1000);", true);
            }
            else
            {
                string mensajeError = "";
                if (fechaInicio!=null || fechaFin!=null || estado!=null)
                {
                    mensajeError = "No se pudo descargar la lista de entradas. Verifica los filtros seleccionados.";
                }
                else
                {
                    mensajeError = "No se pudo descargar la lista de entradas. No posees alguna entrada.";
                }
                ScriptManager.RegisterStartupScript(this, GetType(), "mostrarModal", $@"setTimeout(function() {{
                    mostrarModalError('Error en la descarga', '{mensajeError}');}}, 1000);", true);
            }
            txtFechaInicio.Text = txtFechaFin.Text = "";
            rblEstados.ClearSelection();
        }
        protected void BtnAbrir_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            string idConstancia = btn.CommandArgument;
            Response.Redirect("/Presentacion/Ventas/Entrada/ConstanciaEntrada.aspx?idConstancia=" + idConstancia);
        }
        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            DateTime? fechaInicio, fechaFin;
            string estado = null;
            ObtenerFiltros(out fechaInicio, out fechaFin, out estado);
            if (fechaInicio != null && fechaFin != null && fechaInicio > fechaFin)
            {
                lblMensaje.Text = "La fecha de inicio no puede ser mayor que la fecha de fin.";
                return;
            }
            CargarDatos(fechaInicio?.ToString("yyyy-MM-dd"),fechaFin?.ToString("yyyy-MM-dd"),estado);
            MostrarMensaje(fechaInicio, fechaFin, estado);
        }
        protected void btnMostrarTodos_Click(object sender, EventArgs e)
        {
            txtFechaInicio.Text = txtFechaFin.Text="";
            rblEstados.ClearSelection();
            CargarDatos(null, null, null);
            lblMensaje.Text = "Mostrando todas las entradas de hasta un año";
        }
        protected void ObtenerFiltros(out DateTime? fechaInicio, out DateTime? fechaFin, out string estado)
        {
            fechaInicio = DateTime.TryParse(txtFechaInicio.Text, out DateTime fi) ? fi : (DateTime?)null;
            fechaFin = DateTime.TryParse(txtFechaFin.Text, out DateTime ff) ? ff : (DateTime?)null;
            estado = rblEstados.SelectedValue;
        }
        protected void MostrarMensaje(DateTime? fechaInicio, DateTime? fechaFin, string estado)
        {
            StringBuilder mensaje = new StringBuilder("Entradas filtradas de hasta un año");
            if (fechaInicio != null && fechaFin != null) mensaje.Append($" entre {fechaInicio:dd/MM/yyyy} y {fechaFin:dd/MM/yyyy}");
            else if (fechaInicio != null) mensaje.Append($" desde {fechaInicio:dd/MM/yyyy}");
            else if (fechaFin != null) mensaje.Append($" hasta {fechaFin:dd/MM/yyyy}");
            if (!string.IsNullOrEmpty(estado)) mensaje.Append(" con estado " + estado);
            lblMensaje.Text = mensaje.ToString();
        }
    }
}