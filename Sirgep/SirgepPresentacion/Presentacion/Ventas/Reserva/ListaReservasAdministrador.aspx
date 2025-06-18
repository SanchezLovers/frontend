<%@ Page Title="Municipalidad > Reservas" Language="C#" MasterPageFile="~/MainLayout.Master" AutoEventWireup="true" CodeBehind="ListaReservasAdministrador.aspx.cs" Inherits="SirgepPresentacion.Presentacion.Ventas.Reserva.ListaReservasAdministrador" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" runat="server">
    <div class="container-reservas">
        <h1 class="titulo-principal">Municipalidad &gt; Reservas</h1>
        <div class="busqueda-filtros">
            <div class="busqueda">
                <input type="text" id="input_busqueda" runat="server" class="input-busqueda" placeholder="🔍 Buscar" />
            </div>
            <div class="filtros">
                <label class="filtros-label">Filtros:</label>
                <asp:DropDownList ID="ddlFiltros" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlFiltros_SelectedIndexChanged" CssClass="filtro-select">
                    <asp:ListItem Text="Código" Value="codigo" />
                    <asp:ListItem Text="Fecha" Value="fecha" />
                    <asp:ListItem Text="Fecha y Horario" Value="horario" />
                    <asp:ListItem Text="Persona" Value="persona" />
                    <asp:ListItem Text="Espacio" Value="espacio" />
                    <asp:ListItem Text="Distrito" Value="distrito" />
                </asp:DropDownList>
                <asp:DropDownList ID="ddlDistritos" runat="server" CssClass="filtro-select" />
                <asp:TextBox ID="txtFecha" runat="server" CssClass="form-control filtro-fecha" TextMode="Date" />
                <div id="horaContainerIni" runat="server" class="hora-container">
                    <asp:Label ID="lblHoraIni" runat="server" AssociatedControlID="inputHoraIni" Text="Horario Inicio:" />
                    <input type="number" id="inputHoraIni" runat="server" class="filtro-horario" min="0" max="23" step="1" />
                </div>
                <div id="horaContainerFin" runat="server" class="hora-container">
                    <asp:Label ID="lblHoraFin" runat="server" AssociatedControlID="inputHoraFin" Text="Horario Fin:" />
                    <input type="number" id="inputHoraFin" runat="server" class="filtro-horario" min="0" max="23" step="1" />
                </div>
                <div class="filtro-activos-checkbox">
                    <input type="text" class="input-filtro" value="Filtro por Activos" readonly />
                    <asp:CheckBox ID="chkActivos" runat="server" CssClass="checkbox-filtro" />
                </div>
                <asp:Button ID="btnFiltrar" runat="server" Text="Filtrar" CssClass="btn btn-dark mx-2" OnClick="btnFiltrar_Click" />
            </div>
        </div>
        <div class="tabla-reservas">
            <asp:GridView ID="gvReservas" runat="server" AutoGenerateColumns="False" CssClass="tabla-reservas" GridLines="None">
                <Columns>
                    <asp:TemplateField HeaderText="Abrir">
                        <ItemTemplate>
                            <%--<a href='<%# "DetalleReserva.aspx?id=" + Eval("Codigo") %>'>--%>
                            <img src="/Images/icons/open-link.png" alt="Abrir" class="icono-abrir" />
                            </a>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Código">
                        <ItemTemplate>
                            <%# "#" + Convert.ToInt32(Eval("numReserva")).ToString("D3") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Horario">
                        <ItemTemplate>
                            <%# Eval("iniString") + " - " + Eval("finString") %>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:BoundField DataField="fechaReserva" HeaderText="Fecha" DataFormatString="{0:yyyy-MM-dd}" />
                    <asp:BoundField DataField="persona.idPersona" HeaderText="Persona" />
                    <asp:BoundField DataField="espacio.idEspacio" HeaderText="Espacio" />
                    <asp:BoundField DataField="idConstancia" HeaderText="Constancia" />
                    <asp:TemplateField HeaderText="Activo">
                        <ItemTemplate>
                            <span class="icono-activo">
                                <%--<El servicio SOAP devuelve el caracter como ASCII />--%>
                                <%# Eval("activo").ToString() == "65" ? "Sí" : "No" %>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <!-- Cambio de paginado -->
        <asp:Panel ID="pnlPaginacion" runat="server" CssClass="d-flex justify-content-center my-3">
            <asp:Button ID="btnAnterior" runat="server" Text="Anterior" CssClass="btn btn-outline-dark mx-1" OnClick="btnAnterior_Click" />
            <asp:Label ID="lblPagina" runat="server" CssClass="align-self-center mx-2" />
            <asp:Button ID="btnSiguiente" runat="server" Text="Siguiente" CssClass="btn btn-outline-dark mx-1" OnClick="btnSiguiente_Click" />
        </asp:Panel>
    </div>
</asp:Content>

