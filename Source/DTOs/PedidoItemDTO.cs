// Proyecto Final POO — DTO de entrada: PedidoItemDTO
// Reflejo plano y crudo de UNA fila del archivo de pedidos (CSV o JSON).
// Un mismo id_pedido puede repetirse en varias filas, una por cada ítem del pedido.
// Los campos numéricos usan tipo con ? porque se validan fuera del DTO (en el lector).
namespace Proyecto_Final_POO_C_.Source.DTOs
{
    public class PedidoItemDTO
    {
        public string?  IdPedido           { get; set; }
        public string?  EmailCliente       { get; set; }

        // Fecha como string — se parsea con DateTime.TryParseExact en la capa de mapeo.
        public string?  Fecha              { get; set; }

        // "nacional" o "internacional" — se valida en la capa de mapeo.
        public string?  TipoPedido         { get; set; }

        public string?  IdProducto         { get; set; }
        public string?  NombreProducto     { get; set; }
        public string?  CategoriaProducto  { get; set; }

        // Enteros y decimales con ? para detectar filas con valores faltantes o inválidos.
        public int?     Cantidad           { get; set; }
        public decimal? PrecioUnitario     { get; set; }
    }
}
