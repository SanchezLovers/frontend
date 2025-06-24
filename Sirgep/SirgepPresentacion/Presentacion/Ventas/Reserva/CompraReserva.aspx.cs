using System;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using SirgepPresentacion.ReferenciaDisco;
using static SirgepPresentacion.Presentacion.Ventas.Reserva.FormularioEspacio;

namespace SirgepPresentacion.Presentacion.Ventas.Reserva
{
    public partial class CompraReserva : System.Web.UI.Page
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
            Page.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;

            if (!IsPostBack)
            {
                int idEspacio = int.Parse(Request.QueryString["idEspacio"]);
                string fechaR = Request.QueryString["fecha"];
                string horaIni = Request.QueryString["horaIni"];
                string horaFin = Request.QueryString["horaFin"];
                int cantidadHoras = int.Parse(Request.QueryString["cant"]); // Simular cantidad elegida
                espacio espacio = espacioService.buscarEspacio(idEspacio);
                LblEspacio.Text = espacio.nombre;
                LblUbicacionReserva.Text = espacio.ubicacion;
                TimeSpan tFin = TimeSpan.Parse(horaFin).Add(TimeSpan.FromHours(1));


                LblHorarioReserva.Text = $"{horaIni} - {tFin.ToString()}";
                LblFechaReserva.Text = DateTime.Parse(fechaR).ToString("dd/MM/yyyy");

                lblPrecioHora.Text = espacio.precioReserva.ToString();
                LblTotalReserva.Text = (espacio.precioReserva * cantidadHoras).ToString("F2");



            }
        }
        protected void documentoNoValido(object sender, EventArgs e)
        {
            string tipo = ddlTipoDocumento.SelectedValue;
            string numero = txtDNI.Text.Trim();
            string script = "";
            bool v;
            switch (tipo)
            {
                case "DNI":
                    if (!(numero.Length == 8 && Regex.IsMatch(numero, @"^\d{8}$")))
                    {
                        script = "setTimeout(function(){ mostrarModalError('Documento de Identidad Inválido.','el DNI debe tener exactamente 8 dígitos numéricos.'); }, 300);";
                        ScriptManager.RegisterStartupScript(this, GetType(), "mostrarModalError", script, true);
                    }
                    break;

                case "CARNETEXTRANJERIA":
                    v = numero.Length == 12 && Regex.IsMatch(numero, @"^\d{12}$");
                    if (!v)
                    {
                        script = "setTimeout(function(){ mostrarModalError('Documento de Identidad Inválido.','el Carnet de Extranjería debe tener exactamente 12 dígitos numéricos.'); }, 300);";
                        ScriptManager.RegisterStartupScript(this, GetType(), "mostrarModalError", script, true);
                    }
                    break;
                case "PASAPORTE":
                    v = numero.Length >= 8 && numero.Length <= 12 && Regex.IsMatch(numero, @"^[a-zA-Z0-9]+$");
                    if (!v)
                    {
                        script = "setTimeout(function(){ mostrarModalError('Documento de Identidad Inválido.','El número de pasaporte debe tener entre 8 y 12 dígitos alfanuméricos.'); }, 300);";
                        ScriptManager.RegisterStartupScript(this, GetType(), "mostrarModalError", script, true);
                    }
                    break;
                default:
                    script = "setTimeout(function(){ mostrarModalError('Documento de Identidad Inválido.','Elija un tipo de documento.'); }, 300);";
                    ScriptManager.RegisterStartupScript(this, GetType(), "mostrarModalError", script, true);
                    break;
            }

        }

        protected void btnPagar_Click(object sender, EventArgs e)

        {
            string fechaR = Request.QueryString["fecha"];
            string horaIni = Request.QueryString["horaIni"];
            string horaFin = Request.QueryString["horaFin"];
            TimeSpan tIni = TimeSpan.Parse(horaIni);
            TimeSpan tFin = TimeSpan.Parse(horaFin).Add(TimeSpan.FromHours(1));
            
            if (string.IsNullOrWhiteSpace(txtNombres.Text) ||
                string.IsNullOrWhiteSpace(txtApellidoPaterno.Text) ||
               // string.IsNullOrWhiteSpace(txtApellidoMaterno.Text) || -- no es obligatorio
                string.IsNullOrWhiteSpace(txtDNI.Text) ||
                 string.IsNullOrWhiteSpace(txtCorreo.Text))
            {
                string script = "setTimeout(function(){ mostrarModalError('Campos faltantes.','Por favor, complete todos los campos obligatorios.'); }, 300);";
                ScriptManager.RegisterStartupScript(this, GetType(), "mostrarModalError", script, true);

                return;

            }

            documentoNoValido(sender, e);

            // Validar método de pago
            if (string.IsNullOrEmpty(hfMetodoPago.Value))
            {
                string script = "setTimeout(function(){ mostrarModalError('Método de pago faltante.','Debe seleccionar un método de pago.'); }, 300);";
                ScriptManager.RegisterStartupScript(this, GetType(), "mostrarModalError", script, true);
                return;
            }





            string dni = txtDNI.Text.Trim();
            var compradorExistente = compraService.buscarCompradorPorDni(dni);

            //int cantidad = 1;

            int idEspacio = int.Parse(Request.QueryString["idEspacio"]);
            double totalAPagar = double.Parse(LblTotalReserva.Text);

            // ---------- Comprobación de saldo ----------
            if (compradorExistente != null && compradorExistente.monto < totalAPagar)
            {
                string script = "setTimeout(function(){ mostrarModalError('Error en pago.','Saldo insuficiente.'); }, 300);";
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
                    // segundoApellido = txtApellidoMaterno.Text.Trim(),  no obligatorio
                    numDocumento = txtDNI.Text.Trim(),
                    correo = txtCorreo.Text.Trim(),
                    tipoDocumento = eTipoDocumento.DNI,
                    tipoDocumentoSpecified = true,
                    registrado = 0,
                };
                if (txtApellidoMaterno.Text.Length > 0)
                    nuevo.segundoApellido = txtApellidoMaterno.Text.Trim();

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
                string script = "setTimeout(function(){ mostrarModalError('Método de pago incorrecto.','Método de pago desconocido.'); }, 300);";
                ScriptManager.RegisterStartupScript(this, GetType(), "mostrarModalError", script, true);
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
                fecha = DateTime.Now, //fecha pago
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
                finString = tFin.ToString(),


                persona = new persona
                {
                    idPersona = identificadorPersona, // Usar el ID del comprador existente o nuevo

                }, // el objeto comprador
                espacio = espacioService.buscarEspacio(idEspacio)        // el objeto espacio
            };

            //int idReserva = reservaService.insertarReserva(nuevaReserva);
            // };

            //int idReserva = compraService.insertarConstancia(nueva);
            int idConstancia = reservaService.insertarReserva(nuevaReserva);


            string scriptExito =
           "document.getElementById('modalExitoBody').innerText = 'Pago realizado con éxito.';" +
           "var modal = new bootstrap.Modal(document.getElementById('modalExito'));" +
           "modal.show();" +
           "setTimeout(function() {" +
           "  window.location.href = '/Presentacion/Ventas/Reserva/ConstanciaReserva.aspx?idConstancia=" + idConstancia + "';" +
           "}, 1500);"; // 1.5 segundos para que el usuario vea el modal

            ScriptManager.RegisterStartupScript(this, GetType(), "mostrarModalExito", scriptExito, true);

            //Response.Redirect("/Presentacion/Ventas/Entrada/ConstanciaEntrada.aspx?idConstancia=" + idConstancia);
        }

        protected void cvDocumento_ServerValidate(object source, ServerValidateEventArgs args)
        {
            Page.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;

            string tipo = ddlTipoDocumento.SelectedValue;
            string numero = txtDNI.Text.Trim();
            string mensaje = "";

            switch (tipo)
            {
                case "DNI":
                    args.IsValid = numero.Length == 8 && Regex.IsMatch(numero, @"^\d{8}$");
                    if (!args.IsValid) mensaje = "El DNI debe tener exactamente 8 dígitos numéricos.";
                    break;

                case "CARNETEXTRANJERIA":
                    args.IsValid = numero.Length == 12 && Regex.IsMatch(numero, @"^\d{12}$");
                    if (!args.IsValid) mensaje = "El Carnet de Extranjería debe tener exactamente 12 dígitos numéricos.";
                    break;

                case "PASAPORTE":
                    args.IsValid = numero.Length >= 8 && numero.Length <= 12 && Regex.IsMatch(numero, @"^[a-zA-Z0-9]+$");
                    if (!args.IsValid) mensaje = "El Pasaporte debe tener entre 8 y 12 caracteres alfanuméricos (sin símbolos).";
                    break;

                default:
                    args.IsValid = false;
                    mensaje = "Seleccione un tipo de documento válido.";
                    break;
            }

            ((CustomValidator)source).ErrorMessage = mensaje;
        }


    }
}