using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SirgepPresentacion
{
    public partial class PrincipalInvitado : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["tipoUsuario"] = "invitado"; // Asignar el tipo de usuario como invitado
        }

        protected void btnReservar_Click(object sender, EventArgs e)
        {

        }

        protected void btnVerEventos_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Compras/EleccionDistrito.aspx");
        }
    }
}