using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SirgepPresentacion.ReferenciaDisco;

namespace SirgepPresentacion.Presentacion.Usuarios.Comprador
{
    public partial class PerfilComprador : System.Web.UI.Page
    {
        private CompradorWSClient compradorWS;
        protected void Page_Load(object sender, EventArgs e)
        {
            compradorWS = new CompradorWSClient();
            if (!IsPostBack)
            {
                cargarPerfil();
            }
        }
        protected void cargarPerfil()
        {
            //int idComprador = 3;
            int idComprador = int.Parse(Session["idUsuario"].ToString());
            detalleComprador detalleCompradorDTO = compradorWS.buscarDetalleCompradorPorId(idComprador);
            lblNombres.Text = detalleCompradorDTO.nombres;
            lblPrimerApellido.Text = detalleCompradorDTO.primerApellido;
            lblSegundoApellido.Text = detalleCompradorDTO.segundoApellido;
            lblTipoDocumento.Text = detalleCompradorDTO.tipoDocumento;
            lblNumeroDocumento.Text = detalleCompradorDTO.numeroDocumento;
            lblMontoBilletera.Text = "S/. " + detalleCompradorDTO.montoBilletera.ToString();
            lblDepartamento.Text = detalleCompradorDTO.departamentoFavorito;
            lblProvincia.Text = detalleCompradorDTO.provinciaFavorita;
            txtDistrito.Text = detalleCompradorDTO.distritoFavorito.ToUpper();
            lblCorreo.Text = detalleCompradorDTO.correo;
            lblContrasenia.Text = new string('*', detalleCompradorDTO.contrasenia.Length);
        }
        protected void btnGuardarDistrito_Click(object sender, EventArgs e)
        {
            string nuevoDistrito = txtDistrito.Text.ToUpper();
            if (!string.IsNullOrEmpty(nuevoDistrito))
            {
                //int idComprador = 3;
                int idComprador = int.Parse(Session["idUsuario"].ToString());
                bool resultado = compradorWS.actualizarDistritoFavoritoPorIdComprador(nuevoDistrito, idComprador);
                if (resultado)
                {
                    cargarPerfil();
                    //txtDistrito.Text = txtDistrito.Text.ToUpper();
                }
                else
                {
                    Response.Write("<script>alert('El nombre del distrito es incorrecto');</script>");
                }
            }
        }
    }
}