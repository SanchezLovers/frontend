using SirgepPresentacion.ReferenciaDisco;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace SirgepPresentacion.Presentacion.Ventas.Reserva
{
    public partial class ListaReservasAdministrador : System.Web.UI.Page
    {
        private const int ReservasPorPagina = 20;
        private List<reserva> reservas = new List<reserva>();
        ReservaWSClient client = new ReservaWSClient();
        DistritoWSClient distrito = new DistritoWSClient();
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                // Initialize page components or load data here
                ViewState["PaginaActual"] = 1;
                CargarReservas();
                ddlDistritos.DataSource = distrito.listarTodosDistritos().ToList();
                ddlDistritos.DataTextField = "nombre";
                ddlDistritos.DataValueField = "idDistrito";
                ddlDistritos.DataBind();
                ddlDistritos.Items.Insert(0, new ListItem("Seleccione un distrito", ""));
                ddlDistritos.Visible = false;
                txtFecha.Visible = false;
                horaContainerIni.Visible = false;
                horaContainerFin.Visible = false;
            }
        }

        private List<reserva> obtenerTodasLasReservas()
        {
            try
            {
                //Esto es para convertir en una lista
                List<reserva> todasLasReservas = client.listarReservas().ToList();
                return todasLasReservas;
            }
            catch (ArgumentNullException)
            {
                // Si ocurre ArgumentNullException, retorna una lista vacía
                return new List<reserva>();
            }
        }

        protected void CargarPagina()
        {
            int paginaActual = (int)(ViewState["PaginaActual"] ?? 1);
            int totalPaginas = (int)Math.Ceiling((double)reservas.Count / ReservasPorPagina);

            var reservasPagina = reservas
                .Skip((paginaActual - 1) * ReservasPorPagina)
                .Take(ReservasPorPagina)
                .ToList();

            gvReservas.DataSource = reservasPagina;
            gvReservas.DataBind();

            lblPagina.Text = $"Página {paginaActual} de {totalPaginas}";
            btnAnterior.Enabled = paginaActual > 1;
            btnSiguiente.Enabled = paginaActual < totalPaginas;
        }
        protected void CargarReservas()
        {
            reservas = obtenerTodasLasReservas();
            CargarPagina();
        }

        protected void ddlFiltros_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlFiltros.SelectedValue == "fecha")
            {
                txtFecha.Visible = true;
                horaContainerIni.Visible = false;
                horaContainerFin.Visible = false;
                input_busqueda.Disabled = true;
                ddlDistritos.Visible = false;
                //Esto cambia el formato del borde de busqueda cuando estas en un filtro que no se usa
                input_busqueda.Attributes["class"] = "input-busqueda deshabilitado";
            }
            else if(ddlFiltros.SelectedValue == "horario")
            {
                txtFecha.Visible = true;
                horaContainerIni.Visible = true;
                horaContainerFin.Visible = true;
                input_busqueda.Disabled = true;
                ddlDistritos.Visible = false;
                input_busqueda.Attributes["class"] = "input-busqueda deshabilitado";
            }
            else if (ddlFiltros.SelectedValue == "distrito")
            {
                txtFecha.Visible = false;
                horaContainerIni.Visible = false;
                horaContainerFin.Visible = false;
                input_busqueda.Disabled = false;
                ddlDistritos.Visible = true;
                input_busqueda.Attributes["class"] = "input-busqueda deshabilitado";
            }
            else
            {
                txtFecha.Visible = false;
                horaContainerIni.Visible = false;
                horaContainerFin.Visible = false;
                input_busqueda.Disabled = false;
                ddlDistritos.Visible = false;
                input_busqueda.Attributes["class"] = "input-busqueda";
            }
        }

        protected void btnAnterior_Click(object sender, EventArgs e)
        {
            int paginaActual = (int)(ViewState["PaginaActual"] ?? 1);
            if (paginaActual > 1)
                ViewState["PaginaActual"] = paginaActual - 1;
                CargarReservas();
        }

        protected void btnSiguiente_Click(object sender, EventArgs e)
        {
            int paginaActual = (int)(ViewState["PaginaActual"] ?? 1);
            int totalPaginas = (int)Math.Ceiling((double)obtenerTodasLasReservas().Count / ReservasPorPagina);
            if (paginaActual < totalPaginas)
                ViewState["PaginaActual"] = paginaActual + 1;
            CargarReservas();
        }

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            string filtro = ddlFiltros.SelectedValue;
            string textoBusqueda = input_busqueda.Value.Trim();
            bool activo = chkActivos.Checked;
            string horaIni = inputHoraIni.Value.ToString().Trim();
            string horaFin = inputHoraFin.Value.ToString().Trim();

            try
            {
                if ((filtro == "codigo" || filtro == "espacio" || filtro == "persona") && int.TryParse(textoBusqueda, out int codigo))
                {
                    if (filtro == "codigo")
                    {
                        reserva reservaBuscada = client.obtenerPorNumReserva(codigo, activo);
                        if (reservaBuscada != null)
                        {
                            reservas.Add(reservaBuscada);
                        }
                        else
                        {
                            reservas = new List<reserva>();
                        }
                    }
                    else if (filtro == "espacio")
                    {
                        reservas = client.listarReservaPorEspacio(codigo, activo).ToList();
                    }
                    else if (filtro == "persona")
                    {
                        reservas = client.listarReservaPorPersona(codigo, activo).ToList();
                    }
                }
                else if ((filtro == "fecha" || filtro == "horario") && !string.IsNullOrWhiteSpace(txtFecha.Text))
                {
                    DateTime fecha;
                    DateTime.TryParseExact(txtFecha.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out fecha);
                    string fechaStr = fecha.ToString("yyyy-MM-dd");

                    if (filtro == "fecha")
                    {
                        reservas = client.listarPorFecha(fechaStr, activo).ToList();
                    }
                    else if (filtro == "horario")
                    {
                        reservas = client.listarReservaPorHorario(horaIni, horaFin, fechaStr, activo).ToList();
                    }
                }
                else if (filtro == "distrito" && ddlDistritos.SelectedValue != "")
                {
                    int idDistrito = int.Parse(ddlDistritos.SelectedValue);
                    reservas = client.listarReservaPorDistrito(idDistrito, activo).ToList();
                }
                else if (activo)
                {
                    // Caso: No filtro ni texto, solo activo marcado
                    List<reserva> todas = client.listarReservas().ToList();
                    reservas = todas.Where(r => (char)r.activo == 'A').ToList();
                }
                else
                {
                    // Si no se cumple nada, lista vacía o todas las reservas?
                    reservas = client.listarReservas().ToList();
                }
            }
            catch (ArgumentNullException)
            {
                reservas = new List<reserva>();
            }
            CargarPagina();
        }
    }
}