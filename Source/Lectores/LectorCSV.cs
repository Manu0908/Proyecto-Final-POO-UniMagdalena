using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Proyecto_Final_POO_C_.Source.DTOs;
using Proyecto_Final_POO_C_.Source.Interfaces;

namespace Proyecto_Final_POO_C_.Source.Lectores
{
    public class LectorCsv : IImportarDatos
    {
        public List<ClienteDTO> LeerClientes(string ruta)
        {
            var clientesDTO = new List<ClienteDTO>();

            foreach (string linea in File.ReadLines(ruta).Skip(1))
            {
                try
                {
                    string[] datos = linea.Split(',');

                    if (datos.Length < 5)
                    {
                        Console.WriteLine($"[ADVERTENCIA] Fila de cliente ignorada (columnas insuficientes): {linea}");
                        continue;
                    }

                    var cliente = new ClienteDTO
                    {
                        IdCliente   = datos[0].Trim(),
                        Nombre      = datos[1].Trim(),
                        Email       = datos[2].Trim(),
                        Ciudad      = datos[3].Trim(),
                        TipoCliente = datos[4].Trim()
                    };

                    clientesDTO.Add(cliente);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ADVERTENCIA] Error al procesar fila de cliente: {ex.Message} | Fila: {linea}");
                }
            }

            return clientesDTO;
        }

        public List<PedidoItemDTO> LeerPedidos(string ruta)
        {
            var itemsDTO = new List<PedidoItemDTO>();

            foreach (string linea in File.ReadLines(ruta).Skip(1))
            {
                try
                {
                    string[] datos = linea.Split(',');

                    if (datos.Length < 9)
                    {
                        Console.WriteLine($"[ADVERTENCIA] Fila de pedido ignorada (columnas insuficientes): {linea}");
                        continue;
                    }

                    int? cantidad = int.TryParse(datos[7].Trim(), out int cantidadParseada)
                        ? cantidadParseada
                        : null;

                    decimal? precioUnitario = decimal.TryParse(
                        datos[8].Trim(),
                        NumberStyles.Any,
                        CultureInfo.InvariantCulture,
                        out decimal precioParseado)
                        ? precioParseado
                        : null;

                    var item = new PedidoItemDTO
                    {
                        IdPedido          = datos[0].Trim(),
                        EmailCliente      = datos[1].Trim(),
                        Fecha             = datos[2].Trim(),
                        TipoPedido        = datos[3].Trim(),
                        IdProducto        = datos[4].Trim(),
                        NombreProducto    = datos[5].Trim(),
                        CategoriaProducto = datos[6].Trim(),
                        Cantidad          = cantidad,
                        PrecioUnitario    = precioUnitario
                    };

                    itemsDTO.Add(item);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ADVERTENCIA] Error al procesar fila de pedido: {ex.Message} | Fila: {linea}");
                }
            }

            return itemsDTO;
        }
    }
}