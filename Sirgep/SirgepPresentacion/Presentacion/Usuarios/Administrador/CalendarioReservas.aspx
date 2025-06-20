<%@ Page Title="" Language="C#" MasterPageFile="~/MainLayout.Master" AutoEventWireup="true" CodeBehind="CalendarioReservas.aspx.cs" Inherits="SirgepPresentacion.Presentacion.Usuarios.Administrador.CalendarioReservas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Calendario de Reservas
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Encabezado" runat="server">
    <link href="https://cdn.jsdelivr.net/npm/fullcalendar@6.1.8/index.global.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/fullcalendar@6.1.8/index.global.min.js"></script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Contenido" runat="server">
    <div class="container mt-4">
        <h3 class="mb-4">Calendario de Reservas</h3>
            <div class="export-buttons">
                <asp:Button ID="btnExportarPDF" CssClass="btn btn-dark" runat="server" Text="Exportar PDF" OnClick="btnExportarPDF_Click" Enabled="false" />
                <asp:Button ID="btnExportarExcel" CssClass="btn btn-dark" runat="server" Text="Exportar Excel" OnClick="btnExportarExcel_Click" Enabled="false" />
            </div>
        <div id="calendar-block">
            <div id="calendar"></div>
        </div>
    </div>
   <!-- Modal -->
<div class="modal fade" id="detalleReservaModal" tabindex="-1" role="dialog" aria-labelledby="detalleReservaLabel" aria-hidden="true">
  <div class="modal-dialog" role="document">
    <div class="modal-content">
      <div class="modal-header modal-header-rojo text-white">
        <h5 class="modal-title" id="detalleReservaLabel">Detalle de Reserva</h5>
        <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Cerrar"></button>
      </div>
      <div class="modal-body">
        <p><strong>N° Reserva:</strong> <span id="reservaNum"></span></p>
        <p><strong>Espacio:</strong> <span id="reservaEspacio"></span></p>
        <p><strong>Fecha:</strong> <span id="reservaFecha"></span></p>
        <p><strong>Inicio:</strong> <span id="reservaInicio"></span></p>
        <p><strong>Fin:</strong> <span id="reservaFin"></span></p>
        <p><strong>Nombres:</strong> <span id="reservaNombres"></span></p>
        <p><strong>Apellidos:</strong> <span id="reservaApellidos"></span></p>
        <p><strong>Tipo de Documento:</strong> <span id="reservaTipoDoc"></span></p>
        <p><strong>N° Documento:</strong> <span id="reservaNumDoc"></span></p>
      </div>
      <div class="modal-footer">
        <button type="button" class="btn btn-dark" data-bs-dismiss="modal">Cerrar</button>
      </div>
    </div>
  </div>
</div>


    <script>
        var calendar;

        document.addEventListener('DOMContentLoaded', function () {
            var calendarEl = document.getElementById('calendar');
            calendar = new FullCalendar.Calendar(calendarEl, {
                initialView: 'timeGridWeek',
                locale: 'es',
                headerToolbar: {
                    left: 'prev,next today',
                    center: 'title',
                    right: 'dayGridMonth,timeGridWeek,timeGridDay,listWeek'
                },
                events: '/Presentacion/Usuarios/Administrador/CalendarioReservas.aspx?action=getReservas',
                eventClick: function (info) {
                    const props = info.event.extendedProps;
                    document.getElementById('reservaNum').textContent = props.numReserva;
                    document.getElementById('reservaEspacio').textContent = props.espacio;
                    document.getElementById('reservaFecha').textContent = props.fecha;
                    document.getElementById('reservaInicio').textContent = props.horaInicio;
                    document.getElementById('reservaFin').textContent = props.horaFin;
                    document.getElementById('reservaNombres').textContent = props.nombres;
                    document.getElementById('reservaApellidos').textContent = props.apellidos;
                    document.getElementById('reservaTipoDoc').textContent = props.tipoDocumento;
                    document.getElementById('reservaNumDoc').textContent = props.numDocumento;

                    const modal = new bootstrap.Modal(document.getElementById('detalleReservaModal'));
                    modal.show();
                }
            });

            calendar.render();
        });

        

    </script>
</asp:Content>