﻿<%@ Page Title="Municipalidad > Reservas" Language="C#" MasterPageFile="~/MainLayout.Master" AutoEventWireup="true" CodeBehind="ListaReservasAdministrador.aspx.cs" Inherits="SirgepPresentacion.Presentacion.Ventas.Reserva.ListaReservasAdministrador" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" runat="server">
    <div class="container-reservas">
        <h1 class="titulo-principal">Municipalidad &gt; Reservas</h1>
        <div class="busqueda-filtros">
            <div class="busqueda">
                <input type="text" id="input_busqueda" runat="server" class="input-busqueda" placeholder="🔍 Buscar" onkeypress="return buscarOnEnter(event)" />
                <asp:Button ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click" Style="display:none" />
            </div>
            <div class="filtros">
                <label class="filtros-label">Filtros:</label>
                <asp:DropDownList ID="ddlFiltros" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlFiltros_SelectedIndexChanged" CssClass="filtro-select">
                    <asp:ListItem Text="Fecha" Value="fecha" />
                    <asp:ListItem Text="Nombre del distrito" Value="distrito" />
                </asp:DropDownList>
                <asp:DropDownList ID="ddlDistritos" runat="server" CssClass="filtro-select" />
                <asp:TextBox ID="txtFecha" runat="server" CssClass="form-control filtro-fecha" TextMode="Date" />
                <div class="filtro-activos-checkbox">
                    <input type="text" class="input-filtro" value="Filtro por Activos" readonly />
                    <asp:CheckBox ID="chkActivos" runat="server" CssClass="checkbox-filtro" />
                </div>
                <asp:Button ID="btnFiltrar" runat="server" Text="Filtrar" CssClass="btn btn-dark mx-2" OnClick="btnFiltrar_Click" />
            </div>
        </div>
        <asp:GridView ID="gvReservas" runat="server" AutoGenerateColumns="False" CssClass="tabla-reservas" GridLines="None">
            <Columns>
                <asp:TemplateField HeaderText="Abrir">
                    <ItemTemplate>
                        <a href='<%# "ConstanciaReserva.aspx?numReserva=" + Eval("codigo") %>'>
                            <img src="/Images/icons/open-link.png" alt="Abrir" class="icono-abrir" />
                        </a>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Código">
                    <ItemTemplate>
                        <%# "#" + Convert.ToInt32(Eval("codigo")).ToString("D3") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Fecha" HeaderText="Fecha" DataFormatString="{0:yyyy-MM-dd}" />
                <asp:BoundField DataField="Distrito" HeaderText="Distrito" />
                <asp:BoundField DataField="Espacio" HeaderText="Espacio" />
                <asp:BoundField DataField="Correo" HeaderText="Correo del usuario" />
                <asp:TemplateField HeaderText="¿Activo?">
                    <ItemTemplate>
                        <%# Eval("Activo").ToString() == "65" ? "Sí" : "No" %>
                    </ItemTemplate>
                </asp:TemplateField>


            </Columns>
        </asp:GridView>

        <!-- Cambio de paginado -->
        <asp:Panel ID="pnlPaginacion" runat="server" CssClass="d-flex justify-content-center my-3">
            <asp:Button ID="btnAnterior" runat="server" Text="Anterior" CssClass="btn btn-outline-dark mx-1" OnClick="btnAnterior_Click" />
            <asp:Label ID="lblPagina" runat="server" CssClass="align-self-center mx-2" />
            <asp:Button ID="btnSiguiente" runat="server" Text="Siguiente" CssClass="btn btn-outline-dark mx-1" OnClick="btnSiguiente_Click" />
        </asp:Panel>
    </div>
     <!-- Java Script -->
    <script type="text/javascript">
        function buscarOnEnter(e) {
            if (e.key === 'Enter' || e.keyCode === 13) {
                e.preventDefault(); // evitar submit automático
                __doPostBack('<%= btnBuscar.UniqueID %>', ''); // simula click en botón
                return false;
            }
            return true;
        }
    </script>

</asp:Content>
