﻿<%@ Page Title="Iniciar Sesion" Language="C#" MasterPageFile="~/MainLayout.Master" AutoEventWireup="true" CodeBehind="PaginaInicial.aspx.cs" Inherits="SirgepPresentacion.PaginaInicial" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Iniciar Sesión
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Encabezado" runat="server">
    <!--esto es el encabezado-->
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Contenido" runat="server">
    <div class ="row justify-content-center align-items-center" style="min-height: 80vh;">
        <div class="col-md-10 shadow-lg rounded p-5 bg-white">
            <div class="row">
                <!--izquierda-->
                <div class="col-md-6 text-center bg-dark text-white py-4 rounded-start">
                    <h2 class="mb-4">Gobierno Regional de Lima</h2>
                    <img src="/Images/grl/Escudo_Región_Lima_recortado.PNG" alt="Logo GRL" class="img-fluid" style="max-height: 250px;" />
                    <p class="mt-3">Verdad, Firmeza y Lealtad</p>
                </div>
                <!--derecha-->
                <divv class ="col-md-6 px-4">
                    <h4 class="text-center mb-4 fw-bold">Sistema Integral de Reservas y Gestión de Espacios Públicos</h4>

                    <asp:Label ID="lblError" runat="server" CssClass="text-danger fw-bold" />

                    <div class="mb-3">
                        <label for="txtEmail" class="form-label">Correo de su cuenta</label>
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="admin@sirgep.com.pe"></asp:TextBox>
                    </div>

                    <div class="mb-3 position-relative">
                        <label for="txtPassword" class="form-label">*Contraseña:</label>
                        <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Ingrese su Contraseña"></asp:TextBox>
                        <span class="position-absolute top-50 end-0 translate-middle-y me-3" style="cursor:pointer;" onclick="togglePassword()">👁</span>
                    </div>

                    <div class="mb-3 d-flex justify-content-between align-items-center">
                        <div>
                            <asp:CheckBox ID="chkRemember" runat="server" Text="Recordar" />
                        </div>
                        <a href="#" class="text-decoration-none">Olvidé mi contraseña</a>
                    </div>

                    <asp:Button ID="btnLogin" runat="server" Text="Ingresar" CssClass="btn btn-danger w-100 fw-bold" OnClick="btnLogin_Click" />

                    <small class="d-block mt-2 text-muted">*Campo Requerido</small>

                    <div class="text-center mt-3">
                        ¿No tienes una cuenta? <a href="#" class="text-decoration-none">Regístrese</a>
                    </div>
                </divv>
            </div>
        </div>
    </div>
</asp:Content>
