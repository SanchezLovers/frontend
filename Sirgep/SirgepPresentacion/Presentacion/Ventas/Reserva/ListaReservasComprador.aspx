<%@ Page Title="" Language="C#" MasterPageFile="~/MainLayout.Master" AutoEventWireup="true" CodeBehind="ListaReservasComprador.aspx.cs" Inherits="SirgepPresentacion.Presentacion.Ventas.Reserva.ListaReservasComprador" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Encabezado" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Contenido" runat="server">
    <!-- Título principal -->
    <h2 class="fw-bold mb-4">Reservas</h2>
    <!-- Búsqueda y Filtros -->
    <div class="busqueda-filtros mb-4">
        <div class="busqueda mb-2">
            <input type="text" id="input_busqueda" runat="server" class="input-busqueda" placeholder="🔍 Buscar" onkeypress="return buscarOnEnter(event)" />
            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click" Style="display: none" />
        </div>
        <div class="filtros d-flex align-items-center flex-wrap gap-3">
            <label class="fw-bold me-2 mb-0">Filtros:</label>
            <!-- Fecha Desde -->
            <div class="d-flex align-items-center">
                <label class="me-1 mb-0">Desde:</label>
                <asp:TextBox ID="txtFechaInicio" runat="server" CssClass="form-control" TextMode="Date" />
            </div>
            <!-- Fecha Hasta -->
            <div class="d-flex align-items-center">
                <label class="me-1 mb-0">Hasta:</label>
                <asp:TextBox ID="txtFechaFin" runat="server" CssClass="form-control" TextMode="Date" />
            </div>
            <!-- Checkboxes -->
            <div class="d-flex align-items-center">
                <asp:CheckBox ID="chkVigentes" runat="server" CssClass="form-check-input me-1" />
                <label for="chkVigentes" class="me-3 mb-0">Vigentes</label>
                <asp:CheckBox ID="chkFinalizadas" runat="server" CssClass="form-check-input me-1" />
                <label for="chkFinalizadas" class="me-3 mb-0">Finalizadas</label>
                <asp:CheckBox ID="chkCanceladas" runat="server" CssClass="form-check-input me-1" />
                <label for="chkCanceladas" class="me-3 mb-0">Canceladas</label>
            </div>
            <!-- Botón Filtrar -->
            <asp:Button ID="btnFiltrar" runat="server" Text="Filtrar" CssClass="btn btn-dark" OnClick="btnFiltrar_Click" />
        </div>
    </div>
    <!-- Descarga -->
    <div class="container">
        <div class="d-flex justify-content-between mb-3">
            <asp:Button ID="btnDescargarMostrarModal" runat="server" Text="Descargar Lista de Reservas" OnClientClick="setTimeout(function() { mostrarModalExito('Descarga exitosa', 'La lista de reservas fue descargada correctamente.'); }, 1000);" OnClick="btnDescargar_Click" CssClass="btn btn-dark" />
        </div>
    </div>
    <!-- un par de hidden values -->
    <asp:HiddenField ID="hdnReservaAEliminar" runat="server" />
    <asp:HiddenField ID="hdnFechaEliminar" runat="server" />
    <!-- GridView -->
    <asp:GridView ID="GvListaReservasComprador" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true"
        AllowPaging="true" PageSize="5" OnPageIndexChanging="GvListaReservasComprador_PageIndexChanging"
        CssClass="table table-striped table-responsive table-hover">
        <Columns>
            <asp:TemplateField HeaderText="Abrir">
                <ItemTemplate>
                    <asp:LinkButton ID="BtnAbrir" runat="server"
                        CommandArgument='<%# Eval("IdConstancia") %>'
                        OnClick="BtnAbrir_Click"
                        CssClass="btn btn-link"
                        ToolTip="Abrir Reserva">
                        <i class="bi bi-box-arrow-up-right"></i>
                    </asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="NumReserva" HeaderText="Nro Reserva" />
            <asp:BoundField DataField="NombreEspacio" HeaderText="Espacio" />
            <asp:BoundField DataField="Categoria" HeaderText="Categoria" />
            <asp:BoundField DataField="Ubicacion" HeaderText="Ubicación" />
            <asp:BoundField DataField="NombreDistrito" HeaderText="Distrito" />
            <asp:BoundField DataField="Fecha" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy}" HtmlEncode="false" />
            <asp:BoundField DataField="HoraInicio" HeaderText="Hora Inicio" DataFormatString="{0:HH:mm}" HtmlEncode="false" />
            <asp:BoundField DataField="HoraFin" HeaderText="Hora Fin" DataFormatString="{0:HH:mm}" HtmlEncode="false" />
            <asp:TemplateField HeaderText="Opciones">
                <ItemTemplate>
                    <asp:Button ID="btnCancelar" runat="server" CssClass="btn btn-warning btn-sm fw-bold"
                        Text="Cancelar Reserva"
                        OnClientClick='<%# $"mostrarConfReserva(\"{Eval("NumReserva")}\",  \"{((DateTime)Eval("Fecha")).ToString("dd/MM/yyyy")}\"); return false;" %>' />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <div class="modal fade" id="mostrarConfReserva" tabindex="-1" aria-labelledby="mostrarConfReservaLabel" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header modal-header-rojo text-white">
                    <h5 class="modal-title" id="mostrarConfReservaLabel">Ventana de confirmación</h5>
                    <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Cerrar"></button>
                </div>
                <div class="modal-body d-flex align-items-center modal-body-confirmacion">
                    <div class="icono-confirmacion me-3">
                        <div class="icono-circulo">
                            <i class="bi bi-info-lg fs-1"></i>
                        </div>
                    </div>
                    <asp:Label ID="modalConfirmacionCuerpo" runat="server" CssClass="text-danger fw-bold" />
                </div>
                <div class="modal-footer justify-content-center">
                    <button type="button" class="btn btn-dark px-4" data-bs-dismiss="modal"><em>Cerrar</em></button>
                    <asp:Button ID="btnConfirmarAccion" runat="server" CssClass="btn btn-dark px-4" Text="Sí" OnClick="btnConfirmarAccion_Click" />
                </div>
            </div>
        </div>
    </div>
    <script>
        function mostrarConfReserva(id, fecha) {
            var modalElement = document.getElementById('mostrarConfReserva');
            if (!modalElement) {
                console.error("No se encontró el modal.");
                return;
            }
            // ✅ Obtener los elementos correctamente usando ClientID
            var hdnReserva = document.getElementById('<%= hdnReservaAEliminar.ClientID %>');
            var hdnFecha = document.getElementById('<%= hdnFechaEliminar.ClientID %>');
            var label = document.getElementById('<%= modalConfirmacionCuerpo.ClientID %>');
            var btnConfirmar = document.getElementById('<%= btnConfirmarAccion.ClientID %>');
            hdnReserva.value = id;
            hdnFecha.value = fecha;
            var partes = fecha.split('/');
            var fechaReserva = new Date(partes[2], partes[1] - 1, partes[0]); // yyyy, mm (0-indexed), dd
            // Normalizar hora a medianoche para ambas fechas
            var hoy = new Date();
            hoy.setHours(0, 0, 0, 0);
            fechaReserva.setHours(0, 0, 0, 0);
            var texto = "";
            var diferenciaDias = Math.floor((fechaReserva - hoy) / (1000 * 60 * 60 * 24));
            if (fechaReserva <= hoy) {
                texto = "No se puede cancelar una resreva que ya haya pasado.";
                btnConfirmar.style.display = "none";
            } else if (diferenciaDias < 2) {
                texto = "La reserva debe cancelarse con un mínimo de 2 días de anticipación.";
                btnConfirmar.style.display = "none";
            } else {
                texto = "¿Está seguro que desea cancelar su reserva? Este proceso no puede deshacerse.";
                btnConfirmar.style.display = "inline-block";
            }
            label.innerText = texto;
            // Mostrar modal con Bootstrap 5
            var modalEliminar = new bootstrap.Modal(modalElement);
            modalEliminar.show();
        }
    </script>
</asp:Content>