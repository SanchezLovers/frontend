using SirgepPresentacion.ReferenciaDisco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SirgepPresentacion.Presentacion.Ventas.Entrada
{
    public partial class DetalleDeCompra : System.Web.UI.Page
    {
        CompraWSClient compraService;
        protected void Page_Init(object sender, EventArgs e)
        {
            compraService = new CompraWSClient();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //if (Request.QueryString["idEvento"] != null)
                //{
                int idEvento = 1; // Simular ID elegido
                                  //int idEvento = int.Parse(Request.QueryString["idEvento"]);
                evento evento = compraService.buscarEventos(idEvento);

                lblEvento.Text = evento.nombre;
                lblUbicacion.Text = evento.ubicacion;
                lblReferencia.Text = evento.referencia;

                // Obtener parámetros por URL
                //string fechaStr = Request.QueryString["fecha"];
                //string horaIni = Request.QueryString["horaInicio"];
                //string horaFin = Request.QueryString["horaFin"];
                //string cantidadStr = Request.QueryString["cantidad"];



                string fecha = "20/06/2025";
                string horaIni = "15:00";
                string horaFin = "17:00";
                string cantidad = "2";


                lblHorario.Text = $"{horaIni} - {horaFin}";
                lblFecha.Text = DateTime.Parse(fecha).ToString("dd/MM/yyyy");
                lblCantidad.Text = cantidad;

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
            if (string.IsNullOrWhiteSpace(txtNombres.Text) ||
                string.IsNullOrWhiteSpace(txtApellidoPaterno.Text) ||
                string.IsNullOrWhiteSpace(txtApellidoMaterno.Text) ||
                string.IsNullOrWhiteSpace(txtDNI.Text))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Por favor, complete todos los campos.');", true);
                return;
            }

            // Validar método de pago
            if (string.IsNullOrEmpty(hfMetodoPago.Value))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Debe seleccionar un método de pago.');", true);
                return;
            }





            string dni = txtDNI.Text.Trim();
            var compradorExistente = compraService.buscarCompradorPorDni(dni);

            // ---------- Datos que necesitas ----------
            //int cantidad = int.Parse(lblCantidad.Text);       // <— corrección
            int cantidad = 2;


            double precio = compraService.buscarEventos(1).precioEntrada;
            double totalAPagar = precio * cantidad;

            // ---------- Comprobación de saldo ----------
            if (compradorExistente != null && compradorExistente.monto < totalAPagar)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert",
                    "alert('Saldo insuficiente.');", true);
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
                    numDocumento = dni,
                    tipoDocumento = eTipoDocumento.DNI,
                    tipoDocumentoSpecified = true,
                    registrado = 1,
                };
                compraService.insertarComprador(nuevo);
            }
            else
            {
                compradorExistente.monto -= totalAPagar;
                compraService.actualizarComprador(compradorExistente);
            }





            ReferenciaDisco.eMetodoPago mp;

            string metodoPagoSeleccionado = hfMetodoPago.Value;

            bool ok = Enum.TryParse(metodoPagoSeleccionado, ignoreCase: false, out mp);

            if (!ok)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert",
                    "alert('Método de pago desconocido.');", true);
                return;
            }


            // ---------- Insertar constancia ----------
            constancia nueva = new constancia
            {
                fecha = DateTime.Now,
                fechaSpecified = true,
                metodoPago = mp,
                metodoPagoSpecified = true,
                igv = 0.18,
                detallePago = $"Pago realizado por {txtNombres.Text.Trim()} {txtApellidoPaterno.Text.Trim()} con DNI {dni}",
                total = totalAPagar,

            };

            int idConstancia = compraService.insertarConstancia(nueva);

            string script = @"
                alert('Pago realizado con éxito.');
                setTimeout(function() {
                    window.location.href = '/Usuario/Comprador/DetalleEntrada.aspx?idConstancia=" + idConstancia + @"';
                }, 1000); // 1000 milisegundos = 1 segundo
            ";
            ScriptManager.RegisterStartupScript(this, GetType(), "alertAndRedirect", script, true);

            Response.Redirect("/Presentacion/Ventas/Entrada/DetalleEntrada.aspx?idConstancia=" + idConstancia);
        }
    }
}