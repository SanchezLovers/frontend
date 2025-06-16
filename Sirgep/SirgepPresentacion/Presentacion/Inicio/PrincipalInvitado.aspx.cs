using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SirgepPresentacion.Presentacion.Inicio
{
    public partial class PrincipalInvitado : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnReservar_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Funcionalidad no implementada aún...');", true);
        }

        protected void btnVerEventos_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Presentacion/Ubicacion/Distrito/EligeDistrito.aspx");
        }
    }
}