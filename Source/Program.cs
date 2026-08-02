using System;
using System.IO;

namespace Proyecto_Final_POO_C_.Source
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("==================================================");
            Console.WriteLine(" PIPELINE DE ANÁLISIS DE CLIENTES Y COMPRAS — POO ");
            Console.WriteLine("==================================================");

            try
            {
                // 1. Ruta del archivo de clientes
                Console.Write("\nIngrese la ruta del archivo de clientes: ");
                string rutaClientes = Console.ReadLine()?.Trim() ?? string.Empty;
                if (string.IsNullOrEmpty(rutaClientes))
                {
                    throw new ArgumentException("La ruta del archivo de clientes no puede estar vacía.");
                }

                // 2. Formato del archivo de clientes
                Console.Write("Ingrese el formato del archivo de clientes (CSV o JSON): ");
                string formatoClientes = Console.ReadLine()?.Trim().ToUpper() ?? string.Empty;
                if (formatoClientes != "CSV" && formatoClientes != "JSON")
                {
                    throw new ArgumentException("El formato del archivo de clientes debe ser CSV o JSON.");
                }

                // 3. Ruta del archivo de compras
                Console.Write("\nIngrese la ruta del archivo de compras (pedidos): ");
                string rutaPedidos = Console.ReadLine()?.Trim() ?? string.Empty;
                if (string.IsNullOrEmpty(rutaPedidos))
                {
                    throw new ArgumentException("La ruta del archivo de compras no puede estar vacía.");
                }

                // 4. Formato del archivo de compras
                Console.Write("Ingrese el formato del archivo de compras (CSV o JSON): ");
                string formatoPedidos = Console.ReadLine()?.Trim().ToUpper() ?? string.Empty;
                if (formatoPedidos != "CSV" && formatoPedidos != "JSON")
                {
                    throw new ArgumentException("El formato del archivo de compras debe ser CSV o JSON.");
                }

                // 5. Formato del reporte de salida
                Console.Write("\nIngrese el formato de salida para los reportes (JSON o XML): ");
                string formatoSalida = Console.ReadLine()?.Trim().ToUpper() ?? string.Empty;
                if (formatoSalida != "JSON" && formatoSalida != "XML")
                {
                    throw new ArgumentException("El formato de los reportes debe ser JSON o XML.");
                }

                // 6. Carpeta de destino de los reportes
                Console.Write("\nIngrese la carpeta de destino para guardar los reportes (deje vacío para usar la carpeta actual): ");
                string carpetaSalida = Console.ReadLine()?.Trim() ?? string.Empty;
                if (string.IsNullOrEmpty(carpetaSalida))
                {
                    carpetaSalida = Directory.GetCurrentDirectory();
                }

                string ext = formatoSalida.ToLower();
                string rutaReporteProductos = Path.Combine(carpetaSalida, $"reporte_productos.{ext}");
                string rutaReporteClientes = Path.Combine(carpetaSalida, $"reporte_clientes.{ext}");

                // Instanciar y ejecutar el Pipeline
                var procesador = new PipelineProcessor();
                procesador.Ejecutar(
                    rutaClientes, 
                    formatoClientes, 
                    rutaPedidos, 
                    formatoPedidos, 
                    formatoSalida, 
                    rutaReporteProductos, 
                    rutaReporteClientes);

                Console.WriteLine("\n[Programa] ¡Procesamiento completado con éxito!");
            }
            catch (ArgumentException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n[ERROR DE CONFIGURACIÓN] {ex.Message}");
                Console.ResetColor();
            }
            catch (FileNotFoundException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n[ERROR CRÍTICO I/O] Archivo no encontrado: {ex.FileName}");
                Console.WriteLine("Por favor, verifique la ruta e intente de nuevo.");
                Console.ResetColor();
            }
            catch (DirectoryNotFoundException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n[ERROR CRÍTICO I/O] Directorio no encontrado.");
                Console.WriteLine("El directorio especificado para la entrada o salida no existe.");
                Console.ResetColor();
            }
            catch (IOException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n[ERROR CRÍTICO DE LECTURA/ESCRITURA] Falla física en el archivo: {ex.Message}");
                Console.ResetColor();
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n[ERROR CRÍTICO DE PERMISOS] Sin acceso al archivo o directorio: {ex.Message}");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n[ERROR INESPERADO] {ex.Message}");
                Console.ResetColor();
            }
        }
    }
}
