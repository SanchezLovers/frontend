using DocumentFormat.OpenXml.Wordprocessing;
using SirgepPresentacion.ReferenciaDisco;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ListItem = System.Web.UI.WebControls.ListItem;

namespace SirgepPresentacion.Presentacion.Infraestructura.Espacio
{
    public partial class ListaEspacios : System.Web.UI.Page
    {
        private EspacioWS espacioWS;
        private DistritoWS distritoWS;
        private DepartamentoWS departamentoWS;
        private ProvinciaWS provinciaWS;
        private EspacioDiaSemWS diaSemWS;

        protected void Page_Init(object sender, EventArgs e)
        {
            espacioWS = new EspacioWSClient();
            distritoWS = new DistritoWSClient();
            departamentoWS = new DepartamentoWSClient();
            provinciaWS = new ProvinciaWSClient();
            diaSemWS = new EspacioDiaSemWSClient();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarEspacios();
                CargarDistritos();
                CargarDepas();
            }

        }
        public void CargarDepas()
        {
            listarDepasResponse responseDepas = departamentoWS.listarDepas(new listarDepasRequest());
            ddlDepartamentoAgregar.DataSource = responseDepas.@return;
            ddlDepartamentoAgregar.DataTextField = "Nombre";
            ddlDepartamentoAgregar.DataValueField = "IdDepartamento";
            ddlDepartamentoAgregar.DataBind();

            ddlDepartamentoEdit.DataSource = responseDepas.@return;
            ddlDepartamentoEdit.DataTextField = "Nombre";
            ddlDepartamentoEdit.DataValueField = "IdDepartamento";
            ddlDepartamentoEdit.DataBind();

            /* Hardcodeado pq' no lo tenemos en nuestra BD, además hay pocos datos */
            ddlTipoEspacioAgregar.Items.Insert(0, new ListItem("Seleccione un tipo", ""));
            ddlTipoEspacioAgregar.Items.Insert(1, new ListItem("Teatros", "TEATRO"));
            ddlTipoEspacioAgregar.Items.Insert(2, new ListItem("Canchas", "CANCHA"));
            ddlTipoEspacioAgregar.Items.Insert(3, new ListItem("Salones", "SALON"));
            ddlTipoEspacioAgregar.Items.Insert(4, new ListItem("Parques", "PARQUE"));

            if (ddlProvinciaAgregar.Items.Count == 0)
                ddlProvinciaAgregar.Items.Insert(0, new ListItem("Seleccione una provincia", ""));
            if (ddlDistritoAgregar.Items.Count == 0)
                ddlDistritoAgregar.Items.Insert(0, new ListItem("Seleccione un distrito", ""));
        }
        private void CargarDistritos()
        {
            ddlDistrito.DataSource = distritoWS.listarTodosDistritos(new listarTodosDistritosRequest()).@return;
            ddlDistrito.DataTextField = "Nombre";
            ddlDistrito.DataValueField = "IdDistrito";
            ddlDistrito.DataBind();
            if (ddlDistrito.Items[0].Text != "Seleccione un distrito")
                ddlDistrito.Items.Insert(0, new ListItem("Seleccione un distrito", ""));
        }
        private void CargarEspacios()
        {
            listarEspacioResponse response = espacioWS.listarEspacio(new listarEspacioRequest());
            rptEspacios.DataSource = response.@return;
            rptEspacios.DataBind();
        }
        protected void btnAgregarEspacio_Click(object sender, EventArgs e)
        {
            // Limpiar campos si es necesario
            txtNombreEspacioAgregar.Text = "";
            txtUbicacionAgregar.Text = "";
            txtSuperficieAgregar.Text = "";
            txtPrecioReservaAgregar.Text = "";
            txtHoraFinInsert.Text = "";
            txtHoraInicioInsert.Text = "";

            string test = ddlDepartamentoAgregar.Items[0].ToString();
            if (ddlDepartamentoAgregar.Items[0].ToString() != "Escoja un departamento")
                ddlDepartamentoAgregar.Items.Insert(0, new ListItem("Escoja un departamento", ""));
            
            // Asegurarnos de que apunten al primer index
            ddlTipoEspacioAgregar.SelectedIndex = 0;
            ddlDepartamentoAgregar.SelectedIndex = 0;
            ddlProvinciaAgregar.SelectedIndex = 0;
            ddlDistritoAgregar.SelectedIndex = 0;

            // Mostrar el modal usando JavaScript
            abrirModalAgregarEspacio();
        }
        public void MarcarDiasCheck(espacioDiaSem[] dias)
        {
            // Crear una tabla hash con los días atendidos (en mayúsculas para estandarizar)
            HashSet<string> diasAtendidos = new HashSet<string>();
            foreach (espacioDiaSem e in dias)
            {
                diasAtendidos.Add(e.dia.ToString().ToUpperInvariant());
            }

            // Marcar los checkboxes si el día está en el HashSet
            lunesEdit.Checked = diasAtendidos.Contains("LUNES");
            martesEdit.Checked = diasAtendidos.Contains("MARTES");
            miercolesEdit.Checked = diasAtendidos.Contains("MIERCOLES");
            juevesEdit.Checked = diasAtendidos.Contains("JUEVES");
            viernesEdit.Checked = diasAtendidos.Contains("VIERNES");
            sabadoEdit.Checked = diasAtendidos.Contains("SABADO");
            domingoEdit.Checked = diasAtendidos.Contains("DOMINGO");
        }
        protected void btnEditar_Click(object sender, EventArgs e)
        {

            /*CARGAR DATOS*/
            int idEspacio = int.Parse(((Button)sender).CommandArgument.ToString());
            hiddenIdEspacio.Value = idEspacio.ToString();

            espacioDTO espDTO = espacioWS.obtenerEspacioDTO(new obtenerEspacioDTORequest(idEspacio)).@return;
            hiddenIdDistrito.Value = espDTO.idDistrito.ToString();

            /* Hardcodeado pq' no lo tenemos en nuestra BD, además hay pocos datos */
            ddlTipoEspacioEdit.Items.Clear();
            ddlTipoEspacioEdit.Items.Insert(0, new ListItem(espDTO.tipo.ToString(), espDTO.tipo.ToString()));
            ddlTipoEspacioEdit.Items.Insert(1, new ListItem("Teatros", "TEATRO"));
            ddlTipoEspacioEdit.Items.Insert(2, new ListItem("Canchas", "CANCHA"));
            ddlTipoEspacioEdit.Items.Insert(3, new ListItem("Salones", "SALON"));
            ddlTipoEspacioEdit.Items.Insert(4, new ListItem("Parques", "PARQUE"));

            // Leer el valor
            txtNombreEdit.Text = espDTO.nombre;
            txtUbicacionEdit.Text = espDTO.ubicacion;
            txtSuperficieEdit.Text = espDTO.superficie.ToString();
            txtPrecioEdit.Text = espDTO.precioReserva.ToString();
            
            ddlDepartamentoEdit.SelectedValue = espDTO.idDepartamento.ToString();

            ddlProvinciaEdit.DataSource = espDTO.provincias;
            ddlProvinciaEdit.DataTextField = "Nombre";
            ddlProvinciaEdit.DataValueField = "IdProvincia";
            ddlProvinciaEdit.DataBind();
            ddlProvinciaEdit.SelectedValue = espDTO.idProvincia.ToString();

            ddlDistritoEdit.DataSource = espDTO.distritos;
            ddlDistritoEdit.DataTextField = "Nombre";
            ddlDistritoEdit.DataValueField = "IdDistrito";
            ddlDistritoEdit.DataBind();
            ddlDistritoEdit.SelectedValue = espDTO.idDistrito.ToString();

            txtHoraInicioEdit.Text = espDTO.horaInicio.ToString();
            txtHoraFinEdit.Text = espDTO.horaFin.ToString();

            // Cargar los días que el Espacio atiende
            espacioDiaSem[] diasSem = espDTO.dias;
            MarcarDiasCheck(diasSem);

            // Mostrar el modal usando JavaScript
            abrirModalEditarEspacio();
        }

        protected void btnConsultar_Click(object sender, EventArgs e)
        {
            // Text ya es el VALOR y es un string
            string filtroCat = ddlCategoria.Text;
            string filtroDist = ddlDistrito.Text;
            if (filtroCat != "" && filtroDist != "")
            {
                // llamamos a la filtración doble implementada en backend
                listarEspacioDistyCatResponse response = null;
                response = espacioWS.listarEspacioDistyCat(new listarEspacioDistyCatRequest(int.Parse(filtroDist), filtroCat));
                rptEspacios.DataSource = response.@return;
            }
            else if (filtroCat != "")
            {
                listarEspacioPorCategoriaResponse responseFiltroCat = espacioWS.listarEspacioPorCategoria(new listarEspacioPorCategoriaRequest(filtroCat));
                rptEspacios.DataSource = responseFiltroCat.@return;
            }
            else if (filtroDist != "")
            {
                int idDistrito = int.Parse(filtroDist);
                listarEspacioPorDistritoResponse responseFiltroDist = espacioWS.listarEspacioPorDistrito(new listarEspacioPorDistritoRequest(idDistrito));
                rptEspacios.DataSource = responseFiltroDist.@return;
            }
            else CargarEspacios();
            rptEspacios.DataBind();
            // el último caso, es por si no se quieren filtros
        }

        protected void btnConfirmarAccion_Click(object sender, EventArgs e)
        {
            int id = int.Parse(hdnIdAEliminar.Value);

            eliminarLogicoResponse response = espacioWS.eliminarLogico(new eliminarLogicoRequest(id));

            Boolean estado = response.@return;

            if (estado)
            {
                // debemos eliminar también los días de la semana del espacio
                Boolean diaEliminado = diaSemWS.eliminarDiasPorEspacio(new eliminarDiasPorEspacioRequest(id)).@return;
                if (diaEliminado)
                {
                    mostrarModalExitoEsp("ÉXITO AL ELIMINAR EL ESPACIO", "Los días del espacio y el espacio mismo han sido eliminados satisfactoriamente.");
                    CargarEspacios(); // Refresca la tabla
                }
                else
                {
                    mostrarModalErrorEsp("FALLO AL ELIMINAR Dias del Espacio", "Ocurrió un error al momento de eliminar los días del espacio.");
                }
                
            }
            else
            {
                mostrarModalErrorEsp("FALLO AL ELIMINAR ESPACIO", "Ocurrió un error al momento de eliminar el espacio.");
            }

        }

        protected void txtBusqueda_TextChanged(object sender, EventArgs e)
        {
            buscarEspacioPorTextoResponse response = espacioWS.buscarEspacioPorTexto(new buscarEspacioPorTextoRequest(txtBusqueda.Text));
            rptEspacios.DataSource = response.@return;
            rptEspacios.DataBind();
        }

        protected void ddlDepartamentoAgregar_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlDepartamentoAgregar.SelectedValue))
            {
                int idDepartamento = int.Parse(ddlDepartamentoAgregar.SelectedValue);
                listarProvinciaPorDepaResponse responseProvincia = provinciaWS.listarProvinciaPorDepa(
                    new listarProvinciaPorDepaRequest(idDepartamento)
                );

                ddlProvinciaAgregar.DataSource = responseProvincia.@return;
                ddlProvinciaAgregar.DataTextField = "Nombre";
                ddlProvinciaAgregar.DataValueField = "IdProvincia";
                ddlProvinciaAgregar.DataBind();
                ddlProvinciaAgregar.Items.Insert(0, new ListItem("Seleccione una provincia", ""));
            }
            else
            {
                ddlProvinciaAgregar.Items.Clear();
                ddlProvinciaAgregar.Items.Insert(0, new ListItem("Seleccione un departamento primero", ""));
            }

            // Para mantener el modal abierto tras el postback
            abrirModalAgregarEspacio();
            
        }

        protected void ddlProvinciaAgregar_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlProvinciaAgregar.SelectedValue))
            {
                int idProvincia = int.Parse(ddlProvinciaAgregar.SelectedValue);
                listarDistritosFiltradosResponse responseDistrito = distritoWS.listarDistritosFiltrados(
                    new listarDistritosFiltradosRequest(idProvincia)
                );

                ddlDistritoAgregar.DataSource = responseDistrito.@return;
                ddlDistritoAgregar.DataTextField = "Nombre";
                ddlDistritoAgregar.DataValueField = "IdDistrito";
                ddlDistritoAgregar.DataBind();
                ddlDistritoAgregar.Items.Insert(0, new ListItem("Seleccione un distrito", ""));
            }
            else
            {
                ddlDistritoAgregar.Items.Clear();
                ddlDistritoAgregar.Items.Insert(0, new ListItem("Seleccione una provincia primero", ""));
            }

            // Para que el modal siga abierto
            abrirModalAgregarEspacio();
        }

        protected void btnActualizarEspacioEdit_Click(object sender, EventArgs e)
        {

            // Capturar ID del espacio
            int idEspacio = int.Parse(hiddenIdEspacio.Value);
            int idDistritoHdn = int.Parse(hiddenIdDistrito.Value);
            // Leer datos actualizados desde controles del modal
            string nombre = txtNombreEdit.Text.Trim();
            string ubicacion = txtUbicacionEdit.Text.Trim();
            double superficie = double.Parse(txtSuperficieEdit.Text);
            double precioReserva = double.Parse(txtPrecioEdit.Text);
            string tipoEspacioInsumo = ddlTipoEspacioEdit.SelectedValue;
            string horaIni = txtHoraInicioEdit.Text;
            string horaFin = txtHoraFinEdit.Text;

            if (horaIni.Length==5) horaIni += ":00";
            if (horaFin.Length==5) horaFin += ":00";

            eTipoEspacio eTipo;
            eTipoEspacio.TryParse(tipoEspacioInsumo, false, out eTipo);


            espacio espacioActualizar = new espacio()
            {
                idEspacio = idEspacio,
                nombre = nombre,
                ubicacion = ubicacion,
                superficie = superficie,
                precioReserva = precioReserva,
                tipoEspacio = eTipo,
                horarioInicioAtencion = horaIni,
                horarioFinAtencion = horaFin,
                distrito = new distrito()
                {
                    idDistrito = idDistritoHdn
                }
                // asignar departamentos, horarios, etc.
            };

            espacioActualizar.tipoEspacioSpecified = true; // Forzar la serialización

            Boolean actualizado = espacioWS.actualizarEspacio(new actualizarEspacioRequest(espacioActualizar)).@return;

            if (actualizado)
            {
                mostrarModalExitoEsp("ACTUALIZACIÓN EXITOSA", "Espacio actualizado exitosamente.");
                CargarEspacios();
            }
            else
            {
                mostrarModalErrorEsp("ERROR AL ACTUALIZAR ESPACIO", "Ocurrió un error al momento de actualizar el espacio.");
            }
        }

        protected void btnGuardarInsertado_Click(object sender, EventArgs e)
        {
            // Capturar ID del espacio
            int idDistritoHdn = int.Parse(hiddenIdDistrito.Value);
            // Leer datos actualizados desde controles del modal
            string nombreInsert = txtNombreEspacioAgregar.Text.Trim();
            string ubicacionInsert = txtUbicacionAgregar.Text.Trim();
            double superficieInsert = double.Parse(txtSuperficieAgregar.Text);
            double precioReservaInsert = double.Parse(txtPrecioReservaAgregar.Text);
            string tipoEspacioInsumoInsert = ddlTipoEspacioAgregar.SelectedValue;
            string horaIniInsert = txtHoraInicioInsert.Text;
            string horaFinInsert = txtHoraFinInsert.Text;

            eTipoEspacio eTipo;
            eTipoEspacio.TryParse(tipoEspacioInsumoInsert, false, out eTipo);
            espacio espacioInsertar = new espacio()
            {
                idEspacio = 0,
                nombre = nombreInsert,
                ubicacion = ubicacionInsert,
                superficie = superficieInsert,
                precioReserva = precioReservaInsert,
                tipoEspacio = eTipo,
                horarioInicioAtencion = horaIniInsert + ":00",
                horarioFinAtencion = horaFinInsert + ":00",
                distrito = new distrito()
                {
                    idDistrito = idDistritoHdn
                }
            };

            espacioInsertar.tipoEspacioSpecified = true; // Forzar la serialización

            int insertado = espacioWS.insertarEspacio(new insertarEspacioRequest(espacioInsertar)).@return;

            if (insertado > 0)
            {
                // Ahora debemos insertar los días en los que estará funcionando el espacio
                string dias = diasSeleccionados.Value; // <== Accede directamente al valor del input oculto

                if (!string.IsNullOrWhiteSpace(dias))
                {
                    string[] diasArray = dias.Split(',');

                    foreach (string diaSem in diasArray)
                    {
                        eDiaSemana diaSemParsed;
                        eDiaSemana.TryParse<eDiaSemana>(diaSem, ignoreCase: true, out diaSemParsed);
                        // Insertamos cada día que se escogió
                        espacioDiaSem espacioDiaSem = new espacioDiaSem()
                        {
                            idEspacio = insertado,
                            dia = diaSemParsed
                        };
                        espacioDiaSem.diaSpecified = true;
                        Boolean diaInsertado = diaSemWS.insertarDia(new insertarDiaRequest(espacioDiaSem)).@return;
                        if (!diaInsertado)
                        {
                            // Mensaje de fallo
                            mostrarModalErrorEsp("ERROR AL INSERTAR DIA de SEMANA", "Se produjo un error al insertar el día de la semana del espacio.");
                            return;
                        }
                    }
                    //mostrarModalExitoEsp("ÉXITO", "Días insertados correctamente.");
                }

                // Mensaje de éxito
                mostrarModalExitoEsp("ESPACIO INSERTADO CON ÉXITO", "Se ha guardado el espacio satisfactoriamente.");
                CargarEspacios();
                string nombreDistrito = distritoWS.buscarDistPorId(new buscarDistPorIdRequest(espacioInsertar.distrito.idDistrito)).@return.nombre;
                string asunto = $"¡Nuevo espacio disponible en tu distrito favorito {nombreDistrito}: {espacioInsertar.nombre}!";
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
                          <h2>¡Nuevo espacio en tu distrito favorito {nombreDistrito}!</h2>
                        </div>
                        <div class='body'>
                          <p>Estimado usuario,</p>
                          <p>Nos complace informarte que se ha registrado un nuevo espacio disponible en tu distrito favorito: <strong>{nombreDistrito}</strong>.</p>
                          <ul class='details'>
                            <li><strong>📌 Nombre:</strong> {espacioInsertar.nombre}</li>
                            <li><strong>🏷 Tipo:</strong> {espacioInsertar.tipoEspacio}</li>
                            <li><strong>📍 Ubicación:</strong> {espacioInsertar.ubicacion}</li>
                            <li><strong>📐 Superficie:</strong> {espacioInsertar.superficie} m²</li>
                            <li><strong>💵 Precio de Reserva:</strong> S/ {espacioInsertar.precioReserva}</li>
                            <li><strong>⏰ Horario de Atención:</strong> {espacioInsertar.horarioInicioAtencion} - {espacioInsertar.horarioFinAtencion}</li>
                          </ul>
                          <p>Si estás interesado, te invitamos a revisar más detalles o reservar tu espacio.</p>
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
                bool resultado = espacioWS.enviarCorreosCompradoresPorDistritoDeEspacio(new enviarCorreosCompradoresPorDistritoDeEspacioRequest(asunto, contenido, espacioInsertar.distrito.idDistrito)).@return;
                if (resultado)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "mostrarModal", "setTimeout(function() { " +
                        "mostrarModalExito('Correos enviados exitosamente', 'Se enviaron correos a los compradores cuyo distrito favorito coincide con el distrito del espacio registrado.'); " +
                        "}, 1000);", true);
                }
            }
            else
            {
                // Mensaje de fallo
                mostrarModalErrorEsp("ERROR AL INSERTAR ESPACIO", "Se produjo un error al insertar el espacio.");
            }
        }

        protected void ddlDistritoAgregar_SelectedIndexChanged(object sender, EventArgs e)
        {
            hiddenIdDistrito.Value = ddlDistritoAgregar.SelectedValue.ToString();
        }

        protected void txtHoraFinInsert_TextChanged(object sender, EventArgs e)
        {
            string horaInicioStr = txtHoraInicioInsert.Text.Trim();
            string horaFinStr = txtHoraFinInsert.Text.Trim();

            // Verificar que ambas no estén vacías
            if (string.IsNullOrEmpty(horaInicioStr) || string.IsNullOrEmpty(horaFinStr))
            {
                lblError.Text = "Las horas no pueden estar vacías.";
                // Para que el modal siga abierto
                abrirModalAgregarEspacio();
                return;
            }

            TimeSpan horaInicio = TimeSpan.Parse(txtHoraInicioInsert.Text);
            TimeSpan horaFin = TimeSpan.Parse(txtHoraFinInsert.Text);

            if (horaInicio.Minutes != 0 || horaFin.Minutes != 0)
            {
                lblError.Text = "Las horas deben finalizar en :00";
                // Para que el modal siga abierto
                abrirModalAgregarEspacio();
                return;
            }

            // Parsear a TimeSpan

            bool inicioOk = TimeSpan.TryParse(horaInicioStr, out horaInicio);
            bool finOk = TimeSpan.TryParse(horaFinStr, out horaFin);

            if (!inicioOk || !finOk)
            {
                lblError.Text = "Formato de hora no válido.";
                // Para que el modal siga abierto
                abrirModalAgregarEspacio();
                return;
            }

            // Validar que hora inicio < hora fin
            if (horaInicio >= horaFin)
            {
                lblError.Text = "La hora de inicio debe ser menor que la hora de fin.";
                // Para que el modal siga abierto
                abrirModalAgregarEspacio();
                return;
            }

            // Si todo está bien, borrar el error
            lblError.Text = "";
            // Para que el modal siga abierto
            abrirModalAgregarEspacio();
        }
        public void abrirModalAgregarEspacio()
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "abrirModalPaso1",
                    "var modal1 = new bootstrap.Modal(document.getElementById('modalPaso1')); modal1.show();", true);
        }

        public void abrirModalEditarEspacio()
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "abrirModalEdicion",
                "var modalEdicion = new bootstrap.Modal(document.getElementById('modalEdicionEspacio')); modalEdicion.show();", true);
        }

        public void mostrarModalExitoEsp(string titulo, string mensaje)
        {
            string script = $@"
                Sys.Application.add_load(function () {{
                    mostrarModalExito('{titulo}', '{mensaje}');
                }});
            ";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "mostrarModalExito", script, true);
        }

        public void mostrarModalErrorEsp(string titulo, string mensaje)
        {
            string script = $@"
                Sys.Application.add_load(function () {{
                    mostrarModalExito('{titulo}', '{mensaje}');
                }});
            ";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "mostrarModalError", script, true);
        }

        protected void ddlDepartamentoEdit_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlDepartamentoEdit.SelectedValue))
            {
                int idDepartamento = int.Parse(ddlDepartamentoEdit.SelectedValue);
                listarProvinciaPorDepaResponse responseProvincia = provinciaWS.listarProvinciaPorDepa(
                    new listarProvinciaPorDepaRequest(idDepartamento)
                );

                ddlProvinciaEdit.DataSource = responseProvincia.@return;
                ddlProvinciaEdit.DataTextField = "Nombre";
                ddlProvinciaEdit.DataValueField = "IdProvincia";
                ddlProvinciaEdit.DataBind();
                ddlProvinciaEdit.Items.Insert(0, new ListItem("Seleccione una provincia", ""));
                ddlDistritoEdit.Items.Clear();
                ddlDistritoEdit.Items.Insert(0, new ListItem("Seleccione una provincia primero", ""));
            }
            else
            {
                ddlProvinciaEdit.Items.Clear();
                ddlProvinciaEdit.Items.Insert(0, new ListItem("Seleccione un departamento primero", ""));
            }

            // Para mantener el modal abierto tras el postback
            abrirModalEditarEspacio();
        }

        protected void ddlProvinciaEdit_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlProvinciaEdit.SelectedValue))
            {
                int idProvincia = int.Parse(ddlProvinciaEdit.SelectedValue);
                listarDistritosFiltradosResponse responseDistrito = distritoWS.listarDistritosFiltrados(
                    new listarDistritosFiltradosRequest(idProvincia)
                );

                ddlDistritoEdit.DataSource = responseDistrito.@return;
                ddlDistritoEdit.DataTextField = "Nombre";
                ddlDistritoEdit.DataValueField = "IdDistrito";
                ddlDistritoEdit.DataBind();
                ddlDistritoEdit.Items.Insert(0, new ListItem("Seleccione un distrito", ""));
            }
            else
            {
                ddlDistritoEdit.Items.Clear();
                ddlDistritoEdit.Items.Insert(0, new ListItem("Seleccione una provincia primero", ""));
            }

            // Para que el modal siga abierto
            abrirModalEditarEspacio();
        }

        protected void ddlDistritoEdit_SelectedIndexChanged(object sender, EventArgs e)
        {
            hiddenIdDistrito.Value = ddlDistritoEdit.SelectedValue.ToString();
        }
    }
}