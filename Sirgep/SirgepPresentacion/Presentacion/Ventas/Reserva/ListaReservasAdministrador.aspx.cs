using SirgepPresentacion.ReferenciaDisco;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SirgepPresentacion.Presentacion.Ventas.Reserva
{
    public partial class ListaReservasAdministrador : System.Web.UI.Page
    {
        private const int ReservasPorPagina = 20;
        private List<reservaDTO> Reservas
        {
            get => ViewState["Reservas"] as List<reservaDTO> ?? new List<reservaDTO>();
            set => ViewState["Reservas"] = value;
        }

        private ReservaWSClient client;
        private DistritoWSClient distrClient;

        protected void Page_Init(object sender, EventArgs e)
        {
            client = new ReservaWSClient();
            distrClient = new DistritoWSClient();
            CargarPagina();

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["PaginaActual"] = 1;

                // Solo carga los filtros, no las reservas
                ddlDistritos.DataSource = distrClient.listarTodosDistritos().ToList();
                ddlDistritos.DataTextField = "nombre";
                ddlDistritos.DataValueField = "idDistrito";
                ddlDistritos.DataBind();
                ddlDistritos.Items.Insert(0, new ListItem("Seleccione un distrito", ""));
                ddlDistritos.Visible = false;

                Reservas = client.listarTodasReservas().ToList();
                CargarPagina();
            }
        }

        protected void CargarPagina()
        {
            int paginaActual = (int)(ViewState["PaginaActual"] ?? 1);
            var reservas = Reservas;

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

        protected void ddlFiltros_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlFiltros.SelectedValue == "distrito")
            {
                txtFecha.Visible = false;
                ddlDistritos.Visible = true;
            }
            else
            {
                txtFecha.Visible = true;
                ddlDistritos.Visible = false;
            }
            CargarPagina();
        }

        protected void btnAnterior_Click(object sender, EventArgs e)
        {
            int paginaActual = (int)(ViewState["PaginaActual"] ?? 1);
            if (paginaActual > 1)
                ViewState["PaginaActual"] = paginaActual - 1;

            CargarPagina();
        }

        protected void btnSiguiente_Click(object sender, EventArgs e)
        {
            int paginaActual = (int)(ViewState["PaginaActual"] ?? 1);
            int totalPaginas = (int)Math.Ceiling((double)Reservas.Count / ReservasPorPagina);

            if (paginaActual < totalPaginas)
                ViewState["PaginaActual"] = paginaActual + 1;

            CargarPagina();
        }

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            string filtro = ddlFiltros.SelectedValue;
            string textoBusqueda = input_busqueda.Value.Trim();
            bool activo = chkActivos.Checked;

            try
            {
                List<reservaDTO> filtradas = new List<reservaDTO>();
                if (filtro == "fecha" && !string.IsNullOrWhiteSpace(txtFecha.Text))
                {
                    if (DateTime.TryParseExact(txtFecha.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime fecha))
                    {
                        string fechaStr = fecha.ToString("yyyy-MM-dd");
                        filtradas = client.listarReservaPorFecha(fechaStr, activo).ToList();
                    }
                }
                else if (filtro == "distrito" && ddlDistritos.SelectedValue != "")
                {
                    int idDistrito = int.Parse(ddlDistritos.SelectedValue);
                    filtradas = client.listarReservaPorDistrito(idDistrito, activo).ToList();
                }
                else
                {
                    var todas = client.listarTodasReservas().ToList();
                    filtradas = activo ? todas.Where(r => (char)r.activo == 'A').ToList() : todas;
                }

                Reservas = filtradas;
                ViewState["PaginaActual"] = 1;
                CargarPagina();
            }
            catch
            {
                Reservas = new List<reservaDTO>();
                CargarPagina();
            }
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            string textoBusqueda = input_busqueda.Value.Trim().ToLower();

            try
            {
                // Usar el listado actual ya filtrado (almacenado en ViewState)
                var listadoActual = Reservas;

                // Aplicar filtro de búsqueda solo si hay texto
                if (!string.IsNullOrEmpty(textoBusqueda))
                {
                    listadoActual = listadoActual.Where(r =>
                        ("#" + r.codigo.ToString("D3")).ToLower().Contains(textoBusqueda) ||
                        r.fecha.ToString("yyyy-MM-dd").Contains(textoBusqueda) ||
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
        }
    }
}
