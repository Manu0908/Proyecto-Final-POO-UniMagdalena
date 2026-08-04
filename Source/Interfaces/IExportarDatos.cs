using System.Collections.Generic;
using Proyecto_Final_POO_C_.Source.DTOs;

namespace Proyecto_Final_POO_C_.Source.Interfaces
{
    public interface IExportarDatos
    {
        void EscribirReporteProductos(List<ReporteProductoDTO> productos, string ruta);
        void EscribirReporteClientes(List<ReporteClienteDTO> clientes, string ruta);
    }
}
