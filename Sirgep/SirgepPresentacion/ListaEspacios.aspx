<%@ Page Title="Lista de Espacios" Language="C#" MasterPageFile="~/MainLayout.Master" AutoEventWireup="true" CodeBehind="ListaEspacios.aspx.cs" Inherits="SirgepPresentacion.ListaEspacios" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Encabezado" runat="server">
    <h2 class="mt-4">Gestión de Espacios</h2>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Contenido" runat="server">
    <div class="card">
        <div class="card-header bg-primary text-white">
            Agregar Nuevo Espacio
        </div>
        <div class="card-body">
            <!-- Ejemplo de formulario -->
            <asp:Panel runat="server" CssClass="mb-3">
                <div class="mb-3">
                    <label for="txtNombre" class="form-label">Nombre del espacio:</label>
                    <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" />
                </div>

                <div class="mb-3">
                    <label for="txtUbicacion" class="form-label">Ubicación:</label>
                    <asp:TextBox ID="txtUbicacion" runat="server" CssClass="form-control" />
                </div>

                <asp:Button ID="btnAgregar" runat="server" CssClass="btn btn-success" Text="Agregar" OnClick="btnAgregar_Click" />
            </asp:Panel>
        </div>
    </div>
</asp:Content>
