using System;
using System.Web.UI;

namespace SirgepPresentacion.Presentacion.Inicio
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
            Response.Redirect("/Presentacion/Infraestructura/Espacio/ListaEspacios.aspx");
        }

        protected void btnConsultarReservas_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Presentacion/Ventas/Reserva/ListaReservasAdministrador.aspx");
        }

        protected void btnManejarEventos_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Presentacion/Infraestructura/Evento/GestionaEventos.aspx");
        }

        protected void btnConsultarEntradas_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Presentacion/Ventas/Entrada/ListaEntradas.aspx"); // Ajusta la ruta si es diferente
        }
    }
}
