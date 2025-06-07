using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;
using SirgepPresentacion.ReferenciaDisco;

namespace SirgepPresentacion.Presentacion.Ubicacion.Distrito
{
    public partial class EligeDistrito : System.Web.UI.Page
    {
        protected DepartamentoWSClient departamentoWS;
        protected ProvinciaWSClient provinciaWS;
        protected DistritoWSClient distritoWS;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                departamentoWS = new DepartamentoWSClient();
                provinciaWS = new ProvinciaWSClient();
                distritoWS = new DistritoWSClient();

                ddlDepartamento.DataSource = departamentoWS.listarDepas();
                ddlDepartamento.DataTextField = "Nombre";
                ddlDepartamento.DataValueField = "IdDepartamento";
                ddlDepartamento.DataBind();
                ddlDepartamento.Items.Insert(0, new ListItem("Seleccione Departamento", ""));
            }
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

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            if(!(ddlDepartamento.SelectedIndex > 0))
            {
                // Mostrar un mensaje de error si no se ha seleccionado un distrito
                lblError.Visible = true;
                lblError.Text = "Por favor, seleccione un departamento.";
            }
            else if(!(ddlProvincia.SelectedIndex > 0))
            {
                // Mostrar un mensaje de error si no se ha seleccionado un distrito
                lblError.Visible = true;
                lblError.Text = "Por favor, seleccione una provincia.";
            }
            else if(!(ddlDistrito.SelectedIndex > 0))
            {
                // Mostrar un mensaje de error si no se ha seleccionado un distrito
                lblError.Visible = true;
                lblError.Text = "Por favor, seleccione un distrito.";
            }
            else
            {
                // Obtener el valor seleccionado del DropDownList
                string idDistrito = ddlDistrito.SelectedValue;
                string nombreDistrito = ddlDistrito.SelectedItem.Text;

                // Construir la URL con el parámetro idDistrito
                string url = "/Presentacion/Infraestructura/Evento/ListaEvento.aspx?idDistrito={idDistrito}&nombreDistrito={nombreDistrito}";

                // Redirigir a la página de compras
                Response.Redirect(url);
            }
        }
    }
}