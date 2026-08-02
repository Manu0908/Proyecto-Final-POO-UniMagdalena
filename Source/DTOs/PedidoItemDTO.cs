// Proyecto Final POO — DTO de entrada: PedidoItemDTO
// Reflejo plano y crudo de UNA fila del archivo de pedidos (CSV o JSON).
// Un mismo id_pedido puede repetirse en varias filas, una por cada ítem del pedido.
// Los campos numéricos usan tipo con ? porque se validan fuera del DTO (en el lector).
using System.Text.Json.Serialization;

namespace Proyecto_Final_POO_C_.Source.DTOs
{
    public class PedidoItemDTO
    {
        [JsonPropertyName("id_pedido")]
        public string?  IdPedido           { get; set; }

        [JsonPropertyName("email_cliente")]
        public string?  EmailCliente       { get; set; }

        // Fecha como string — se parsea con DateTime.TryParseExact en la capa de mapeo.
        [JsonPropertyName("fecha")]
        public string?  Fecha              { get; set; }

        // "nacional" o "internacional" — se valida en la capa de mapeo.
        [JsonPropertyName("tipo_pedido")]
        public string?  TipoPedido         { get; set; }

        [JsonPropertyName("id_producto")]
        public string?  IdProducto         { get; set; }

        [JsonPropertyName("nombre_producto")]
        public string?  NombreProducto     { get; set; }

        [JsonPropertyName("categoria_producto")]
        public string?  CategoriaProducto  { get; set; }

        // Enteros y decimales con ? para detectar filas con valores faltantes o inválidos.
        [JsonPropertyName("cantidad")]
        public int?     Cantidad           { get; set; }

        [JsonPropertyName("precio_unitario")]
        public decimal? PrecioUnitario     { get; set; }
    }
}
