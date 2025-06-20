using System;
using System.Web.UI.WebControls;
using SirgepPresentacion.ReferenciaDisco;

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
            int idComprador = 2;
            GvListaEntradasComprador.DataSource = entradaWS.listarDetalleEntradasPorComprador(idComprador);
            detalleEntradaDTO detalleEntradaDTO = new detalleEntradaDTO();

            GvListaEntradasComprador.DataBind();
        }
        protected void btnDescargar_Click(object sender, EventArgs e)
        {
            int idComprador = 2;
            entradaWS.crearLibroExcelEntradas(idComprador);
        }

        protected void BtnAbrir_Click(object sender, EventArgs e)
        {
            //LinkButton btn = (LinkButton)sender;
            //string numEntrada = btn.CommandArgument;
            string numEntrada = "2";
            Response.Redirect("/Presentacion/Ventas/Entrada/ConstanciaEntrada.aspx?NumEntrada=" + numEntrada);
        }
    }
}