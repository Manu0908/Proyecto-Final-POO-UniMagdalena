using System.Text.Json.Serialization;

namespace Proyecto_Final_POO_C_.Source.DTOs
{
    public class ReporteClienteDTO
    {
        [JsonPropertyName("id_cliente")]
        public string IdCliente             { get; set; } = string.Empty;

        [JsonPropertyName("nombre")]
        public string Nombre                { get; set; } = string.Empty;

        [JsonPropertyName("email")]
        public string Email                 { get; set; } = string.Empty;

        [JsonPropertyName("ciudad")]
        public string Ciudad                { get; set; } = string.Empty;

        [JsonPropertyName("tipo_cliente")]
        public string TipoCliente           { get; set; } = string.Empty;

        [JsonPropertyName("es_frecuente")]
        public bool    EsFrecuente          { get; set; }

        [JsonPropertyName("total_acumulado_compras")]
        public decimal TotalAcumuladoCompras { get; set; }

        [JsonPropertyName("pedido_mas_costoso")]
        public PedidoReporteDTO? PedidoMasCostoso { get; set; }
    }
}
