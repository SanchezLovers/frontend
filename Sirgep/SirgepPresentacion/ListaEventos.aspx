<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListaEventos.aspx.cs" Inherits="SirgepPresentacion.ListaEventos" MasterPageFile="~/MainLayout.Master" %>

<asp:Content ID="HeaderLinkContent1" ContentPlaceHolderID="HeaderLinkContent" runat="server">
    <a href="PrincipalAdministrador.aspx" class="d-flex align-items-center me-3 text-decoration-none header-link">
        <img src="/Images/grl/Escudo_Región_Lima_recortado.PNG" alt="Escudo Región Lima" class="navbar-brand-img me-3" style="height: 40px;" />
        <div class="text-white">
            <strong>Gobierno Regional de Lima</strong>
        </div>
    </a>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Municipalidad &gt; Eventos
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Contenido" runat="server">
    <!-- Título principal -->
    <h2 class="fw-bold mb-4" style="font-size: 2.8rem;">Municipalidad &gt; Eventos</h2>

    <!-- Búsqueda -->
    <div class="row mb-3">
        <div class="col-12 col-md-6">
            <div class="input-group">
                <span class="input-group-text bg-white border-end-0">🔍</span>
                <input type="text" class="form-control border-start-0" placeholder="Buscar" style="font-size: 1rem;" />
            </div>
        </div>
    </div>

    <!-- Filtros -->
   <div class="row mb-4 align-items-end">
    <div class="col-12 col-md-4 col-lg-3 mb-3 mb-md-0">
        <label class="fw-bold mb-2" style="font-size:1.2rem;">Filtros:</label>
        <asp:DropDownList ID="ddlActividad" runat="server" CssClass="form-select" style="max-width:320px;">
            <asp:ListItem Text="Filtro por Actividad" Value="" />
            <asp:ListItem Text="Fútbol" Value="Futbol" />
            <asp:ListItem Text="Vóley" Value="Voley" />
            <asp:ListItem Text="Auditorio" Value="Auditorio" />
        </asp:DropDownList>
    </div>
    <div class="col-12 col-md-4 col-lg-3 mb-3 mb-md-0 d-flex align-items-end">
        <div class="input-group" style="max-width:320px;">
            <input type="text" class="form-control" value="Filtro por Fechas" readonly style="background: #fff; cursor: default;"/>
            <span class="input-group-text bg-white px-2">
                <input type="checkbox" id="chkFiltroFechas" class="form-check-input m-0" style="width:1.1em;height:1.1em;" onclick="toggleFechas()" />
            </span>
        </div>
    </div>
    <div class="col-12 col-md-4 col-lg-6 d-flex align-items-end justify-content-end">
        <div class="d-flex align-items-end w-100" id="filtrosFechas" style="display:none;">
            <div class="me-2">
                <label class="mb-1" style="font-size:0.95rem;">Fecha Inicio:</label>
                <input type="date" class="form-control form-control-sm" style="width:140px;display:inline-block;" placeholder="DD/MM/AAAA" />
            </div>
            <div class="me-2">
                <label class="mb-1" style="font-size:0.95rem;">Fecha Fin:</label>
                <input type="date" class="form-control form-control-sm" style="width:140px;display:inline-block;" placeholder="DD/MM/AAAA" />
            </div>
        </div>
        <asp:Button ID="btnConsultar" runat="server" Text="Consultar" CssClass="btn btn-dark px-4 fw-semibold fst-italic ms-2" />
    </div>
</div>


    <!-- Tabla -->
    <div class="table-responsive">
        <table class="table table-bordered text-center align-middle" style="background: #f7f7f7;">
            <thead class="table-light fw-bold">
                <tr>
                    <th>Abrir</th>
                    <th>Código</th>
                    <th>Categoría</th>
                    <th>Evento Reservado</th>
                    <th>Acciones</th>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater ID="rptEventos" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <a href='<%# Eval("LinkDetalle") %>'>
                                    <img src="/Images/icons/open-link.png" alt="Abrir" style="width: 24px;" />
                                </a>
                            </td>
                            <td><%# Eval("Codigo") %></td>
                            <td><%# Eval("Categoria") %></td>
                            <td><%# Eval("EventoReservado") %></td>
                            <td>
                                <asp:Button ID="btnEditar" runat="server" CssClass="btn btn-warning btn-sm fw-bold me-2" Text="Editar" CommandArgument='<%# Eval("Id") %>' OnClick="btnEditar_Click" />
                                <asp:Button ID="btnEliminar" runat="server" CssClass="btn btn-danger btn-sm fw-bold"
                                    Text="Eliminar" CommandArgument='<%# Eval("Id") %>'
                                    OnClientClick='<%# $"mostrarModalConfirmacion({Eval("Id")}); return false;" %>' />

                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
    </div>

    <!-- Botón de añadir -->
    <div class="text-end mt-3">
        <button class="btn btn-dark fw-bold fst-italic px-4">Añadir Evento</button>
    </div>

    <script type="text/javascript">
        function toggleFechas() {
            var check = document.getElementById('chkFiltroFechas');
            var filtros = document.getElementById('filtrosFechas');
            filtros.style.display = check.checked ? 'flex' : 'none';
        }
    </script>
</asp:Content>
