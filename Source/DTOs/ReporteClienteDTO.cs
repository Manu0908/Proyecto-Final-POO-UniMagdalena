// Proyecto Final POO — DTO de reporte: ReporteClienteDTO
// Agrupa los datos ya validados y calculados de un cliente para su exportación final.
// En este punto los datos ya fueron procesados, por eso se eliminan los ? en los campos clave.
namespace Proyecto_Final_POO_C_.Source.DTOs
{
    public class ReporteClienteDTO
    {
        public string IdCliente             { get; set; } = string.Empty;
        public string Nombre                { get; set; } = string.Empty;
        public string Email                 { get; set; } = string.Empty;
        public string Ciudad                { get; set; } = string.Empty;

        // "natural" o "empresarial".
        public string TipoCliente           { get; set; } = string.Empty;

        public bool    EsFrecuente          { get; set; }
        public decimal TotalAcumuladoCompras { get; set; }

        // Composición: el pedido más costoso del cliente con todos sus detalles.
        // Puede ser null si el cliente no tiene pedidos válidos asociados.
        public PedidoReporteDTO? PedidoMasCostoso { get; set; }
    }
}
