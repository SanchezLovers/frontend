<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListaEspacios.aspx.cs" Inherits="SirgepPresentacion.ListaEspacios" MasterPageFile="~/MainLayout.Master" %>

<asp:Content ID="HeaderLinkContent1" ContentPlaceHolderID="HeaderLinkContent" runat="server">
    <a href="PrincipalAdministrador.aspx" class="d-flex align-items-center me-3 text-decoration-none header-link">
        <img src="/Images/grl/Escudo_Región_Lima_recortado.PNG" alt="Escudo Región Lima" class="navbar-brand-img me-3" style="height: 40px;" />
        <div class="text-white">
            <strong>Gobierno Regional de Lima</strong>
        </div>
    </a>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Municipalidad > Espacios
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Contenido" runat="server">
    <!-- Título principal -->
    <h2 class="fw-bold mb-4">Municipalidad &gt; Espacios</h2>

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
        <div class="col-12 col-md-6">
            <label class="fw-bold mb-2" style="font-size: 1.3rem;">Filtros:</label>
            <asp:DropDownList ID="ddlCategoria" runat="server" CssClass="form-select w-100" AutoPostBack="true" OnSelectedIndexChanged="ddlCategoria_SelectedIndexChanged">
                <asp:ListItem Text="Filtro por Fechas" Value="0" />
                <asp:ListItem Text="Filtrar" Value="1" />
            </asp:DropDownList>
        </div>
        <div class="col-12 col-md-6 text-md-end mt-3 mt-md-0">
            <asp:Button ID="btnConsultar" runat="server" Text="Consultar" CssClass="btn btn-dark px-4 fw-semibold fst-italic" OnClick="btnConsultar_Click" />
        </div>
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
        <asp:Button ID="btnAgregarEspacio" runat="server" CssClass="btn btn-dark fw-bold" Text="Añadir" OnClick="btnAgregarEspacio_Click" />
    </div>
    <!-- Modal Paso 1: Datos del Espacio -->
    <div class="modal fade" id="modalPaso1" tabindex="-1" aria-labelledby="modalPaso1Label" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header bg-dark text-white">
                    <h5 class="modal-title" id="modalPaso1Label">Registrar Espacio</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Cerrar"></button>
                </div>
                <div class="modal-body">
                    <div class="mb-3">
                        <label>Nombre del espacio:</label>
                        <asp:TextBox ID="txtNombreEspacio" runat="server" CssClass="form-control" />
                    </div>
                    <div class="mb-3">
                        <label>Tipo de espacio:</label>
                        <asp:DropDownList ID="ddlTipoEspacio" runat="server" CssClass="form-select">
                        </asp:DropDownList>
                    </div>
                    <div class="mb-3">
                        <label>Ubicación (Dirección):</label>
                        <asp:TextBox ID="txtUbicacion" runat="server" CssClass="form-control" />
                    </div>
                    <div class="mb-3">
                        <label>Superficie (m²):</label>
                        <asp:TextBox ID="txtSuperficie" runat="server" CssClass="form-control" TextMode="Number" />
                    </div>
                    <div class="mb-3">
                        <label>Precio de reserva (S/):</label>
                        <asp:TextBox ID="txtPrecio" runat="server" CssClass="form-control" TextMode="Number" />
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                    <button type="button" class="btn btn-dark" onclick="abrirPaso2()">Siguiente</button>
                </div>
            </div>
        </div>
    </div>

    <!-- Modal Paso 2: Horario -->
    <div class="modal fade" id="modalPaso2" tabindex="-1" aria-labelledby="modalPaso2Label" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header bg-dark text-white">
                    <h5 class="modal-title" id="modalPaso2Label">Horario del Espacio</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Cerrar"></button>
                </div>
                <div class="modal-body">
                    <div class="mb-3">
                        <label>Nombre del espacio:</label>
                        <asp:TextBox ID="txtNombreEspacioResumen" runat="server" CssClass="form-control" Enabled="false" />
                    </div>

                    <div class="row g-2 align-items-end mb-3">
                        <div class="col-md-4">
                            <label>Día:</label>
                            <asp:DropDownList ID="ddlDiaSemana" runat="server" CssClass="form-select">
                                <asp:ListItem Text="Lunes" Value="Lunes" />
                                <asp:ListItem Text="Martes" Value="Martes" />
                                <asp:ListItem Text="Miércoles" Value="Miércoles" />
                                <asp:ListItem Text="Jueves" Value="Jueves" />
                                <asp:ListItem Text="Viernes" Value="Viernes" />
                                <asp:ListItem Text="Sábado" Value="Sábado" />
                                <asp:ListItem Text="Domingo" Value="Domingo" />
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-3">
                            <label>Inicio:</label>
                            <asp:TextBox ID="txtHoraInicio" runat="server" CssClass="form-control" TextMode="Time" />
                        </div>
                        <div class="col-md-3">
                            <label>Fin:</label>
                            <asp:TextBox ID="txtHoraFin" runat="server" CssClass="form-control" TextMode="Time" />
                        </div>
                        <div class="col-md-2 text-end">
                            <asp:Button ID="btnAgregarHorario" runat="server" CssClass="btn btn-success w-100" Text="Añadir" OnClick="btnAgregarHorario_Click" />
                        </div>
                    </div>

                    <!-- Listado horarios añadidos -->
                    <asp:Repeater ID="rptHorarios" runat="server">
                        <HeaderTemplate>
                            <table class="table table-bordered">
                                <thead>
                                    <tr>
                                        <th>Día</th>
                                        <th>Hora Inicio</th>
                                        <th>Hora Fin</th>
                                    </tr>
                                </thead>
                                <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td><%# Eval("Dia") %></td>
                                <td><%# Eval("HoraInicio") %></td>
                                <td><%# Eval("HoraFin") %></td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </tbody>
                        </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" onclick="abrirPaso1()">Regresar</button>
                    <asp:Button ID="btnGuardarEspacio" runat="server" CssClass="btn btn-dark" Text="Aceptar" OnClick="btnGuardarEspacio_Click" />
                </div>
            </div>
        </div>
    </div>

    <!-- Modal de Confirmación -->
    <div class="modal fade" id="modalConfirmacion" tabindex="-1" aria-labelledby="modalConfirmacionLabel" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">

                <div class="modal-header modal-header-rojo text-white">
                    <h5 class="modal-title" id="modalConfirmacionLabel">Ventana de confirmación</h5>
                    <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Cerrar"></button>
                </div>

                <div class="modal-body d-flex align-items-center modal-body-confirmacion">
                    <div class="icono-confirmacion me-3">
                        <div class="icono-circulo">
                            <span class="icono-texto">i</span>
                        </div>
                    </div>
                    <div id="modalConfirmacionBody" class="fs-5">
                        ¿Está seguro que desea realizar esta acción?
                    </div>
                </div>

                <div class="modal-footer justify-content-center">
                    <button type="button" class="btn btn-dark px-4" data-bs-dismiss="modal"><em>No</em></button>
                    <asp:Button ID="btnConfirmarAccion" runat="server" CssClass="btn btn-dark px-4" Text="Sí" OnClick="btnConfirmarAccion_Click" />
                </div>

            </div>
        </div>
    </div>

    <asp:HiddenField ID="hdnIdAEliminar" runat="server" />


    <script type="text/javascript">
        function abrirPaso1() {
            // Oculta el modal actual (paso 2)
            var modal2 = bootstrap.Modal.getInstance(document.getElementById('modalPaso2'));
            if (modal2) modal2.hide();

            // Muestra el modal del paso anterior (paso 1)
            var modal1 = new bootstrap.Modal(document.getElementById('modalPaso1'));
            modal1.show();
        }



        function abrirPaso2() {
            // copiar nombre al segundo modal
            document.getElementById('<%= txtNombreEspacioResumen.ClientID %>').value = document.getElementById('<%= txtNombreEspacio.ClientID %>').value;
            var modal1 = bootstrap.Modal.getInstance(document.getElementById('modalPaso1'));
            modal1.hide();

            var modal2 = new bootstrap.Modal(document.getElementById('modalPaso2'));
            modal2.show();
        }
    </script>
    <script type="text/javascript">
        function mostrarModalConfirmacion(id) {
            // Actualizar mensaje si deseas personalizarlo dinámicamente
            document.getElementById('modalConfirmacionBody').innerText = "¿Está seguro que desea eliminar este espacio?";

            // Guardar el ID en campo oculto
            document.getElementById('<%= hdnIdAEliminar.ClientID %>').value = id;

            // Mostrar el modal
            var modal = new bootstrap.Modal(document.getElementById('modalConfirmacion'));
            modal.show();
        }
    </script>


</asp:Content>
