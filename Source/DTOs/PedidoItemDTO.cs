using System.Text.Json.Serialization;

namespace Proyecto_Final_POO_C_.Source.DTOs
{
    public class PedidoItemDTO
    {
        [JsonPropertyName("id_pedido")]
        public string?  IdPedido           { get; set; }

        [JsonPropertyName("email_cliente")]
        public string?  EmailCliente       { get; set; }

        [JsonPropertyName("fecha")]
        public string?  Fecha              { get; set; }

        [JsonPropertyName("tipo_pedido")]
        public string?  TipoPedido         { get; set; }

        [JsonPropertyName("id_producto")]
        public string?  IdProducto         { get; set; }

        [JsonPropertyName("nombre_producto")]
        public string?  NombreProducto     { get; set; }

        [JsonPropertyName("categoria_producto")]
        public string?  CategoriaProducto  { get; set; }

        [JsonPropertyName("cantidad")]
        public int?     Cantidad           { get; set; }

        [JsonPropertyName("precio_unitario")]
        public decimal? PrecioUnitario     { get; set; }
    }
}
