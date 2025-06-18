<%@ Page Title="" Language="C#" MasterPageFile="~/MainLayout.Master" AutoEventWireup="true" CodeBehind="PerfilComprador.aspx.cs" Inherits="SirgepPresentacion.Presentacion.Usuarios.Comprador.PerfilComprador" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Encabezado" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Contenido" runat="server">
    <div class="container">
        <h1 class="mb-4">
            Informacion Registrada
        </h1>
        <!-- DATOS DEL PERFIL-->
        <div class="card mb-3">
            <div class="card-header modal-header-rojo text-white">Datos del Perfil</div>
            <div class="card-body row g-3">
                <div class="col-12 col-md-4">
                    <strong>Nombres:</strong> <asp:Label ID="lblNombres" runat="server" CssClass="ms-2" />
                </div>
                <div class="col-12 col-md-4">
                    <strong>Primer Apellido:</strong> <asp:Label ID="lblPrimerApellido" runat="server" CssClass="ms-2" />
                </div>
                <div class="col-12 col-md-4">
                    <strong>Segundo Apellido:</strong> <asp:Label ID="lblSegundoApellido" runat="server" CssClass="ms-2" />
                </div>
                <div class="col-12 col-md-4">
                    <strong>Tipo Documento:</strong> <asp:Label ID="lblTipoDocumento" runat="server" CssClass="ms-2" />
                </div>
                <div class="col-12 col-md-4">
                    <strong>Numero Documento:</strong> <asp:Label ID="lblNumeroDocumento" runat="server" CssClass="ms-2" />
                </div>
                <div class="col-12 col-md-4">
                    <strong>Monto Billetera:</strong> <asp:Label ID="lblMontoBilletera" runat="server" CssClass="ms-2" />
                </div>
            </div>
        </div>
        <!-- DATOS DE UBICACION-->
        <div class="card mb-3">
            <div class="card-header modal-header-rojo text-white">Datos de Ubicacion</div>
            <div class="card-body row g-3">
                <div class="col-12 col-md-4">
                    <strong>Departamento:</strong> <asp:Label ID="lblDepartamento" runat="server" CssClass="ms-2" />
                </div>
                <div class="col-12 col-md-4">
                    <strong>Provincia:</strong> <asp:Label ID="lblProvincia" runat="server" CssClass="ms-2" />
                </div>
    <div class="col-12 col-md-4">
        <strong>Distrito:</strong><br />
        <asp:TextBox ID="txtDistrito" runat="server"
            CssClass="form-control form-control-sm text-dark mb-2" />

        <asp:Button ID="btnGuardarDistrito" runat="server"
            Text="Guardar"
            CssClass="btn btn-dark btn-sm"
            OnClick="btnGuardarDistrito_Click" />
    </div>
            </div>
        </div>
        <!-- DATOS DE CUENTA-->
        <div class="card mb-3">
            <div class="card-header modal-header-rojo text-white">Datos de Cuenta</div>
            <div class="card-body row g-3">
                <div class="col-12 col-md-4">
                    <strong>Correo:</strong> <asp:Label ID="lblCorreo" runat="server" CssClass="ms-2" />
                </div>
                <div class="col-12 col-md-4">
                    <strong>Contraseña:</strong> <asp:Label ID="lblContrasenia" runat="server" CssClass="ms-2" />
                </div>
            </div>
        </div>
    </div>

</asp:Content>
