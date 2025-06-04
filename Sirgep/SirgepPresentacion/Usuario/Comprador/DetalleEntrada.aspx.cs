using System;

namespace SirgepPresentacion.Usuario.Comprador
{
    public partial class DetalleEntrada : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Datos del evento
                lblNumEntrada.Text = "001";
                lblEvento.Text = "Concierto de Rock Clásico";
                lblUbicacion.Text = "Estadio Nacional";
                lblHorario.Text = "19:00 - 22:00";
                lblFecha.Text = "15/07/2025";

                // Datos del comprador
                lblNombres.Text = "Jorge";
                lblApellidos.Text = "Pérez Torres";
                lblDNI.Text = "12345678";
                lblTelefono.Text = "987654321";
                lblCorreo.Text = "jorge.perez@example.com";

                // Datos del pago
                lblMetodoPago.Text = "Tarjeta de crédito";
                lblNumeroPago.Text = "PAY-000123456";
            }
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect("../../Inicio/PrincipalInvitado.aspx");
        }

        protected void btnDescargar_Click(object sender, EventArgs e)
        {
            // Mostrar el modal (popup)
            string script = "mostrarModal();";
            ClientScript.RegisterStartupScript(this.GetType(), "mostrarModal", script, true);
        }
    }
}