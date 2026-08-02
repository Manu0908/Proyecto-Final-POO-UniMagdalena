// Proyecto Final POO Clase Padre: Cliente
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace Proyecto_Final_POO_C_.Source.Modelo
{
    public abstract class Cliente
    {   
        private const string PatronEmail = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

        // Explicación del patrón (Por Daniel M):

        //Nota: El @ antes del string sirve para indicarle a C# que interprete las barras invertidas \ de forma literal sin escapar caracteres.

        // ^[^@\s]+  -> Inicia con uno o más caracteres que NO son '@' ni espacios.

        // @  -> Obliga a tener exactamente un símbolo '@'.

        // [^@\s]+  -> Seguido del nombre del dominio (ej: gmail, hotmail, unimagdalena).

        // \.  -> Obliga a tener al menos un punto '.'.

        // [^@\s]+$  -> Termina con la extensión del dominio (ej: com, edu.co, org, net).

        //Explicación de porque hago esto: En el enunciado del proyecto final se nos informo que solo se permiten correos validos, yo supuse, uno que pertenezca a un dominio aceptable, como el comun "email.com", pero ya que exiten mas dominios que si podrian ser aceptados si cumplen con el orden adecuado, investigando cree este patron que debe tener todo correo que pueda ser aceptado.
        private string _id;

        private string _nombre;

        private string _email;

        private string _ciudad;

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
                    throw new ArgumentException("La ID no puede ser nula o estar vacia");
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
                    throw new ArgumentException("El nombre no puede ser nulo o estar vacio");
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
                    throw new ArgumentException("El email no puede estar vacio");
                }
                else if (!Regex.IsMatch(value, PatronEmail))
                {
                    throw new ArgumentException($"El formato del email '{value}' no es un correo electronico valido");
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