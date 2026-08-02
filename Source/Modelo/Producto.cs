// Proyecto Final POO Clase Normal: Producto
using System;

namespace Proyecto_Final_POO_C_.Source.Modelo
{
    public class Producto
    {
        private decimal _preciounitario;
        private string _idproducto;
        private string _nombreproducto;
        private string _categoria;

        private Producto() {}

        public Producto(decimal preciounitario, string idproducto, string nombreproducto, string categoria)
        {   
            PrecioUnitario = preciounitario;
            IDProducto = idproducto;
            NombreProducto = nombreproducto;
            Categoria = categoria;
            NumeroVentas = 0;
        }

        public decimal PrecioUnitario
        {
            get { return _preciounitario; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "El precio por unidad debe ser mayor a cero.");
                }
                _preciounitario = value;
            }
        }

        public string IDProducto
        {
            get { return _idproducto; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("La id del producto no puede estar vacia.");
                }
                _idproducto = value;
            }
        }

        public string NombreProducto
        {
            get { return _nombreproducto; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("El nombre del producto no puede estar vacio.");
                }
                _nombreproducto = value;
            }
        }

        public string Categoria
        {
            get { return _categoria; }
            set { _categoria = value; }
        }

        public int NumeroVentas { get; set; }
    }
}