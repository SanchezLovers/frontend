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
            Response.Redirect("ListaEspacios.aspx");
        }

        protected void btnConsultarReservas_Click(object sender, EventArgs e)
        {
            Response.Redirect("ListaReservas.aspx"); // Ajusta la ruta si es diferente
        }

        protected void btnManejarEventos_Click(object sender, EventArgs e)
        {
            Response.Redirect("ListaEventos.aspx");
        }

        protected void btnConsultarEntradas_Click(object sender, EventArgs e)
        {
            Response.Redirect("ListaEntradas.aspx"); // Ajusta la ruta si es diferente
        }
    }
}
