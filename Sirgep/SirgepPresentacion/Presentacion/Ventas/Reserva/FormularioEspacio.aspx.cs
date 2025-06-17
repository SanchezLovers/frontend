using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.EnterpriseServices;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;
using SirgepPresentacion.ReferenciaDisco;

namespace SirgepPresentacion.Presentacion.Ventas.Reserva
{
    public partial class FormularioEspacio : System.Web.UI.Page
    {
        private EspacioWSClient espacioWS;
        protected DepartamentoWSClient departamentoWS;
        protected ProvinciaWSClient provinciaWS;
        protected DistritoWSClient distritoWS;
        protected HorarioEspacioWSClient horarioWS;
        public class Horario
        {
            public DateTime horaInicio { get; set; }
            public bool disponible { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            departamentoWS = new DepartamentoWSClient();
            provinciaWS = new ProvinciaWSClient();
            distritoWS = new DistritoWSClient();
            espacioWS = new EspacioWSClient();
            horarioWS = new HorarioEspacioWSClient();

            if (!IsPostBack)
            {
                CargarDepartamentos();
                txtFecha.Attributes["min"] = DateTime.Now.AddDays(2).ToString("yyyy-MM-dd");

                ddlDistrito.Items.Insert(0, new ListItem("Seleccione Distrito", ""));
                ddlProvincia.Items.Insert(0, new ListItem("Seleccione Provincia", ""));
                ddlEspacio.Items.Insert(0, new ListItem("Seleccione Espacio", ""));

                /* Hardcodeado pq' no lo tenemos en nuestra BD, además hay pocos datos */
                ddlCategoria.Items.Insert(0, new ListItem("Seleccione un tipo", ""));
                ddlCategoria.Items.Insert(1, new ListItem("Teatros", "TEATRO"));
                ddlCategoria.Items.Insert(2, new ListItem("Canchas", "CANCHA"));
                ddlCategoria.Items.Insert(3, new ListItem("Salones", "SALON"));
                ddlCategoria.Items.Insert(4, new ListItem("Parques", "PARQUE"));
            }
        }

        protected void CargarDepartamentos()
        {
            // Reemplazar con llamada a tu WS
            ddlDepartamento.DataSource = departamentoWS.listarDepas();
            ddlDepartamento.DataTextField = "Nombre";
            ddlDepartamento.DataValueField = "IdDepartamento";
            ddlDepartamento.DataBind();
            ddlDepartamento.Items.Insert(0, new ListItem("Seleccione Departamento", ""));
        }

        protected void ddlDepartamento_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idDepto;
            provinciaWS = new ProvinciaWSClient();

            if (int.TryParse(ddlDepartamento.SelectedValue, out idDepto))
            {
                ddlProvincia.DataSource = provinciaWS.listarProvinciaPorDepa(idDepto);

                ddlProvincia.DataTextField = "Nombre";
                ddlProvincia.DataValueField = "IdProvincia";
                ddlProvincia.DataBind();
                ddlProvincia.Enabled = true;
                ddlProvincia.Items.Insert(0, new ListItem("Seleccione Provincia", ""));
            }
            else
            {
                // Manejar el caso en que la conversión falle
                ddlProvincia.Enabled = false;
            }
        }

        protected void ddlProvincia_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idProv;
            if (int.TryParse(ddlProvincia.SelectedValue, out idProv))
            {
                distritoWS = new DistritoWSClient();

                ddlDistrito.DataSource = distritoWS.listarDistritosFiltrados(idProv);
                ddlDistrito.DataTextField = "Nombre";
                ddlDistrito.DataValueField = "IdDistrito";
                ddlDistrito.DataBind();
                ddlDistrito.Enabled = true;
                ddlDistrito.Items.Insert(0, new ListItem("Seleccione Distrito", ""));
            }
            else
            {
                // Manejar el caso en que la conversión falle
                ddlDistrito.Enabled = false;
            }
        }

        protected void ddlDistrito_SelectedIndexChanged(object sender, EventArgs e)
        {
            // no sé si se deben cargar distritos sin categoría?
            int idDistrito = int.Parse(ddlDistrito.SelectedValue);
            ddlEspacio.DataSource = espacioWS.listarEspacioPorDistrito(idDistrito);
            ddlEspacio.DataTextField = "Nombre";
            ddlEspacio.DataValueField = "idEspacio";
            ddlEspacio.DataBind();
            ddlEspacio.Enabled = true;
            ddlEspacio.Items.Insert(0, new ListItem("Seleccione Espacio", ""));
        }

        protected void ddlEspacio_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idEspacio = int.Parse(ddlEspacio.SelectedValue);
            lblPrecioHora.Text = espacioWS.buscarEspacio(idEspacio).precioReserva.ToString();
            lblPrecioHora.Text = $"Precio por hora: S/ {espacioWS.buscarEspacio(idEspacio).precioReserva.ToString()}";
            lblPrecioTotal.Text = "Precio total seleccionado: Seleccione un horario";
        }

        protected void ddlCategoria_SelectedIndexChanged(object sender, EventArgs e)
        {
            //  int idCat = int.Parse(ddlCategoria.SelectedValue);
            // int idDist = int.Parse(ddlDistrito.SelectedValue);

            string filtroCat = ddlCategoria.Text;
            string filtroDist = ddlDistrito.Text;
            if (filtroCat != "" && filtroDist != "")
            {
                // llamamos a la filtración doble implementada en backend
                listarEspacioDistyCatResponse response = null;
                ddlEspacio.DataSource = espacioWS.listarEspacioDistyCat(int.Parse(filtroDist), filtroCat);

                ddlEspacio.DataTextField = "nombre";
                ddlEspacio.DataValueField = "idEspacio";
                ddlEspacio.DataBind();
                ddlEspacio.Items.Insert(0, new ListItem("Seleccione Espacio", ""));
            }
            else
            {
                // Si no hay filtro, no mostramos nada
                ddlEspacio.Items.Clear();
                ddlEspacio.Items.Insert(0, new ListItem("Seleccione Espacio", ""));
            }

        }

        protected void btnConsultarHorarios_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlEspacio.SelectedValue) && !string.IsNullOrEmpty(txtFecha.Text))
            {
                int idEspacio = int.Parse(ddlEspacio.SelectedValue);
                string fecha = txtFecha.Text;

                var horarios = horarioWS.listarHorariosDelEspacioYDia(idEspacio, fecha);

                rptHorarios.DataSource = horarios;
                rptHorarios.DataBind();

                lblPrecioHora.Text = "Precio por hora: S/ 5.00";
                lblPrecioTotal.Text = "Precio total seleccionado: S/ 0.00"; // Recalcula con JS si deseas
            }
        }
        protected void btnReservar_Click(object sender, EventArgs e)
        {
            lblError.Visible = false;

            if (!(ddlEspacio.SelectedIndex > 0))
            {
                lblError.Visible = true;
                lblError.Text = "Por favor, seleccione un Espacio.";
            }
            else if (string.IsNullOrEmpty(txtFecha.Text))
            {
                lblError.Visible = true;
                lblError.Text = "Por favor, seleccione una Fecha.";
            }
            else
            {
                List<TimeSpan> horasSeleccionadas = new List<TimeSpan>();
                int cant = 0;
                foreach (RepeaterItem item in rptHorarios.Items)
                {
                    CheckBox chk = (CheckBox)item.FindControl("chkHorario");
                    Label lbl = (Label)item.FindControl("lblHora");

                    if (chk.Checked)
                    {
                        cant++;
                        if (TimeSpan.TryParse(lbl.Text, out TimeSpan hora))
                        {
                            horasSeleccionadas.Add(hora);
                        }
                    }
                }

                if (horasSeleccionadas.Count == 0)
                {
                    lblError.Visible = true;
                    lblError.Text = "Por favor, seleccione al menos un horario disponible.";
                }
                else
                {
                    horasSeleccionadas.Sort(); // porsiaca no esten ordenadas
                    string horaIni = horasSeleccionadas.First().ToString(@"hh\:mm");
                    string horaFin = horasSeleccionadas.Last().Add(TimeSpan.FromHours(1)).ToString(@"hh\:mm"); // siempre osn de 1 hora
                    int idEspacio = int.Parse(ddlEspacio.SelectedValue);
                    string fecha = txtFecha.Text;

                    string url = $"/Presentacion/Ventas/Reserva/DetalleReserva.aspx?idEspacio={idEspacio}&fecha={fecha}&horaIni={horaIni}&horaFin={horaFin}&cant={cant}";

                    Response.Redirect(url);
                }
            }
        }

    }
}