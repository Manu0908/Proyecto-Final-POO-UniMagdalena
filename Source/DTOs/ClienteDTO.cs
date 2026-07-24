using System;
using System.Collections.Generic;
using System.Text;
namespace Proyecto_Final_POO_C_.Source.DTOs
{
    //id_cliente:   Identificador del cliente.
    //nombre:       Nombre completo(obligatorio, no vacío).
    //email:        Dirección de correo electrónico válida(obligatorio, campo de unión).
    //ciudad:       Ciudad de residencia(puede ser vacía).
    //tipo_cliente: Clasificación("natural" o "empresarial").

    // Dado que es un Data Transfer Object, lo mas correcto es tomar cada entrada como string, dado que no es seguro que la
    // informacion se reciba de acuerdo con las reglas de negocio de la empresa.
    public class ClienteDTO
    {
        public string? IdCliente { get; set; }
        public string? Nombre { get; set; }
        public string? Email { get; set; }
        public string? Ciudad { get; set; }
        public string? TipoCliente { get; set; }
    }
}
