using System.Text.Json.Serialization;

namespace Proyecto_Final_POO_C_.Source.DTOs
{
    public class ReporteProductoDTO
    {
        [JsonPropertyName("id_producto")]
        public string  IDProducto     { get; set; } = string.Empty;

        [JsonPropertyName("nombre_producto")]
        public string  NombreProducto { get; set; } = string.Empty;

        [JsonPropertyName("categoria")]
        public string  Categoria      { get; set; } = string.Empty;

        [JsonPropertyName("precio_unitario")]
        public decimal PrecioUnitario { get; set; }

        [JsonPropertyName("numero_ventas")]
        public int     NumeroVentas   { get; set; }
    }
}
