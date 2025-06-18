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
        private ReservaWSClient client = new ReservaWSClient();
        private DistritoWSClient distrClient = new DistritoWSClient();
        private EspacioWSClient espClient = new EspacioWSClient();

        private Dictionary<int, espacio> cacheEspacios = new Dictionary<int, espacio>();
        private Dictionary<int, distrito> cacheDistritos = new Dictionary<int, distrito>();

        private List<reserva> Reservas
        {
            get => ViewState["Reservas"] as List<reserva> ?? new List<reserva>();
            set => ViewState["Reservas"] = value;
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
                txtFecha.Visible = false;
            }
        }

        private espacio ObtenerEspacio(int idEspacio)
        {
            if (!cacheEspacios.ContainsKey(idEspacio))
                cacheEspacios[idEspacio] = espClient.buscarEspacio(idEspacio);

            return cacheEspacios[idEspacio];
        }

        private distrito ObtenerDistrito(int idDistrito)
        {
            if (!cacheDistritos.ContainsKey(idDistrito))
                cacheDistritos[idDistrito] = distrClient.buscarDistPorId(idDistrito);

            return cacheDistritos[idDistrito];
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

            var reservasDTO = reservasPagina.Select(r =>
            {
                var esp = ObtenerEspacio(r.espacio.idEspacio);
                var dist = ObtenerDistrito(esp.distrito.idDistrito);

                return new
                {
                    NumReserva = r.numReserva,
                    IniString = r.iniString,
                    FinString = r.finString,
                    FechaReserva = r.fechaReserva,
                    PersonaId = r.persona.idPersona,
                    NombreEspacio = esp.nombre,
                    NombreDistrito = dist.nombre,
                };
            }).ToList();

            gvReservas.DataSource = reservasDTO;
            gvReservas.DataBind();

            lblPagina.Text = $"Página {paginaActual} de {totalPaginas}";
            btnAnterior.Enabled = paginaActual > 1;
            btnSiguiente.Enabled = paginaActual < totalPaginas;
        }

        protected void ddlFiltros_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlFiltros.SelectedValue == "fecha" || ddlFiltros.SelectedValue == "horario")
            {
                txtFecha.Visible = true;
                input_busqueda.Disabled = true;
                ddlDistritos.Visible = false;
                input_busqueda.Attributes["class"] = "input-busqueda deshabilitado";
            }
            else if (ddlFiltros.SelectedValue == "distrito")
            {
                txtFecha.Visible = false;
                input_busqueda.Disabled = false;
                ddlDistritos.Visible = true;
                input_busqueda.Attributes["class"] = "input-busqueda deshabilitado";
            }
            else
            {
                txtFecha.Visible = false;
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
                List<reserva> filtradas = new List<reserva>();

                if ((filtro == "codigo" || filtro == "espacio" || filtro == "persona") && int.TryParse(textoBusqueda, out int codigo))
                {
                    if (filtro == "codigo")
                    {
                        var res = client.obtenerPorNumReserva(codigo, activo);
                        if (res != null) filtradas.Add(res);
                    }
                    else if (filtro == "espacio")
                    {
                        filtradas = client.listarReservaPorEspacio(codigo, activo).ToList();
                    }
                    else if (filtro == "persona")
                    {
                        filtradas = client.listarReservaPorPersona(codigo, activo).ToList();
                    }
                }
                else if (filtro == "fecha" && !string.IsNullOrWhiteSpace(txtFecha.Text))
                {
                    if (DateTime.TryParseExact(txtFecha.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime fecha))
                    {
                        string fechaStr = fecha.ToString("yyyy-MM-dd");
                        filtradas = client.listarPorFecha(fechaStr, activo).ToList();
                    }
                }
                else if (filtro == "distrito" && ddlDistritos.SelectedValue != "")
                {
                    int idDistrito = int.Parse(ddlDistritos.SelectedValue);
                    filtradas = client.listarReservaPorDistrito(idDistrito, activo).ToList();
                }
                else
                {
                    var todas = client.listarReservas().ToList();
                    filtradas = activo ? todas.Where(r => (char)r.activo == 'A').ToList() : todas;
                }

                Reservas = filtradas;
                ViewState["PaginaActual"] = 1;
                cacheEspacios.Clear();
                cacheDistritos.Clear();
                CargarPagina();
            }
            catch
            {
                Reservas = new List<reserva>();
                CargarPagina();
            }
        }
    }
}

