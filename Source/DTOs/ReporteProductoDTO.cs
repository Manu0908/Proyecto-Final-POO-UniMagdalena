// Proyecto Final POO — DTO de reporte: ReporteProductoDTO
// Agrupa los datos ya validados de un producto para su exportación final.
// En este punto los datos ya fueron procesados, por eso se eliminan los ?.
namespace Proyecto_Final_POO_C_.Source.DTOs
{
    public class ReporteProductoDTO
    {
        public string  IDProducto     { get; set; } = string.Empty;
        public string  NombreProducto { get; set; } = string.Empty;
        public string  Categoria      { get; set; } = string.Empty;
        public decimal PrecioUnitario { get; set; }

        // Total acumulado de unidades vendidas de este producto en todos los pedidos válidos.
        public int     NumeroVentas   { get; set; }
    }
}
