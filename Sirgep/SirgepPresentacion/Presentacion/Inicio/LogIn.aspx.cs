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
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["tipoUsuario"] = "invitado"; // Asignar tipo de usuario por defecto
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Presentacion/Inicio/PrincipalAdministrador.aspx");
        }
    }
}