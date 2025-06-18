using System;
using System.Web.UI;

namespace SirgepPresentacion.Presentacion.Infraestructura.Evento
{
    public partial class GestionaEventos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ddlFiltroFechas_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnEditar_Click(object sender, EventArgs e)
        {

        }

        protected void btnMostrarModalAgregarEvento_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "abrirModalEvento",
        "var modalEvento = new bootstrap.Modal(document.getElementById('modalAgregarEvento')); modalEvento.show();", true);
        }
    }
}