using System;
using System.Web.UI.WebControls;
using SirgepPresentacion.ReferenciaDisco;

namespace SirgepPresentacion.Usuario.Comprador
{
    public partial class DetalleEntrada : System.Web.UI.Page
    {
        private EntradaWSClient entradaWS;
        //private CompradorWSClient compradorWS;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                entradaWS = new EntradaWSClient();
                int idEntrada = 1;// int.Parse((sender as Button).CommandArgument);
                entrada entradaDomain = entradaWS.buscarEntrada(idEntrada);
                System.Console.WriteLine(entradaDomain.persona.idPersona);
                comprador compradorDomain = entradaWS.buscarCompradorDeEntrada(entradaDomain.persona.idPersona);
                funcion funcionDomain = entradaWS.buscarFuncionDeEntrada(entradaDomain.funcion.idFuncion);
                evento eventoDomain = entradaWS.buscarEventoDeEntrada(funcionDomain.evento.idEvento);
                distrito distritoDomain = entradaWS.buscarDistritoDeEntrada(eventoDomain.distrito.idDistrito);

                // Datos de la entrada
                lblNumEntrada.Text = entradaDomain.numEntrada.ToString();
                // Datos del evento
                lblEvento.Text = eventoDomain.nombre;
                lblUbicacion.Text = eventoDomain.ubicacion;
                lblReferencias.Text = eventoDomain.referencia;
                lblDistrito.Text = distritoDomain.nombre;
                // Datos de la funcion
                lblFechaFuncion.Text = funcionDomain.fecha.ToString("dd/MM/yyyy");
                lblHoraInicio.Text = funcionDomain.horaInicio.Hour.ToString("00") + ":" + funcionDomain.horaInicio.Minute.ToString("00");
                lblHoraFin.Text = funcionDomain.horaFin.Hour.ToString("00") + ":" + funcionDomain.horaFin.Minute.ToString("00");
                //Datos del comprador
                lblNombres.Text = compradorDomain.nombres;
                lblApellidos.Text = compradorDomain.primerApellido + " " + compradorDomain.segundoApellido;
                lblTipoDocumento.Text = compradorDomain.tipoDocumento.ToString();
                lblTNumDocumento.Text = compradorDomain.numDocumento.ToString();
                lblCorreo.Text = compradorDomain.correo;
                // Datos de la constancia del pago
                lblFechaConstancia.Text = entradaDomain.fecha.ToString("dd/MM/yyyy");
                lblMetodoPago.Text = entradaDomain.metodoPago.ToString();
                lblDetallePago.Text = entradaDomain.detallePago.ToString();
                //lblPrecio.Text = eventoDomain.precioEntrada.ToString("C2");
                lblTotal.Text = "S/. "+entradaDomain.total.ToString();
            }
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            // Mostrar el modal de feedback
            string script = "mostrarModalFeedback();";
            ClientScript.RegisterStartupScript(this.GetType(), "mostrarModalFeedback", script, true);
        }

        protected void btnDescargar_Click(object sender, EventArgs e)
        {
            // Mostrar el modal (popup)
            string script = "mostrarModal();";
            ClientScript.RegisterStartupScript(this.GetType(), "mostrarModal", script, true);
        }
    }
}