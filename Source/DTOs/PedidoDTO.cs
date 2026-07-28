// Proyecto Final POO — DTO de reporte: PedidoReporteDTO
// Representa un pedido ya procesado y validado, con sus ítems agrupados y totales calculados.
// Se usa como campo de composición en ReporteClienteDTO (pedido más costoso del cliente).
namespace Proyecto_Final_POO_C_.Source.DTOs
{
    public class PedidoReporteDTO
    {
        public string IdPedido          { get; set; } = string.Empty;
        public string Fecha             { get; set; } = string.Empty;

        // "nacional" o "internacional".
        public string TipoPedido        { get; set; } = string.Empty;

        // Ítems individuales que componen este pedido (ya agrupados por IdPedido).
        public List<PedidoItemDTO> Items { get; set; } = new List<PedidoItemDTO>();

        // Totales calculados a partir de los ítems y el tipo de pedido.
        public decimal SubTotal         { get; set; }
        public decimal ImpuestoAplicado { get; set; }
        public decimal TotalFinal       { get; set; }
    }
}