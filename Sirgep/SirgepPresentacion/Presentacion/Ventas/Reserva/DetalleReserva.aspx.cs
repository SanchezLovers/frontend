using System;
using System.Web.UI;
using SirgepPresentacion.ReferenciaDisco;
using static SirgepPresentacion.Presentacion.Ventas.Reserva.FormularioEspacio;

namespace SirgepPresentacion.Presentacion.Ventas.Reserva
{
    public partial class DetalleReserva : System.Web.UI.Page
    {
        EspacioWSClient espacioService;
        CompraWSClient compraService;
        ReservaWSClient reservaService;

        protected void Page_Init(object sender, EventArgs e)
        {
            espacioService = new EspacioWSClient();
            compraService = new CompraWSClient();
            reservaService = new ReservaWSClient();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            //string url = $"/Presentacion/Ventas/Reserva/DetalleReserva.aspx?idEspacio={idEspacio}&fecha={fecha}&horaIni={horaIni}&horaFin={horaFin}&cant={cant}";

            if (!IsPostBack)
            {
                //int idEspacio = 1; // Simular ID elegido
                int idEspacio = int.Parse(Request.QueryString["idEspacio"]);
                string fechaR = Request.QueryString["fecha"];
                string horaIni = Request.QueryString["horaIni"];
                string horaFin = Request.QueryString["horaFin"];
                int cantidadHoras = int.Parse(Request.QueryString["cant"]); // Simular cantidad elegida
                espacio espacio = espacioService.buscarEspacio(idEspacio);
                LblEspacio.Text = espacio.nombre;
                LblUbicacionReserva.Text = espacio.ubicacion;

                //string fechaR = "20/06/2025";
                //string horaIni = "15:00";
                //string horaFin = "17:00";
                //DateTime fecha = DateTime.Parse("20/06/2025");
                // DateTime horaIni = fecha.Date.AddHours(15); // 15:00
                //DateTime horaFin = fecha.Date.AddHours(17); // 17:00

                //int cantidadHoras = 2;

                LblHorarioReserva.Text = $"{horaIni} - {horaFin}";
                LblFechaReserva.Text = DateTime.Parse(fechaR).ToString("dd/MM/yyyy");

                //double cantidadHoras = (DateTime.Parse(horaFin) - DateTime.Parse(horaIni)).TotalHours;

                lblPrecioHora.Text = espacio.precioReserva.ToString();
                LblTotalReserva.Text = (espacio.precioReserva * cantidadHoras).ToString("F2");



            }
        }

        protected void btnPagar_Click(object sender, EventArgs e)

        {
            string fechaR = "20/06/2025";
            var horaIni = "15:00";
            var horaFin = "17:00";
            TimeSpan tIni = TimeSpan.Parse(horaIni);
            TimeSpan tFin = TimeSpan.Parse(horaFin);

            //TimeSpan horarioIni = TimeSpan.Parse(horaIni);
            //TimeSpan horarioFin = TimeSpan.Parse(horaFin);
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
                string script = "document.getElementById('modalErrorBody').innerText = \"Por favor, complete todos los campos obligatorios.\";" +
                "var modal = new bootstrap.Modal(document.getElementById('modalError'));" +
                "modal.show();";
                ScriptManager.RegisterStartupScript(this, GetType(), "mostrarModalError", script, true);
                return;

            }

            // Validar método de pago
            if (string.IsNullOrEmpty(hfMetodoPago.Value))
            {
                //ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Debe seleccionar un método de pago.');", true);
                //return;
                string script = "document.getElementById('modalErrorBody').innerText = \"Debe seleccionar un método de pago.\";" +
                "var modal = new bootstrap.Modal(document.getElementById('modalError'));" +
                "modal.show();";
                ScriptManager.RegisterStartupScript(this, GetType(), "mostrarModalError", script, true);
                return;
            }





            string dni = txtDNI.Text.Trim();
            var compradorExistente = compraService.buscarCompradorPorDni(dni);

            // ---------- Datos que necesitas ----------
            //int cantidad = int.Parse(lblCantidad.Text);       // <— corrección
            int cantidad = 2;


            double precio = espacioService.buscarEspacio(1).precioReserva;
            double totalAPagar = precio * cantidad;

            // ---------- Comprobación de saldo ----------
            if (compradorExistente != null && compradorExistente.monto < totalAPagar)
            {
                //ScriptManager.RegisterStartupScript(this, GetType(), "alert",
                //  "alert('Saldo insuficiente.');", true);
                //return;
                string script = "document.getElementById('modalErrorBody').innerText = \"Saldo insuficiente.\";" +
                "var modal = new bootstrap.Modal(document.getElementById('modalError'));" +
                "modal.show();";
                ScriptManager.RegisterStartupScript(this, GetType(), "mostrarModalError", script, true);
                return;
            }

            // ---------- Insertar / actualizar comprador ----------

            comprador compradorFinal = null;
            int identificadorPersona;
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
                identificadorPersona = compraService.insertarComprador(nuevo);
                compradorFinal = nuevo; // Guardar el nuevo comprador para usarlo más adelante
            }
            else
            {
                compradorExistente.monto -= totalAPagar;
                compraService.actualizarComprador(compradorExistente);
                compradorFinal = compradorExistente; // Usar el comprador existente
                identificadorPersona = compradorExistente.idPersona; // Obtener el ID del comprador existente
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
            //constancia nueva = new constancia
            //{
            //  fecha = DateTime.Now,
            //  fechaSpecified = true,
            //  metodoPago = mp,
            //  metodoPagoSpecified = true,
            //   igv = 0.18,
            //  detallePago = $"Pago realizado por {txtNombres.Text.Trim()} {txtApellidoPaterno.Text.Trim()} con DNI {dni}",
            //   total = totalAPagar,

            // };

            // int idConstancia = compraService.insertarConstancia(nueva);

            reserva nuevaReserva = new reserva
            {
                // Campos heredados de constancia
                fecha = DateTime.Now,
                fechaSpecified = true,
                metodoPago = mp,
                metodoPagoSpecified = true,
                igv = 0.18,
                detallePago = $"Pago realizado por {txtNombres.Text.Trim()} {txtApellidoPaterno.Text.Trim()} con DNI {dni}",
                total = totalAPagar,

                // Campos propios de reserva
                fechaReserva = DateTime.Parse(fechaR),  // Convertir a DateTime
                fechaReservaSpecified = true,
                //horarioIni = new ReferenciaDisco.localTime
                //{
                //hour = tIni.Hours,
                //minute = tIni.Minutes,
                //second = tIni.Seconds

                //},
                //horarioFin = new ReferenciaDisco.localTime
                //{
                //   hour = tFin.Hours,
                //   minute = tFin.Minutes,
                //   second = tFin.Seconds
                //},

                //horarioIni = horaIni,
                //horarioFin = horaFin,

                // Relaciones
                //horarioIni = reservaService.convertirALocalTime(horaIni), // Convertir a localTime
                // horarioFin = reservaService.convertirALocalTime(horaFin),
                iniString = horaIni,
                finString = horaFin,


                persona = new persona
                {
                    idPersona = identificadorPersona, // Usar el ID del comprador existente o nuevo

                }, // el objeto comprador
                espacio = espacioService.buscarEspacio(1)        // el objeto espacio
            };

            //int idReserva = reservaService.insertarReserva(nuevaReserva);
            // };

            //int idReserva = compraService.insertarConstancia(nueva);
            int idReserva = reservaService.insertarReserva(nuevaReserva); 


            string scriptExito =
           "document.getElementById('modalExitoBody').innerText = 'Pago realizado con éxito.';" +
           "var modal = new bootstrap.Modal(document.getElementById('modalExito'));" +
           "modal.show();" +
           "setTimeout(function() {" +
           "  window.location.href = '/Presentacion/Ventas/Reserva/ConstanciaReserva.aspx?numReserva=" + idReserva + "';" +
           "}, 1500);"; // 1.5 segundos para que el usuario vea el modal

            ScriptManager.RegisterStartupScript(this, GetType(), "mostrarModalExito", scriptExito, true);

            //Response.Redirect("/Presentacion/Ventas/Entrada/ConstanciaEntrada.aspx?idConstancia=" + idConstancia);
        }


    }
}