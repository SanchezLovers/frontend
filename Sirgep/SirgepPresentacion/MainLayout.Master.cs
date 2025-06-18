using SirgepPresentacion.ReferenciaDisco;
using System;
using System.Web.UI;

namespace SirgepPresentacion
{
    public partial class MainLayout : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string tipoUsuario = Session["tipoUsuario"] as string;

            if (tipoUsuario == null)
            {
                // No ha iniciado sesión, es invitado por defecto
                tipoUsuario = "invitado";
                Session["tipoUsuario"] = "invitado"; // puedes asignarlo si deseas
            }

            switch (tipoUsuario.ToLower())
            {
                case "administrador":
                    liAdminMenu.Visible = true;
                    liUsuarioMenu.Visible = false;
                    liIngresar.Visible = false;
                    break;

                case "comprador":
                    liAdminMenu.Visible = false;
                    liUsuarioMenu.Visible = true;
                    liIngresar.Visible = false;
                    break;

                case "invitado":
                    liAdminMenu.Visible = false;
                    liUsuarioMenu.Visible = false;
                    liIngresar.Visible = true;
                    string page = System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToLower();
                    if (page == "login.aspx")
                        liIngresar.Visible = false; // Si estás en la página de invitado, no mostrar el botón de ingresar

                    break;
                default:
                    liAdminMenu.Visible = false;
                    liUsuarioMenu.Visible = false;
                    liIngresar.Visible = false;
                    break;
            }
            hdnTipoUsuario.Value = Session["tipoUsuario"] as string ?? "invitado";
        }

        protected void btnIngresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Presentacion/Inicio/LogIn.aspx");
        }

        protected void lnkLogo_Click(object sender, EventArgs e)
        {
            redirigirInicio();
        }

        protected void redirigirInicio()
        {
            string tipoUsuario = Session["tipoUsuario"] as string;

            switch (tipoUsuario.ToLower())
            {
                case "administrador":
                    Response.Redirect("/Presentacion/Inicio/PrincipalAdministrador.aspx");
                    break;

                case "comprador":
                    Response.Redirect("/Presentacion/Inicio/PrincipalInvitado.aspx");
                    break;

                case "invitado":
                    Response.Redirect("/Presentacion/Inicio/PrincipalInvitado.aspx");
                    break;
                default:
                    Console.WriteLine("Tipo de usuario no reconocido: " + tipoUsuario);
                    break;
            }
        }

        protected void lnkPerfil_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Presentacion/Usuario/Perfil.aspx");
        }

        protected void lnkReservasComprador_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Presentacion/Ventas/Reserva/ListaReservasComprador.aspx");
        }

        protected void lnkEntradasComprador_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Presentacion/Ventas/Entrada/ListaEntradasComprador.aspx");
        }

        protected void lnkCerrarSesion_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Response.Redirect("/Presentacion/Inicio/PrincipalInvitado.aspx");
        }

        protected void lnkEspaciosAdmin_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Presentacion/Usuario/Admin/ListaEspacios.aspx");
        }

        protected void lnkEventosAdmin_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Presentacion/Usuario/Admin/ListaEventos.aspx");
        }
        protected void btnEnviarFeedback_Click(object sender, EventArgs e)
        {
            string comentario = txtComentarioFeedback.Text.Trim();
            int puntaje = ObtenerPuntajeFeedback();

            // Llamar al servicio web CalificacionWSCliente
            CalificacionWSClient calificacionService = new CalificacionWSClient();
            string tipoServicio = Session["tipoServicio"] as string;
            calificacionService.calificarServicio(puntaje, comentario, tipoServicio);

            // Cerrar el modal (opcional, si quieres mostrar el cierre visual)
            string script = "var modal = bootstrap.Modal.getOrCreateInstance(document.getElementById('modalFeedback')); modal.hide();";
            ScriptManager.RegisterStartupScript(this, GetType(), "cerrarModalFeedback", script, true);

            Response.Redirect("/Presentacion/Inicio/PrincipalInvitado.aspx");
        }

        private int ObtenerPuntajeFeedback()
        {
            // Leer el valor del HiddenField directamente
            int puntaje = 0;
            if (int.TryParse(calificacionFeedback.Value, out puntaje))
            {
                return puntaje;
            }
            return 0; // Valor por defecto si no se seleccionó nada
        }
    }
}