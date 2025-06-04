<%@ Page Language="C#" MasterPageFile="~/MainLayout.Master" AutoEventWireup="true" CodeBehind="DetalleEntrada.aspx.cs" Inherits="SirgepPresentacion.Usuario.Comprador.DetalleEntrada" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Detalle de Entrada
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Contenido" runat="server">
    <div class="container">
        <h1 class="mb-4">
            Detalle de Entrada #<asp:Label ID="lblNumEntrada" runat="server" Text="000" />
        </h1>

        <div class="card mb-3">
            <div class="card-header modal-header-rojo text-white">
                Datos del Evento
            </div>
            <div class="card-body">
                <div class="mb-2"><strong>Nombre:</strong> <asp:Label ID="lblEvento" runat="server" Text="" CssClass="ms-2" /></div>
                <div class="mb-2"><strong>Ubicación:</strong> <asp:Label ID="lblUbicacion" runat="server" Text="" CssClass="ms-2" /></div>
                <div class="mb-2"><strong>Horario:</strong> <asp:Label ID="lblHorario" runat="server" Text="" CssClass="ms-2" /></div>
                <div class="mb-2"><strong>Fecha:</strong> <asp:Label ID="lblFecha" runat="server" Text="" CssClass="ms-2" /></div>
            </div>
        </div>

        <div class="card mb-3">
            <div class="card-header modal-header-rojo text-white">
                Datos del Comprador
            </div>
            <div class="card-body">
                <div class="mb-2"><strong>Nombres:</strong> <asp:Label ID="lblNombres" runat="server" Text="" CssClass="ms-2" /></div>
                <div class="mb-2"><strong>Apellidos:</strong> <asp:Label ID="lblApellidos" runat="server" Text="" CssClass="ms-2" /></div>
                <div class="mb-2"><strong>DNI:</strong> <asp:Label ID="lblDNI" runat="server" Text="" CssClass="ms-2" /></div>
                <div class="mb-2"><strong>Teléfono:</strong> <asp:Label ID="lblTelefono" runat="server" Text="" CssClass="ms-2" /></div>
                <div class="mb-2"><strong>Correo:</strong> <asp:Label ID="lblCorreo" runat="server" Text="" CssClass="ms-2" /></div>
            </div>
        </div>

        <div class="card mb-3">
            <div class="card-header modal-header-rojo text-white">
                Datos del Pago
            </div>
            <div class="card-body">
                <div class="mb-2"><strong>Método de pago:</strong> <asp:Label ID="lblMetodoPago" runat="server" Text="" CssClass="ms-2" /></div>
                <div class="mb-2"><strong>Número de pago:</strong> <asp:Label ID="lblNumeroPago" runat="server" Text="" CssClass="ms-2" /></div>
            </div>
        </div>

        <div class="d-flex justify-content-between">
            <asp:Button ID="btnVolver" runat="server" Text="Volver" CssClass="btn btn-dark" OnClick="btnVolver_Click" />
            <asp:Button ID="btnDescargar" runat="server" Text="Descargar" CssClass="btn btn-dark" OnClick="btnDescargar_Click" UseSubmitBehavior="false" />
        </div>
    </div>

    <!-- Modal de Descarga Completada -->
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
        function mostrarModalFeedback() {
            var modal = new bootstrap.Modal(document.getElementById('modalFeedback'));
            modal.show();
        }
        function mostrarModal() {
            var modal = new bootstrap.Modal(document.getElementById('miModal'));
            modal.show();
        }
    </script>
</asp:Content>