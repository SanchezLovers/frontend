using SirgepPresentacion.ReferenciaDisco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SirgepPresentacion.Presentacion.Inicio
{
    public partial class LogIn : System.Web.UI.Page
    {
        PersonaWSClient personaWS = new PersonaWSClient();
        public LogIn()
        {
            personaWS = new PersonaWSClient();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["tipoUsuario"] = "invitado"; // Asignar tipo de usuario por defecto
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            // Validar que datos no esten vacios
            if (string.IsNullOrWhiteSpace(txtEmail.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Por favor, complete todos los campos.');", true);//Cambiar por modal de error
                return;
            }
            string correo = txtEmail.Text.Trim();
            string contrasena = txtPassword.Text.Trim();
            int resultado = personaWS.validarCuenta(correo,contrasena);
            if (resultado>0)
            {
                //Session["idUsuario"] = personaWS.obtenerIdPorCorreo(correo);
            }
            switch (resultado)
            {
                case 0:
                    // Usuario no existe
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Credenciales fallidas.');", true);//Cambiar por modal de error
                    break;
                case 1:
                    // Usuario es administrador
                    Session["tipoUsuario"] = "administrador";
                    Response.Redirect("/Presentacion/Inicio/PrincipalAdministrador.aspx");
                    break;
                case 2:
                    // Usuario es cliente
                    Session["tipoUsuario"] = "comprador";
                    Response.Redirect("/Presentacion/Inicio/PrincipalInvitado.aspx");
                    break;
                case -1:
                    // Credenciales no pertenecen a ninguna cuenta
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Credenciales no pertenecen a ninguna cuenta.');", true);//Cambiar por modal de error
                    break;
                default:
                    // Error desconocido
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Error desconocido.');", true);//Cambiar por modal de error
                    break;
            }
        }
    }
}