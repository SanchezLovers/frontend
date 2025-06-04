using System;
using System.Web.UI;

namespace SirgepPresentacion
{
    public partial class PrincipalAdministrador : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Lógica al cargar la página (si es necesario)
            Session["tipoUsuario"] = "administrador";
        }

        protected void btnManejarEspacios_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Usuario/Admin/ListaEspacios.aspx");
        }

        protected void btnConsultarReservas_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Usuario/Admin/ListaReservas.aspx"); // Ajusta la ruta si es diferente
        }

        protected void btnManejarEventos_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Usuario/Admin/ListaEventos.aspx");
        }

        protected void btnConsultarEntradas_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Usuario/Admin/ListaEntradas.aspx"); // Ajusta la ruta si es diferente
        }
    }
}
