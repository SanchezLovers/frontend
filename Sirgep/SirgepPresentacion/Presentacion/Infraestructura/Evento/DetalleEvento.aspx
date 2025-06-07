﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MainLayout.Master" AutoEventWireup="true" CodeBehind="DetalleEvento.aspx.cs" Inherits="SirgepPresentacion.Presentacion.Infraestructura.Evento.DetalleEvento" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Contenido" runat="server">
    <div class="container">
        <h1 class="mb-4">
            <asp:Label ID="lblTitulo" runat="server" Text=""></asp:Label><br />
        </h1>
        <p class="mb-3">
            <strong>Descripción</strong><br />
            <asp:Literal ID="litDescripcion" runat="server" />
        </p>
        <p class="mb-3" style="font-size: 1.25rem; font-weight: 500;">
            <asp:Label ID="lblUbicacion" runat="server" Text=""></asp:Label><br />
        </p>
        <p class="mt-2" style="font-size: 0.9rem; color: gray;">
            <asp:Label ID="lblReferencia" runat="server" Text=""></asp:Label><br />
        </p>
        <asp:HiddenField ID="cantGeneralInvisible" runat="server" />
        <asp:HiddenField ID="cantFuncionInvisible" runat="server" />
        <div class="row mb-3 align-items-end">
            <div class="col-md-6">
                <label for="ddlFunciones" class="form-label">Funciones</label>
                <asp:DropDownList ID="ddlFunciones" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="ddlFunciones_SelectedIndexChanged">
                    <asp:ListItem Text="Seleccione una función" Value="" />
                </asp:DropDownList>
            </div>
            <div class="col-md-3">
                <asp:Button ID="btnComprar" runat="server" Text="Comprar" CssClass="btn btn-primary w-100 mt-2" OnClick="btnComprar_Click" Enabled="false" />
            </div>
        </div>
    </div>
</asp:Content>
