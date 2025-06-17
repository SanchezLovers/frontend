using System;

namespace SirgepPresentacion.Presentacion.Inicio
{
    public partial class PrincipalInvitado : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["tipoUsuario"] = "invitado"; // Asignar el tipo de usuario como invitado
        }

        protected void btnReservar_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Presentacion/Ventas/Reserva/FormularioEspacio.aspx");
        }

        protected void btnVerEventos_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Presentacion/Ubicacion/Distrito/EligeDistrito.aspx");
        }
    }
}