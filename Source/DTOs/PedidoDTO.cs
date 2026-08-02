// Proyecto Final POO — DTO de reporte: PedidoReporteDTO
// Representa un pedido ya procesado y validado, con sus ítems agrupados y totales calculados.
// Se usa como campo de composición en ReporteClienteDTO (pedido más costoso del cliente).
using System.Text.Json.Serialization;

namespace Proyecto_Final_POO_C_.Source.DTOs
{
    public class PedidoReporteDTO
    {
        [JsonPropertyName("id_pedido")]
        public string IdPedido          { get; set; } = string.Empty;

        [JsonPropertyName("fecha")]
        public string Fecha             { get; set; } = string.Empty;

        // "nacional" o "internacional".
        [JsonPropertyName("tipo_pedido")]
        public string TipoPedido        { get; set; } = string.Empty;

        // Ítems individuales que componen este pedido (ya agrupados por IdPedido).
        [JsonPropertyName("items")]
        public List<PedidoItemDTO> Items { get; set; } = new List<PedidoItemDTO>();

        // Totales calculados a partir de los ítems y el tipo de pedido.
        [JsonPropertyName("subtotal")]
        public decimal SubTotal         { get; set; }

        [JsonPropertyName("impuesto_aplicado")]
        public decimal ImpuestoAplicado { get; set; }

        [JsonPropertyName("total_final")]
        public decimal TotalFinal       { get; set; }
    }
}