// Proyecto Final POO — Clase hija: ClienteNatural
namespace Proyecto_Final_POO_C_.Source.Modelo
{
    public class ClienteNatural : Cliente
    {
        public ClienteNatural(string id, string nombre, string email, string ciudad)
            : base(id, nombre, email, ciudad)
        {
        }

        // Es frecuente si ha realizado más de 5 compras en el historial.
        public override bool EsFrecuente(int cantidadCompras, decimal totalInvertido)
        {
            return cantidadCompras > 5;
        }
    }
}