using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Proyecto_Final_POO_C_.Source.DTOs;
using Proyecto_Final_POO_C_.Source.Escritores;
using Proyecto_Final_POO_C_.Source.Interfaces;
using Proyecto_Final_POO_C_.Source.Lectores;
using Proyecto_Final_POO_C_.Source.Modelo;

namespace Proyecto_Final_POO_C_.Source
{
    public class PipelineProcessor
    {
        private const string PatronEmail = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

        public void Ejecutar(
            string rutaClientes, 
            string formatoClientes, 
            string rutaPedidos, 
            string formatoPedidos, 
            string formatoSalida, 
            string rutaReporteProductos,
            string rutaReporteClientes)
        {
            Console.WriteLine("\n[Pipeline] Iniciando procesamiento...");

            IImportarDatos lectorClientes = LectorFactory.ObtenerLector(formatoClientes);
            IImportarDatos lectorPedidos = LectorFactory.ObtenerLector(formatoPedidos);

            List<ClienteDTO> clientesRaw = lectorClientes.LeerClientes(rutaClientes);
            List<PedidoItemDTO> pedidosRaw = lectorPedidos.LeerPedidos(rutaPedidos);

            Console.WriteLine($"[Pipeline] Se leyeron {clientesRaw.Count} registros de clientes crudos.");
            Console.WriteLine($"[Pipeline] Se leyeron {pedidosRaw.Count} filas de pedidos crudas.");

            var clientesMap = new Dictionary<string, Cliente>(StringComparer.OrdinalIgnoreCase);
            int clientesIgnorados = 0;

            foreach (var dto in clientesRaw)
            {
                try
                {
                    if (dto == null) continue;
                    Cliente cliente = MapearCliente(dto, clientesMap);
                    clientesMap.Add(cliente.Email, cliente);
                }
                catch (ProcesamientoPipelineException ex)
                {
                    Console.WriteLine($"[ADVERTENCIA] {ex.Message}");
                    clientesIgnorados++;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ADVERTENCIA] Error de dominio al procesar cliente: {ex.Message}");
                    clientesIgnorados++;
                }
            }

            var productosCatalog = new Dictionary<string, Producto>(StringComparer.OrdinalIgnoreCase);
            var todosLosPedidos = new List<Pedido>();
            var pedidosHuerfanos = new List<Pedido>();
            int pedidosIgnorados = 0;

            var pedidosAgrupados = pedidosRaw
                .Where(p => p != null && !string.IsNullOrWhiteSpace(p.IdPedido))
                .GroupBy(p => p.IdPedido!.Trim());

            foreach (var grupo in pedidosAgrupados)
            {
                try
                {
                    string idPedido = grupo.Key;
                    var primeraFila = grupo.First();
                    Pedido pedido = MapearPedidoCabecera(idPedido, primeraFila);

                    foreach (var filaItem in grupo)
                    {
                        try
                        {
                            PedidoItem item = MapearPedidoItem(idPedido, filaItem, productosCatalog);
                            pedido.Items.Add(item);
                        }
                        catch (ProcesamientoPipelineException ex)
                        {
                            Console.WriteLine($"[ADVERTENCIA] {ex.Message}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"[ADVERTENCIA] Error en ítem de pedido '{idPedido}': {ex.Message}");
                        }
                    }

                    if (pedido.Items.Count == 0)
                    {
                        Console.WriteLine($"[ADVERTENCIA] Pedido '{idPedido}' ignorado porque no contiene ítems válidos.");
                        pedidosIgnorados++;
                        continue;
                    }

                    todosLosPedidos.Add(pedido);

                    if (clientesMap.TryGetValue(pedido.EmailCliente, out Cliente? clienteAsociado))
                    {
                        clienteAsociado.Pedidos.Add(pedido);
                    }
                    else
                    {
                        pedidosHuerfanos.Add(pedido);
                    }
                }
                catch (ProcesamientoPipelineException ex)
                {
                    Console.WriteLine($"[ADVERTENCIA] {ex.Message}");
                    pedidosIgnorados++;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ADVERTENCIA] Error de dominio al procesar pedido: {ex.Message}");
                    pedidosIgnorados++;
                }
            }

            Console.WriteLine($"[Pipeline] Clientes válidos procesados: {clientesMap.Count} (Ignorados/Duplicados: {clientesIgnorados})");
            Console.WriteLine($"[Pipeline] Pedidos válidos procesados: {todosLosPedidos.Count} (Ignorados: {pedidosIgnorados})");
            Console.WriteLine($"[Pipeline] Pedidos huérfanos detectados: {pedidosHuerfanos.Count}");

            List<ReporteProductoDTO> productosReporte = productosCatalog.Values
                .Select(p => new ReporteProductoDTO
                {
                    IDProducto = p.IDProducto,
                    NombreProducto = p.NombreProducto,
                    Categoria = p.Categoria,
                    PrecioUnitario = p.PrecioUnitario,
                    NumeroVentas = p.NumeroVentas
                })
                .OrderBy(p => p.IDProducto)
                .ToList();

            List<ReporteClienteDTO> clientesReporte = clientesMap.Values
                .Select(c =>
                {
                    decimal totalAcumulado = c.ObtenerTotalAcumulado();
                    Pedido? masCostoso = c.ObtenerPedidoMasCostoso();
                    PedidoReporteDTO? pedidoReporteDto = null;

                    if (masCostoso != null)
                    {
                        pedidoReporteDto = new PedidoReporteDTO
                        {
                            IdPedido = masCostoso.IDPedido,
                            Fecha = masCostoso.FechaCompra.ToString("yyyy-MM-dd"),
                            TipoPedido = masCostoso is PedidoNacional ? "nacional" : "internacional",
                            SubTotal = masCostoso.CalcularValorSinImpuestos(),
                            ImpuestoAplicado = masCostoso.CalcularImpuestoAplicado(),
                            TotalFinal = masCostoso.CalcularValorTotalConImpuestos(),
                            Items = masCostoso.Items.Select(item => new PedidoItemDTO
                            {
                                IdPedido = masCostoso.IDPedido,
                                EmailCliente = masCostoso.EmailCliente,
                                Fecha = masCostoso.FechaCompra.ToString("yyyy-MM-dd"),
                                TipoPedido = masCostoso is PedidoNacional ? "nacional" : "internacional",
                                IdProducto = item.ProductoAsociado.IDProducto,
                                NombreProducto = item.ProductoAsociado.NombreProducto,
                                CategoriaProducto = item.ProductoAsociado.Categoria,
                                Cantidad = item.Cantidad,
                                PrecioUnitario = item.PrecioUnitario
                            }).ToList()
                        };
                    }

                    return new ReporteClienteDTO
                    {
                        IdCliente = c.ID,
                        Nombre = c.Nombre,
                        Email = c.Email,
                        Ciudad = c.Ciudad,
                        TipoCliente = c is ClienteNatural ? "natural" : "empresarial",
                        EsFrecuente = c.EsFrecuente(c.Pedidos.Count, totalAcumulado),
                        TotalAcumuladoCompras = totalAcumulado,
                        PedidoMasCostoso = pedidoReporteDto
                    };
                })
                .OrderBy(c => c.IdCliente)
                .ToList();

            IExportarDatos escritor = EscritorFactory.ObtenerEscritor(formatoSalida);
            
            string? dirSalidaP = Path.GetDirectoryName(rutaReporteProductos);
            if (!string.IsNullOrEmpty(dirSalidaP) && !Directory.Exists(dirSalidaP))
            {
                Directory.CreateDirectory(dirSalidaP);
            }
            string? dirSalidaC = Path.GetDirectoryName(rutaReporteClientes);
            if (!string.IsNullOrEmpty(dirSalidaC) && !Directory.Exists(dirSalidaC))
            {
                Directory.CreateDirectory(dirSalidaC);
            }

            escritor.EscribirReporteProductos(productosReporte, rutaReporteProductos);
            escritor.EscribirReporteClientes(clientesReporte, rutaReporteClientes);

            Console.WriteLine($"[Pipeline] Reportes exportados exitosamente en formato {formatoSalida.ToUpper()}.");

            decimal ventasTotales = todosLosPedidos.Sum(p => p.CalcularValorTotalConImpuestos());
            int totalPedidosNacionales = todosLosPedidos.Count(p => p is PedidoNacional);
            int totalPedidosInternacionales = todosLosPedidos.Count(p => p is PedidoInternacional);
            int totalClientesNaturales = clientesMap.Values.Count(c => c is ClienteNatural);
            int totalClientesEmpresariales = clientesMap.Values.Count(c => c is ClienteEmpresarial);

            Console.WriteLine("\n==================================================");
            Console.WriteLine("          RESUMEN GENERAL DEL PROCESAMIENTO");
            Console.WriteLine("==================================================");
            Console.WriteLine($"Ventas totales del negocio (con imp.): {ventasTotales.ToString("C", CultureInfo.GetCultureInfo("es-CO"))}");
            Console.WriteLine($"Pedidos Nacionales:                    {totalPedidosNacionales}");
            Console.WriteLine($"Pedidos Internacionales:               {totalPedidosInternacionales}");
            Console.WriteLine($"Clientes Naturales registrados:        {totalClientesNaturales}");
            Console.WriteLine($"Clientes Empresariales registrados:    {totalClientesEmpresariales}");
            Console.WriteLine($"Pedidos huérfanos registrados:         {pedidosHuerfanos.Count}");
            Console.WriteLine("==================================================\n");
        }

        private Cliente MapearCliente(ClienteDTO dto, Dictionary<string, Cliente> clientesMap)
        {
            string id = dto.IdCliente?.Trim() ?? string.Empty;
            string nombre = dto.Nombre?.Trim() ?? string.Empty;
            string email = dto.Email?.Trim() ?? string.Empty;
            string ciudad = dto.Ciudad?.Trim() ?? string.Empty;
            string tipo = dto.TipoCliente?.Trim().ToLower() ?? string.Empty;

            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(email))
            {
                throw new ProcesamientoPipelineException($"Cliente omitido (id, nombre y email son obligatorios): ID='{id}', Nombre='{nombre}', Email='{email}'");
            }

            if (!Regex.IsMatch(email, PatronEmail))
            {
                throw new ProcesamientoPipelineException($"Cliente '{nombre}' omitido por formato de email inválido: '{email}'");
            }

            if (clientesMap.ContainsKey(email))
            {
                throw new ProcesamientoPipelineException($"Cliente omitido por correo duplicado: '{email}'");
            }

            if (tipo == "natural")
            {
                return new ClienteNatural(id, nombre, email, ciudad);
            }
            else if (tipo == "empresarial")
            {
                return new ClienteEmpresarial(id, nombre, email, ciudad);
            }
            else
            {
                throw new ProcesamientoPipelineException($"Cliente '{nombre}' omitido por tipo no reconocido: '{tipo}'");
            }
        }

        private Pedido MapearPedidoCabecera(string idPedido, PedidoItemDTO primeraFila)
        {
            string emailCliente = primeraFila.EmailCliente?.Trim() ?? string.Empty;
            string fechaStr = primeraFila.Fecha?.Trim() ?? string.Empty;
            string tipoPedido = primeraFila.TipoPedido?.Trim().ToLower() ?? string.Empty;

            if (string.IsNullOrEmpty(emailCliente) || string.IsNullOrEmpty(fechaStr) || string.IsNullOrEmpty(tipoPedido))
            {
                throw new ProcesamientoPipelineException($"Pedido '{idPedido}' omitido por datos incompletos en la cabecera.");
            }

            if (!Regex.IsMatch(emailCliente, PatronEmail))
            {
                throw new ProcesamientoPipelineException($"Pedido '{idPedido}' omitido por formato de email inválido: '{emailCliente}'");
            }

            DateTime fechaCompra;
            string[] formatosFecha = { "yyyy-MM-dd", "dd/MM/yyyy", "yyyy/MM/dd", "d/M/yyyy", "yyyy-MM-dd HH:mm:ss" };
            if (!DateTime.TryParseExact(fechaStr, formatosFecha, CultureInfo.InvariantCulture, DateTimeStyles.None, out fechaCompra))
            {
                if (!DateTime.TryParse(fechaStr, out fechaCompra))
                {
                    throw new ProcesamientoPipelineException($"Pedido '{idPedido}' omitido por formato de fecha inválido: '{fechaStr}'");
                }
            }

            if (tipoPedido == "nacional")
            {
                return new PedidoNacional(idPedido, fechaCompra, emailCliente);
            }
            else if (tipoPedido == "internacional")
            {
                return new PedidoInternacional(idPedido, fechaCompra, emailCliente);
            }
            else
            {
                throw new ProcesamientoPipelineException($"Pedido '{idPedido}' omitido por tipo no reconocido: '{tipoPedido}'");
            }
        }

        private PedidoItem MapearPedidoItem(string idPedido, PedidoItemDTO filaItem, Dictionary<string, Producto> productosCatalog)
        {
            string idProducto = filaItem.IdProducto?.Trim() ?? string.Empty;
            string nombreProducto = filaItem.NombreProducto?.Trim() ?? string.Empty;
            string categoria = filaItem.CategoriaProducto?.Trim() ?? string.Empty;
            int cantidad = filaItem.Cantidad ?? 0;
            decimal precioUnitario = filaItem.PrecioUnitario ?? 0m;

            if (string.IsNullOrEmpty(idProducto) || string.IsNullOrEmpty(nombreProducto))
            {
                throw new ProcesamientoPipelineException($"Ítem en pedido '{idPedido}' omitido por producto sin ID o nombre.");
            }

            if (cantidad <= 0 || precioUnitario <= 0m)
            {
                throw new ProcesamientoPipelineException($"Ítem en pedido '{idPedido}' omitido por cantidad o precio no válidos (<= 0).");
            }

            if (!productosCatalog.TryGetValue(idProducto, out Producto? producto))
            {
                producto = new Producto(precioUnitario, idProducto, nombreProducto, categoria);
                productosCatalog.Add(idProducto, producto);
            }

            producto.NumeroVentas += cantidad;

            return new PedidoItem(producto, cantidad, precioUnitario);
        }
    }
}
