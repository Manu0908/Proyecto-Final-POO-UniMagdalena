// Proyecto Final POO — DTO de reporte: ReporteProductoDTO
// Agrupa los datos ya validados de un producto para su exportación final.
// En este punto los datos ya fueron procesados, por eso se eliminan los ?.
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

        // Total acumulado de unidades vendidas de este producto en todos los pedidos válidos.
        [JsonPropertyName("numero_ventas")]
        public int     NumeroVentas   { get; set; }
    }
}
