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
            <asp:Button ID="btnDescargarMostrarModal" runat="server" Text="Descargar Lista de Entradas" OnClientClick="setTimeout(function() { mostrarModalExitoPersonalizado('Descarga exitosa', 'La lista de entradas fue descargada correctamente.'); }, 1000);" OnClick="btnDescargar_Click" CssClass="btn btn-dark" />
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
                        <i class="bi bi-box-arrow-up-right"></i>
                    </asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <!-- MODAL -->
    <script type="text/javascript">
        function mostrarModalExitoPersonalizado(titulo, mensaje) {
            // Cambia el título y mensaje del modal de éxito
            document.getElementById("modalExitoLabel").innerText = titulo;
            document.getElementById("modalExitoBody").innerText = mensaje;
            // Muestra el modal
            var myModal = new bootstrap.Modal(document.getElementById('modalExito'));
            myModal.show();
        }
    </script>
</asp:Content>