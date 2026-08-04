using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Proyecto_Final_POO_C_.Source.DTOs;
using Proyecto_Final_POO_C_.Source.Interfaces;

namespace Proyecto_Final_POO_C_.Source.Lectores
{
    public class LectorJson : IImportarDatos
    {
        public List<ClienteDTO> LeerClientes(string ruta)
        {
            string json = File.ReadAllText(ruta);

            var opciones = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var lista = JsonSerializer.Deserialize<List<ClienteDTO>>(json, opciones);
            return lista ?? new List<ClienteDTO>();
        }

        public List<PedidoItemDTO> LeerPedidos(string ruta)
        {
            string json = File.ReadAllText(ruta);

            var opciones = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var lista = JsonSerializer.Deserialize<List<PedidoItemDTO>>(json, opciones);
            return lista ?? new List<PedidoItemDTO>();
        }
    }
}
