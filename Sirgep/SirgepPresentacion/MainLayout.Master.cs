using System;
using System.Web.UI;

namespace SirgepPresentacion
{
    public partial class MainLayout : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Se usa el ToLower para evitar problemas con mayúsculas y minúsculas en la comparación
            string page = System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToLower();

            if (page == "principalinvitado.aspx")
            {
                liAdminMenu.Visible = false;
                liIngresar.Visible = true;
                liUsuarioMenu.Visible = false;
            }
            else if (page == "paginainicial.aspx")
            {
                liAdminMenu.Visible = false;
                liIngresar.Visible = false;
                liUsuarioMenu.Visible = false;
            }
            else
            {
                liAdminMenu.Visible = true;
                liIngresar.Visible = false;
                liUsuarioMenu.Visible = false;
            }
        }

        protected void btnIngresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/PaginaInicial.aspx");
        }
    }
}