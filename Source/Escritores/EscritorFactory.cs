using System;
using Proyecto_Final_POO_C_.Source.Interfaces;

namespace Proyecto_Final_POO_C_.Source.Escritores
{
    public static class EscritorFactory
    {
        public static IExportarDatos ObtenerEscritor(string formato)
        {
            if (string.IsNullOrWhiteSpace(formato))
            {
                throw new ArgumentException("El formato del escritor no puede estar vacío.");
            }

            return formato.Trim().ToUpper() switch
            {
                "JSON" => new EscritorJson(),
                "XML" => new EscritorXml(),
                _ => throw new ArgumentException($"Formato de reporte no soportado: {formato}")
            };
        }
    }
}
