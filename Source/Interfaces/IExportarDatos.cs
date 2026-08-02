// Proyecto Final POO — Interfaz: IExportarDatos
// Contrato del patrón Strategy para la escritura de los reportes finales.
// Cada implementación concreta (EscritorJSON, EscritorXML) serializa los mismos datos
// en un formato distinto sin que el pipeline lo sepa.
using Proyecto_Final_POO_C_.Source.DTOs;

namespace Proyecto_Final_POO_C_.Source.Interfaces
{
    public interface IExportarDatos
    {
        // Escribe el listado de productos analizados en el archivo indicado por ruta.
        void EscribirReporteProductos(List<ReporteProductoDTO> productos, string ruta);

        // Escribe el listado de clientes analizados en el archivo indicado por ruta.
        void EscribirReporteClientes(List<ReporteClienteDTO> clientes, string ruta);
    }
}
