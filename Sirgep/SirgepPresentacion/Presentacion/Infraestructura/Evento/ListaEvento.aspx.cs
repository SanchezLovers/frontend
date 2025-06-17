using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using SirgepPresentacion.ReferenciaDisco;

namespace SirgepPresentacion.Presentacion.Infraestructura.Evento
{
    public partial class ListaEvento : System.Web.UI.Page
    {
        private const int EventosPorPagina = 20;
        EventoWSClient cliente = new EventoWSClient();
        protected void Page_Load(object sender, EventArgs e)
        {
            // Verifica si es la primera vez que se carga la página
            if (!IsPostBack)
            {
                //Aqui se coloca desde que pagina empezar a ver el listado de eventos
                ViewState["PaginaActual"] = 1;
                lblDistrito.Text = Request.QueryString["nombreDistrito"];
                CargarEventos();
            }
        }

        private List<evento> obtenertodosloseventos()
        {
            int.TryParse(Request.QueryString["idDistrito"], out int idDistrito);
            try
            {
                //Esto es para convertir en una lista
                List<evento> todosLosEventos = cliente.listarEventoPorDistrito(idDistrito).ToList();
                return todosLosEventos;
            }
            catch (ArgumentNullException)
            {
                // Si ocurre ArgumentNullException, retorna una lista vacía
                return new List<evento>();
            }
        }

        // Carga los eventos en el Repeater con paginación
        private void CargarEventos()
        {
            int paginaActual = (int)(ViewState["PaginaActual"] ?? 1);
            //Esto es para convertir en una lista
            int.TryParse(Request.QueryString["idDistrito"], out int idDistrito);

            List<evento> todosLosEventos = obtenertodosloseventos();

            int totalPaginas = (int)Math.Ceiling((double)todosLosEventos.Count / EventosPorPagina);

            var eventosPagina = todosLosEventos
            .Skip((paginaActual - 1) * EventosPorPagina)
            .Take(EventosPorPagina)
            .ToList();

            rptEventos.DataSource = eventosPagina;
            rptEventos.DataBind();

            // Actualiza controles de paginación
            lblPagina.Text = $"Página {paginaActual} de {totalPaginas}";
            btnAnterior.Enabled = paginaActual > 1;
            btnSiguiente.Enabled = paginaActual < totalPaginas;
        }

        //Es el boton de paginación para ir a la página anterior y siguiente
        protected void btnAnterior_Click(object sender, EventArgs e)
        {
            int paginaActual = (int)(ViewState["PaginaActual"] ?? 1);
            if (paginaActual > 1)
                ViewState["PaginaActual"] = paginaActual - 1;
            CargarEventos();
        }

        protected void btnSiguiente_Click(object sender, EventArgs e)
        {
            int paginaActual = (int)(ViewState["PaginaActual"] ?? 1);
            int totalPaginas = (int)Math.Ceiling((double)obtenertodosloseventos().Count / EventosPorPagina);
            if (paginaActual < totalPaginas)
                ViewState["PaginaActual"] = paginaActual + 1;
            CargarEventos();
        }


        //Este seria el boton de compra de eventos que deberia de dirigir a la pantalla de compra
        protected void btnComprar_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Presentacion/Infraestructura/Evento/DetalleEvento.aspx?idEvento=" + ((Button)sender).CommandArgument);
        }

        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Presentacion/Ubicacion/Distrito/EligeDistrito.aspx");
        }
    }
}