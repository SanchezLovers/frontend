using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SirgepPresentacion.ReferenciaDisco;

namespace SirgepPresentacion.Presentacion.Usuarios.Comprador
{
    public partial class ListaComprador : System.Web.UI.Page
    {
        private CompradorWS compradorWS;
        protected void Page_Init(object sender, EventArgs e)
        {
            compradorWS = new CompradorWSClient();

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarUsuarios();

            }
        }

        private void CargarUsuarios(string filtro = "")
        {
            var request = new listarCompradoresDTORequest();
            var response = compradorWS.listarCompradoresDTO(request);
            var compradores = response.@return;

            filtro = filtro?.ToLower() ?? "";

            var listaFiltrada = compradores
                .Where(c =>
                    string.IsNullOrEmpty(filtro) ||
                    (c.nombres != null && c.nombres.ToLower().Contains(filtro)) ||
                    (c.primerApellido != null && c.primerApellido.ToLower().Contains(filtro)) ||
                    (c.segundoApellido != null && c.segundoApellido.ToLower().Contains(filtro)) ||
                    (c.correo != null && c.correo.ToLower().Contains(filtro)) ||
                    (c.numeroDocumento != null && c.numeroDocumento.ToLower().Contains(filtro))
                )
                .Select(c =>
                {
                    var requestUltimaCompra = new obtenerUltimaCompraPorDocumentoRequest();
                    requestUltimaCompra.numeroDocumento = c.numeroDocumento;

                    var responseUltimaCompra = compradorWS.obtenerUltimaCompraPorDocumento(requestUltimaCompra);
                    string fechaUltimaCompraStr = responseUltimaCompra.@return;

                    DateTime? fechaUltimaCompra = null;
                    if (DateTime.TryParse(fechaUltimaCompraStr, out DateTime fecha))
                        fechaUltimaCompra = fecha;

                    bool puedeEliminar = PuedeEliminar(fechaUltimaCompra);

                    string ultimaCompraStr = fechaUltimaCompra.HasValue
                    ? fechaUltimaCompra.Value.ToString("yyyy-MM-dd")
                    : "No ha comprado aún";

                    return new
                    {
                        c.idComprador,
                        c.nombres,
                        c.primerApellido,
                        c.segundoApellido,
                        c.tipoDocumento,
                        c.numeroDocumento,
                        c.correo,
                        UltimaCompra = ultimaCompraStr,
                        PuedeEliminar = puedeEliminar
                    };
                })
                .ToList();

            GvListaCompradores.DataSource = listaFiltrada;
            GvListaCompradores.DataBind();
        }




        // Lógica para determinar si puede eliminarse (más de 3 años desde la última compra)
        private bool PuedeEliminar(DateTime? fechaUltimaCompra)
        {
            if (!fechaUltimaCompra.HasValue)
                return false; // Si nunca compró, se puede eliminar

            return fechaUltimaCompra.Value.AddYears(3) <= DateTime.Now;
        }

        // Evento para el botón de búsqueda (ajusta según tus filtros)
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            string busqueda = txtBusqueda.Text.Trim().ToLower();
            CargarUsuarios(busqueda);
        }

        // Evento para confirmar eliminación
        protected void btnConfirmarAccion_Click(object sender, EventArgs e)
        {
            int id = int.Parse(hdnIdAEliminar.Value);

            // Crear el request
            var request = new eliminarUsuarioCompradorRequest();
            request.idComprador = id; 

            //Llamar al método pasando el request
            compradorWS.eliminarUsuarioComprador(request);

            //Recargar la lista
            CargarUsuarios();
        }


    }
}