using SirgepPresentacion.ReferenciaDisco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SirgepPresentacion.Presentacion.Ventas.Entrada
{
    public partial class CompraEntrada : System.Web.UI.Page
    {
        CompraWSClient compraService;
        FuncionWSClient fWs;
        EntradaWSClient entradaWS;
        PersonaWSClient personaWS;
        EventoWSClient eventoWS;
        protected void Page_Init(object sender, EventArgs e)
        {
            compraService = new CompraWSClient();
            fWs = new FuncionWSClient();
            entradaWS = new EntradaWSClient();
            personaWS = new PersonaWSClient();
            eventoWS = new EventoWSClient();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //if (Request.QueryString["idEvento"] != null)
                //{
                //int idEntrada = int.Parse((sender as Button).CommandArgument);

                   
                //int idFuncion = 1; // Simular ID elegido
                int idFuncion= int.Parse(Request.QueryString["idFuncion"]);
                var funcion = fWs.buscarFuncionId(idFuncion); // Simular ID de función
                evento evento = compraService.buscarEventos(funcion.evento.idEvento);

                lblEvento.Text = evento.nombre;
                lblUbicacion.Text = evento.ubicacion;
                lblReferencia.Text = evento.referencia;

                // Obtener parámetros por URL
                //string fechaStr = Request.QueryString["fecha"];
                //string horaIni = Request.QueryString["horaInicio"];
                //string horaFin = Request.QueryString["horaFin"];
                //string cantidadStr = Request.QueryString["cantidad"];

                


                string fecha = funcion.fecha.ToString();
                string horaIni = funcion.horaInicio.ToString();
                string horaFin = funcion.horaFin.ToString();
                string cantidad = "1"; //siempre es 1


                lblHorario.Text = $"{horaIni} - {horaFin}";
                lblFecha.Text = DateTime.Parse(fecha).ToString("dd/MM/yyyy");
                lblCantidad.Text = cantidad;
                lblTotal.Text = evento.precioEntrada.ToString();

                // Guardar en ViewState si lo vas a usar en el botón pagar
                //ViewState["IdEvento"] = idEvento;
                //ViewState["Fecha"] = fechaStr;
                //ViewState["HoraInicio"] = horaIni;
                //ViewState["HoraFin"] = horaFin;
                //ViewState["Cantidad"] = cantidadStr;
                //}
            }
        }

        protected void btnPagar_Click(object sender, EventArgs e)
        {
            // Validar campos obligatorios
            //aqui el if
            if (string.IsNullOrWhiteSpace(txtNombres.Text) ||
                string.IsNullOrWhiteSpace(txtApellidoPaterno.Text) ||
                string.IsNullOrWhiteSpace(txtApellidoMaterno.Text) ||
                string.IsNullOrWhiteSpace(txtDNI.Text) ||
                 string.IsNullOrWhiteSpace(txtCorreo.Text))
            {
                //ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Por favor, complete todos los campos.');", true);
                //return;
                string script = "setTimeout(function(){ mostrarModalError('Campos faltantes.','Por favor, complete todos los campos obligatorios.'); }, 300);";
                ScriptManager.RegisterStartupScript(this, GetType(), "mostrarModalError", script, true);
                return;

            }

            // Validar método de pago
            if (string.IsNullOrEmpty(hfMetodoPago.Value))
            {
                //ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Debe seleccionar un método de pago.');", true);
                //return;
                string script = "setTimeout(function(){ mostrarModalError('Método de pago faltante.','Debe seleccionar un método de pago.'); }, 300);";
                ScriptManager.RegisterStartupScript(this, GetType(), "mostrarModalError", script, true);
                return;
            }





            string dni = txtDNI.Text.Trim();
            var compradorExistente = compraService.buscarCompradorPorDni(dni);
            int idPersona1;
            // ---------- Datos que necesitas ----------
            //int cantidad = int.Parse(lblCantidad.Text);       // <— corrección
            int cantidad = 1;


            double precio = compraService.buscarEventos(1).precioEntrada;
            double totalAPagar = precio;

            // ---------- Comprobación de saldo ----------
            if (compradorExistente != null && compradorExistente.monto < totalAPagar)
            {
                //ScriptManager.RegisterStartupScript(this, GetType(), "alert",
                //  "alert('Saldo insuficiente.');", true);
                //return;
                string script = "setTimeout(function(){ mostrarModalError('Error en pago.','Saldo insuficiente.'); }, 300);";
                ScriptManager.RegisterStartupScript(this, GetType(), "mostrarModalError", script, true);
                return;
            }

            // ---------- Insertar / actualizar comprador ----------
            if (compradorExistente == null)
            {
                comprador nuevo = new comprador
                {
                    nombres = txtNombres.Text.Trim(),
                    primerApellido = txtApellidoPaterno.Text.Trim(),
                    segundoApellido = txtApellidoMaterno.Text.Trim(),
                    numDocumento = txtDNI.Text.Trim(),
                    correo = txtCorreo.Text.Trim(),
                    tipoDocumento = eTipoDocumento.DNI,
                    tipoDocumentoSpecified = true,
                    registrado = 0,
                };
                idPersona1 = compraService.insertarComprador(nuevo);
                //idPersona1 = nuevo.idPersona; // Obtener el ID del nuevo comprador
            }
            else
            {
                compradorExistente.monto -= totalAPagar;
                compraService.actualizarComprador(compradorExistente);
                idPersona1 = compradorExistente.idPersona;
            }





            ReferenciaDisco.eMetodoPago mp;

            string metodoPagoSeleccionado = hfMetodoPago.Value;

            bool ok = Enum.TryParse(metodoPagoSeleccionado, ignoreCase: false, out mp);

            if (!ok)
            {
                string script = "setTimeout(function(){ mostrarModalError('Método de pago incorrecto.','Método de pago desconocido.'); }, 300);";
                ScriptManager.RegisterStartupScript(this, GetType(), "mostrarModalError", script, true);
                return;
            }
            //int idFuncion = 1; // Simular ID elegido
            int idFuncion= int.Parse(Request.QueryString["idFuncion"]);
            var funcion = fWs.buscarFuncionId(idFuncion); // Simular ID de función
            evento evento = compraService.buscarEventos(funcion.evento.idEvento);
            int numE = evento.cantEntradasVendidas+1;
            evento.cantEntradasVendidas= numE;
            // Actualizar el evento con la nueva cantidad de entradas vendidas
            //evento.canti
            eventoWS.actualizarEvento(evento);
            // ---------- Insertar constancia ----------

            entrada nEntrada = new entrada
            {
                numEntrada = numE,
                fecha = DateTime.Now,
                fechaSpecified = true,
                metodoPago = mp,
                metodoPagoSpecified = true,
                detallePago = $"Pago realizado por {txtNombres.Text.Trim()} {txtApellidoPaterno.Text.Trim()} con DNI {dni}",//aqui
                total = totalAPagar,
                igv = 0.18,
                persona = new persona
                {
                    idPersona = idPersona1,
                },
                funcion = new funcion
                {
                    idFuncion = int.Parse(Request.QueryString["idFuncion"]),
                    //idFuncion = 1, // Simular ID de función
                },
            };

            //int idConstancia=compraService.insertarConstancia(nueva);
            int idEntrada = entradaWS.insertarEntrada(nEntrada);
            string scriptExito = "setTimeout(function(){ mostrarModalExito('Pago exitoso.','El pago se ha realizado con éxito.'); }, 300);";

            ScriptManager.RegisterStartupScript(this, GetType(), "mostrarModalExito", scriptExito, true);
            Response.Redirect("/Presentacion/Ventas/Entrada/ConstanciaEntrada.aspx?idEntrada=" + idEntrada);
        }
    }
}