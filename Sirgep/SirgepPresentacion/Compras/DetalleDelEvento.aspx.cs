using SirgepPresentacion.ReferenciaDisco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SirgepPresentacion.Compras
{
    public partial class DetalleDelEvento : System.Web.UI.Page
    {
        protected EventoWSAnaGClient eventoWS;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //                int idEvento = int.Parse(Request.QueryString["idEvento"]);
                eventoWS = new EventoWSAnaGClient();
                int idEvento = 1; // Simulación de ID de evento, reemplazar con el ID real obtenido de la URL o base de datos
                CargarDatosEvento(idEvento);
                CargarFunciones(idEvento);
            }
        }

        private void CargarDatosEvento(int idEvento)
        {
            evento e = eventoWS.buscarPorID(idEvento);
            if (e != null)
            {
                lblTitulo.Text = e.nombre;
                litDescripcion.Text = e.descripcion;
                lblUbicacion.Text = e.ubicacion;
                lblReferencia.Text = e.referencia;
                int a = (e.cantEntradasDispo + e.cantEntradasVendidas);
                string cadena = a.ToString();
                cantGeneralInvisible.Value = cadena;
                /*
                lblTitulo.Text = "Titulo del evento";
                litDescripcion.Text = "Semper simul";
                lblUbicacion.Text = "av bolivar 123";
                lblReferencia.Text = "edificio 123";
                */
            }
        }

        private void CargarFunciones(int idEvento)
        {
            ddlFunciones.Items.Clear();
            ddlFunciones.Items.Add(new ListItem("Seleccione una función", ""));
            int cantFunciones = 0; //para contar las funciones y dividir la cantidad de entradas entre el num de funciones
            var funciones = eventoWS.listarFuncionesDeEvento(idEvento);

            foreach (var funcion in funciones)
            {
                cantFunciones++;
                ddlFunciones.Items.Add(new ListItem($"{funcion.horaInicio}", funcion.idFuncion.ToString()));
            }
            int cg = int.Parse(cantGeneralInvisible.Value);
            int a = cg / cantFunciones;
            string cadena = a.ToString();
            cantFuncionInvisible.Value = cadena;//asignar la cantidad de entradas por funcion
        }

        protected void ddlFunciones_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnComprar.Enabled = false; //deshabilitar el botón de compra al cambiar la función

            if (int.TryParse(ddlFunciones.SelectedValue, out int idFuncion))
            {
                int cantEF = int.Parse(cantFuncionInvisible.Value);
                eventoWS = new EventoWSAnaGClient();
                int disponibilidad = eventoWS.cantEntradasDisponibles(idFuncion, cantEF);
                if (disponibilidad > 0)
                {
                    btnComprar.Enabled = true; //habilitar el botón de compra si hay entradas disponibles
                    ViewState["idFuncion"] = idFuncion; //guardar el id de la función seleccionada en el ViewState
                    //aquí debería habilitar el botón para darle click a comprar o algo así
                }
            }
        }
        protected void btnComprar_Click(object sender, EventArgs e)
        {
            if (ViewState["FuncionSeleccionada"] != null)
            {
                int idFuncion = (int)ViewState["FuncionSeleccionada"];
                //Response.Redirect($"DetalleDeCompra.aspx?idFuncion={idFuncion}");
            }
        }
    }
}