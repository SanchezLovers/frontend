<%@ Page Language="C#" MasterPageFile="~/MainLayout.Master" AutoEventWireup="true" CodeBehind="DetalleReserva.aspx.cs" Inherits="SirgepPresentacion.Presentacion.Ventas.Reserva.DetalleReserva" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Contenido" runat="server">
    <div class="container">
        <h1 class="mb-4">
            Detalle de Reserva #<asp:Label ID="lblNumReserva" runat="server" Text="000" />
        </h1>
        
        <!-- DATOS DEL ESPACIO -->
        <div class="card mb-3">
            <div class="card-header modal-header-rojo text-white">Datos del Espacio</div>
            <div class="card-body row g-3">
                <div class="col-12 col-md-4">
                    <strong>Nombre:</strong> <asp:Label ID="lblEspacio" runat="server" CssClass="ms-2" />
                </div>
                <div class="col-12 col-md-4">
                    <strong>Tipo Espacio:</strong> <asp:Label ID="lblTipoEspacio" runat="server" CssClass="ms-2" />
                </div>
                <div class="col-12">
                    <strong>Superficie:</strong> <asp:Label ID="lblSuperficie" runat="server" CssClass="ms-2" />
                </div>
                <div class="col-12 col-md-4">
                    <strong>Ubicación:</strong> <asp:Label ID="lblUbicacion" runat="server" CssClass="ms-2" />
                </div>
                <div class="col-12 col-md-4">
                    <strong>Distrito:</strong> <asp:Label ID="lblDistrito" runat="server" CssClass="ms-2" />
                </div>
            </div>
        </div>
        <!-- DATOS DE LA RESERVA -->
        <div class="card mb-3">
            <div class="card-header modal-header-rojo text-white">Datos de la Reserva</div>
            <div class="card-body row g-3">
                <div class="col-12 col-md-4">
                    <strong>Fecha:</strong> <asp:Label ID="lblFechaReserva" runat="server" CssClass="ms-2" />
                </div>
                <div class="col-12 col-md-4">
                    <strong>Hora Inicio:</strong> <asp:Label ID="lblHoraInicio" runat="server" CssClass="ms-2" />
                </div>
                <div class="col-12 col-md-4">
                    <strong>Hora Fin:</strong> <asp:Label ID="lblHoraFin" runat="server" CssClass="ms-2" />
                </div>
            </div>
        </div>

        <!-- DATOS DEL COMPRADOR -->
        <div class="card mb-3">
            <div class="card-header modal-header-rojo text-white">Datos del Comprador</div>
            <div class="card-body row g-3">
                <!-- Fila con dos columnas -->
                <div class="col-12 col-md-4">
                    <strong>Nombres:</strong> <asp:Label ID="lblNombres" runat="server" CssClass="ms-2" />
                </div>
                <div class="col-12 col-md-4">
                    <strong>Apellidos:</strong> <asp:Label ID="lblApellidos" runat="server" CssClass="ms-2" />
                </div>
                <!-- Fila con tres columnas -->
                <div class="col-12 col-md-4">
                    <strong>Tipo de Documento:</strong> <asp:Label ID="lblTipoDocumento" runat="server" CssClass="ms-2" />
                </div>
                <div class="col-12 col-md-4">
                    <strong>Número de Documento:</strong> <asp:Label ID="lblTNumDocumento" runat="server" CssClass="ms-2" />
                </div>
                <div class="col-12 col-md-4">
                    <strong>Correo:</strong> <asp:Label ID="lblCorreo" runat="server" CssClass="ms-2" />
                </div>
            </div>
        </div>

        <!-- DATOS DE LA CONSTANCIA -->
        <div class="card mb-3">
            <div class="card-header modal-header-rojo text-white">Datos de la Constancia del Pago</div>
            <div class="card-body row g-3">
                <div class="col-12 col-md-4">
                    <strong>Fecha:</strong> <asp:Label ID="lblFechaConstancia" runat="server" CssClass="ms-2" />
                </div>
                <div class="col-12 col-md-4">
                    <strong>Método de pago:</strong> <asp:Label ID="lblMetodoPago" runat="server" CssClass="ms-2" />
                </div>
                <div class="col-12 col-md-4">
                    <strong>Total:</strong> <asp:Label ID="lblTotal" runat="server" CssClass="ms-2" />
                </div>
                <div class="col-12">
                    <strong>Detalle de pago:</strong> <asp:Label ID="lblDetallePago" runat="server" CssClass="ms-2" />
                </div>
            </div>
        </div>

        <!-- BOTONES -->
        <div class="d-flex justify-content-between mb-3">
            <asp:Button ID="btnVolver" runat="server" Text="Volver" CssClass="btn btn-dark" OnClick="btnVolver_Click" />
            <asp:Button ID="btnDescargar" runat="server" Text="Descargar" CssClass="btn btn-dark" OnClick="btnDescargar_Click" UseSubmitBehavior="false" />
        </div>
    </div>

    <!-- MODAL -->
    <div class="modal fade" id="miModal" tabindex="-1" aria-labelledby="miModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header modal-header-rojo text-white">
                    <h5 class="modal-title" id="miModalLabel">Descarga Completada</h5>
                    <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Cerrar"></button>
                </div>
                <div class="modal-body">
                    Descarga completada correctamente.
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-dark" data-bs-dismiss="modal">Cerrar</button>
                </div>
            </div>
        </div>
    </div>

    <script>
        function mostrarModal() {
            var modal = new bootstrap.Modal(document.getElementById('miModal'));
            modal.show();
        }
    </script>
</asp:Content>