using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Proyecto_Final_POO_C_.Source.DTOs
{
    public class PedidoReporteDTO
    {
        [JsonPropertyName("id_pedido")]
        public string IdPedido          { get; set; } = string.Empty;

        [JsonPropertyName("fecha")]
        public string Fecha             { get; set; } = string.Empty;

        [JsonPropertyName("tipo_pedido")]
        public string TipoPedido        { get; set; } = string.Empty;

        [JsonPropertyName("items")]
        public List<PedidoItemDTO> Items { get; set; } = new List<PedidoItemDTO>();

        [JsonPropertyName("subtotal")]
        public decimal SubTotal         { get; set; }

        [JsonPropertyName("impuesto_aplicado")]
        public decimal ImpuestoAplicado { get; set; }

        [JsonPropertyName("total_final")]
        public decimal TotalFinal       { get; set; }
    }
}