﻿using SirgepPresentacion.ReferenciaDisco;
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
            int id = resultado / 10;
            int tipo = resultado % 10;
            string script;
            switch (tipo)
            {
                case 0:
                    script = "setTimeout(function(){ mostrarModalError('Credenciales fallidas.','Las credenciales no son correctas. Intente nuevamente.'); }, 300);";
                    ClientScript.RegisterStartupScript(this.GetType(), "mostrarModalError", script, true);
                    break;
                case 1:
                    // Usuario es administrador
                    Session["tipoUsuario"] = "administrador";
                    Session["idUsuario"] = id;
                    Response.Redirect("/Presentacion/Inicio/PrincipalAdministrador.aspx");
                    break;
                case 2:
                    // Usuario es cliente
                    Session["tipoUsuario"] = "comprador";
                    Session["idUsuario"] = id;
                    Response.Redirect("/Presentacion/Inicio/PrincipalInvitado.aspx");
                    break;
                case -1:
                    // Credenciales no pertenecen a ninguna cuenta
                    script = "setTimeout(function(){ mostrarModalError('Credenciales inexistentes.','Las credenciales no pertenecen a ninguna cuenta. Intente nuevamente.'); }, 300);";
                    ClientScript.RegisterStartupScript(this.GetType(), "mostrarModalError", script, true);
                    break;
                default:
                    // Error desconocido
                    script = "setTimeout(function(){ mostrarModalError('Error Desconocido.','Intente nuevamente.'); }, 300);";
                    ClientScript.RegisterStartupScript(this.GetType(), "mostrarModalError", script, true);
                    break;
            }
        }
    }
}