// Proyecto Final POO — Estrategia concreta: LectorCSV
// Implementa IImportarDatos para leer archivos delimitados por coma (CSV).
// Manejo de errores:
//   - Errores de I/O (archivo no encontrado, sin permisos) se propagan hacia arriba
//     para que el Program.cs los capture y detenga la ejecución.
//   - Errores de dominio por fila inválida (columnas faltantes, valores corruptos) se
//     capturan dentro del foreach para que el proceso continúe con las demás filas.
using System.Globalization;
using Proyecto_Final_POO_C_.Source.DTOs;
using Proyecto_Final_POO_C_.Source.Interfaces;

namespace Proyecto_Final_POO_C_.Source.Lectores
{
    public class LectorCsv : IImportarDatos
    {
        // ──────────────────────────────────────────────────────────────
        // CLIENTES
        // Columnas esperadas: id_cliente, nombre, email, ciudad, tipo_cliente
        // ──────────────────────────────────────────────────────────────
        public List<ClienteDTO> LeerClientes(string ruta)
        {
            var clientesDTO = new List<ClienteDTO>();

            // File.ReadLines lanza IOException si el archivo no existe o no se puede leer.
            // Esa excepción no se captura aquí: debe detener el programa (error técnico de I/O).
            foreach (string linea in File.ReadLines(ruta).Skip(1))
            {
                try
                {
                    string[] datos = linea.Split(',');

                    // Validación básica de columnas antes de mapear.
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
                    // Error de dominio por fila corrupta: se registra y el proceso continúa.
                    Console.WriteLine($"[ADVERTENCIA] Error al procesar fila de cliente: {ex.Message} | Fila: {linea}");
                }
            }

            return clientesDTO;
        }

        // ──────────────────────────────────────────────────────────────
        // PEDIDOS
        // Columnas esperadas: id_pedido, email_cliente, fecha, tipo_pedido,
        //                     id_producto, nombre_producto, categoria_producto,
        //                     cantidad, precio_unitario
        // Nota: un mismo id_pedido puede aparecer en varias filas (una por ítem).
        // ──────────────────────────────────────────────────────────────
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

                    // Cantidad y precio se intentan parsear; si fallan se asigna null
                    // y la capa de mapeo decidirá si la fila es válida o se descarta.
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
                        Fecha             = datos[2].Trim(),   // String crudo; se parsea fuera con TryParseExact.
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