<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListaEspacios.aspx.cs" Inherits="SirgepPresentacion.ListaEspacios" MasterPageFile="~/MainLayout.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Lista de Espacios
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Contenido" runat="server">
    <div class="d-flex justify-content-between align-items-center mb-4">
        <h2>Gestión de Espacios</h2>
        <asp:Button ID="btnAgregarEspacio" runat="server" CssClass="btn btn-primary" Text="Agregar Espacio" OnClick="btnAgregarEspacio_Click" />
    </div>

    <table class="table table-striped table-bordered">
        <thead class="table-dark">
            <tr>
                <th>ID</th>
                <th>Nombre</th>
                <th>Descripción</th>
                <th>Acciones</th>
            </tr>
        </thead>
        <tbody>
            <asp:Repeater ID="rptEspacios" runat="server">
                <ItemTemplate>
                    <tr>
                        <td><%# Eval("Id") %></td>
                        <td><%# Eval("Nombre") %></td>
                        <td><%# Eval("Descripcion") %></td>
                        <td>
                            <asp:Button ID="btnEditar" runat="server" CssClass="btn btn-warning btn-sm" Text="Editar" CommandArgument='<%# Eval("Id") %>' OnClick="btnEditar_Click" />
                            <asp:Button ID="btnEliminar" runat="server" CssClass="btn btn-danger btn-sm" Text="Eliminar" CommandArgument='<%# Eval("Id") %>' OnClick="btnEliminar_Click" />
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </tbody>
    </table>
</asp:Content>
