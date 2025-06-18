<%@ Page Title="" Language="C#" MasterPageFile="~/MainLayout.Master" AutoEventWireup="true" CodeBehind="CompraEntrada.aspx.cs" Inherits="SirgepPresentacion.Presentacion.Ventas.Entrada.CompraEntrada" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Encabezado" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Contenido" runat="server">
    <style>
        .btn-metodo {
            border: 2px solid #ccc;
            background: #fff;
            border-radius: 10px;
            padding: 10px 20px;
            cursor: pointer;
            transition: border-color 0.2s, box-shadow 0.2s;
            min-width: 100px;
            text-align: center;
        }

        .btn-metodo.selected, .btn-metodo:focus {
            border-color: #007bff;
            box-shadow: 0 0 0 2px #007bff33;
        }

        .btn-metodo img {
            display: block;
            margin: 0 auto 5px auto;
        }
    </style>
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
            <p><strong>Total: S/</strong>    <asp:Label ID="lblTotal"    runat="server" /></p>

            <!-- Aqui -->
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
            <div class="mb-2">
            <asp:TextBox 
                ID="txtCorreo" runat="server" CssClass="form-control" 
                placeholder="Correo electrónico" TextMode="Email" />
        </div>
            
        </div>

        <!-- Panel de pago y resumen -->
        <div class="col-md-6" style="position: relative;">
            <!--<h4>Elige tu método de pago</h4>
            <asp:RadioButtonList ID="rblPago" runat="server" RepeatDirection="Horizontal">
                <asp:ListItem Text="Yape" Value="YAPE"/>
                <asp:ListItem Text="Plin"    Value="PLIN" />
                <asp:ListItem Text="Tarjeta" Value="TARJETA" />
            </asp:RadioButtonList>-->
            <div id="timerContainer" style="position: absolute; top: 0; right: -9px; background: #fff; border-radius: 8px; padding: 8px 16px; box-shadow: 0 2px 8px rgba(0,0,0,0.1); font-size: 18px;">
                Tiempo restante: <span id="timer">01:00</span>
            </div>
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
            
            
            <asp:Button ID="btnPagar" runat="server" CssClass="btn btn-danger mt-3" Text="Pagar" OnClick="btnPagar_Click" />
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

    <script type="text/javascript">
        var totalSeconds = 60;
        var timerInterval = setInterval(function () {
            totalSeconds--;
            var minutes = Math.floor(totalSeconds / 60);
            var seconds = totalSeconds % 60;
            document.getElementById('timer').textContent =
                (minutes < 10 ? '0' : '') + minutes + ':' +
                (seconds < 10 ? '0' : '') + seconds;

            if (totalSeconds <= 0) {
                clearInterval(timerInterval);
                // Mostrar modal de error del MainLayout
                document.getElementById('modalErrorBody').innerText = "El tiempo para completar la reserva ha expirado.";
                var modal = new bootstrap.Modal(document.getElementById('modalError'));
                modal.show();
                // Redirigir después de 2 segundos
                setTimeout(function () {
                    window.location.href = '/Presentacion/Ubicacion/Distrito/EligeDistrito.aspx';
                }, 2000);
            }
        }, 1000);
    </script>
    <script type="text/javascript">
        // Detener el temporizador al hacer clic en "Pagar"
        document.addEventListener('DOMContentLoaded', function () {
            var btnPagar = document.getElementById('<%= btnPagar.ClientID %>');
            if (btnPagar) {
                btnPagar.addEventListener('click', function () {
                    if (typeof timerInterval !== 'undefined') {
                        clearInterval(timerInterval);
                    }
                });
            }
        });
</script>
</asp:Content>