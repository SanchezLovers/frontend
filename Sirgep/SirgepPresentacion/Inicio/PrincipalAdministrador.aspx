<%@ Page Title="Principal Administrador" Language="C#" MasterPageFile="~/MainLayout.Master" AutoEventWireup="true" CodeBehind="PrincipalAdministrador.aspx.cs" Inherits="SirgepPresentacion.PrincipalAdministrador" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" runat="server">
    <div class="container">
        <h2 class="mb-3 fw-bold">Bienvenido al Espacio Administrativo</h2>
        <p class="fw-semibold">Municipalidad XXXXXXXXX</p>

        <div class="row mt-4 align-items-start">
            <!-- Columna izquierda: bloques de gestión -->
            <div class="col-md-6">
                <h5 class="fw-semibold mb-3">Gestionar Espacios</h5>
                <div class="row mb-4">
                    <div class="col-6 mb-2">
                        <asp:Button ID="btnManejarEspacios" runat="server" CssClass="btn btn-dark w-100" Text="Manejar Espacios" OnClick="btnManejarEspacios_Click" />
                    </div>
                    <div class="col-6 mb-2">
                        <asp:Button ID="btnConsultarReservas" runat="server" CssClass="btn btn-dark w-100" Text="Consultar Reservas" OnClick="btnConsultarReservas_Click" />
                    </div>
                </div>

                <h5 class="fw-semibold mb-3">Gestionar Eventos</h5>
                <div class="row mb-4">
                    <div class="col-6 mb-2">
                        <asp:Button ID="btnManejarEventos" runat="server" CssClass="btn btn-dark w-100" Text="Manejar Eventos" OnClick="btnManejarEventos_Click" />
                    </div>
                    <div class="col-6 mb-2">
                        <asp:Button ID="btnConsultarEntradas" runat="server" CssClass="btn btn-dark w-100" Text="Consultar Entradas" OnClick="btnConsultarEntradas_Click" />
                    </div>
                </div>
            </div>
            <!-- Columna derecha: imagen -->
            <div class="col-md-6 d-flex justify-content-center align-items-start">
                <img src="/Images/principal/lima_evento.jpg" class="img-fluid rounded" alt="Evento Lima" style="max-width: 80%;" />
            </div>
        </div>
    </div>
</asp:Content>