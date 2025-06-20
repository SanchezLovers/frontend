﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListaEspacios.aspx.cs" Inherits="SirgepPresentacion.Presentacion.Infraestructura.Espacio.ListaEspacios" MasterPageFile="~/MainLayout.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Municipalidad > Espacios
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Contenido" runat="server">
    <asp:HiddenField ID="hdnIdAEliminar" runat="server" />
    <!-- Título principal -->
    <h2 class="fw-bold mb-4">Municipalidad &gt; Espacios</h2>

    <!-- Búsqueda -->
    <div class="mb-3">
        <asp:TextBox OnTextChanged="txtBusqueda_TextChanged" ID="txtBusqueda" runat="server" CssClass="form-control" Placeholder="🔍 Buscar" AutoPostBack="true"/>
    </div>

    <!-- Filtros -->
    <div class="mb-4">
        <label class="fw-bold">Filtros:</label>
        <asp:DropDownList ID="ddlCategoria" runat="server" CssClass="form-select w-25 d-inline-block mx-2">
                <asp:ListItem Text="Seleccione una categoría" Value="" />
                <asp:ListItem Text="Teatros" Value="TEATRO" />
                <asp:ListItem Text="Canchas" Value="CANCHA" />
                <asp:ListItem Text="Salones" Value="SALON" />
                <asp:ListItem Text="Parques" Value="PARQUE" />
        </asp:DropDownList>
        <asp:DropDownList ID="ddlDistrito" runat="server" CssClass="form-select w-25 d-inline-block mx-2">
                <asp:ListItem Text="Seleccione un distrito" Value="" />
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
                                <a href="ListaEspacios.aspx">
                                    <img src="/Images/icons/open-link.png" alt="Abrir" style="width: 24px;" />
                                </a>
                            </td>
                            <td><%# Eval("IdEspacio") %></td>
                            <td><%# Eval("TipoEspacio") %></td>
                            <td><%# Eval("Nombre") %></td>
                            <td>
                                <asp:Button ID="btnEditar" runat="server" CssClass="btn btn-warning btn-sm fw-bold me-2" Text="Editar" CommandArgument='<%# Eval("IdEspacio") %>' OnClick="btnEditar_Click" />
                                <asp:Button ID="btnEliminar" runat="server" CssClass="btn btn-danger btn-sm fw-bold"
                                Text="Eliminar" CommandArgument='<%# Eval("IdEspacio") %>'
                                OnClientClick='<%# $"mostrarConfEspacio({Eval("IdEspacio")}); return false;" %>' />
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
    <!-- Modal Paso 1: Datos del Espacio [ AGREGAR UN ESPACIO ] -->
    <div class="modal fade" id="modalPaso1" tabindex="-1" aria-labelledby="modalPaso1Label" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header" style="background-color: #f10909;">
                    <h2 class="modal-title" id="modalPaso1Label" style="color: white">Añadir - Espacio</h2>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Cerrar"></button>
                </div>

                <!--Cuerpo del modal [ AGREGAR UN ESPACIO ] -->
                <div class="modal-body">

                    <div class="row">
                        <div class="mb-3 col-md-6">
                            <label>Nombre del espacio</label>
                            <asp:TextBox ID="txtNombreEspacioAgregar" runat="server" CssClass="form-control" Placeholder="Inserte nombre del espacio" />
                        </div>
                        <div class="mb-3 col-md-4">
                            <label>Tipo de espacio</label>
                            <asp:DropDownList ID="ddlTipoEspacioAgregar" runat="server" CssClass="form-select">
                            </asp:DropDownList>
                        </div>
                    </div>

                    <div class="row">
                        <div class="mb-3 col-md-6">
                            <label>Ubicación</label>
                            <asp:TextBox ID="txtUbicacionAgregar" runat="server" CssClass="form-control" Placeholder="Inserte ubicación" />
                        </div>
                        <div class="mb-3 col-md-4">
                            <label>Departamento</label>
                            <asp:DropDownList ID="ddlDepartamentoAgregar" runat="server" CssClass="form-select" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlDepartamentoAgregar_SelectedIndexChanged">
                            </asp:DropDownList>
                        </div>
                    </div>

                    <div class="row">
                        <div class="mb-3 col-md-6">
                            <label>Superficie (m²)</label>
                            <asp:TextBox ID="txtSuperficieAgregar" runat="server" CssClass="form-control" TextMode="Number" Placeholder="Inserte la superficie" />
                        </div>
                        <div class="mb-3 col-md-4">
                            <label>Provincia</label>
                            <asp:DropDownList ID="ddlProvinciaAgregar" runat="server" CssClass="form-select" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlProvinciaAgregar_SelectedIndexChanged">
                            </asp:DropDownList>
                        </div>
                    </div>


                    <div class="row">
                        <div class="mb-3 col-md-6">
                            <label>Precio de reserva por hora</label>
                            <asp:TextBox ID="txtPrecioReservaAgregar" runat="server" CssClass="form-control" TextMode="Number" Placeholder="Inserte el precio de la reserva por hora" />
                        </div>
                        <div class="mb-3 col-md-4">
                            <label>Distrito</label>
                            <asp:DropDownList ID="ddlDistritoAgregar" runat="server" CssClass="form-select" OnSelectedIndexChanged="ddlDistritoAgregar_SelectedIndexChanged">
                            </asp:DropDownList>
                        </div>
                    </div>

                    <div>
                        <h6>Añadir horario de atención</h6>
                    </div>

                    <div class="row">
                        <div class="mb-3 col-md-2">
                            <label>Hora Inicio</label>
                            <asp:TextBox ID="txtHoraInicioInsert" runat="server" TextMode="Time" OnTextChanged="txtHoraFinInsert_TextChanged" AutoPostBack="true" Placeholder="00:00"></asp:TextBox>

                        </div>
                        <div class="mb-3 col-md-2">
                            <label>Hora Fin</label>
                            <asp:TextBox ID="txtHoraFinInsert" runat="server" TextMode="Time" OnTextChanged="txtHoraFinInsert_TextChanged" Placeholder="00:00" AutoPostBack="true"></asp:TextBox>
                        </div>

                        <asp:Label ID="lblError" runat="server" ForeColor="Red" />
                    </div>

                    <div>
                        <h6>Añadir días de atención</h6>
                    </div>

                    <!-- Caja de los Días de Atención -->
                    <div class="card p-3 mb-3">
                        <div class="row">
                            <div class="col-6 col-md-4">
                                <div class="form-check">
                                    <input class="form-check-input" type="checkbox" id="lunes">
                                    <label class="form-check-label" for="lunes">Lunes</label>
                                </div>
                            </div>
                            <div class="col-6 col-md-4">
                                <div class="form-check">
                                    <input class="form-check-input" type="checkbox" id="martes">
                                    <label class="form-check-label" for="martes">Martes</label>
                                </div>
                            </div>
                            <div class="col-6 col-md-4">
                                <div class="form-check">
                                    <input class="form-check-input" type="checkbox" id="miercoles">
                                    <label class="form-check-label" for="miercoles">Miércoles</label>
                                </div>
                            </div>
                            <div class="col-6 col-md-4">
                                <div class="form-check">
                                    <input class="form-check-input" type="checkbox" id="jueves">
                                    <label class="form-check-label" for="jueves">Jueves</label>
                                </div>
                            </div>
                            <div class="col-6 col-md-4">
                                <div class="form-check">
                                    <input class="form-check-input" type="checkbox" id="viernes">
                                    <label class="form-check-label" for="viernes">Viernes</label>
                                </div>
                            </div>
                            <div class="col-6 col-md-4">
                                <div class="form-check">
                                    <input class="form-check-input" type="checkbox" id="sabado">
                                    <label class="form-check-label" for="sabado">Sábado</label>
                                </div>
                            </div>
                            <div class="col-6 col-md-4">
                                <div class="form-check">
                                    <input class="form-check-input" type="checkbox" id="domingo">
                                    <label class="form-check-label" for="domingo">Domingo</label>
                                </div>
                            </div>
                        </div>
                    </div>
                    <!-- FIN DE Caja de los Días de Atención -->

                </div>

                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                    <asp:Button CssClass="active" ID="btnGuardarInsertado" runat="server" Text="Guardar" OnClick="btnGuardarInsertado_Click"/>
                </div>
            </div>
        </div>
    </div>

                
    <!------------------------------------- Modal de Edición de Espacios ---------------------------------------------------------------------------------->

    <!-- Datos del Espacio [ EDICION ESPACIO ] -->
    <div class="modal fade" id="modalEdicionEspacio" tabindex="-1" aria-labelledby="modalEdicionLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header" style="background-color: #f10909;">
                    <h2 class="modal-title" id="modalEdicionLabel" style="color: white">Editar - Espacio</h2>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Cerrar"></button>
                </div>

                <!--Cuerpo del modal [ EDICION ESPACIO ] -->
                <div class="modal-body">

                    <div class="row">
                        <div class="mb-3 col-md-6">
                            <label>Nombre del espacio</label>
                            <asp:TextBox ID="txtNombreEdit" runat="server" CssClass="form-control" Placeholder="Inserte nombre del espacio" />
                        </div>
                        <div class="mb-3 col-md-4">
                            <label>Tipo de espacio</label>
                            <asp:DropDownList ID="ddlTipoEspacioEdit" runat="server" CssClass="form-select">
                            </asp:DropDownList>
                        </div>
                    </div>

                    <div class="row">
                        <div class="mb-3 col-md-6">
                            <label>Ubicación</label>
                            <asp:TextBox ID="txtUbicacionEdit" runat="server" CssClass="form-control" Placeholder="Inserte ubicación" />
                        </div>
                        <div class="mb-3 col-md-4">
                            <label>Departamento</label>
                            <asp:DropDownList ID="ddlDepartamentoEdit" runat="server" CssClass="form-select" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlDepartamentoAgregar_SelectedIndexChanged">
                            </asp:DropDownList>
                        </div>
                    </div>

                    <div class="row">
                        <div class="mb-3 col-md-6">
                            <label>Superficie (m²)</label>
                            <asp:TextBox ID="txtSuperficieEdit" runat="server" CssClass="form-control" TextMode="Number" Placeholder="Inserte la superficie" />
                        </div>
                        <div class="mb-3 col-md-4">
                            <label>Provincia</label>
                            <asp:DropDownList ID="ddlProvinciaEdit" runat="server" CssClass="form-select" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlProvinciaAgregar_SelectedIndexChanged">
                            </asp:DropDownList>
                        </div>
                    </div>


                    <div class="row">
                        <div class="mb-3 col-md-6">
                            <label>Precio de reserva por hora</label>
                            <asp:TextBox ID="txtPrecioEdit" runat="server" CssClass="form-control" TextMode="Number" Placeholder="Inserte el precio de la reserva por hora" />
                        </div>
                        <div class="mb-3 col-md-4">
                            <label>Distrito</label>
                            <asp:DropDownList ID="ddlDistritoEdit" runat="server" CssClass="form-select">
                            </asp:DropDownList>
                        </div>
                    </div>

                    <div>
                        <h6>Añadir horario de atención</h6>
                    </div>

                    <div class="row">
                        <div class="mb-3 col-md-2">
                            <label>Hora Inicio</label>
                            <asp:TextBox ID="txtHoraInicioEdit" runat="server" TextMode="Time" Placeholder="00:00"></asp:TextBox>

                        </div>
                        <div class="mb-3 col-md-2">
                            <label>Hora Fin</label>
                            <asp:TextBox ID="txtHoraFinEdit" runat="server" TextMode="Time" Placeholder="00:00"></asp:TextBox>
                        </div>
                    </div>

                    <div>
                        <h6>Añadir días de atención</h6>
                    </div>

                    <!-- Caja de los Días de Atención -->
                    <div class="card p-3 mb-3">
                        <div class="row">
                            <div class="col-6 col-md-4">
                                <div class="form-check">
                                    <input class="form-check-input" type="checkbox" id="lunesEdit">
                                    <label class="form-check-label" for="lunes">Lunes</label>
                                </div>
                            </div>
                            <div class="col-6 col-md-4">
                                <div class="form-check">
                                    <input class="form-check-input" type="checkbox" id="martesEdit">
                                    <label class="form-check-label" for="martes">Martes</label>
                                </div>
                            </div>
                            <div class="col-6 col-md-4">
                                <div class="form-check">
                                    <input class="form-check-input" type="checkbox" id="miercolesEdit">
                                    <label class="form-check-label" for="miercoles">Miércoles</label>
                                </div>
                            </div>
                            <div class="col-6 col-md-4">
                                <div class="form-check">
                                    <input class="form-check-input" type="checkbox" id="juevesEdit">
                                    <label class="form-check-label" for="jueves">Jueves</label>
                                </div>
                            </div>
                            <div class="col-6 col-md-4">
                                <div class="form-check">
                                    <input class="form-check-input" type="checkbox" id="viernesEdit">
                                    <label class="form-check-label" for="viernes">Viernes</label>
                                </div>
                            </div>
                            <div class="col-6 col-md-4">
                                <div class="form-check">
                                    <input class="form-check-input" type="checkbox" id="sabadoEdit">
                                    <label class="form-check-label" for="sabado">Sábado</label>
                                </div>
                            </div>
                            <div class="col-6 col-md-4">
                                <div class="form-check">
                                    <input class="form-check-input" type="checkbox" id="domingoEdit">
                                    <label class="form-check-label" for="domingo">Domingo</label>
                                </div>
                            </div>
                        </div>
                    </div>
                    <!-- FIN DE Caja de los Días de Atención -->

                    <asp:HiddenField ID="hiddenIdEspacio" runat="server" />
                    <asp:HiddenField ID="hiddenIdDistrito" runat="server" />

                </div>

                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                    <asp:Button CssClass="alert-success" ID="btnActualizarEspacioEdit" runat="server" Text="Actualizar" OnClick="btnActualizarEspacioEdit_Click" CommandArgument='<%# Eval("idEspacio") %>'/>
                </div>
            </div>
        </div>
    </div>

    <!---------------------------- FIN DE EDICION MODAL  --------------------------------------------->

    <!-- Modal de Confirmación -->
    <div class="modal fade" id="mostrarConfEspacio" tabindex="-1" aria-labelledby="mostrarConfEspacioLabel" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">

                <div class="modal-header modal-header-rojo text-white">
                    <h5 class="modal-title" id="mostrarConfEspacioLabel">Ventana de confirmación</h5>
                    <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Cerrar"></button>
                </div>

                <div class="modal-body d-flex align-items-center modal-body-confirmacion">
                    
                    <div class="icono-confirmacion me-3">
                        <div class="icono-circulo">
                            <i class="bi bi-info-lg fs-1"></i>
                        </div>
                    </div>

                    <div id="modalConfirmacionCuerpo" class="fs-5">
                        ¿Está seguro que desea eliminar este registro?
                    </div>
                </div>

                <div class="modal-footer justify-content-center">
                    <button type="button" class="btn btn-dark px-4" data-bs-dismiss="modal"><em>No</em></button>
                    <asp:Button ID="btnConfirmarAccion" runat="server" CssClass="btn btn-dark px-4" Text="Sí" OnClick="btnConfirmarAccion_Click" />
                </div>

            </div>
        </div>
    </div>

    <script type="text/javascript">
        function abrirPaso1() {
            // Oculta el modal actual (paso 2)
            var modal2 = bootstrap.Modal.getInstance(document.getElementById('modalPaso2'));
            if (modal2) modal2.hide();

            // Muestra el modal del paso anterior (paso 1)
            var modal1 = new bootstrap.Modal(document.getElementById('modalPaso1'));
            modal1.show();
        }

    </script>

    <!-- Abrir modal de Edición -->
    <script type="text/javascript">
        function abrirModalEdicion() {
            var modalEdicion = bootstrap.Modal.getInstance(document.getElementById('modalEdicionEspacio'));
            modalEdicion.show();
        }
        function validarHoras() {
            const horaInicio = document.getElementById('<%= txtHoraInicioInsert.ClientID %>').value;
            const horaFin = document.getElementById('<%= txtHoraFinInsert.ClientID %>').value;

            if (!horaInicio || !horaFin) {
                alert("Ambas horas deben estar completas.");
                return false;
            }

            if (horaInicio >= horaFin) {
                alert("La hora de inicio debe ser menor que la hora de fin.");
                return false;
            }

            return true;
        }
        function mostrarConfEspacio(id) {
        // Obtener correctamente el ID generado por ASP.NET
        var hiddenField = document.getElementById('<%= hdnIdAEliminar.ClientID %>');
            if (hiddenField) {
                hiddenField.value = id;
            // Mostrar el modal aquí si lo necesitas
            console.log("ID asignado:", id);
            } else {
                    console.error("No se encontró el campo oculto.");
                return; // Importante: salir de la función si no se encuentra el campo oculto
            }

            // Obtener el elemento del modal
            var modalElement = document.getElementById('mostrarConfEspacio');
            if (!modalElement) {
                    console.error("No se encontró el elemento del modal.");
                return; // Importante: salir de la función si no se encuentra el modal
            }

            // Crear una instancia del modal de Bootstrap
            var modalEliminar = new bootstrap.Modal(modalElement);

            // Mostrar el modal
            modalEliminar.show();
        }
    </script>

</asp:Content>
