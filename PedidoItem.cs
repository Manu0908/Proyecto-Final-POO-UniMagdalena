// Proyecto Final POO Clase Normal: PedidoItem
namespace Proyecto_Final_POO_C_{
    public class PedidoItem
    {
        private int _cantidad;
        private decimal _preciounitario;

        private PedidoItem() {}

        public PedidoItem(Producto productoAsociado, int cantidad, decimal preciounitario)
        {
            ProductoAsociado = productoAsociado ?? throw new ArgumentNullException(nameof(productoAsociado), "El producto asociado no puede ser nulo.");

            Cantidad = cantidad;
            PrecioUnitario = preciounitario;
        }

        public Producto ProductoAsociado { get; set; }

        public int Cantidad
        {
            get { return _cantidad; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "La cantidad comprada debe ser mayor a cero.");
                }
                _cantidad = value;
            }
        }

        public decimal PrecioUnitario
        {
            get { return _preciounitario; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "El precio unitario del item debe ser mayor a cero.");
                }
                _preciounitario = value;
            }
        }

        public decimal CalcularSubtotalItem()
        {
            return Cantidad * _preciounitario;
        }
    }
}