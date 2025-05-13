using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SirgepPresentacion
{
    public partial class ListaEspacios : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarEspacios();
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
            // Redirigir a la página de creación de espacios
            Response.Redirect("AgregarEspacio.aspx");
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
    }
}