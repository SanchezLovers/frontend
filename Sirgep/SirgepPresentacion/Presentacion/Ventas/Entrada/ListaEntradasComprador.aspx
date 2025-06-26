<%@ Page Title="" Language="C#" MasterPageFile="~/MainLayout.Master" AutoEventWireup="true" CodeBehind="ListaEntradasComprador.aspx.cs" Inherits="SirgepPresentacion.Presentacion.Ventas.Entrada.ListaEntradasComprador" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Encabezado" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Contenido" runat="server">
    <!-- Título principal -->
    <h2 class="fw-bold mb-4">Entradas</h2>
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
            <asp:Button ID="btnDescargarMostrarModal" runat="server" Text="Descargar Lista de Entradas" OnClientClick="setTimeout(function() { mostrarModalExito('Descarga exitosa', 'La lista de entradas fue descargada correctamente.'); }, 1000);" OnClick="btnDescargar_Click" CssClass="btn btn-dark" />
        </div>
    </div>
    <!-- GridView -->
    <asp:GridView ID="GvListaEntradasComprador" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true"
        AllowPaging="true" PageSize="5" OnPageIndexChanging="GvListaEntradasComprador_PageIndexChanging"
        CssClass="table table-striped table-responsive table-hover">
        <Columns>
            <asp:BoundField DataField="NumEntrada" HeaderText="Nro Entrada" />
            <asp:BoundField DataField="NombreEvento" HeaderText="Evento" />
            <asp:BoundField DataField="Ubicacion" HeaderText="Ubicación" />
            <asp:BoundField DataField="NombreDistrito" HeaderText="Distrito" />
            <asp:BoundField DataField="FechaFuncion" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy}" HtmlEncode="false" />
            <asp:BoundField DataField="HoraInicio" HeaderText="Hora Inicio" DataFormatString="{0:HH:mm}" HtmlEncode="false" />
            <asp:BoundField DataField="HoraFin" HeaderText="Hora Fin" DataFormatString="{0:HH:mm}" HtmlEncode="false" />
            <asp:TemplateField HeaderText="Abrir">
                <ItemTemplate>
                    <asp:LinkButton ID="BtnAbrir" runat="server"
                        CommandArgument='<%# Eval("IdConstancia") %>'
                        OnClick="BtnAbrir_Click"
                        CssClass="btn btn-link"
                        ToolTip="Abrir entrada">
                        <i class="bi bi-box-arrow-up-right"></i>
                    </asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</asp:Content>