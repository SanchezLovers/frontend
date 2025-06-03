using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SirgepPresentacion
{
    public partial class ListaEspacios : System.Web.UI.Page
    {
        private DataTable Horarios
        {
            get
            {
                if (Session["Horarios"] == null)
                {
                    var dt = new DataTable();
                    dt.Columns.Add("Dia");
                    dt.Columns.Add("HoraInicio");
                    dt.Columns.Add("HoraFin");
                    Session["Horarios"] = dt;
                }
                return (DataTable)Session["Horarios"];
            }
            set
            {
                Session["Horarios"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarEspacios();

                ddlTipoEspacio.Items.Add("Cancha de Grass");
                ddlTipoEspacio.Items.Add("Cancha de Cemento");
                ddlTipoEspacio.Items.Add("Auditorio");

            }

        }

        private void CargarEspacios()
        {
            // Simulación de datos. Reemplaza esto con tu lógica para obtener los datos desde la base de datos.
            var espacios = new List<dynamic>
    {
        new { Id = 1, Codigo = "ESP001", Categoria = "Cancha de Grass", EspacioReservado = "C.D. Tupac Amaru", LinkDetalle = "DetalleEspacio.aspx?id=1" },
        new { Id = 2, Codigo = "ESP002", Categoria = "Cancha de Cemento", EspacioReservado = "C.D. Mama Ocllo", LinkDetalle = "DetalleEspacio.aspx?id=2" }
    };

            rptEspacios.DataSource = espacios;
            rptEspacios.DataBind();
        }



        protected void btnAgregarEspacio_Click(object sender, EventArgs e)
        {
            // Limpiar campos si es necesario
            txtNombreEspacio.Text = "";
            ddlTipoEspacio.SelectedIndex = 0;
            txtUbicacion.Text = "";
            txtSuperficie.Text = "";
            txtPrecio.Text = "";

            // Mostrar el modal usando JavaScript
            ScriptManager.RegisterStartupScript(this, this.GetType(), "abrirModalPaso1",
                "var modal1 = new bootstrap.Modal(document.getElementById('modalPaso1')); modal1.show();", true);
        }


        protected void btnEditar_Click(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            var id = btn.CommandArgument;
            // Redirigir a la página de edición con el ID del espacio
            Response.Redirect($"EditarEspacio.aspx?id={id}");
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            var id = btn.CommandArgument;
            // Lógica para eliminar el espacio con el ID proporcionado
            // Por ejemplo: EliminarEspacioPorId(int.Parse(id));
            CargarEspacios(); // Recargar la lista después de eliminar
        }

        protected void btnConsultar_Click(object sender, EventArgs e)
        {

        }
        protected void btnAgregarHorario_Click(object sender, EventArgs e)
        {
            var dt = Horarios;

            DataRow fila = dt.NewRow();
            fila["Dia"] = ddlDiaSemana.SelectedValue;
            fila["HoraInicio"] = txtHoraInicio.Text;
            fila["HoraFin"] = txtHoraFin.Text;
            dt.Rows.Add(fila);

            Horarios = dt;

            rptHorarios.DataSource = dt;
            rptHorarios.DataBind();
        }

        protected void btnGuardarEspacio_Click(object sender, EventArgs e)
        {
            string nombre = txtNombreEspacio.Text;
            string tipo = ddlTipoEspacio.SelectedValue;
            string ubicacion = txtUbicacion.Text;
            string superficie = txtSuperficie.Text;
            string precio = txtPrecio.Text;
            DataTable horarios = Horarios;

            // Aquí haces tu lógica de guardado (a DB, API, archivo, etc.)

            // Limpiar al final
            Session["Horarios"] = null;
            rptHorarios.DataSource = null;
            rptHorarios.DataBind();

            // Mensaje de éxito
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertSuccess",
                "alert('Espacio guardado exitosamente');", true);
        }
        protected void btnConfirmarAccion_Click(object sender, EventArgs e)
        {
            int id = int.Parse(hdnIdAEliminar.Value);

            // Aquí va la lógica para eliminar el espacio
            // Ejemplo: EliminarEspacioPorId(id);

            CargarEspacios(); // Refresca la tabla

            // Opcional: Mostrar mensaje de éxito
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertEliminar",
                "alert('Espacio eliminado exitosamente');", true);
        }

    }

}