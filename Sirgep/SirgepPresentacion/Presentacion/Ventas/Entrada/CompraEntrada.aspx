<%@ Page Title="" Language="C#" MasterPageFile="~/MainLayout.Master" AutoEventWireup="true" CodeBehind="CompraEntrada.aspx.cs" Inherits="SirgepPresentacion.Presentacion.Ventas.Entrada.DetalleDeCompra" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Encabezado" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Contenido" runat="server">
    <div class="row">
        <!-- Panel de datos del evento -->
        <div class="col-md-6">
            <h3>Detalle de Compra</h3>
            <p><strong>Evento:</strong> <asp:Label ID="lblEvento" runat="server" /></p>
            <p><strong>Ubicación:</strong> <asp:Label ID="lblUbicacion" runat="server" /></p>
            <p><strong>Referencia:</strong> <asp:Label ID="lblReferencia" runat="server" /></p>
            <p><strong>Horario:</strong> <asp:Label ID="lblHorario" runat="server" /></p>
            <p><strong>Fecha:</strong> <asp:Label ID="lblFecha" runat="server" /></p>
            <p><strong>Cantidad:</strong> <asp:Label ID="lblCantidad" runat="server" /></p>

            <h4>Datos del comprador:</h4>
            <div class="mb-2">
                <asp:TextBox ID="txtNombres" runat="server" CssClass="form-control" placeholder="Nombres" />
            </div>
            <div class="mb-2">
                <asp:TextBox ID="txtApellidoPaterno" runat="server" CssClass="form-control" placeholder="Apellido paterno" />
            </div>
            <div class="mb-2">
                <asp:TextBox ID="txtApellidoMaterno" runat="server" CssClass="form-control" placeholder="Apellido materno" />
            </div>
            <div class="mb-2">
                <asp:TextBox ID="txtDNI" runat="server" CssClass="form-control" placeholder="DNI" />
            </div>
            
        </div>

        <!-- Panel de pago y resumen -->
        <div class="col-md-6">
            <!--<h4>Elige tu método de pago</h4>
            <asp:RadioButtonList ID="rblPago" runat="server" RepeatDirection="Horizontal">
                <asp:ListItem Text="Yape" Value="YAPE"/>
                <asp:ListItem Text="Plin"    Value="PLIN" />
                <asp:ListItem Text="Tarjeta" Value="TARJETA" />
            </asp:RadioButtonList>-->
            
            <h4>Elige tu método de pago</h4>
            <div class="d-flex gap-3 mb-3" id="metodosPago">
                <button type="button" class="btn-metodo" data-value="YAPE" onclick="seleccionarMetodo(this)">
                    <img src="<%= ResolveUrl("~/Images/metodosDePago/Yape.png") %>" alt="Yape" style="height:48px;"><br />
                    Yape
                </button>
                <button type="button" class="btn-metodo" data-value="PLIN" onclick="seleccionarMetodo(this)">
                    <img src="<%= ResolveUrl("~/Images/metodosDePago/Plin.png") %>" alt="Plin" style="height:48px;"><br />
                    Plin
                </button>
                <button type="button" class="btn-metodo" data-value="TARJETA" onclick="seleccionarMetodo(this)">
                    <img src="<%= ResolveUrl("~/Images/metodosDePago/Tarjeta.png") %>" alt="Tarjeta" style="height:48px;"><br />
                    Tarjeta
                </button>
                <asp:HiddenField ID="hfMetodoPago" runat="server" />
            </div>
            <asp:Button ID="btnPagar" runat="server" CssClass="btn btn-danger mt-3" Text="Pagar" CommandArgument='<%# Eval("idConstancia") %>' OnClick="btnPagar_Click" />
        </div>
    </div>
    <script type="text/javascript">
    function seleccionarMetodo(btn) {
        document.querySelectorAll('.btn-metodo').forEach(function (b) {
            b.classList.remove('selected');
        });
        btn.classList.add('selected');
        document.getElementById('<%= hfMetodoPago.ClientID %>').value = btn.getAttribute('data-value');
    }

    // Restaurar selección después del postback
    window.onload = function () {
        var seleccionado = document.getElementById('<%= hfMetodoPago.ClientID %>').value;
        if (seleccionado) {
            document.querySelectorAll('.btn-metodo').forEach(function (btn) {
                if (btn.getAttribute('data-value') === seleccionado) {
                    btn.classList.add('selected');
                }
            });
        }
    }
</script>



</asp:Content>
