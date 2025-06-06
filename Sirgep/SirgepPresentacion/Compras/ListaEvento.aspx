<%@ Page Title="" Language="C#" MasterPageFile="~/MainLayout.Master" AutoEventWireup="true" CodeBehind="ListaEvento.aspx.cs" Inherits="SirgepPresentacion.Compras.ListaEvento" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Lista de Eventos
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Contenido" runat="server">
    <div class="mb-4 d-flex align-items-center">
    <asp:Button ID="btnRegresar" runat="server" Text="&lt; Regresar" CssClass="btn btn-outline-secondary me-3" OnClick="btnRegresar_Click" />
    <span class="fw-bold fs-4 me-3">Eventos del distrito:</span>
    <asp:Label ID="lblDistrito" runat="server" CssClass="fw-bold fs-4" />
</div>
    <hr />
    <!-- Repeater -->
    <asp:Repeater ID="rptEventos" runat="server">
        <HeaderTemplate>
            <div class="row">
        </HeaderTemplate>
        <ItemTemplate>
            <div class="col-md-6 mb-2">
                <div class="card h-100 shadow-sm">
                    <div class="row g-0">
                        <div class="col-4 d-flex align-items-center">

                        </div>
                        <div class="col-8">
                            <div class="card-body">
                                <h5 class="card-title fw-bold mb-1"><%# Eval("Nombre") %></h5>
                                <div class="mb-1"><%# Eval("ubicacion") %> - <%# Eval("referencia") %></div>
                                <div class="text-muted small mb-2"><%# Eval("descripcion") %></div>
                                <asp:Button ID="btnComprar" runat="server" Text="Comprar" CssClass="btn btn-dark btn-sm float-end me-1 mb-3" CommandArgument='<%# Eval("idEvento") %>' OnClick="btnComprar_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ItemTemplate>
        <FooterTemplate>
            </div>

        </FooterTemplate>
    </asp:Repeater>
    <!-- Cambio de paginado -->
    <!-- <asp:Panel ID="pnlPaginacion" runat="server" CssClass="d-flex justify-content-center my-3">
        <asp:Button ID="btnAnterior" runat="server" Text="Anterior" CssClass="btn btn-outline-dark mx-1" OnClick="btnAnterior_Click" />
        <asp:Label ID="lblPagina" runat="server" CssClass="align-self-center mx-2" />
        <asp:Button ID="btnSiguiente" runat="server" Text="Siguiente" CssClass="btn btn-outline-dark mx-1" OnClick="btnSiguiente_Click" />
    </asp:Panel>-->
</asp:Content>
