<%@ Page Title="" Language="C#" MasterPageFile="~/MainLayout.Master" AutoEventWireup="true" CodeBehind="ListaEntradasComprador.aspx.cs" Inherits="SirgepPresentacion.Presentacion.Ventas.Entrada.ListaEntradasComprador" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Encabezado" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Contenido" runat="server">
    <!-- Título principal -->
    <h2 class="fw-bold mb-4">Entradas</h2>

    <!-- Búsqueda -->
    
    <div class="container">
        <!-- BOTONES -->
        <div class="d-flex justify-content-between mb-3">
            <asp:Button ID="btnDescargar" runat="server" Text="Descargar" CssClass="btn btn-dark" OnClick="btnDescargar_Click" UseSubmitBehavior="false" />
        </div>
    </div>

    <!-- Tabla -->
    <asp:GridView ID="GvListaEntradasComprador" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true"
        AllowPaging="true" PageSize="5" OnPageIndexChanging="GvListaEntradasComprador_PageIndexChanging"
        CssClass="table table-striped table-responsive table-hover">
        <Columns>
            <asp:BoundField DataField="NumEntrada" HeaderText="Nro Entrada" />
            <asp:BoundField DataField="NombreEvento" HeaderText="Evento" />
            <asp:BoundField DataField="Ubicacion" HeaderText="Ubicación" />
            <asp:BoundField DataField="NombreDistrito" HeaderText="Distrito" />
            <asp:BoundField DataField="Fecha" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy}" HtmlEncode="false" />
            <asp:BoundField DataField="HoraInicio" HeaderText="Hora Inicio" DataFormatString="{0:HH:mm}" HtmlEncode="false" />
            <asp:BoundField DataField="HoraFin" HeaderText="Hora Fin" DataFormatString="{0:HH:mm}" HtmlEncode="false" />
            <asp:TemplateField HeaderText="Abrir">
                <ItemTemplate>
                    <asp:LinkButton ID="BtnAbrir" runat="server"
                        CommandArgument='<%# Eval("NumEntrada") %>'
                        OnClick="BtnAbrir_Click"
                        CssClass="btn btn-link"
                        ToolTip="Abrir entrada">
                        <i class="fa-solid fa-arrow-right"></i>
                    </asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
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