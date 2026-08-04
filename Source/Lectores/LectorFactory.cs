using System;
using Proyecto_Final_POO_C_.Source.Interfaces;

namespace Proyecto_Final_POO_C_.Source.Lectores
{
    public static class LectorFactory
    {
        public static IImportarDatos ObtenerLector(string formato)
        {
            if (string.IsNullOrWhiteSpace(formato))
            {
                throw new ArgumentException("El formato del lector no puede estar vacío.");
            }

            string fmt = formato.Trim().ToUpper();
            if (fmt == "CSV")
            {
                return new LectorCsv();
            }
            else if (fmt == "JSON")
            {
                return new LectorJson();
            }
            else
            {
                throw new ArgumentException($"Formato de entrada no soportado: {formato}");
            }
        }
    }
}
