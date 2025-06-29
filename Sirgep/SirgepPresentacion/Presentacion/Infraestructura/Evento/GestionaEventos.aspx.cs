using SirgepPresentacion.ReferenciaDisco;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static SirgepPresentacion.Presentacion.Ventas.Reserva.FormularioEspacio;

namespace SirgepPresentacion.Presentacion.Infraestructura.Evento
{
    public partial class GestionaEventos : System.Web.UI.Page
    {
        private EventoWS eventoWS;
        private FuncionWS funcionWS;
        private DepartamentoWS depaWS;
        private ProvinciaWS provWS;
        private DistritoWS distWS;

        private const int MODAL_AGREGAR = 0;
        private const int MODAL_EDITAR = 1;
        private int PaginaActual
        {
            get => ViewState["PaginaActual"] != null ? (int)ViewState["PaginaActual"] : 1;
            set => ViewState["PaginaActual"] = value;
        }

        private const int TAM_PAGINA = 10;
        private int TotalPaginas = 1; // se calcula en CargarEspacios()

        protected void Page_Init(object sender, EventArgs e)
        {
            eventoWS = new EventoWSClient();
            funcionWS = new FuncionWSClient();
            depaWS = new DepartamentoWSClient();
            provWS = new ProvinciaWSClient();
            distWS = new DistritoWSClient();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtFechaFuncion.Attributes["min"] = DateTime.Now.ToString("yyyy-MM-dd");
                PaginaActual = 1;
                CargarEventos();
            }
        }
        protected void Paginar_Click(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "Anterior") PaginaActual--;
            if (e.CommandName == "Siguiente") PaginaActual++;

            CargarEventos();
        }

        public void realizarPaginado(evento[] response)
        {
            if (response == null)
            {
                mostrarModalErrorEvento("RESULTADO DE BUSQUEDA", "No se encontraron eventos con los parámetros actuales; se listarán todos los eventos...");
                CargarEventos();
                return;
            }
            var todos = response.ToList(); // lo convertimos a lista

            int totalEspacios = todos.Count;
            TotalPaginas = (int)Math.Ceiling((double)totalEspacios / TAM_PAGINA);

            if (PaginaActual < 1) PaginaActual = 1;
            if (PaginaActual > TotalPaginas) PaginaActual = TotalPaginas;

            // Datos solo para esta página
            var paginaActual = todos.Skip((PaginaActual - 1) * TAM_PAGINA).Take(TAM_PAGINA).ToList();

            // Cargar al repeater
            rptEventos.DataSource = paginaActual;
            rptEventos.DataBind();

            // Footer: actualizar label y botones
            if (rptEventos.Controls.Count > 0)
            {
                var footer = rptEventos.Controls[rptEventos.Controls.Count - 1];

                var lblPagina = footer.FindControl("lblPaginaFootEvento") as Label;
                if (lblPagina != null)
                    lblPagina.Text = $"Página {PaginaActual} / {TotalPaginas}";

                var btnAnterior = footer.FindControl("btnAnteriorFootEvento") as Button;
                var btnSiguiente = footer.FindControl("btnSiguienteFootEvento") as Button;

                if (btnAnterior != null)
                    btnAnterior.Enabled = PaginaActual > 1;

                if (btnSiguiente != null)
                    btnSiguiente.Enabled = PaginaActual < TotalPaginas;
            }
        }

        protected void CargarEventos()
        {
            evento[] eventos= eventoWS.listarEvento(new listarEventoRequest()).@return; ;
            realizarPaginado(eventos);
        }

        protected void ddlFiltroFechas_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblError.Text = "";
        }
        public void cargarUbicacion()
        {
            /* Depas */
            ddlDepaAgregar.DataSource = depaWS.listarDepas(new listarDepasRequest()).@return;
            ddlDepaAgregar.DataTextField = "Nombre";
            ddlDepaAgregar.DataValueField = "idDepartamento";
            ddlDepaAgregar.DataBind();
            ddlDepaAgregar.Items.Insert(0, new ListItem("Seleccione un departamento", ""));

            /* Provincias */
            // ddlProvAgregar.DataSource = provWS.listarProvinciaPorDepa(new // id).@return;
        }

        protected void btnMostrarModalAgregarEvento_Click(object sender, EventArgs e)
        {
            cargarUbicacion();
            abrirModalAgregar();
        }

        protected void txtBusqueda_TextChanged(object sender, EventArgs e)
        {
            buscarEventoPorTextoResponse response = eventoWS.buscarEventoPorTexto(new buscarEventoPorTextoRequest(txtBusqueda.Text));
            evento[] eventos = response.@return;
            realizarPaginado(eventos);
        }

        public void LimpiarDatosAgregados()
        {
            txtFechaInicioEvento.Text = "";
            txtFechaInicioEvento.Text="";
            txtNomEvent.Text = "";
            txtDescAgregar.Text = "";
            txtUbicacionAgregar.Text = "";
            ddlDistAgregar.SelectedValue = ""; // obteniendo ID de distrito seleccionado
            txtPrecioEntrada.Text = "";
            txtDisponibles.Text = "";
            txtVendidas.Text = "";
            txtReferencia.Text = "";
            lblError.Text = "";
        }
        private bool ValidarDatosEvento(string txtNombre, string txtDesc, string txtUbi, string txtPrecio,
                string txtDispo, string txtVend, string txtRef, string ddlDepas, string ddlDist, string ddlProv, bool fileUpdate, int pagina)
        {
            // Validación de existencia de fecha mínima
            if (Session["fechaMinima"] == null)
            {
                MostrarError("Debe agregar al menos 1 función.", pagina);
                return false;
            }

            // Función local para validar que no sea solo números
            bool ContieneLetras(string texto) => !string.IsNullOrWhiteSpace(texto) && texto.Any(c => char.IsLetter(c));

            if (!ContieneLetras(txtNombre))
            {
                MostrarError("El nombre debe contener letras.", pagina);
                return false;
            }

            if (!ContieneLetras(txtDesc))
            {
                MostrarError("La descripción debe contener letras.", pagina);
                return false;
            }

            if (!ContieneLetras(txtUbi))
            {
                MostrarError("La ubicación debe contener letras.", pagina);
                return false;
            }

            if (!ContieneLetras(txtRef))
            {
                MostrarError("La referencia debe contener letras.", pagina);
                return false;
            }

            // Validar campos obligatorios de texto
            if (string.IsNullOrWhiteSpace(txtNombre))
            {
                MostrarError("Debe ingresar el nombre del evento.", pagina);
                return false;
            }
            
            if(txtNombre.Length > 45)
            {
                MostrarError("La longitud del nombre ha superado los 45 caracteres.", pagina);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtDesc))
            {
                MostrarError("Debe ingresar la descripción del evento.", pagina);
                return false;
            }

            if (txtDesc.Length > 350)
            {
                MostrarError("La longitud de la descripción ha superado los 350 caracteres.", pagina);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtUbi))
            {
                MostrarError("Debe ingresar la ubicación del evento.", pagina);
                return false;
            }

            if (txtUbi.Length > 45)
            {
                MostrarError("La longitud de la ubicación ha superado los 45 caracteres.", pagina);
                return false;
            }

            if (string.IsNullOrWhiteSpace(ddlDepas) || ddlDepas == "0")
            {
                MostrarError("Debe seleccionar un departamento.", pagina);
                return false;
            }

            if (string.IsNullOrWhiteSpace(ddlProv) || ddlProv == "0")
            {
                MostrarError("Debe seleccionar una provincia.", pagina);
                return false;
            }

            if (string.IsNullOrWhiteSpace(ddlDist) || ddlDist == "0")
            {
                MostrarError("Debe seleccionar un distrito.", pagina);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtRef))
            {
                MostrarError("Debe ingresar la referencia del evento.", pagina);
                return false;
            }

            if (txtRef.Length > 45)
            {
                MostrarError("La longitud de la referencia ha superado los 45 caracteres.", pagina);
                return false;
            }

            // Validar números: Precio, Disponibles, Vendidas
            if (!int.TryParse(txtPrecio, out int precio) || precio < 0 || precio > 1000)
            {
                MostrarError("El precio de la entrada debe ser un número entre 0 y 1000.", pagina);
                return false;
            }

            if (!int.TryParse(txtDispo, out int disponibles) || disponibles < 0 || disponibles > 1000)
            {
                MostrarError("La cantidad de entradas disponibles debe ser un número entre 0 y 1000.", pagina);
                return false;
            }

            int.TryParse(txtVend, out int vendidas);
            if (vendidas < 0 || vendidas > 1000)
            {
                MostrarError("La cantidad de entradas vendidas debe ser un número entre 0 y 1000.", pagina);
                return false;
            }

            if(vendidas > disponibles)
            {
                MostrarError("La cantidad de entradas vendidas debe ser menor que la cantidad disponible (total de entradas).", pagina);
                return false;
            }

            if (!fileUpdate)
            {
                MostrarError("Debe seleccionar una imagen obligatoriamente",pagina);
                return false;
            }

            // Si todo es válido, retornar true
            return true;
        }

        public void guardarImgEvento(ref string urlParaBD)
        {
            // 1. Obtener nombre original y asegurar que sea único
            string nombreArchivo = Path.GetFileName(fuAgregar.FileName);
            string nombreUnico = Guid.NewGuid().ToString() + Path.GetExtension(nombreArchivo); // evita sobrescribir

            // 2. Ruta física en el servidor
            string rutaRelativa = "~/Images/img/eventos/" + nombreUnico;
            string rutaFisica = Server.MapPath(rutaRelativa); // obtiene la ruta absoluta

            // 3. Guardar archivo
            fuAgregar.SaveAs(rutaFisica);
            // 4. URL relativa para la base de datos
            urlParaBD = "Images/img/eventos/" + nombreUnico;
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            if (!ValidarDatosEvento(txtNomEvent.Text, txtDescAgregar.Text,
                txtUbicacionAgregar.Text, txtPrecioEntrada.Text, txtDisponibles.Text,
                txtVendidas.Text, txtReferencia.Text, ddlDepaAgregar.Text, ddlDistAgregar.Text,
                ddlProvAgregar.Text, fuAgregar.HasFile, MODAL_AGREGAR)) return;

            string fechaInicioEvento = Session["fechaMinima"].ToString();
            string fechaFinEvento = Session["fechaMaxima"].ToString();
            string nombreEvento = txtNomEvent.Text;
            string descripcionEvento = txtDescAgregar.Text;
            string ubicacionEvento = txtUbicacionAgregar.Text;
            string idDistritoEvento = ddlDistAgregar.SelectedValue; // obteniendo ID de distrito seleccionado
            string precioEntradaEvento = txtPrecioEntrada.Text;
            string cantDisponibles = txtDisponibles.Text;
            string cantVendidas = txtVendidas.Text;
            string referenciaEvento = txtReferencia.Text;
            string urlParaBd = "";
            guardarImgEvento(ref urlParaBd);
            evento eventoAgregar = new evento
            {
                fecha_inicio = fechaInicioEvento.Split(' ')[0],
                fecha_fin = fechaFinEvento.Split(' ')[0],
                nombre = nombreEvento,
                descripcion = descripcionEvento,
                ubicacion = ubicacionEvento,
                distrito = new distrito
                {
                    idDistrito = int.Parse(idDistritoEvento)
                },
                precioEntrada = double.Parse(precioEntradaEvento),
                cantEntradasVendidas = int.Parse(cantVendidas),
                cantEntradasDispo = int.Parse(cantDisponibles),
                referencia = referenciaEvento,
                archivoImagen = urlParaBd // añadimos la imagen a la query
            };

            int id = eventoWS.insertarEvento(new insertarEventoRequest(eventoAgregar)).@return;

            if (id >= 1)
            {
                // se insertó correctamente
                
                // agregar todas las funciones del evento a la base de datos
                foreach (ListItem item in ddlFuncionesAgregar.Items)
                {
                    if (item.Text[0] == 'P') continue;
                    string valorDdl = item.Text; // "2025-06-17 - 20:00 a 17:59"

                    // Separar por " - "
                    string[] partes = valorDdl.Split(new[] { " - " }, StringSplitOptions.None);

                    string fechaFuncion = partes[0];                   // "2025-06-17"
                    string horas = partes.Length > 1 ? partes[1] : ""; // "20:00 a 17:59"

                    // Separar por " a "
                    string[] rangoHoras = horas.Split(new[] { " a " }, StringSplitOptions.None);

                    string horaIniFuncion = rangoHoras.Length > 0 ? rangoHoras[0] : "";
                    string horaFinFuncion = rangoHoras.Length > 1 ? rangoHoras[1] : "";

                    funcion funcionAgregar = new funcion()
                    {
                        fecha = fechaFuncion,
                        horaInicio = horaIniFuncion,
                        horaFin = horaFinFuncion,
                        evento = new evento
                        {
                            idEvento = id
                        }
                    };
                    int idFuncion = funcionWS.insertarFuncion(new insertarFuncionRequest(funcionAgregar)).@return;

                    if (idFuncion < 1)
                    {
                        Console.WriteLine();
                        mostrarModalErrorEvento("VENTANA DE ERROR", "Error al insertar la función con ID: " + idFuncion);
                        return;
                    }
                }

                string nombreDistrito = distWS.buscarDistPorId(new buscarDistPorIdRequest(eventoAgregar.distrito.idDistrito)).@return.nombre;
                string asunto = $"¡Nuevo evento en tu distrito favorito {nombreDistrito}: {eventoAgregar.nombre}!";
                string contenido = $@"
                    <html>
                    <head>
                      <style>
                        body {{
                          font-family: 'Segoe UI', sans-serif;
                          background-color: #f8f9fa;
                          margin: 0;
                          padding: 0;
                        }}
                        .container {{
                          max-width: 600px;
                          margin: 20px auto;
                          background-color: #fff;
                          border: 1px solid #dee2e6;
                          border-radius: 8px;
                          padding: 30px;
                          box-shadow: 0 4px 12px rgba(0,0,0,0.08);
                        }}
                        .header {{
                          background-color: #f10909;
                          color: white;
                          padding: 15px;
                          border-radius: 6px 6px 0 0;
                          text-align: center;
                        }}
                        .header h2 {{
                          margin: 0;
                          font-size: 22px;
                          color: white;
                        }}
                        .body {{
                          color: #212529;
                          padding: 20px 0;
                          font-size: 16px;
                        }}
                        .details {{
                          background-color: #f1f3f5;
                          border-radius: 6px;
                          padding: 15px;
                          margin-bottom: 20px;
                          list-style: none;
                        }}
                        .details li {{
                          margin-bottom: 10px;
                          font-size: 15px;
                        }}
                        .details li strong {{
                          color: #000;
                          font-weight: bold;
                        }}
                        .cta {{
                          display: inline-block;
                          background-color: #f10909;
                          color: #fff !important;
                          padding: 10px 20px;
                          border-radius: 5px;
                          text-decoration: none;
                          font-weight: bold;
                          margin-top: 10px;
                        }}
                        .cta:hover {{
                          background-color: #c40808;
                        }}
                        .logo {{
                          text-align: center;
                          margin-top: 20px;
                        }}
                        .logo img {{
                          width: 100px;
                        }}
                        .footer {{
                          text-align: center;
                          font-size: 12px;
                          color: #6c757d;
                          margin-top: 20px;
                        }}
                      </style>
                    </head>
                    <body>
                      <div class='container'>
                        <div class='header'>
                          <h2>¡No te pierdas este evento en tu distrito favorito {nombreDistrito}!</h2>
                        </div>
                        <div class='body'>
                          <p>Nos alegra informarte que se ha registrado un nuevo evento en tu distrito favorito: <strong>{nombreDistrito}</strong>.</p>
                          <ul class='details'>
                            <li><strong>🎉 Nombre del evento:</strong> {eventoAgregar.nombre}</li>
                            <li><strong>📅 Fecha:</strong> {eventoAgregar.fecha_inicio} al {eventoAgregar.fecha_fin}</li>
                            <li><strong>📍 Ubicación:</strong> {eventoAgregar.ubicacion}</li>
                            <li><strong>🗺 Referencia:</strong> {eventoAgregar.referencia}</li>
                            <li><strong>🎟 Entradas disponibles:</strong> {eventoAgregar.cantEntradasDispo}</li>
                            <li><strong>💵 Precio por entrada:</strong> S/ {eventoAgregar.precioEntrada}</li>
                          </ul>
                          <p>Si deseas más información o comprar entradas, haz clic en el botón de abajo:</p>
                          <a href='https://localhost:44360/Presentacion/Inicio/PrincipalInvitado.aspx' class='cta'>Ver más</a>
                        </div>
                        <div class='logo'>
                          <img src='https://upload.wikimedia.org/wikipedia/commons/4/43/Escudo_Regi%C3%B3n_Lima.png' alt='Logo Región Lima' />
                        </div>
                        <div class='footer'>
                          Este mensaje fue enviado automáticamente por el sistema SIRGEP.<br>
                          © 2025 Gobierno Regional de Lima
                        </div>
                      </div>
                    </body>
                    </html>";
                bool resultado = eventoWS.enviarCorreosCompradoresPorDistritoDeEvento(new enviarCorreosCompradoresPorDistritoDeEventoRequest(asunto, contenido, eventoAgregar.distrito.idDistrito)).@return;
                if (resultado)
                {
                    mostrarModalExitoEvento("VENTANA DE ÉXITO", "Se insertó el EVENTO correctamente y se enviarán correos a los compradores cuyo distrito favorito coincide con el distrito del evento registrado.");
                    CargarEventos();
                    return;
                }
                mostrarModalErrorEvento("VENTANA DE ERROR", "No se pudo enviar los correos apropiadamente, pero sí se insertó el EVENTO.");
            }
            else
            {
                // error al insertar
                mostrarModalErrorEvento("VENTANA DE ERROR", "Error al insertar el evento");
            }

            LimpiarDatosAgregados();
        }

        protected void ddlProvAgregar_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Cuando la provincia cambia, debemos listar los distritos!!!
            if (!string.IsNullOrEmpty(ddlProvAgregar.SelectedValue))
            {
                int idProvincia = int.Parse(ddlProvAgregar.SelectedValue);
                listarDistritosFiltradosResponse responseDistrito = distWS.listarDistritosFiltrados(
                    new listarDistritosFiltradosRequest(idProvincia)
                );

                ddlDistAgregar.DataSource = responseDistrito.@return;
                ddlDistAgregar.DataTextField = "Nombre";
                ddlDistAgregar.DataValueField = "IdDistrito";
                ddlDistAgregar.DataBind();
                ddlDistAgregar.Items.Insert(0, new ListItem("Seleccione un distrito", ""));
            }
            else
            {
                ddlDistAgregar.Items.Clear();
                ddlDistAgregar.Items.Insert(0, new ListItem("Seleccione una provincia primero", ""));
            }

            // Para mantener el modal abierto tras el postback
            abrirModalAgregar();
        }

        protected void ddlDepaAgregar_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlDepaAgregar.SelectedValue))
            {
                int idDepartamento = int.Parse(ddlDepaAgregar.SelectedValue);
                listarProvinciaPorDepaResponse responseProvincia = provWS.listarProvinciaPorDepa(
                    new listarProvinciaPorDepaRequest(idDepartamento)
                );

                ddlProvAgregar.DataSource = responseProvincia.@return;
                ddlProvAgregar.DataTextField = "Nombre";
                ddlProvAgregar.DataValueField = "IdProvincia";
                ddlProvAgregar.DataBind();
                ddlProvAgregar.Items.Insert(0, new ListItem("Seleccione una provincia", ""));
            }
            else
            {
                ddlProvAgregar.Items.Clear();
                ddlProvAgregar.Items.Insert(0, new ListItem("Seleccione un departamento primero", ""));
            }

            // Para mantener el modal abierto tras el postback
            abrirModalAgregar();
        }

        public void abrirModalAgregar()
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "abrirModalEvento",
            "var modalEvento = new bootstrap.Modal(document.getElementById('modalAgregarEvento')); modalEvento.show();", true);
        }

        protected void btnAgregarFuncion_Click(object sender, EventArgs e)
        {
            lblErrorAgregar.Text = "";
            string fecha = txtFechaFuncion.Text.Trim();
            string horaInicio = txtHoraIniFuncion.Text.Trim();
            string horaFin = txtHoraFinFuncion.Text.Trim();

            string valor = $"{fecha}_{horaInicio}_{horaFin}";
            string texto = $"{fecha} - {horaInicio} a {horaFin}";

            const int MODAL_AGREGAR = 0;
            if (!ValidaFuncion(fecha,horaInicio,horaFin,valor, MODAL_AGREGAR)) return;
            lblErrorEditar.Text = "";
            DateTime.TryParse(fecha, out DateTime fechaSeleccionada);

            ddlFuncionesAgregar.Items.Add(new ListItem(texto, valor));
            ActualizarFechasMinMax(fechaSeleccionada);
            LimpiarFunciones();
            // Para mantener el modal abierto tras el postback
            ScriptManager.RegisterStartupScript(this, this.GetType(), "abrirModalEvento",
            "var modalEvento = new bootstrap.Modal(document.getElementById('modalAgregarEvento')); modalEvento.show();", true);
        }

        public bool validaFormatoHora(TimeSpan tsInicio, TimeSpan tsFin)
        {
            bool EsValido(TimeSpan t) =>
                    t.Seconds == 0 &&
                (t.Minutes == 0 || t.Minutes == 15 || t.Minutes == 30 || t.Minutes == 45);

            return EsValido(tsInicio) && EsValido(tsFin);
        }
        private bool ValidaFuncion(string fecha, string horaInicio, string horaFin, string valor, int pagina)
        {
            // Validación: campos vacíos
            if (string.IsNullOrWhiteSpace(fecha) || string.IsNullOrWhiteSpace(horaInicio) || string.IsNullOrWhiteSpace(horaFin))
            {
                MostrarError("Por favor complete todos los campos de la función.", pagina);
                return false;
            }

            // Validación: fecha en el mismo año y no pasada
            if (!DateTime.TryParse(fecha, out DateTime fechaSeleccionada))
            {
                MostrarError("La fecha no tiene un formato válido.", pagina);
                return false;
            }

            DateTime hoy = DateTime.Today;
            if (fechaSeleccionada.Year != hoy.Year)
            {
                MostrarError("La fecha debe estar en el año actual.", pagina);
                return false;
            }

            if (fechaSeleccionada < hoy)
            {
                MostrarError("La fecha no puede ser anterior a hoy.", pagina);
                return false;
            }

            // Validación: hora inicio < hora fin
            if (!TimeSpan.TryParse(horaInicio, out TimeSpan tsInicio) || !TimeSpan.TryParse(horaFin, out TimeSpan tsFin))
            {
                MostrarError("Las horas no tienen un formato válido.", pagina);
                return false;
            }

            if (tsInicio >= tsFin)
            {
                MostrarError("La hora de inicio debe ser menor a la hora de fin.", pagina);
                return false;
            }

            if (!validaFormatoHora(tsInicio, tsFin))
            {
                MostrarError("La horas deben terminar en algún múltiplo de 15 (00,15,30,45).", pagina);
                return false;
            }

            // validación de duplicados
            if (ddlFuncionesAgregar.Items.Cast<ListItem>().Any(i => i.Value == valor))
            {
                MostrarError("Esa función ya ha sido agregada.", pagina);
                return false;
            }

            return true;
        }

        public void LimpiarFunciones()
        {
            txtFechaFuncion.Text = "";
            txtHoraIniFuncion.Text = "";
            txtHoraFinFuncion.Text = "";

            txtFechaEditar.Text = "";
            txtHoraIniEditar.Text = "";
            txtHoraFinEditar.Text = "";
        }

        private void MostrarError(string mensaje, int pagina)
        {
            if(pagina == MODAL_AGREGAR)
            {
                lblErrorAgregar.Text = mensaje;
                abrirModalAgregar();
                return;
            }
            lblErrorEditar.Text = mensaje;
            abrirModalEdicion();
        }

        private void ActualizarFechasMinMax(DateTime fecha)
        {
            DateTime? fechaMinima = Session["fechaMinima"] as DateTime?;
            DateTime? fechaMaxima = Session["fechaMaxima"] as DateTime?;

            if (fechaMinima == null || fecha < fechaMinima)
                Session["fechaMinima"] = fecha;

            if (fechaMaxima == null || fecha > fechaMaxima)
                Session["fechaMaxima"] = fecha;
        }

        public bool validarFechas(string fechaIniFiltro, string fechaFinFiltro)
        {
            if ((fechaIniFiltro!="" && string.IsNullOrEmpty(fechaFinFiltro)) || (fechaFinFiltro!="" && string.IsNullOrEmpty(fechaIniFiltro)))
            {
                mostrarModalErrorEvento("VALIDACION", "Por favor, complete la fecha faltante.");
                return false;
            }

            if (DateTime.TryParse(fechaIniFiltro, out DateTime fechaInicio) && DateTime.TryParse(fechaFinFiltro, out DateTime fechaFin))
            {
                if(fechaInicio > fechaFin)
                {
                    mostrarModalErrorEvento("VALIDACION","La fecha de inicio no puede ser mayor a la fecha fin");
                    return false;
                }
            }
            return true;
        }

        protected void btnConsultar_Click(object sender, EventArgs e)
        {
            string fechaIniFiltro = txtFechaInicioFiltro.Text;
            string fechaFinFiltro = txtFechaFinFiltro.Text;

            if(!validarFechas(fechaIniFiltro,fechaFinFiltro)) return;
            buscarEventosPorFechasResponse rsp = null;
            if (!string.IsNullOrEmpty(fechaFinFiltro) && !string.IsNullOrEmpty(fechaIniFiltro))
            {
                rsp = eventoWS.buscarEventosPorFechas(new buscarEventosPorFechasRequest(fechaIniFiltro, fechaFinFiltro));
                if (rsp.@return != null)
                {
                    realizarPaginado(rsp.@return);
                }
                else
                {
                    mostrarModalErrorEvento("RESULTADO DE BUSQUEDA", "No se encontraron eventos siguiendo el cirterio de los filtros seleccionados. Se listarán todos los eventos.");
                    CargarEventos();
                }
            }
            else
            {
                mostrarModalErrorEvento("RESULTADO DE BUSQUEDA", "No se detectó ninguna fecha. Se listarán todos los eventos.");
                CargarEventos();
            }
            
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string id = btn.CommandArgument;
            hdnIdEvento.Value = id;
            ScriptManager.RegisterStartupScript(
            this,
            GetType(),
            "mostrarModal",
            $"var modal = new bootstrap.Modal(document.getElementById('{modalConfirmacionEliminado.ClientID}')); modal.show();",
            true
            );
        }

        protected void btnConfirmarEliminado_Click(object sender, EventArgs e)
        {
            int idEvento = int.Parse(hdnIdEvento.Value);

            // Lógica para eliminar...
            Boolean eliminado = eventoWS.eliminarLogico(new eliminarLogicoRequest(idEvento)).@return;
            if (!eliminado)
            {
                // error al eliminar
                mostrarModalErrorEvento("VENTANA DE ERROR", "El evento no fue eliminado correctamente.");
            }
            else
            {
                mostrarModalExitoEvento("VENTANA DE ÉXITO", "El evento fue eliminado correctamente.");
            }
            CargarEventos();
        }

        protected void ddlProvEditar_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Cuando la provincia cambia, debemos listar los distritos!!!
            if (!string.IsNullOrEmpty(ddlProvEditar.SelectedValue))
            {
                int idProvincia = int.Parse(ddlProvEditar.SelectedValue);
                listarDistritosFiltradosResponse responseDistrito = distWS.listarDistritosFiltrados(
                    new listarDistritosFiltradosRequest(idProvincia)
                );

                ddlDistEditar.DataSource = responseDistrito.@return;
                ddlDistEditar.DataTextField = "Nombre";
                ddlDistEditar.DataValueField = "IdDistrito";
                ddlDistEditar.DataBind();
                ddlDistEditar.Items.Insert(0, new ListItem("Seleccione un distrito", ""));
                if (!ddlDistEditar.Enabled) ddlDistEditar.Enabled = true;
            }
            else
            {
                limpiarDistEdicion();
            }
            abrirModalEdicion();
        }

        protected void ddlDistEditar_SelectedIndexChanged(object sender, EventArgs e)
        {
            abrirModalEdicion();
        }

        protected void ddlDepasEditar_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Cuando la provincia cambia, debemos listar los distritos!!!
            if (!string.IsNullOrEmpty(ddlDepasEditar.SelectedValue))
            {
                int idDepa = int.Parse(ddlDepasEditar.SelectedValue);
                listarProvinciaPorDepaResponse responseDepas = provWS.listarProvinciaPorDepa(
                    new listarProvinciaPorDepaRequest(idDepa)
                );

                ddlProvEditar.DataSource = responseDepas.@return;
                ddlProvEditar.DataTextField = "Nombre";
                ddlProvEditar.DataValueField = "IdProvincia";
                ddlProvEditar.DataBind();
                ddlProvEditar.Items.Insert(0, new ListItem("Seleccione una provincia", ""));
                if (!ddlProvEditar.Enabled) ddlProvEditar.Enabled=true;
            }
            else
            {
                ddlProvEditar.Items.Clear();
                ddlProvEditar.Items.Insert(0, new ListItem("Seleccione un departamento primero", ""));
            }
            limpiarDistEdicion();
            abrirModalEdicion();
        }

        public void limpiarDistEdicion()
        {
            // reiniciar distritos
            ddlDistEditar.Items.Clear();
            ddlDistEditar.Items.Insert(0, new ListItem("Seleccione una provincia primero", ""));
        }

        protected void btnEditar_Click(object sender, EventArgs e)
        {
            // ahora tengo que popular el modal con los datos!
            Button btn = (Button)sender;
            string id = btn.CommandArgument;
            int idEvento = int.Parse(id);
            hdnIdEvento.Value = idEvento.ToString();
            eventoDTO eventoEditar = eventoWS.buscarEventoDTOporID(new buscarEventoDTOporIDRequest(idEvento)).@return;

            // llenar los campos con la información del evento traído
            txtNombreEditar.Text = eventoEditar.nombre;
            txtDescEditar.Text = eventoEditar.descripcion;
            txtUbiEditar.Text = eventoEditar.ubicacion;
            txtRefEditar.Text = eventoEditar.referencia;
            txtPrecioEditar.Text = eventoEditar.precioEntrada.ToString();
            txtDispoEditar.Text = eventoEditar.cantEntradasDispo.ToString();
            txtVendEditar.Text = eventoEditar.cantEntradasVendidas.ToString();

            ddlDepasEditar.DataTextField = "Nombre";
            ddlDepasEditar.DataValueField = "IdDepartamento";
            ddlDepasEditar.Items.Insert(0, new ListItem(eventoEditar.nombreProv.ToString(), eventoEditar.idDepa.ToString()));
            ddlDepasEditar.SelectedValue = eventoEditar.idDepa.ToString();
            ddlDepasEditar.Enabled = false;

            ddlProvEditar.DataTextField = "Nombre";
            ddlProvEditar.DataValueField = "IdProvincia";
            ddlProvEditar.Items.Insert(0, new ListItem(eventoEditar.nombreProv.ToString(), eventoEditar.idProv.ToString()));
            ddlProvEditar.SelectedValue = eventoEditar.idProv.ToString();
            ddlProvEditar.Enabled = false;

            ddlDistEditar.DataTextField = "Nombre";
            ddlDistEditar.DataValueField = "IdDistrito";
            ddlDistEditar.Items.Insert(0, new ListItem(eventoEditar.nombreDist.ToString(), eventoEditar.idDist.ToString()));
            ddlDistEditar.SelectedValue = eventoEditar.idDist.ToString();
            ddlDistEditar.Enabled = false;

            // llenar las funciones
            ddlFuncEditar.DataSource = funcionWS.listarFuncionesPorIdEvento(new listarFuncionesPorIdEventoRequest(idEvento)).@return;
            var listaFunciones = funcionWS.listarFuncionesPorIdEvento(
                new listarFuncionesPorIdEventoRequest(idEvento)
            ).@return;

            var listaFormateada = listaFunciones.Select(f => new {
                Texto = $"{f.fecha:yyyy-MM-dd} - {f.horaInicio.Substring(0,5)} a {f.horaFin.Substring(0,5)}",
                Valor = f.idFuncion
            }).ToList();

            ddlFuncEditar.DataSource = listaFormateada;
            ddlFuncEditar.DataTextField = "Texto";
            ddlFuncEditar.DataValueField = "Valor";
            ddlFuncEditar.DataBind();
            ddlFuncEditar.Items.Insert(0, new ListItem("Presione para ver las funciones del evento", ""));
            abrirModalEdicion();
        }
        public void abrirModalEdicion()
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "abrirModalEditar",
            "var modalEvento = new bootstrap.Modal(document.getElementById('modalEditarEvento')); modalEvento.show();", true);
        }

        protected void btnAgregarFuncionEditar_Click(object sender, EventArgs e)
        {
            string fecha = txtFechaEditar.Text.Trim();
            string horaInicio = txtHoraIniEditar.Text.Trim();
            string horaFin = txtHoraFinEditar.Text.Trim();

            string valor = $"{fecha}_{horaInicio}_{horaFin}";
            string texto = $"{fecha} - {horaInicio} a {horaFin}";

            const int MODAL_EDITAR = 2;
            // 2 hace referencia al Modal Editar
            if (!ValidaFuncion(fecha, horaInicio, horaFin, valor, MODAL_EDITAR)) return;
            DateTime.TryParse(fecha, out DateTime fechaSeleccionada);

            ddlFuncEditar.Items.Add(new ListItem(texto, valor));
            ActualizarFechasMinMax(fechaSeleccionada);
            LimpiarFunciones();
            abrirModalEdicion();
        }
        public void ActualizarFechasMinMaxEditar()
        {
            DateTime fechaMin = DateTime.MaxValue;
            DateTime fechaMax = DateTime.MinValue;

            foreach (ListItem item in ddlFuncEditar.Items)
            {
                string texto = item.Text;

                // Validación rápida: salto si el ítem está vacío o no tiene el formato esperado
                if (string.IsNullOrWhiteSpace(texto) || !texto.Contains("-")) continue;

                // Separar por " - " para obtener la fecha
                string[] partes = texto.Split(new[] { " - " }, StringSplitOptions.None);
                if (partes.Length < 2) continue;

                string fechaStr = partes[0].Trim();

                if (DateTime.TryParse(fechaStr, out DateTime fecha))
                {
                    if (fecha < fechaMin) fechaMin = fecha;
                    if (fecha > fechaMax) fechaMax = fecha;
                }
            }

            // Guardar en sesión si encontramos alguna fecha válida
            if (fechaMin != DateTime.MaxValue)
                Session["fechaMinima"] = fechaMin;

            if (fechaMax != DateTime.MinValue)
                Session["fechaMaxima"] = fechaMax;
        }
        protected void btnAceptarEditar_Click(object sender, EventArgs e)
        {
            ActualizarFechasMinMaxEditar();

            if (!ValidarDatosEvento(txtNombreEditar.Text, txtDescEditar.Text, txtUbiEditar.Text, txtPrecioEditar.Text,
                txtDispoEditar.Text, txtVendEditar.Text, txtRefEditar.Text, ddlDepasEditar.Text, ddlDistEditar.Text, ddlProvEditar.Text,true, MODAL_EDITAR)) return;

            string nombreEvento = txtNombreEditar.Text;
            string descripcionEvento = txtDescEditar.Text;
            string ubicacionEvento = txtUbiEditar.Text;
            string idDistritoEvento = ddlDistEditar.SelectedValue; // obteniendo ID de distrito seleccionado
            string precioEntradaEvento = txtPrecioEditar.Text;
            string cantDisponibles = txtDispoEditar.Text;
            string cantVendidas = txtVendEditar.Text;
            string referenciaEvento = txtRefEditar.Text;
            
            string fechaInicioEvento = Session["fechaMinima"].ToString();
            string fechaFinEvento = Session["fechaMaxima"].ToString();

            evento eventoActualizar = new evento
            {
                idEvento = int.Parse(hdnIdEvento.Value),
                fecha_inicio = fechaInicioEvento.Split(' ')[0],
                fecha_fin = fechaFinEvento.Split(' ')[0],
                nombre = nombreEvento,
                descripcion = descripcionEvento,
                ubicacion = ubicacionEvento,
                distrito = new distrito
                {
                    idDistrito = int.Parse(idDistritoEvento)
                },
                precioEntrada = double.Parse(precioEntradaEvento),
                cantEntradasVendidas = int.Parse(cantVendidas),
                cantEntradasDispo = int.Parse(cantDisponibles),
                referencia = referenciaEvento
            };

            Boolean actualizado = eventoWS.actualizarEvento(new actualizarEventoRequest(eventoActualizar)).@return;

            if (actualizado)
            {
                mostrarModalExitoEvento("VENTANA DE ÉXITO", "EVENTO actualizado exitosamente");
                CargarEventos();
            }
            else
            {
                mostrarModalErrorEvento("VENTANA DE ERROR", "Ocurrió un problema al actualizar el EVENTO");
            }
        }
        public void mostrarModalExitoEvento(string titulo, string mensaje)
        {
            string script = $@"
                Sys.Application.add_load(function () {{
                    mostrarModalExito('{titulo}', '{mensaje}');
                }});
            ";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "mostrarModalExito", script, true);
        }

        public void mostrarModalErrorEvento(string titulo, string mensaje)
        {
            string script = $@"
                Sys.Application.add_load(function () {{
                    mostrarModalError('{titulo}', '{mensaje}');
                }});
            ";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "mostrarModalError", script, true);
        }

        protected void btnEditUbigeo_Click(object sender, EventArgs e)
        {
            ddlDepasEditar.Enabled = true;
            ddlDepasEditar.DataSource = depaWS.listarDepas(new listarDepasRequest()).@return;
            ddlDepasEditar.DataBind();
            ddlDepasEditar.Items.Insert(0, new ListItem("Seleccione un departamento", ""));
            abrirModalEdicion();
        }

        public void mostrarModalFoto(string dataUrl)
        {
            // script para cambiar src del <img> y mostrar el modal
            string script = $@"
            Sys.Application.add_load(function () {{
                document.getElementById('imgPreviewModal').src = '{dataUrl}';
                var myModal = new bootstrap.Modal(document.getElementById('modalPreview'));
                myModal.show();
            }});";

            ScriptManager.RegisterStartupScript(this, this.GetType(), "mostrarPreview", script, true);
        }

        protected void lnkVerImagen_Command(object sender, CommandEventArgs e)
        {
            string ruta = e.CommandArgument as string;

            // Si no hay ruta o es una cadena vacía, no hacemos nada
            if (string.IsNullOrWhiteSpace(ruta) || ruta == "null")
                return;

            mostrarModalFoto("/" + ruta); // Asegúrate de que comience con "/"
        }
    }
}