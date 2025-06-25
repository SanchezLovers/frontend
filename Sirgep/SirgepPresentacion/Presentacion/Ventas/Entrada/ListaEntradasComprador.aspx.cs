using SirgepPresentacion.ReferenciaDisco;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI.WebControls;

namespace SirgepPresentacion.Presentacion.Ventas.Entrada
{
    public partial class ListaEntradasComprador : System.Web.UI.Page
    {
        private EntradaWSClient entradaWS;
        protected void Page_Load(object sender, EventArgs e)
        {
            entradaWS = new EntradaWSClient();
            CargarDatos();
        }
        protected void GvListaEntradasComprador_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GvListaEntradasComprador.PageIndex = e.NewPageIndex;
            CargarDatos();
            //GvListaEntradasComprador.DataBind();
        }
        protected void CargarDatos()
        {
            int idComprador = int.Parse(Session["idUsuario"].ToString());
            //int idComprador = 3;
            GvListaEntradasComprador.DataSource = entradaWS.listarDetalleEntradasPorComprador(idComprador);
            detalleEntradaDTO detalleEntradaDTO = new detalleEntradaDTO();
            GvListaEntradasComprador.DataBind();
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