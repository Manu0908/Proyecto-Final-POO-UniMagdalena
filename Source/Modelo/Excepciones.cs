using System;

namespace Proyecto_Final_POO_C_.Source.Modelo
{
    public class ClienteInvalidoException : ArgumentException
    {
        public ClienteInvalidoException(string message) : base(message) { }
    }

    public class ProductoInvalidoException : ArgumentException
    {
        public ProductoInvalidoException(string message) : base(message) { }
        public ProductoInvalidoException(string message, string paramName) : base(message, paramName) { }
    }

    public class PedidoInvalidoException : ArgumentException
    {
        public PedidoInvalidoException(string message) : base(message) { }
        public PedidoInvalidoException(string message, string paramName) : base(message, paramName) { }
    }

    public class ProcesamientoPipelineException : Exception
    {
        public ProcesamientoPipelineException(string message) : base(message) { }
        public ProcesamientoPipelineException(string message, Exception innerException) : base(message, innerException) { }
    }
}
