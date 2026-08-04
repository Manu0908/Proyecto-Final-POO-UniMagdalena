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

            string fmt = formato.Trim().ToUpper();
            if (fmt == "JSON")
            {
                return new EscritorJson();
            }
            else if (fmt == "XML")
            {
                return new EscritorXml();
            }
            else
            {
                throw new ArgumentException($"Formato de reporte no soportado: {formato}");
            }
        }
    }
}
