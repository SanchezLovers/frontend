using SirgepPresentacion.ReferenciaDisco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SirgepPresentacion.Presentacion.Ventas.Entrada
{
    public partial class ListaEntradasComprador : System.Web.UI.Page
    {
        private EntradaWSClient entradaWS;
        protected void Page_Init(object sender, EventArgs e)
        {
            entradaWS = new EntradaWSClient();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                entradaWS = new EntradaWSClient();
            }
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            // Mostrar el modal de feedback
            string script = "mostrarModalFeedback();";
            ClientScript.RegisterStartupScript(this.GetType(), "mostrarModalFeedback", script, true);
        }

        protected void btnDescargar_Click(object sender, EventArgs e)
        {
            int idComprador = 2;
            entradaWS.crearLibroExcelEntradas(idComprador);
        }
    }
}