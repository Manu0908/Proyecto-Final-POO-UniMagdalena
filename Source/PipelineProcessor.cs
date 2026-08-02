// Proyecto Final POO — Capa de Servicios: PipelineProcessor
// Orquesta todo el pipeline: Carga -> Limpia/Valida -> Relaciona -> Analiza -> Exporta
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

            // ──────────────────────────────────────────────────────────────
            // 1. CARGA DE DATOS (LECTURA)
            // ──────────────────────────────────────────────────────────────
            IImportarDatos lectorClientes = LectorFactory.ObtenerLector(formatoClientes);
            IImportarDatos lectorPedidos = LectorFactory.ObtenerLector(formatoPedidos);

            List<ClienteDTO> clientesRaw = lectorClientes.LeerClientes(rutaClientes);
            List<PedidoItemDTO> pedidosRaw = lectorPedidos.LeerPedidos(rutaPedidos);

            Console.WriteLine($"[Pipeline] Se leyeron {clientesRaw.Count} registros de clientes crudos.");
            Console.WriteLine($"[Pipeline] Se leyeron {pedidosRaw.Count} filas de pedidos crudas.");

            // ──────────────────────────────────────────────────────────────
            // 2. PROCESAMIENTO Y VALIDACIÓN DE CLIENTES
            // ──────────────────────────────────────────────────────────────
            var clientesMap = new Dictionary<string, Cliente>(StringComparer.OrdinalIgnoreCase);
            int clientesIgnorados = 0;

            foreach (var dto in clientesRaw)
            {
                try
                {
                    if (dto == null) continue;

                    string id = dto.IdCliente?.Trim() ?? string.Empty;
                    string nombre = dto.Nombre?.Trim() ?? string.Empty;
                    string email = dto.Email?.Trim() ?? string.Empty;
                    string ciudad = dto.Ciudad?.Trim() ?? string.Empty;
                    string tipo = dto.TipoCliente?.Trim().ToLower() ?? string.Empty;

                    // Validaciones rápidas para registrar advertencia clara antes de instanciar el dominio
                    if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(email))
                    {
                        Console.WriteLine($"[ADVERTENCIA] Cliente omitido (id, nombre y email son obligatorios): ID='{id}', Nombre='{nombre}', Email='{email}'");
                        clientesIgnorados++;
                        continue;
                    }

                    if (!Regex.IsMatch(email, PatronEmail))
                    {
                        Console.WriteLine($"[ADVERTENCIA] Cliente '{nombre}' omitido por formato de email inválido: '{email}'");
                        clientesIgnorados++;
                        continue;
                    }

                    if (clientesMap.ContainsKey(email))
                    {
                        Console.WriteLine($"[ADVERTENCIA] Cliente omitido por correo duplicado: '{email}'");
                        clientesIgnorados++;
                        continue;
                    }

                    Cliente cliente;
                    if (tipo == "natural")
                    {
                        cliente = new ClienteNatural(id, nombre, email, ciudad);
                    }
                    else if (tipo == "empresarial")
                    {
                        cliente = new ClienteEmpresarial(id, nombre, email, ciudad);
                    }
                    else
                    {
                        Console.WriteLine($"[ADVERTENCIA] Cliente '{nombre}' omitido por tipo no reconocido: '{tipo}'");
                        clientesIgnorados++;
                        continue;
                    }

                    clientesMap.Add(email, cliente);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ADVERTENCIA] Error de dominio al procesar cliente: {ex.Message}");
                    clientesIgnorados++;
                }
            }

            // ──────────────────────────────────────────────────────────────
            // 3. PROCESAMIENTO Y VALIDACIÓN DE PEDIDOS (AGRUPACIÓN POR ID)
            // ──────────────────────────────────────────────────────────────
            var productosCatalog = new Dictionary<string, Producto>(StringComparer.OrdinalIgnoreCase);
            var todosLosPedidos = new List<Pedido>();
            var pedidosHuerfanos = new List<Pedido>();
            int pedidosIgnorados = 0;

            // Agrupar filas crudas por ID del Pedido
            var pedidosAgrupados = pedidosRaw
                .Where(p => p != null && !string.IsNullOrWhiteSpace(p.IdPedido))
                .GroupBy(p => p.IdPedido!.Trim());

            foreach (var grupo in pedidosAgrupados)
            {
                try
                {
                    string idPedido = grupo.Key;
                    
                    // Tomar metadatos generales del pedido a partir de la primera fila
                    var primeraFila = grupo.First();
                    string emailCliente = primeraFila.EmailCliente?.Trim() ?? string.Empty;
                    string fechaStr = primeraFila.Fecha?.Trim() ?? string.Empty;
                    string tipoPedido = primeraFila.TipoPedido?.Trim().ToLower() ?? string.Empty;

                    // Validar metadatos de cabecera del pedido
                    if (string.IsNullOrEmpty(emailCliente) || string.IsNullOrEmpty(fechaStr) || string.IsNullOrEmpty(tipoPedido))
                    {
                        Console.WriteLine($"[ADVERTENCIA] Pedido '{idPedido}' omitido por datos incompletos en la cabecera.");
                        pedidosIgnorados++;
                        continue;
                    }

                    if (!Regex.IsMatch(emailCliente, PatronEmail))
                    {
                        Console.WriteLine($"[ADVERTENCIA] Pedido '{idPedido}' omitido por formato de email inválido: '{emailCliente}'");
                        pedidosIgnorados++;
                        continue;
                    }

                    // Intentar parsear la fecha de compra
                    DateTime fechaCompra;
                    string[] formatosFecha = { "yyyy-MM-dd", "dd/MM/yyyy", "yyyy/MM/dd", "d/M/yyyy", "yyyy-MM-dd HH:mm:ss" };
                    if (!DateTime.TryParseExact(fechaStr, formatosFecha, CultureInfo.InvariantCulture, DateTimeStyles.None, out fechaCompra))
                    {
                        if (!DateTime.TryParse(fechaStr, out fechaCompra))
                        {
                            Console.WriteLine($"[ADVERTENCIA] Pedido '{idPedido}' omitido por formato de fecha inválido: '{fechaStr}'");
                            pedidosIgnorados++;
                            continue;
                        }
                    }

                    Pedido pedido;
                    if (tipoPedido == "nacional")
                    {
                        pedido = new PedidoNacional(idPedido, fechaCompra, emailCliente);
                    }
                    else if (tipoPedido == "internacional")
                    {
                        pedido = new PedidoInternacional(idPedido, fechaCompra, emailCliente);
                    }
                    else
                    {
                        Console.WriteLine($"[ADVERTENCIA] Pedido '{idPedido}' omitido por tipo no reconocido: '{tipoPedido}'");
                        pedidosIgnorados++;
                        continue;
                    }

                    // Validar y agregar ítems
                    foreach (var filaItem in grupo)
                    {
                        try
                        {
                            string idProducto = filaItem.IdProducto?.Trim() ?? string.Empty;
                            string nombreProducto = filaItem.NombreProducto?.Trim() ?? string.Empty;
                            string categoria = filaItem.CategoriaProducto?.Trim() ?? string.Empty;
                            int cantidad = filaItem.Cantidad ?? 0;
                            decimal precioUnitario = filaItem.PrecioUnitario ?? 0m;

                            if (string.IsNullOrEmpty(idProducto) || string.IsNullOrEmpty(nombreProducto))
                            {
                                Console.WriteLine($"[ADVERTENCIA] Ítem en pedido '{idPedido}' omitido por producto sin ID o nombre.");
                                continue;
                            }

                            if (cantidad <= 0 || precioUnitario <= 0m)
                            {
                                Console.WriteLine($"[ADVERTENCIA] Ítem en pedido '{idPedido}' omitido por cantidad o precio no válidos (<= 0).");
                                continue;
                            }

                            // Obtener o registrar producto en el catálogo global
                            if (!productosCatalog.TryGetValue(idProducto, out Producto? producto))
                            {
                                producto = new Producto(precioUnitario, idProducto, nombreProducto, categoria);
                                productosCatalog.Add(idProducto, producto);
                            }

                            // Incrementar el número acumulado de ventas del producto
                            producto.NumeroVentas += cantidad;

                            // Crear y agregar ítem de pedido
                            var item = new PedidoItem(producto, cantidad, precioUnitario);
                            pedido.Items.Add(item);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"[ADVERTENCIA] Error en ítem de pedido '{idPedido}': {ex.Message}");
                        }
                    }

                    // Si el pedido no quedó con ítems válidos, se ignora por completo
                    if (pedido.Items.Count == 0)
                    {
                        Console.WriteLine($"[ADVERTENCIA] Pedido '{idPedido}' ignorado porque no contiene ítems válidos.");
                        pedidosIgnorados++;
                        continue;
                    }

                    todosLosPedidos.Add(pedido);

                    // Relacionar pedido con el cliente
                    if (clientesMap.TryGetValue(emailCliente, out Cliente? clienteAsociado))
                    {
                        clienteAsociado.Pedidos.Add(pedido);
                    }
                    else
                    {
                        // Pedido huérfano (compra con email no existente en clientes)
                        pedidosHuerfanos.Add(pedido);
                    }
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

            // ──────────────────────────────────────────────────────────────
            // 4. MAPEO A DTOs DE REPORTE
            // ──────────────────────────────────────────────────────────────
            // A. Reporte de Productos
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

            // B. Reporte de Clientes
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

            // ──────────────────────────────────────────────────────────────
            // 5. EXPORTACIÓN DE REPORTES (ESCRITURA)
            // ──────────────────────────────────────────────────────────────
            IExportarDatos escritor = EscritorFactory.ObtenerEscritor(formatoSalida);
            
            // Garantizar la existencia del directorio destino
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

            // ──────────────────────────────────────────────────────────────
            // 6. RESUMEN EN CONSOLA
            // ──────────────────────────────────────────────────────────────
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
    }
}
