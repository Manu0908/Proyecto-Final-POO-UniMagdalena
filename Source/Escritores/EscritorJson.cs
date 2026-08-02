// Proyecto Final POO — Estrategia concreta: EscritorJson
// Implementa IExportarDatos para escribir reportes en formato JSON.
using System.IO;
using System.Text.Json;
using Proyecto_Final_POO_C_.Source.DTOs;
using Proyecto_Final_POO_C_.Source.Interfaces;

namespace Proyecto_Final_POO_C_.Source.Escritores
{
    public class EscritorJson : IExportarDatos
    {
        public void EscribirReporteProductos(List<ReporteProductoDTO> productos, string ruta)
        {
            var opciones = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            string json = JsonSerializer.Serialize(productos, opciones);
            File.WriteAllText(ruta, json);
        }

        public void EscribirReporteClientes(List<ReporteClienteDTO> clientes, string ruta)
        {
            var opciones = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            string json = JsonSerializer.Serialize(clientes, opciones);
            File.WriteAllText(ruta, json);
        }
    }
}
