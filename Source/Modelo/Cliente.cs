using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Proyecto_Final_POO_C_.Source.Modelo
{
    public abstract class Cliente
    {   
        private const string PatronEmail = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

        private string _id = string.Empty;

        private string _nombre = string.Empty;

        private string _email = string.Empty;

        private string _ciudad = string.Empty;

        protected Cliente (string id , string nombre , string email , string ciudad )
        {
            ID = id;
            Nombre = nombre;
            Email = email;
            _ciudad = ciudad;
        }

        public string ID
        {
            get {return _id;}
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ClienteInvalidoException("La ID no puede ser nula o estar vacia");
                }
                _id = value;
            }
        }

        public string Nombre
        {
            get {return _nombre;}
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ClienteInvalidoException("El nombre no puede ser nulo o estar vacio");
                }
                _nombre = value;
            }
        } 

        public string Email
        {
            get {return _email;}
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ClienteInvalidoException("El email no puede estar vacio");
                }
                else if (!Regex.IsMatch(value, PatronEmail))
                {
                    throw new ClienteInvalidoException($"El formato del email '{value}' no es un correo electronico valido");
                }
                _email = value;
            }
        } 

        public string Ciudad
        {
            get{return _ciudad;}
            set{_ciudad = value;}
        }   

        public List<Pedido> Pedidos { get; } = new List<Pedido>();

        public decimal ObtenerTotalAcumulado()
        {
            decimal total = 0m;
            foreach (var pedido in Pedidos)
            {
                total += pedido.CalcularValorTotalConImpuestos();
            }
            return total;
        }

        public Pedido? ObtenerPedidoMasCostoso()
        {
            if (Pedidos.Count == 0)
            {
                return null;
            }

            Pedido masCostoso = Pedidos[0];
            foreach (var pedido in Pedidos)
            {
                if (pedido.CalcularValorTotalConImpuestos() > masCostoso.CalcularValorTotalConImpuestos())
                {
                    masCostoso = pedido;
                }
            }
            return masCostoso;
        }

        public abstract bool EsFrecuente(int cantidadCompras, decimal totalInvertido);  
    }
}