using SirgepPresentacion.ReferenciaDisco;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SirgepPresentacion.Presentacion.Infraestructura.Evento
{
    public partial class GestionaEventos : System.Web.UI.Page
    {
        private EventoWS eventoWS;
        private FuncionWS funcionWS;
        private DepartamentoWS depaWS;
        private ProvinciaWS provWS;
        private DistritoWS distWS;

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
                CargarEventos();
            }
        }

        protected void CargarEventos()
        {
            rptEventos.DataSource = eventoWS.listarEvento(new listarEventoRequest()).@return;
            rptEventos.DataBind();
        }

        protected void ddlFiltroFechas_SelectedIndexChanged(object sender, EventArgs e)
        {

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

            ScriptManager.RegisterStartupScript(this, this.GetType(), "abrirModalEvento",
        "var modalEvento = new bootstrap.Modal(document.getElementById('modalAgregarEvento')); modalEvento.show();", true);
        }

        protected void txtBusqueda_TextChanged(object sender, EventArgs e)
        {
            buscarEventoPorTextoResponse response = eventoWS.buscarEventoPorTexto(new buscarEventoPorTextoRequest(txtBusqueda.Text));
            rptEventos.DataSource = response.@return;
            rptEventos.DataBind();
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
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
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
                referencia = referenciaEvento
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
                        Console.WriteLine("Error al insertar la función con ID: " + idFuncion);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "modalError",
                        "var modalEvento = bootstrap.Modal.getInstance(document.getElementById('modalError')); modalEvento.show();", true);
                        return;
                    }
                }

                ScriptManager.RegisterStartupScript(this, this.GetType(), "modalExito",
                    "var modalEvento = bootstrap.Modal.getInstance(document.getElementById('modalExito')); modalEvento.hide();", true);
                CargarEventos();
            }
            else
            {
                // error al insertar
                ScriptManager.RegisterStartupScript(this, this.GetType(), "modalError",
                    "var modalEvento = bootstrap.Modal.getInstance(document.getElementById('modalError')); modalEvento.show();", true);
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
            ScriptManager.RegisterStartupScript(this, this.GetType(), "abrirModalEvento",
            "var modalEvento = new bootstrap.Modal(document.getElementById('modalAgregarEvento')); modalEvento.show();", true);
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
            ScriptManager.RegisterStartupScript(this, this.GetType(), "abrirModalEvento",
            "var modalEvento = new bootstrap.Modal(document.getElementById('modalAgregarEvento')); modalEvento.show();", true);
        }

        protected void btnAgregarFuncion_Click(object sender, EventArgs e)
        {
            string fecha = txtFechaFuncion.Text.Trim();
            string horaInicio = txtHoraIniFuncion.Text.Trim();
            string horaFin = txtHoraFinFuncion.Text.Trim();

            string valor = $"{fecha}_{horaInicio}_{horaFin}";
            string texto = $"{fecha} - {horaInicio} a {horaFin}";

            // Validación: campos vacíos
            if (string.IsNullOrWhiteSpace(fecha) || string.IsNullOrWhiteSpace(horaInicio) || string.IsNullOrWhiteSpace(horaFin))
            {
                MostrarError("Por favor complete todos los campos de la función.");
                return;
            }

            // Validación: fecha en el mismo año y no pasada
            if (!DateTime.TryParse(fecha, out DateTime fechaSeleccionada))
            {
                MostrarError("La fecha no tiene un formato válido.");
                return;
            }

            DateTime hoy = DateTime.Today;
            if (fechaSeleccionada.Year != hoy.Year)
            {
                MostrarError("La fecha debe estar en el año actual.");
                return;
            }

            if (fechaSeleccionada < hoy)
            {
                MostrarError("La fecha no puede ser anterior a hoy.");
                return;
            }

            // Validación: hora inicio < hora fin
            if (!TimeSpan.TryParse(horaInicio, out TimeSpan tsInicio) || !TimeSpan.TryParse(horaFin, out TimeSpan tsFin))
            {
                MostrarError("Las horas no tienen un formato válido.");
                return;
            }

            if (tsInicio >= tsFin)
            {
                MostrarError("La hora de inicio debe ser menor a la hora de fin.");
                return;
            }

            // validación de duplicados
            if (ddlFuncionesAgregar.Items.Cast<ListItem>().Any(i => i.Value == valor))
            {
                MostrarError("Esa función ya ha sido agregada.");
                return;
            }

            ddlFuncionesAgregar.Items.Add(new ListItem(texto, valor));
            ActualizarFechasMinMax(fechaSeleccionada);
            LimpiarFunciones();
            // Para mantener el modal abierto tras el postback
            ScriptManager.RegisterStartupScript(this, this.GetType(), "abrirModalEvento",
            "var modalEvento = new bootstrap.Modal(document.getElementById('modalAgregarEvento')); modalEvento.show();", true);
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

        private void MostrarError(string mensaje)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "errorMsg", $"alert('{mensaje}');", true);
            // Para mantener el modal abierto tras el postback
            ScriptManager.RegisterStartupScript(this, this.GetType(), "abrirModalEvento",
            "var modalEvento = new bootstrap.Modal(document.getElementById('modalAgregarEvento')); modalEvento.show();", true);
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

        protected void btnConsultar_Click(object sender, EventArgs e)
        {
            string fechaIniFiltro = txtFechaInicioFiltro.Text;
            string fechaFinFiltro = txtFechaFinFiltro.Text;
            buscarEventosPorFechasResponse rsp =  eventoWS.buscarEventosPorFechas( new buscarEventosPorFechasRequest(fechaIniFiltro,fechaFinFiltro) );
            if(rsp.@return != null)
            {
                rptEventos.DataSource = rsp.@return;
                rptEventos.DataBind();
            }
            else
            {
                MostrarError("Hubo un error al listar los eventos mediante el filtro de fechas");
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
                ScriptManager.RegisterStartupScript(this, GetType(), "mostrarModalError",
                "mostrarModalPorId('modalError');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "mostrarModalExito",
                "mostrarModalPorId('modalExito');", true);

                ScriptManager.RegisterStartupScript(this, this.GetType(), "exitoEliminarEvento", $"alert('Evento eliminado EXITOSAMENTE');", true);
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
            evento eventoEditar = eventoWS.buscarEventoPorID(new buscarEventoPorIDRequest(idEvento)).@return;

            // llenar los campos con la información del evento traído
            txtNombreEditar.Text = eventoEditar.nombre;
            txtDescEditar.Text = eventoEditar.descripcion;
            txtUbiEditar.Text = eventoEditar.ubicacion;
            txtRefEditar.Text = eventoEditar.referencia;
            txtPrecioEditar.Text = eventoEditar.precioEntrada.ToString();
            txtDispoEditar.Text = eventoEditar.cantEntradasDispo.ToString();
            txtVendEditar.Text = eventoEditar.cantEntradasVendidas.ToString();

            // cargar objetos a mostrar
            distrito dist = distWS.buscarDistPorId(new buscarDistPorIdRequest(eventoEditar.distrito.idDistrito)).@return;
            provincia prov = provWS.buscarProvinciaPorId(new buscarProvinciaPorIdRequest(dist.provincia.idProvincia)).@return;
            departamento depa = depaWS.buscarDepaPorId(new buscarDepaPorIdRequest(prov.departamento.idDepartamento)).@return;

            // llenar distrito
            ddlDistEditar.DataSource = distWS.listarDistritosFiltrados(new listarDistritosFiltradosRequest(depa.idDepartamento)).@return;
            ddlDistEditar.DataTextField = "Nombre";
            ddlDistEditar.DataValueField = "IdDistrito";
            ddlDistEditar.DataBind();
            ddlDistEditar.SelectedValue = dist.idDistrito.ToString();

            // llenar la provincia
            ddlProvEditar.DataSource = provWS.listarProvinciaPorDepa(new listarProvinciaPorDepaRequest(depa.idDepartamento)).@return;
            ddlProvEditar.DataTextField = "Nombre";
            ddlProvEditar.DataValueField = "IdProvincia";
            ddlProvEditar.DataBind();
            ddlProvEditar.SelectedValue = prov.idProvincia.ToString();

            // llenar el departamento
            ddlDepasEditar.DataSource = depaWS.listarDepas(new listarDepasRequest()).@return;
            ddlDepasEditar.DataTextField = "Nombre";
            ddlDepasEditar.DataValueField = "IdDepartamento";
            ddlDepasEditar.DataBind();
            ddlDepasEditar.SelectedValue = depa.idDepartamento.ToString();

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

            // Validación: campos vacíos
            if (string.IsNullOrWhiteSpace(fecha) || string.IsNullOrWhiteSpace(horaInicio) || string.IsNullOrWhiteSpace(horaFin))
            {
                MostrarError("Por favor complete todos los campos de la función.");
                return;
            }

            // Validación: fecha en el mismo año y no pasada
            if (!DateTime.TryParse(fecha, out DateTime fechaSeleccionada))
            {
                MostrarError("La fecha no tiene un formato válido.");
                return;
            }

            DateTime hoy = DateTime.Today;
            if (fechaSeleccionada.Year != hoy.Year)
            {
                MostrarError("La fecha debe estar en el año actual.");
                return;
            }

            if (fechaSeleccionada < hoy)
            {
                MostrarError("La fecha no puede ser anterior a hoy.");
                return;
            }

            // Validación: hora inicio < hora fin
            if (!TimeSpan.TryParse(horaInicio, out TimeSpan tsInicio) || !TimeSpan.TryParse(horaFin, out TimeSpan tsFin))
            {
                MostrarError("Las horas no tienen un formato válido.");
                return;
            }

            if (tsInicio >= tsFin)
            {
                MostrarError("La hora de inicio debe ser menor a la hora de fin.");
                return;
            }

            // validación de duplicados
            if (ddlFuncionesAgregar.Items.Cast<ListItem>().Any(i => i.Value == valor))
            {
                MostrarError("Esa función ya ha sido agregada.");
                return;
            }

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
            string fechaInicioEvento = Session["fechaMinima"].ToString();
            string fechaFinEvento = Session["fechaMaxima"].ToString();
            string nombreEvento = txtNombreEditar.Text;
            string descripcionEvento = txtDescEditar.Text;
            string ubicacionEvento = txtUbiEditar.Text;
            string idDistritoEvento = ddlDistEditar.SelectedValue; // obteniendo ID de distrito seleccionado
            string precioEntradaEvento = txtPrecioEditar.Text;
            string cantDisponibles = txtDispoEditar.Text;
            string cantVendidas = txtVendEditar.Text;
            string referenciaEvento = txtRefEditar.Text;

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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertActualizar",
                    "alert('EVENTO actualizado exitosamente');", true);
                CargarEventos();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertActualizar",
                    "alert('Ocurrió un problema al actualizar el EVENTO');", true);
            }
        }
    }
}