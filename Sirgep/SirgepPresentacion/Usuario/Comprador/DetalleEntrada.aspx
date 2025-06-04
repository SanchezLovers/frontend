<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DetalleEntrada.aspx.cs" Inherits="SirgepPresentacion.Usuario.Comprador.DetalleEntrada" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Entradas</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            margin: 0;
            background-color: #f2f2f2;
        }
        .barra-superior {
            background-color: #f10909; /* rojo nuevo */
            height: 60px;
        }
        .contenido {
            padding: 20px;
        }
        .titulo {
            font-size: 28px;
            font-weight: bold;
            margin-bottom: 20px;
        }
        .seccion {
            background-color: #fff;
            border-radius: 8px;
            padding: 20px;
            margin-bottom: 20px;
            box-shadow: 0 2px 5px rgba(0,0,0,0.1);
        }
        .seccion h2 {
            margin-top: 0;
        }
        .info {
            display: flex;
            flex-wrap: wrap;
            gap: 10px;
        }
        .campo {
            flex: 1 1 200px;
        }
        .campo strong {
            display: block;
            margin-bottom: 5px;
        }
        .botones {
            text-align: center;
        }
        .boton {
            padding: 10px 20px;
            font-size: 16px;
            margin: 5px;
            border: none;
            border-radius: 5px;
            cursor: pointer;
            background-color: #f10909; /* rojo nuevo */
            color: #fff;
        }
        .boton:hover {
            background-color: #c10707; /* un poco más oscuro */
        }
        .modal {
            display: none;
            position: fixed;
            z-index: 999;
            left: 0;
            top: 0;
            width: 100%;
            height: 100%;
            overflow: auto;
            background-color: rgba(0,0,0,0.5);
        }
        .modal-content {
            background-color: #fff;
            margin: 10% auto;
            padding: 20px;
            border-radius: 8px;
            width: 90%;
            max-width: 400px;
            box-shadow: 0 2px 10px rgba(0,0,0,0.3);
            text-align: center;
        }
        .close {
            color: #aaa;
            float: right;
            font-size: 22px;
            font-weight: bold;
            cursor: pointer;
        }
        .close:hover {
            color: black;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="barra-superior"></div>

        <div class="contenido">
            <div class="titulo">Entradas #<asp:Label ID="lblNumEntrada" runat="server" Text="000" /></div>

            <div class="seccion">
                <h2>Datos del evento</h2>
                <div class="info">
                    <div class="campo"><strong>Nombre:</strong> <asp:Label ID="lblEvento" runat="server" /></div>
                    <div class="campo"><strong>Ubicación:</strong> <asp:Label ID="lblUbicacion" runat="server" /></div>
                    <div class="campo"><strong>Horario:</strong> <asp:Label ID="lblHorario" runat="server" /></div>
                    <div class="campo"><strong>Fecha:</strong> <asp:Label ID="lblFecha" runat="server" /></div>
                </div>
            </div>

            <div class="seccion">
                <h2>Datos del comprador</h2>
                <div class="info">
                    <div class="campo"><strong>Nombres:</strong> <asp:Label ID="lblNombres" runat="server" /></div>
                    <div class="campo"><strong>Apellidos:</strong> <asp:Label ID="lblApellidos" runat="server" /></div>
                    <div class="campo"><strong>DNI:</strong> <asp:Label ID="lblDNI" runat="server" /></div>
                    <div class="campo"><strong>Teléfono:</strong> <asp:Label ID="lblTelefono" runat="server" /></div>
                    <div class="campo"><strong>Correo:</strong> <asp:Label ID="lblCorreo" runat="server" /></div>
                </div>
            </div>

            <div class="seccion">
                <h2>Datos del pago</h2>
                <div class="info">
                    <div class="campo"><strong>Método de pago:</strong> <asp:Label ID="lblMetodoPago" runat="server" /></div>
                    <div class="campo"><strong>Número de pago:</strong> <asp:Label ID="lblNumeroPago" runat="server" /></div>
                </div>
            </div>

            <div class="botones">
                <asp:Button ID="btnVolver" runat="server" Text="Volver" CssClass="boton" OnClick="btnVolver_Click" />
                <asp:Button ID="btnDescargar" runat="server" Text="Descargar" CssClass="boton" OnClick="btnDescargar_Click" UseSubmitBehavior="false" />
            </div>
        </div>

        <div id="miModal" class="modal">
            <div class="modal-content">
                <span class="close" onclick="cerrarModal()">&times;</span>
                <h2>Descarga Completada Correctamente</h2>
                <button class="boton" type="button" onclick="cerrarModal()">Cerrar</button>
            </div>
        </div>

        <script>
            function mostrarModal() {
                document.getElementById("miModal").style.display = "block";
            }

            function cerrarModal() {
                document.getElementById("miModal").style.display = "none";
            }

            window.onclick = function (event) {
                var modal = document.getElementById("miModal");
                if (event.target == modal) {
                    modal.style.display = "none";
                }
            }

            // Cambia el <title> usando el contenido del Label cuando la página termine de cargar
            window.onload = function () {
                var lbl = document.getElementById("<%= lblNumEntrada.ClientID %>");
                if (lbl) {
                    document.title = "Entradas #" + lbl.innerText;
                }
            };
        </script>
    </form>
</body>
</html>