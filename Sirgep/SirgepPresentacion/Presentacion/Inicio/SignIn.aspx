<%@ Page Title="" Language="C#" MasterPageFile="~/MainLayout.Master" AutoEventWireup="true" CodeBehind="SignIn.aspx.cs" Inherits="SirgepPresentacion.Presentacion.Inicio.SignIn" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server" />
<asp:Content ID="Content2" ContentPlaceHolderID="Encabezado" runat="server" />
<asp:Content ID="Content3" ContentPlaceHolderID="Contenido" runat="server">
    <div class="row justify-content-center align-items-center" style="min-height: 80vh;">
        <div class="col-md-10 shadow-lg rounded p-5 bg-white">
            <div class="row">
                <!--izquierda-->
                <div class="col-md-6 d-flex flex-column justify-content-center align-items-center bg-dark text-white py-4 rounded-start">
                    <!-- Contenedor de imagen con tamaño fijo -->
                    <div class="w-100 d-flex justify-content-center mb-3" style="max-height: 450px; overflow: hidden;">
                        <img src="/Images/img/ImagenSI1.JPG" class="img-fluid rounded-3 shadow" style="max-height: 450px; object-fit: cover;" alt="Imagen 1" />
                    </div>
                    <div class="w-100 d-flex justify-content-center mb-3" style="max-height: 450px; overflow: hidden;">
                        <img src="/Images/img/ImagenSI2.JPG" class="img-fluid rounded-3 shadow" style="max-height: 450px; object-fit: cover;" alt="Imagen 2" />
                    </div>
                    <div class="w-100 d-flex justify-content-center" style="max-height: 450px; overflow: hidden;">
                        <img src="/Images/img/ImagenSI3.JPG" class="img-fluid rounded-3 shadow" style="max-height: 450px; object-fit: cover;" alt="Imagen 3" />
                    </div>
                </div>
                <!--derecha-->
                <div class="col-md-6 px-4">
                    <div class ="mt-4">
                        <div class="w-100 text-center">
                         <h3 class="fw-bold mb-3">Registrarse</h3>
                            <div class="mb-4">
                                ¿Tienes una cuenta?
                                <asp:LinkButton ID="lnkIniciarSesion" runat="server" CausesValidation="false" OnClick="lnkIniciarSesion_Click">
                                    Inicia sesión
                                </asp:LinkButton>
                            </div>
                        </div>

                    <div class="row g-3">

                        <!-- Grupo: Datos personales -->
                        <div class="p-3 border rounded bg-light mb-3">
                            <h5 class="fw-semibold">Datos personales</h5>
                            <p class="mb-2">Ingrese aquí su nombre, primer apellido y, si aplica, segundo apellido.</p>

                            <div class="mb-2">
                                <asp:TextBox ID="txtNombres" runat="server" CssClass="form-control" placeholder="Nombres"></asp:TextBox>
                                <asp:RequiredFieldValidator ControlToValidate="txtNombres" runat="server" ErrorMessage="*Campo obligatorio" CssClass="text-danger" Display="Dynamic" />
                            </div>
                            <div class="mb-2">
                                <asp:TextBox ID="txtApellidoPaterno" runat="server" CssClass="form-control" placeholder="Primer Apellido"></asp:TextBox>
                                <asp:RequiredFieldValidator ControlToValidate="txtApellidoPaterno" runat="server" ErrorMessage="*Campo obligatorio" CssClass="text-danger" Display="Dynamic" />
                            </div>
                            <div class="mb-2">
                                <asp:TextBox ID="txtApellidoMaterno" runat="server" CssClass="form-control" placeholder="Segundo Apellido (opcional)"></asp:TextBox>
                            </div>
                        </div>

                        <!-- Grupo: Documento -->
                        <div class="p-3 border rounded bg-light mb-3">
                            <h5 class="fw-semibold">Documento de Identidad</h5>
                            <p class="mb-2">Complete los datos de su documento de identificación.</p>

                            <div class="mb-2">
                                <asp:DropDownList ID="ddlTipoDocumento" runat="server" CssClass="form-select">
                                    <asp:ListItem Text="Seleccione tipo de documento" Value="" />
                                    <asp:ListItem Text="DNI" Value="DNI" />
                                    <asp:ListItem Text="Carnet de Extranjería" Value="CARNETEXTRANJERIA" />
                                    <asp:ListItem Text="Pasaporte" Value="PASAPORTE" />
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ControlToValidate="ddlTipoDocumento" InitialValue="" runat="server" ErrorMessage="*Campo obligatorio" CssClass="text-danger" Display="Dynamic" />
                            </div>
                            <div class="mb-2">
                                <asp:TextBox ID="txtNumeroDocumento" runat="server" CssClass="form-control" placeholder="Número de Documento"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidator1" runat="server" ControlToValidate="txtNumeroDocumento" CssClass="text-danger" Display="Dynamic" OnServerValidate="cvDocumento_ServerValidate" ValidateEmptyText="true" />
                                <asp:RequiredFieldValidator ControlToValidate="txtNumeroDocumento" runat="server" ErrorMessage="*Campo obligatorio" CssClass="text-danger" Display="Dynamic" />
                            </div>
                        </div>

                        <!-- Grupo: Ubicación -->
                        <div class="p-3 border rounded bg-light mb-3">
                            <h5 class="fw-semibold">Ubicación</h5>
                            <p class="mb-2">Elija aquí el distrito en el que vive o del que busca eventos.</p>
                            <div class="row">
                                <div class="col-md-4 mb-2">
                                    <asp:DropDownList ID="ddlDepartamento" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="ddlDepartamento_SelectedIndexChanged" />
                                </div>
                                <div class="col-md-4 mb-2">
                                    <asp:DropDownList ID="ddlProvincia" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="ddlProvincia_SelectedIndexChanged" />
                                </div>
                                <div class="col-md-4 mb-2">
                                    <asp:DropDownList ID="ddlDistrito" runat="server" CssClass="form-select" />
                                </div>
                            </div>
                        </div>

                        <!-- Grupo: Cuenta -->
                        <div class="p-3 border rounded bg-light mb-3">
                            <h5 class="fw-semibold">Cuenta</h5>
                            <p class="mb-2">Ingrese las credenciales y datos de su nueva cuenta.</p>

                            <div class="mb-2">
                                <asp:TextBox ID="txtUsuario" runat="server" CssClass="form-control" placeholder="Nombre de usuario"></asp:TextBox>
                                <asp:RequiredFieldValidator ControlToValidate="txtUsuario" runat="server" ErrorMessage="*Campo obligatorio" CssClass="text-danger" Display="Dynamic" />
                            </div>
                            <div class="mb-2">
                                <asp:TextBox ID="txtCorreo" runat="server" CssClass="form-control"
                                    placeholder="Correo electrónico" />

                                <!-- Validación de campo vacío -->
                                <asp:RequiredFieldValidator ControlToValidate="txtCorreo" runat="server"
                                    ErrorMessage="*Campo obligatorio" CssClass="text-danger" Display="Dynamic" />

                                <!-- Validación de formato -->
                                <asp:RegularExpressionValidator ControlToValidate="txtCorreo" runat="server"
                                    ErrorMessage="*Correo inválido" CssClass="text-danger" Display="Dynamic"
                                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />

                                <!-- Validación de existencia (custom) -->
                                <asp:CustomValidator ID="cvCorreo" runat="server"
                                    ControlToValidate="txtCorreo"
                                    OnServerValidate="cvCorreo_ServerValidate"
                                    ErrorMessage="Este correo ya está registrado"
                                    CssClass="text-danger" Display="Dynamic" ValidateEmptyText="true" />
                            </div>
                            <div class="mb-2">
                                <asp:TextBox ID="txtContraseña" runat="server" CssClass="form-control" TextMode="Password" placeholder="Contraseña"></asp:TextBox>
                                <asp:RequiredFieldValidator ControlToValidate="txtContraseña" runat="server" ErrorMessage="*Campo obligatorio" CssClass="text-danger" Display="Dynamic" />
                            </div>
                            <div class="mb-2">
                                <asp:TextBox ID="txtMonto" runat="server" CssClass="form-control" placeholder="Monto a agregar (mínimo S/ 50, múltiplos de 10)"></asp:TextBox>
                                <asp:CustomValidator ID="cvMonto" runat="server" ErrorMessage="*Monto inválido" CssClass="text-danger" Display="Dynamic" OnServerValidate="cvMonto_ServerValidate" />
                            </div>
                        </div>

                        <!-- Botón -->
                        <div class="col-12 text-center mt-3">
                            <asp:Button ID="btnCrearCuenta" runat="server" CssClass="btn btn-dark px-4" Text="Crear cuenta" OnClick="btnCrearCuenta_Click" />
                            <asp:Label ID="lblCorreoError" runat="server" CssClass="text-danger" />
                        </div>

                    </div>
                </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
