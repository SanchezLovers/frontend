<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DetalleEntrada.aspx.cs" Inherits="SirgepPresentacion.DetalleEntrada" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Entradas #<%= Request.QueryString["numero"] ?? "000" %></title>
    <style>
        body {
            margin: 0;
            font-family: Arial, sans-serif;
            background-color: #f5f5f5;
            line-height: 1.5;
            color: #333;
        }

        .barra-superior {
            background-color: #d90000;
            height: 40px;
            width: 100%;
        }

        .contenido {
            max-width: 900px;
            width: 90%;
            margin: 40px auto;
            background: white;
            padding: 20px 25px;
            box-shadow: 0 2px 8px rgba(0,0,0,0.1);
            border-radius: 6px;
        }

        .titulo {
            font-size: 2rem;
            font-weight: bold;
            margin-bottom: 25px;
            color: #d90000;
            text-align: center;
        }

        .bloque {
            margin-bottom: 30px;
        }

        .bloque .etiqueta {
            font-weight: bold;
            color: #555;
        }

        .botones {
            margin-top: 20px;
            display: flex;
            gap: 12px;
            justify-content: center;
            flex-wrap: wrap;
        }

        .boton {
            padding: 10px 20px;
            background-color: #1e1e1e;
            color: white !important;
            border: none;
            border-radius: 4px;
            cursor: pointer;
            font-size: 1rem;
            transition: background-color 0.3s ease;
            min-width: 120px;
            text-align: center;
            display: inline-block;
        }

        .boton:hover {
            background-color: #444;
        }

        @media (max-width: 600px) {
            .contenido {
                width: 95%;
                margin: 20px auto;
                padding: 15px 20px;
            }

            .titulo {
                font-size: 1.5rem;
                margin-bottom: 20px;
            }

            .botones {
                flex-direction: column;
                gap: 10px;
            }

            .boton {
                width: 100%;
                min-width: auto;
                font-size: 1.1rem;
            }

            .bloque div {
                font-size: 0.95rem;
                margin-bottom: 6px;
            }
        }
    </style>
</head>
<body>
    <div class="barra-superior"></div>
    <form id="form1" runat="server">
        <div class="contenido">
            <div class="titulo">Entradas #<asp:Label ID="lblNumEntrada" runat="server" Text="000" /></div>

            <div class="bloque">
                <div><span class="etiqueta">Evento:</span> <asp:Label ID="lblEvento" runat="server" /></div>
                <div><span class="etiqueta">Ubicación:</span> <asp:Label ID="lblUbicacion" runat="server" /></div>
                <div><span class="etiqueta">Horario:</span> <asp:Label ID="lblHorario" runat="server" /></div>
                <div><span class="etiqueta">Fecha:</span> <asp:Label ID="lblFecha" runat="server" /></div>
            </div>

            <div class="bloque">
                <div><strong>Datos del comprador:</strong></div>
                <div><span class="etiqueta">Nombres:</span> <asp:Label ID="lblNombres" runat="server" /></div>
                <div><span class="etiqueta">Apellidos:</span> <asp:Label ID="lblApellidos" runat="server" /></div>
                <div><span class="etiqueta">DNI:</span> <asp:Label ID="lblDNI" runat="server" /></div>
                <div><span class="etiqueta">Teléfono:</span> <asp:Label ID="lblTelefono" runat="server" /></div>
                <div><span class="etiqueta">Correo:</span> <asp:Label ID="lblCorreo" runat="server" /></div>
            </div>

            <div class="bloque">
                <div><strong>Método de Pago:</strong></div>
                <div><asp:Label ID="lblMetodoPago" runat="server" /></div>
                <div><span class="etiqueta">Número de pago / tarjeta:</span> <asp:Label ID="lblNumeroPago" runat="server" /></div>
            </div>

            <div class="botones">
                <asp:Button ID="btnVolver" runat="server" Text="Volver" CssClass="boton" OnClick="btnVolver_Click" />
                <asp:Button ID="btnDescargar" runat="server" Text="Descargar" CssClass="boton" />
            </div>
        </div>
    </form>
</body>
</html>
