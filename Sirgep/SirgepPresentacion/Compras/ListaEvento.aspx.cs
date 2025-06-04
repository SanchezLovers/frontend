using SirgepPresentacion.ReferenciaDisco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SirgepPresentacion.Compras
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
                //CargarRegiones();
                //CargarProvincias();
                //CargarDistritos();
                CargarEventos();
            }
        }

        //Los metodos con readonly son para poder usar el dropdownlist de regiones, provincias y distritos (hardcodeados)
        // Datos de prueba para regiones
        private static readonly List<ListItem> Regiones = new List<ListItem>
            {
                new ListItem("Lima", "1"),
                new ListItem("Callao", "2")
            };

        // Provincias por región
        private static readonly Dictionary<string, List<ListItem>> ProvinciasPorRegion = new Dictionary<string, List<ListItem>>
            {
                { "1", new List<ListItem> { new ListItem("Lima", "1"), new ListItem("Huaral", "2") } }, // Región Lima
                { "2", new List<ListItem> { new ListItem("Callao", "3") } } // Región Callao
            };

        // Distritos por provincia
        private static readonly Dictionary<string, List<ListItem>> DistritosPorProvincia = new Dictionary<string, List<ListItem>>
            {
                { "1", new List<ListItem> { new ListItem("Pueblo Libre", "1"), new ListItem("Miraflores", "2") } }, // Provincia Lima
                { "2", new List<ListItem> { new ListItem("Huaral Centro", "3") } }, // Provincia Huaral
                { "3", new List<ListItem> { new ListItem("Bellavista", "4") } } // Provincia Callao
            };

        //Los carga son para los dropdownlist de regiones, provincias y distritos
        //private void CargarRegiones()
        //{
        //    ddlRegion.DataSource = Regiones;
        //    ddlRegion.DataTextField = "Text";
        //    ddlRegion.DataValueField = "Value";
        //    ddlRegion.DataBind();
        //    ddlRegion.Items.Insert(0, new ListItem("-- Seleccione Región --", ""));
        //}

        //private void CargarProvincias()
        //{
        //    var regionId = ddlRegion.SelectedValue;
        //    ddlProvincia.Items.Clear();

        //    if (!string.IsNullOrEmpty(regionId) && ProvinciasPorRegion.ContainsKey(regionId))
        //    {
        //        ddlProvincia.DataSource = ProvinciasPorRegion[regionId];
        //        ddlProvincia.DataTextField = "Text";
        //        ddlProvincia.DataValueField = "Value";
        //        ddlProvincia.DataBind();
        //    }
        //    ddlProvincia.Items.Insert(0, new ListItem("-- Seleccione Provincia --", ""));
        //}

        //private void CargarDistritos()
        //{
        //    var provinciaId = ddlProvincia.SelectedValue;
        //    ddlDistrito.Items.Clear();

        //    if (!string.IsNullOrEmpty(provinciaId) && DistritosPorProvincia.ContainsKey(provinciaId))
        //    {
        //        ddlDistrito.DataSource = DistritosPorProvincia[provinciaId];
        //        ddlDistrito.DataTextField = "Text";
        //        ddlDistrito.DataValueField = "Value";
        //        ddlDistrito.DataBind();
        //    }
        //    ddlDistrito.Items.Insert(0, new ListItem("-- Seleccione Distrito --", ""));
        //}

        //este metodo es para obtener todos los eventos(hardcode), pero se ha comentado porque no se usa en este momento
        //puedes comentar usando las teclas rapidas ctrl+k+c y descomentas con ctrl+k+u
        private List<dynamic> obtenertodosloseventos()
        {
            return new List<dynamic>
                {
                    // simula una lista de 21 eventos
                    new { imagenurl = "/images/eventos/parqueelcarmen.jpg", categoria = "carrera", tipo = "deporte", nombre = "carrera familiar 3k", lugar = "parque el carmen, pueblo libre", descripcionpublico = "para niños y adultos", id = 1 },
                    new { imagenurl = "/images/eventos/voley.jpg", categoria = "juego", tipo = "deporte", nombre = "vóley femenino", lugar = "parque de la exposición, pueblo libre", descripcionpublico = "para jóvenes", id = 2 },
                    new { imagenurl = "/images/eventos/triathlon.jpg", categoria = "categoría", tipo = "deporte", nombre = "triathlon verano 2025", lugar = "circuito de playas, chorrillos", descripcionpublico = "deportistas", id = 3 },
                    new { imagenurl = "/images/eventos/ajedrez.jpg", categoria = "torneo", tipo = "deporte", nombre = "torneo de ajedrez", lugar = "centro cultural, lima", descripcionpublico = "para todas las edades", id = 4 },
                    new { imagenurl = "/images/eventos/baile.jpg", categoria = "baile", tipo = "cultural", nombre = "festival de baile", lugar = "plaza mayor, lima", descripcionpublico = "para familias", id = 5 },
                    new { imagenurl = "/images/eventos/teatro.jpg", categoria = "teatro", tipo = "cultural", nombre = "obra de teatro", lugar = "teatro municipal", descripcionpublico = "para adultos", id = 6 },
                    new { imagenurl = "/images/eventos/musica.jpg", categoria = "música", tipo = "cultural", nombre = "concierto de rock", lugar = "estadio nacional", descripcionpublico = "para jóvenes", id = 7 },
                    new { imagenurl = "/images/eventos/feria.jpg", categoria = "feria", tipo = "comercial", nombre = "feria gastronómica", lugar = "parque kennedy", descripcionpublico = "para todos", id = 8 },
                    new { imagenurl = "/images/eventos/ciclismo.jpg", categoria = "ciclismo", tipo = "deporte", nombre = "ciclismo urbano", lugar = "av. arequipa", descripcionpublico = "para ciclistas", id = 9 },
                    new { imagenurl = "/images/eventos/charla.jpg", categoria = "charla", tipo = "educativo", nombre = "charla de salud", lugar = "centro de salud", descripcionpublico = "para la comunidad", id = 10 },
                    new { imagenurl = "/images/eventos/charla.jpg", categoria = "charla", tipo = "educativo", nombre = "charla de salud", lugar = "centro de salud", descripcionpublico = "para la comunidad", id = 11 },
                    new { imagenurl = "/images/eventos/charla.jpg", categoria = "charla", tipo = "educativo", nombre = "charla de salud", lugar = "centro de salud", descripcionpublico = "para la comunidad", id = 12 },
                    new { imagenurl = "/images/eventos/charla.jpg", categoria = "charla", tipo = "educativo", nombre = "charla de salud", lugar = "centro de salud", descripcionpublico = "para la comunidad", id = 13 },
                    new { imagenurl = "/images/eventos/charla.jpg", categoria = "charla", tipo = "educativo", nombre = "charla de salud", lugar = "centro de salud", descripcionpublico = "para la comunidad", id = 14 },
                    new { imagenurl = "/images/eventos/charla.jpg", categoria = "charla", tipo = "educativo", nombre = "charla de salud", lugar = "centro de salud", descripcionpublico = "para la comunidad", id = 15 },
                    new { imagenurl = "/images/eventos/charla.jpg", categoria = "charla", tipo = "educativo", nombre = "charla de salud", lugar = "centro de salud", descripcionpublico = "para la comunidad", id = 16 },
                    new { imagenurl = "/images/eventos/charla.jpg", categoria = "charla", tipo = "educativo", nombre = "charla de salud", lugar = "centro de salud", descripcionpublico = "para la comunidad", id = 17 },
                    new { imagenurl = "/images/eventos/charla.jpg", categoria = "charla", tipo = "educativo", nombre = "charla de salud", lugar = "centro de salud", descripcionpublico = "para la comunidad", id = 18 },
                    new { imagenurl = "/images/eventos/charla.jpg", categoria = "charla", tipo = "educativo", nombre = "charla de salud", lugar = "centro de salud", descripcionpublico = "para la comunidad", id = 19 },
                    new { imagenurl = "/images/eventos/charla.jpg", categoria = "charla", tipo = "educativo", nombre = "charla de salud", lugar = "centro de salud", descripcionpublico = "para la comunidad", id = 20 },
                    new { imagenurl = "/images/eventos/charla.jpg", categoria = "charla", tipo = "educativo", nombre = "charla de salud", lugar = "centro de salud", descripcionpublico = "para la comunidad", id = 21 }

                };
            //guardamos el id del distrito seleccionado
            //if (int.tryparse(ddldistrito.selectedvalue, out int iddistrito))
            //{
            //    //se guarda el listado en un array para la conversion a una lista dinamica
            //    var eventosarray = cliente.listareventopordistrito(iddistrito);
            //    list<evento> eventospordistrito = eventosarray.tolist();
            //    return eventospordistrito;
            //}
            //else
            //{
            //    // retorna una lista vacía si no hay distrito seleccionado o el valor no es válido
            //    var eventosarray = cliente.listarevento();
            //    list<evento> eventos = eventosarray.tolist();
            //    return eventos;
            //}
        }

        // Carga los eventos en el Repeater con paginación
        private void CargarEventos()
        {
            int paginaActual = (int)(ViewState["PaginaActual"] ?? 1);
            //Esto es para convertir en una lista
            int.TryParse(Request.QueryString["idDistrito"], out int idDistrito);



            List<evento> todosLosEventos = cliente.listarEventoPorDistrito(idDistrito).ToList();
            //var todosLosEventos = obtenertodosloseventos();

            int totalPaginas = (int)Math.Ceiling((double)obtenertodosloseventos().Count / EventosPorPagina);

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
            var todosLosEventos = cliente.listarEvento().ToList();
            int totalPaginas = (int)Math.Ceiling((double)obtenertodosloseventos().Count / EventosPorPagina);
            if (paginaActual < totalPaginas)
                ViewState["PaginaActual"] = paginaActual + 1;
            CargarEventos();
        }

        // Métodos para manejar los cambios en los dropdowns de regiones, provincias y distritos
        protected void ddlRegion_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Si tienes provincias y distritos, llama a sus métodos de carga aquí
            //CargarProvincias();
            //CargarDistritos();
            CargarEventos();
        }

        protected void ddlProvincia_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Si tienes distritos, llama a su método de carga aquí
            //CargarDistritos();
            CargarEventos();
        }

        protected void ddlDistrito_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarEventos();
        }

        //Este seria el boton de compra de eventos que deberia de dirigir a la pantalla de compra
        protected void btnComprar_Click(object sender, EventArgs e)
        {
            // Aquí puedes agregar la lógica para procesar la compra del evento seleccionado.
            // Por ejemplo, podrías obtener el ID del evento con:
            // var btn = (Button)sender;
            // string idEvento = btn.CommandArgument;
        }

        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Compras/EleccionDistrito.aspx");
        }
    }
}