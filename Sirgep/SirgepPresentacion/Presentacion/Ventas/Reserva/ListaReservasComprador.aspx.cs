using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using SirgepPresentacion.ReferenciaDisco;

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
            int idComprador = 2;
            GvListaReservasComprador.DataSource = reservaWS.listarDetalleReservasPorComprador(idComprador);
            GvListaReservasComprador.DataBind();
        }

        protected void btnDescargar_Click(object sender, EventArgs e)
        {
            int idComprador = 2;
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
    }
}