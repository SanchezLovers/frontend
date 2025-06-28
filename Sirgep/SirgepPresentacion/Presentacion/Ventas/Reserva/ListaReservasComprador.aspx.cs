using iTextSharp.text.pdf.codec.wmf;
using SirgepPresentacion.Presentacion.Ventas.Entrada;
using SirgepPresentacion.ReferenciaDisco;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SirgepPresentacion.Presentacion.Ventas.Reserva
{
    public partial class ListaReservasComprador : System.Web.UI.Page
    {
        private ReservaWSClient reservaWS;
        private detalleReservaDTO[] listaReservasComprador
        {
            get => ViewState["listaReservasComprador"] as detalleReservaDTO[] ?? new detalleReservaDTO[0];
            set => ViewState["listaReservasComprador"] = value;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            reservaWS = new ReservaWSClient();
            if (!IsPostBack)
            {
                int idComprador = 3;
                //int idComprador = int.Parse(Session["idUsuario"].ToString());
                listaReservasComprador = reservaWS.listarReservasPorComprador(idComprador, null, null, null);
                GvListaReservasComprador.DataSource = listaReservasComprador;
                GvListaReservasComprador.DataBind();
            }
        }
        protected void GvListaReservasComprador_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GvListaReservasComprador.PageIndex = e.NewPageIndex;
            GvListaReservasComprador.DataSource = listaReservasComprador;
            GvListaReservasComprador.DataBind();
        }
        protected void CargarDatos(string fechaInicio, string fechaFin, string estado)
        {
            //int idComprador = int.Parse(Session["idUsuario"].ToString());
            int idComprador = 3;
            detalleReservaDTO[] listaDetalleReservas = reservaWS.listarReservasPorComprador(idComprador, fechaInicio, fechaFin, estado);
            if (listaDetalleReservas == null)
            {
                listaDetalleReservas = new detalleReservaDTO[0];
            }
            GvListaReservasComprador.DataSource = listaDetalleReservas;
            GvListaReservasComprador.DataBind();
        }
        protected void btnDescargar_Click(object sender, EventArgs e)
        {
            int idComprador = int.Parse(Session["idUsuario"].ToString());
            //int idComprador = 3;
            DateTime? fechaInicio, fechaFin;
            string estado = null;
            ObtenerFiltros(out fechaInicio, out fechaFin, out estado);
            bool resultado = reservaWS.crearLibroExcelReservas(idComprador, fechaInicio?.ToString("yyyy-MM-dd"), fechaFin?.ToString("yyyy-MM-dd"), estado);
            if (resultado)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "mostrarModal", $@"setTimeout(function() {{
                    mostrarModalExito('Descarga exitosa', 'La lista de reservas fue descargada correctamente.');}}, 1000);", true);
            }
            else
            {
                string mensajeError = "";
                if (fechaInicio != null || fechaFin != null || estado != null)
                {
                    mensajeError = "No se pudo descargar la lista de reservas. Verifica los filtros seleccionados.";
                }
                else
                {
                    mensajeError = "No se pudo descargar la lista de reservas. No posees alguna reserva.";
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
            Response.Redirect("/Presentacion/Ventas/Reserva/ConstanciaReserva.aspx?idConstancia=" + idConstancia);
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
            CargarDatos(fechaInicio?.ToString("yyyy-MM-dd"), fechaFin?.ToString("yyyy-MM-dd"), estado);
            MostrarMensaje(fechaInicio, fechaFin, estado);
        }
        protected void btnMostrarTodos_Click(object sender, EventArgs e)
        {
            txtFechaInicio.Text = txtFechaFin.Text = "";
            rblEstados.ClearSelection();
            GvListaReservasComprador.DataSource = listaReservasComprador;
            GvListaReservasComprador.DataBind();
            lblMensaje.Text = "Mostrando todas las reservas de hasta un año";
        }
        protected void ObtenerFiltros(out DateTime? fechaInicio, out DateTime? fechaFin, out string estado)
        {
            fechaInicio = DateTime.TryParse(txtFechaInicio.Text, out DateTime fi) ? fi : (DateTime?)null;
            fechaFin = DateTime.TryParse(txtFechaFin.Text, out DateTime ff) ? ff : (DateTime?)null;
            estado = rblEstados.SelectedValue;
        }
        protected void MostrarMensaje(DateTime? fechaInicio, DateTime? fechaFin, string estado)
        {
            StringBuilder mensaje = new StringBuilder("Reservas filtradas de hasta un año");
            if (fechaInicio != null && fechaFin != null) mensaje.Append($" entre {fechaInicio:dd/MM/yyyy} y {fechaFin:dd/MM/yyyy}");
            else if (fechaInicio != null) mensaje.Append($" desde {fechaInicio:dd/MM/yyyy}");
            else if (fechaFin != null) mensaje.Append($" hasta {fechaFin:dd/MM/yyyy}");
            if (!string.IsNullOrEmpty(estado)) mensaje.Append(" con estado " + estado);
            lblMensaje.Text = mensaje.ToString();
        }

        protected void btnConfirmarAccion_Click(object sender, EventArgs e)
        {
            int id = int.Parse(hdnReservaAEliminar.Value);

            reservaWS.cancelarReserva(id);
            // Boolean estado = response.@return;
            CargarDatos(null,null,null); // Refresca la tabla

            // Opcional: Mostrar mensaje de éxito
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertEliminar",
                "alert('Reserva Cancelada exitosamente');", true);


        }
    }
}