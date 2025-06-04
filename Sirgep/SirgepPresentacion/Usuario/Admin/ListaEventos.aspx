<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListaEventos.aspx.cs" Inherits="SirgepPresentacion.ListaEventos" MasterPageFile="~/MainLayout.Master" %>

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
    <div class="mb-4">
        <label class="fw-bold d-block mb-3" style="font-size: 1.2rem;">Filtros:</label>

        <div class="row g-3 align-items-end">
            <!-- 1. Actividad -->
            <div class="col-12 col-md-6 col-lg-3">
                <asp:DropDownList ID="ddlActividad" runat="server" CssClass="form-select w-100">
                    <asp:ListItem Text="Filtro por Actividad" Value="" />
                    <asp:ListItem Text="Fútbol" Value="Futbol" />
                    <asp:ListItem Text="Vóley" Value="Voley" />
                    <asp:ListItem Text="Auditorio" Value="Auditorio" />
                </asp:DropDownList>
            </div>

            <!-- 2. Checkbox de filtro por fecha -->
            <div class="col-12 col-md-6 col-lg-3">
                <div class="input-group">
                    <input type="text" class="form-control bg-white" value="Filtro por Fechas" readonly />
                    <span class="input-group-text bg-white px-2">
                        <input type="checkbox" id="chkFiltroFechas" class="form-check-input m-0"
                            style="width: 1.1em; height: 1.1em;" onclick="toggleFechas()" />
                    </span>
                </div>
            </div>

            <!-- 3. Fechas inicio / fin en una fila -->
            <div class="col-12 col-md-6 col-lg-3" id="filtrosFechas" style="display: none;">
                <div class="row gx-2">
                    <div class="col-6">
                        <label class="mb-1 w-100" style="font-size: 0.85rem;">Inicio:</label>
                        <input type="date" class="form-control form-control-sm" />
                    </div>
                    <div class="col-6">
                        <label class="mb-1 w-100" style="font-size: 0.85rem;">Fin:</label>
                        <input type="date" class="form-control form-control-sm" />
                    </div>
                </div>
            </div>

            <!-- 4. Botón consultar alineado a la derecha -->
            <div class="col-12 col-md-6 col-lg-3">
                <div class="d-flex justify-content-lg-end">
                    <asp:Button ID="btnConsultar" runat="server" Text="Consultar"
                        CssClass="btn btn-dark px-4 fw-semibold fst-italic" />
                </div>
            </div>
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
        <asp:Button ID="btnMostrarModalAgregarEvento" runat="server" CssClass="btn btn-dark fw-bold fst-italic px-4"
    Text="Añadir Evento" OnClick="btnMostrarModalAgregarEvento_Click" />
    </div>

    <div class="modal fade" id="modalAgregarEvento" tabindex="-1" aria-labelledby="modalAgregarEventoLabel" aria-hidden="true">
        <div class="modal-dialog modal-xl modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header bg-dark text-white">
                    <h5 class="modal-title" id="modalAgregarEventoLabel">Añadir Evento</h5>
                    <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Cerrar"></button>
                </div>

                <div class="modal-body">
                    <!-- Campos del evento -->
                    <div class="row g-3">
                        <!-- Nombre del evento -->
                        <div class="col-md-6">
                            <label class="form-label fw-semibold">Nombre del Evento:</label>
                            <input type="text" class="form-control" placeholder="Ingrese el nombre" />
                        </div>

                        <!-- Categoría -->
                        <div class="col-md-6">
                            <label class="form-label fw-semibold">Categoría:</label>
                            <select class="form-select">
                                <option value="">Seleccione categoría</option>
                                <option>Fútbol</option>
                                <option>Vóley</option>
                                <option>Auditorio</option>
                            </select>
                        </div>

                        <!-- Dirección -->
                        <div class="col-12">
                            <label class="form-label fw-semibold">Ubicación (Dirección):</label>
                            <input type="text" class="form-control" placeholder="Ingrese dirección" />
                        </div>

                        <!-- Descripción -->
                        <div class="col-12">
                            <label class="form-label fw-semibold">Descripción del Evento:</label>
                            <textarea class="form-control" rows="3" placeholder="Ingrese una descripción..."></textarea>
                        </div>

                        <!-- Foto -->
                        <div class="col-md-6">
                            <label class="form-label fw-semibold">Foto:</label>
                            <input type="file" class="form-control" />
                        </div>
                        <div class="col-md-6 d-flex align-items-end">
                            <button type="button" class="btn btn-outline-primary w-100" onclick="verFoto()">
                                Ver Foto
                            </button>
                        </div>
                    </div>

                    <hr class="my-4" />

                    <!-- Lista de funciones -->
                    <h6 class="fw-bold mb-3">Funciones del Evento:</h6>
                    <div class="table-responsive mb-3">
                        <table class="table table-bordered align-middle text-center">
                            <thead class="table-light">
                                <tr>
                                    <th>Fecha</th>
                                    <th>Hora Inicio</th>
                                    <th>Hora Fin</th>
                                    <th>Acciones</th>
                                </tr>
                            </thead>
                            <tbody id="tablaFunciones">
                                <!-- Aquí se añadirán las funciones dinámicamente -->
                            </tbody>
                        </table>
                    </div>

                    <!-- Añadir función -->
                    <div class="row g-3 align-items-end">
                        <div class="col-md-4">
                            <label class="form-label">Fecha:</label>
                            <input type="date" class="form-control" id="fechaFuncion" />
                        </div>
                        <div class="col-md-3">
                            <label class="form-label">Hora Inicio:</label>
                            <input type="time" class="form-control" id="horaInicioFuncion" />
                        </div>
                        <div class="col-md-3">
                            <label class="form-label">Hora Fin:</label>
                            <input type="time" class="form-control" id="horaFinFuncion" />
                        </div>
                        <div class="col-md-2">
                            <button type="button" class="btn btn-success w-100" onclick="agregarFuncion()">Añadir</button>
                        </div>
                    </div>
                </div>

                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                    <button type="button" class="btn btn-primary">Aceptar</button>
                </div>
            </div>
        </div>
    </div>


    <script type="text/javascript">
        function toggleFechas() {
            var check = document.getElementById('chkFiltroFechas');
            var filtros = document.getElementById('filtrosFechas');
            filtros.style.display = check.checked ? 'block' : 'none';
        }
        function verFoto() {
            alert("Mostrar vista previa de la foto (puedes implementar esto con FileReader si deseas)");
        }

        function agregarFuncion() {
            const fecha = document.getElementById('fechaFuncion').value;
            const horaInicio = document.getElementById('horaInicioFuncion').value;
            const horaFin = document.getElementById('horaFinFuncion').value;

            if (!fecha || !horaInicio || !horaFin) {
                alert("Por favor complete todos los campos de la función.");
                return;
            }

            const tbody = document.getElementById("tablaFunciones");
            const row = document.createElement("tr");
            row.innerHTML = `
            <td>${fecha}</td>
            <td>${horaInicio}</td>
            <td>${horaFin}</td>
            <td>
                <button class="btn btn-danger btn-sm" onclick="this.closest('tr').remove()">Eliminar</button>
            </td>
        `;
            tbody.appendChild(row);

            // Limpiar inputs
            document.getElementById('fechaFuncion').value = '';
            document.getElementById('horaInicioFuncion').value = '';
            document.getElementById('horaFinFuncion').value = '';
        }
    </script>
</asp:Content>
