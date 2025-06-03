using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;
using SirgepPresentacion.ReferenciaDisco;

namespace SirgepPresentacion.Compras
{
    public partial class EleccionDistrito : System.Web.UI.Page
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
    }
}