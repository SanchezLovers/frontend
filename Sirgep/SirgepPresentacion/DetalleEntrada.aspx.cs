using System;
using SirgepPresentacion;

namespace SirgepPresentacion
{
    public partial class DetalleEntrada : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblEvento.Text = Session["eventoNombre"]?.ToString();
                lblUbicacion.Text = Session["eventoUbicacion"]?.ToString();
                lblHorario.Text = Session["eventoHorario"]?.ToString();
                lblFecha.Text = Session["eventoFecha"]?.ToString();

                lblNombres.Text = Session["compradorNombres"]?.ToString();
                lblApellidos.Text = Session["compradorApellidos"]?.ToString();
                lblDNI.Text = Session["compradorDNI"]?.ToString();
                lblTelefono.Text = Session["compradorTelefono"]?.ToString();
                lblCorreo.Text = Session["compradorCorreo"]?.ToString();

                lblMetodoPago.Text = Session["metodoPago"]?.ToString();
                lblNumeroPago.Text = Session["numeroPago"]?.ToString();
            }
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            // Redirigir a la página de inicio
            Response.Redirect("PrincipalInvitado.aspx");
        }
    }
}
