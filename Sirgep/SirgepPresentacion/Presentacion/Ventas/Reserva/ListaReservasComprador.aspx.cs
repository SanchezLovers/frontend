using SirgepPresentacion.ReferenciaDisco;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SirgepPresentacion.Presentacion.Ventas.Reserva
{
    public partial class ListaReservasComprador : System.Web.UI.Page
    {
        private ReservaWSClient reservaWS;
        protected void Page_Init(object sender, EventArgs e)
        {
            reservaWS = new ReservaWSClient();
            CargarDatos();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }
        }
        protected void GvListaReservasComprador_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GvListaReservasComprador.PageIndex = e.NewPageIndex;
            CargarDatos();
            //GvListaEntradasComprador.DataBind();
        }
        protected void CargarDatos()
        {
            //int idComprador = int.Parse(Session["idUsuario"].ToString());
            int idComprador = 3;
            GvListaReservasComprador.DataSource = reservaWS.listarDetalleReservasPorComprador(idComprador);
            GvListaReservasComprador.DataBind();
        }

        protected void btnDescargar_Click(object sender, EventArgs e)
        {
            int idComprador = int.Parse(Session["idUsuario"].ToString());
            //int idComprador = 3;
            reservaWS.crearLibroExcelReservas(idComprador);
        }
        protected void BtnAbrir_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            string idConstancia = btn.CommandArgument;
            Response.Redirect("/Presentacion/Ventas/Reserva/ConstanciaReserva.aspx?idConstancia=" + idConstancia);
        }

        protected void btnConfirmarAccion_Click(object sender, EventArgs e)
        {
            int id = int.Parse(hdnReservaAEliminar.Value);
            reservaWS.cancelarReserva(id);
            // Boolean estado = response.@return;
            CargarDatos(); // Refresca la tabla
            // Opcional: Mostrar mensaje de éxito
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertEliminar",
                "alert('Reserva Cancelada exitosamente');", true);
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            string textoBusqueda = input_busqueda.Value.Trim().ToLower();
            /*
            try
            {
                // Usar el listado actual ya filtrado (almacenado en ViewState)
                var listadoActual = Reservas;

                // Aplicar filtro de búsqueda solo si hay texto
                if (!string.IsNullOrEmpty(textoBusqueda))
                {
                    listadoActual = listadoActual.Where(r =>
                        ("#" + r.numReserva.ToString("D3")).ToLower().Contains(textoBusqueda) ||
                        r.fechaReserva.ToString("yyyy-MM-dd").Contains(textoBusqueda) ||
                        (r.distrito?.ToLower().Contains(textoBusqueda) ?? false) ||
                        (r.espacio?.ToLower().Contains(textoBusqueda) ?? false) ||
                        (r.correo?.ToLower().Contains(textoBusqueda) ?? false)
                    ).ToList();
                }
                else
                {
                    listadoActual = client.listarTodasReservas().ToList();
                }

                Reservas = listadoActual;
                ViewState["PaginaActual"] = 1;
                CargarPagina();
            }
            catch
            {
                Reservas = new List<reservaDTO>();
                CargarPagina();
            }
            */
        }
        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            List<string> estadosSeleccionados = new List<string>();
            if (chkVigentes.Checked)
                estadosSeleccionados.Add("Vigente");
            if (chkFinalizadas.Checked)
                estadosSeleccionados.Add("Completada");
            if (chkCanceladas.Checked)
                estadosSeleccionados.Add("Cancelada");
        }
    }
}