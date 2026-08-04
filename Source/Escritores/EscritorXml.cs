using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Proyecto_Final_POO_C_.Source.DTOs;
using Proyecto_Final_POO_C_.Source.Interfaces;

namespace Proyecto_Final_POO_C_.Source.Escritores
{
    public class EscritorXml : IExportarDatos
    {
        public void EscribirReporteProductos(List<ReporteProductoDTO> productos, string ruta)
        {
            var serializador = new XmlSerializer(typeof(List<ReporteProductoDTO>), new XmlRootAttribute("Productos"));
            using (var escritor = new StreamWriter(ruta))
            {
                serializador.Serialize(escritor, productos);
            }
        }

        public void EscribirReporteClientes(List<ReporteClienteDTO> clientes, string ruta)
        {
            var serializador = new XmlSerializer(typeof(List<ReporteClienteDTO>), new XmlRootAttribute("Clientes"));
            using (var escritor = new StreamWriter(ruta))
            {
                serializador.Serialize(escritor, clientes);
            }
        }
    }
}
