using SirgepPresentacion.ReferenciaDisco;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SirgepPresentacion.Presentacion.Infraestructura.Espacio
{
    public partial class ListaEspacios : System.Web.UI.Page
    {
        private EspacioWS espacioWS;
        private DistritoWS distritoWS;
        private DepartamentoWS departamentoWS;
        private ProvinciaWS provinciaWS;

        protected void Page_Init(object sender, EventArgs e)
        {
            espacioWS = new EspacioWSClient();
            distritoWS = new DistritoWSClient();
            departamentoWS = new DepartamentoWSClient();
            provinciaWS = new ProvinciaWSClient();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarEspacios();
                CargarDistritos();

                //ddlTipoEspacio.Items.Add("Cancha de Grass");
                //ddlTipoEspacio.Items.Add("Cancha de Cemento");
                //ddlTipoEspacio.Items.Add("Auditorio");

            }

        }

        private void CargarDistritos()
        {
            // por ahora, solo distritos de LIMA
            const int LIMA = 1;
            ddlDistrito.DataSource = distritoWS.listarDistritosFiltrados(new listarDistritosFiltradosRequest(LIMA)).@return;
            ddlDistrito.DataTextField = "Nombre";
            ddlDistrito.DataValueField = "IdDistrito";
            ddlDistrito.DataBind();

            // agregamos lo que nos faltó pq' el DataBind() refresca todo
            // Hay que ponerle el item al inicio especificando que no hay filtro todavía
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
            ddlTipoEspacioAgregar.SelectedIndex = 0;
            txtUbicacionAgregar.Text = "";
            txtSuperficieAgregar.Text = "";
            txtPrecioReservaAgregar.Text = "";
            ddlProvinciaAgregar.Text = "";
            ddlDistritoAgregar.Text = "";

            /* Hardcodeado pq' no lo tenemos en nuestra BD, además hay pocos datos */
            ddlTipoEspacioAgregar.Items.Insert(0, new ListItem("Seleccione un tipo", ""));
            ddlTipoEspacioAgregar.Items.Insert(1, new ListItem("Teatros", "TEATRO"));
            ddlTipoEspacioAgregar.Items.Insert(2, new ListItem("Canchas", "CANCHA"));
            ddlTipoEspacioAgregar.Items.Insert(3, new ListItem("Salones", "SALON"));
            ddlTipoEspacioAgregar.Items.Insert(4, new ListItem("Parques", "PARQUE"));

            listarDepasResponse responseDepas = departamentoWS.listarDepas(new listarDepasRequest());
            ddlDepartamentoAgregar.DataSource = responseDepas.@return;
            ddlDepartamentoAgregar.DataTextField = "Nombre";
            ddlDepartamentoAgregar.DataValueField = "IdDepartamento";
            ddlDepartamentoAgregar.DataBind();

            ddlDepartamentoAgregar.Items.Insert(0, new ListItem("Escoja un departamento", ""));
            ddlProvinciaAgregar.Items.Insert(0, new ListItem("Seleccione una provincia", ""));
            ddlDistritoAgregar.Items.Insert(0, new ListItem("Seleccione un distrito", ""));
            // Mostrar el modal usando JavaScript
            ScriptManager.RegisterStartupScript(this, this.GetType(), "abrirModalPaso1",
                "var modal1 = new bootstrap.Modal(document.getElementById('modalPaso1')); modal1.show();", true);
        }


        protected void btnEditar_Click(object sender, EventArgs e)
        {

            /*CARGAR DATOS*/
            int idEspacio = int.Parse(((Button)sender).CommandArgument.ToString());
            buscarEspacioResponse response = espacioWS.buscarEspacio(new buscarEspacioRequest(idEspacio));

            hiddenIdEspacio.Value = idEspacio.ToString();

            espacio espCargar = response.@return;
            distrito disCargar = distritoWS.buscarDistPorId(new buscarDistPorIdRequest(espCargar.distrito.idDistrito)).@return;
            provincia provCargar = provinciaWS.buscarProvinciaPorId(new buscarProvinciaPorIdRequest(disCargar.provincia.idProvincia)).@return;
            departamento depCargar = departamentoWS.buscarDepaPorId(new buscarDepaPorIdRequest(provCargar.departamento.idDepartamento)).@return;

            hiddenIdDistrito.Value = disCargar.idDistrito.ToString();

            /* Hardcodeado pq' no lo tenemos en nuestra BD, además hay pocos datos */
            ddlTipoEspacioEdit.Items.Insert(0, new ListItem(espCargar.tipoEspacio.ToString(), espCargar.tipoEspacio.ToString()));
            ddlTipoEspacioEdit.Items.Insert(1, new ListItem("Teatros", "TEATRO"));
            ddlTipoEspacioEdit.Items.Insert(2, new ListItem("Canchas", "CANCHA"));
            ddlTipoEspacioEdit.Items.Insert(3, new ListItem("Salones", "SALON"));
            ddlTipoEspacioEdit.Items.Insert(4, new ListItem("Parques", "PARQUE"));

            // Leer el valor
            txtNombreEdit.Text = espCargar.nombre;
            txtUbicacionEdit.Text = espCargar.ubicacion;
            txtSuperficieEdit.Text = espCargar.superficie.ToString();
            txtPrecioEdit.Text = espCargar.precioReserva.ToString();
            
            ddlDepartamentoEdit.DataSource = departamentoWS.listarDepas(new listarDepasRequest()).@return;
            ddlDepartamentoEdit.DataTextField = "Nombre";
            ddlDepartamentoEdit.DataValueField = "IdDepartamento";
            ddlDepartamentoEdit.DataBind();
            ddlDepartamentoEdit.SelectedValue = depCargar.idDepartamento.ToString();

            ddlProvinciaEdit.DataSource = provinciaWS.listarProvinciaPorDepa(new listarProvinciaPorDepaRequest(depCargar.idDepartamento)).@return;
            ddlProvinciaEdit.DataTextField = "Nombre";
            ddlProvinciaEdit.DataValueField = "IdProvincia";
            ddlProvinciaEdit.DataBind();
            ddlProvinciaEdit.SelectedValue = provCargar.idProvincia.ToString();

            ddlDistritoEdit.DataSource = distritoWS.listarDistritosFiltrados(new listarDistritosFiltradosRequest(provCargar.idProvincia)).@return;
            ddlDistritoEdit.DataTextField = "Nombre";
            ddlDistritoEdit.DataValueField = "IdDistrito";
            ddlDistritoEdit.DataBind();
            ddlDistritoEdit.SelectedValue = disCargar.idDistrito.ToString();

            txtHoraInicioEdit.Text = espCargar.horarioInicioAtencion.ToString();
            txtHoraFinEdit.Text = espCargar.horarioFinAtencion.ToString();

            // Mostrar el modal usando JavaScript
            ScriptManager.RegisterStartupScript(this, this.GetType(), "abrirModalEdicion",
                "var modalEdicion = new bootstrap.Modal(document.getElementById('modalEdicionEspacio')); modalEdicion.show();", true);
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            int id = int.Parse(btn.CommandArgument.ToString());

            // Lógica para eliminar el espacio con el ID proporcionado
            eliminarLogicoResponse response = espacioWS.eliminarLogico(new eliminarLogicoRequest(id));
            // Por ejemplo: EliminarEspacioPorId(int.Parse(id));
            CargarEspacios(); // Recargar la lista después de eliminar
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

            // Aquí va la lógica para eliminar el espacio
            // Ejemplo: EliminarEspacioPorId(id);

            eliminarLogicoResponse response = espacioWS.eliminarLogico(new eliminarLogicoRequest(id));

            Boolean estado = response.@return;

            if (estado)
            {
                CargarEspacios(); // Refresca la tabla

                // Opcional: Mostrar mensaje de éxito
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertEliminar",
                    "alert('Espacio eliminado exitosamente');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertEliminar",
                    "alert('Ocurrió un problema al eliminar el espacio');", true);
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
            ScriptManager.RegisterStartupScript(this, this.GetType(), "abrirModalPaso1",
                "var modal1 = new bootstrap.Modal(document.getElementById('modalPaso1')); modal1.show();", true);
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
            ScriptManager.RegisterStartupScript(this, this.GetType(), "abrirModalPaso1",
                "var modal1 = new bootstrap.Modal(document.getElementById('modalPaso1')); modal1.show();", true);
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
                CargarEspacios();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertActualizar",
                    "alert('Espacio actualizado exitosamente');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertActualizar",
                    "alert('Ocurrió un problema al actualizar el espacio');", true);
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
                // Mensaje de éxito
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertSuccess",
                    "alert('Espacio guardado exitosamente');", true);
                CargarEspacios();
            }
            else
            {
                // Mensaje de fallo
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertSuccess",
                    "alert('Se produjo un error al insertar el espacio');", true);
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "abrirModalPaso1",
                    "var modal1 = new bootstrap.Modal(document.getElementById('modalPaso1')); modal1.show();", true);
                return;
            }

            // Parsear a TimeSpan
            TimeSpan horaInicio, horaFin;

            bool inicioOk = TimeSpan.TryParse(horaInicioStr, out horaInicio);
            bool finOk = TimeSpan.TryParse(horaFinStr, out horaFin);

            if (!inicioOk || !finOk)
            {
                lblError.Text = "Formato de hora no válido.";
                // Para que el modal siga abierto
                ScriptManager.RegisterStartupScript(this, this.GetType(), "abrirModalPaso1",
                    "var modal1 = new bootstrap.Modal(document.getElementById('modalPaso1')); modal1.show();", true);
                return;
            }

            // Validar que hora inicio < hora fin
            if (horaInicio >= horaFin)
            {
                lblError.Text = "La hora de inicio debe ser menor que la hora de fin.";
                // Para que el modal siga abierto
                ScriptManager.RegisterStartupScript(this, this.GetType(), "abrirModalPaso1",
                    "var modal1 = new bootstrap.Modal(document.getElementById('modalPaso1')); modal1.show();", true);
                return;
            }

            // Si todo está bien, borrar el error
            lblError.Text = "";
            // Para que el modal siga abierto
            ScriptManager.RegisterStartupScript(this, this.GetType(), "abrirModalPaso1",
                "var modal1 = new bootstrap.Modal(document.getElementById('modalPaso1')); modal1.show();", true);
        }
    }
}