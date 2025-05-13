<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListaEspacios.aspx.cs" Inherits="SirgepPresentacion.ListaEspacios" MasterPageFile="~/MainLayout.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Municipalidad > Espacios
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Contenido" runat="server">
    <!-- Título principal -->
    <h2 class="fw-bold mb-4">Municipalidad &gt; Espacios</h2>

    <!-- Búsqueda -->
    <div class="mb-3">
        <input type="text" class="form-control" placeholder="🔍 Buscar" />
    </div>

    <!-- Filtros -->
    <div class="mb-4">
        <label class="fw-bold">Filtros:</label>
        <asp:DropDownList ID="ddlCategoria" runat="server" CssClass="form-select w-25 d-inline-block mx-2">
        </asp:DropDownList>
        <asp:Button ID="btnConsultar" runat="server" Text="Consultar" CssClass="btn btn-dark" OnClick="btnConsultar_Click" />
    </div>

    <!-- Tabla -->
    <div class="table-responsive">
        <table class="table table-bordered text-center">
            <thead class="table-light fw-bold">
                <tr>
                    <th>Abrir</th>
                    <th>Código</th>
                    <th>Categoría</th>
                    <th>Espacio Reservado</th>
                    <th>Acciones</th>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater ID="rptEspacios" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <a href='<%# Eval("LinkDetalle") %>'>
                                    <img src="/Images/icons/open-link.png" alt="Abrir" style="width: 24px;" />
                                </a>
                            </td>
                            <td><%# Eval("Codigo") %></td>
                            <td><%# Eval("Categoria") %></td>
                            <td><%# Eval("EspacioReservado") %></td>
                            <td>
                                <asp:Button ID="btnEditar" runat="server" CssClass="btn btn-warning btn-sm fw-bold me-2" Text="Editar" CommandArgument='<%# Eval("Id") %>' OnClick="btnEditar_Click" />
                                <asp:Button ID="btnEliminar" runat="server" CssClass="btn btn-danger btn-sm fw-bold" Text="Eliminar" CommandArgument='<%# Eval("Id") %>' OnClick="btnEliminar_Click" />
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
    </div>

    <!-- Botón de añadir -->
    <div class="text-end mt-3">
        <asp:Button ID="btnAgregarEspacio" runat="server" CssClass="btn btn-dark fw-bold" Text="Añadir" OnClick="btnAgregarEspacio_Click" />
    </div>

    
</asp:Content>
