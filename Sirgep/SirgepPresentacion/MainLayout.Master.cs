using System;

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
        }

        protected void btnIngresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Presentacion/Inicio/LogIn.aspx");
        }

        protected void lnkLogo_Click(object sender, EventArgs e)
        {
            string tipoUsuario = Session["tipoUsuario"] as string;

            switch (tipoUsuario.ToLower())
            {
                case "administrador":
                    Response.Redirect("/Presentacion/Inicio/PrincipalAdministrador.aspx");
                    break;

                case "comprador":
                    Response.Redirect("/Presentacion/Inicio/PrincipalComprador.aspx");
                    break;

                case "invitado":
                    Response.Redirect("/Presentacion/Inicio/PrincipalInvitado.aspx");
                    break;
                default:
                    Console.WriteLine("Tipo de usuario no reconocido: " + tipoUsuario);
                    break;
            }
        }

        protected void btnPerfil_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Presentacion/Usuarios/Perfil.aspx");
        }
    }
}