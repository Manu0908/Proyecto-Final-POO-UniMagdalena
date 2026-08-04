# Diagramas UML del Proyecto

Este documento contiene la representación visual de la arquitectura del proyecto utilizando la sintaxis de Mermaid. GitHub renderiza estos diagramas de forma nativa.

---

## 1. Diagrama de Clases Completo

```mermaid
classDiagram
    %% Capa de Dominio (Modelo)
    class Cliente {
        <<abstract>>
        -string _id
        -string _nombre
        -string _email
        -string _ciudad
        +ID string
        +Nombre string
        +Email string
        +Ciudad string
        +Pedidos List~Pedido~
        +ObtenerTotalAcumulado() decimal
        +ObtenerPedidoMasCostoso() Pedido?
        +EsFrecuente(int, decimal)* bool
    }
    class ClienteNatural {
        +EsFrecuente(int, decimal) bool
    }
    class ClienteEmpresarial {
        +EsFrecuente(int, decimal) bool
    }
    Cliente <|-- ClienteNatural
    Cliente <|-- ClienteEmpresarial

    class Pedido {
        <<abstract>>
        -string _idpedido
        -string _emailcliente
        +IDPedido string
        +FechaCompra DateTime
        +EmailCliente string
        +Items List~PedidoItem~
        +CalcularValorSinImpuestos() decimal
        +CalcularImpuestoAplicado()* decimal
        +CalcularValorTotalConImpuestos()* decimal
    }
    class PedidoNacional {
        +CalcularImpuestoAplicado() decimal
        +CalcularValorTotalConImpuestos() decimal
    }
    class PedidoInternacional {
        +CalcularImpuestoAplicado() decimal
        +CalcularValorTotalConImpuestos() decimal
    }
    Pedido <|-- PedidoNacional
    Pedido <|-- PedidoInternacional

    class PedidoItem {
        -int _cantidad
        -decimal _preciounitario
        +ProductoAsociado Producto
        +Cantidad int
        +PrecioUnitario decimal
        +CalcularSubtotalItem() decimal
    }
    class Producto {
        -decimal _preciounitario
        -string _idproducto
        -string _nombreproducto
        -string _categoria
        +PrecioUnitario decimal
        +IDProducto string
        +NombreProducto string
        +Categoria string
        +NumeroVentas int
    }

    Pedido "*" o-- "*" PedidoItem
    PedidoItem --> "1" Producto
    Cliente "1" o-- "*" Pedido

    %% Excepciones Personalizadas
    class ClienteInvalidoException
    class ProductoInvalidoException
    class PedidoInvalidoException
    class ProcesamientoPipelineException

    ClienteInvalidoException --|> ArgumentException
    ProductoInvalidoException --|> ArgumentException
    PedidoInvalidoException --|> ArgumentException
    ProcesamientoPipelineException --|> Exception

    %% Interfaces e I/O
    class IImportarDatos {
        <<interface>>
        +LeerClientes(string) List~ClienteDTO~
        +LeerPedidos(string) List~PedidoItemDTO~
    }
    class IExportarDatos {
        <<interface>>
        +EscribirReporteProductos(List~ReporteProductoDTO~, string)
        +EscribirReporteClientes(List~ReporteClienteDTO~, string)
    }

    class LectorCsv {
        +LeerClientes(string) List~ClienteDTO~
        +LeerPedidos(string) List~PedidoItemDTO~
    }
    class LectorJson {
        +LeerClientes(string) List~ClienteDTO~
        +LeerPedidos(string) List~PedidoItemDTO~
    }
    class EscritorJson {
        +EscribirReporteProductos(List~ReporteProductoDTO~, string)
        +EscribirReporteClientes(List~ReporteClienteDTO~, string)
    }
    class EscritorXml {
        +EscribirReporteProductos(List~ReporteProductoDTO~, string)
        +EscribirReporteClientes(List~ReporteClienteDTO~, string)
    }

    IImportarDatos <|.. LectorCsv
    IImportarDatos <|.. LectorJson
    IExportarDatos <|.. EscritorJson
    IExportarDatos <|.. EscritorXml

    %% Factories y Servicios
    class LectorFactory {
        +ObtenerLector(string) IImportarDatos$
    }
    class EscritorFactory {
        +ObtenerEscritor(string) IExportarDatos$
    }
    class PipelineProcessor {
        +Ejecutar(string, string, string, string, string, string, string) void
        -MapearCliente(ClienteDTO, Dictionary) Cliente
        -MapearPedidoCabecera(string, PedidoItemDTO) Pedido
        -MapearPedidoItem(string, PedidoItemDTO, Dictionary) PedidoItem
    }

    PipelineProcessor ..> LectorFactory : Usa
    PipelineProcessor ..> EscritorFactory : Usa
    PipelineProcessor ..> IImportarDatos : Usa
    PipelineProcessor ..> IExportarDatos : Usa

    %% Capa DTO
    class ClienteDTO
    class PedidoItemDTO
    class PedidoReporteDTO
    class ReporteClienteDTO
    class ReporteProductoDTO

    PipelineProcessor ..> ClienteDTO : Mapea de
    PipelineProcessor ..> PedidoItemDTO : Mapea de
    PipelineProcessor ..> ReporteClienteDTO : Exporta
    PipelineProcessor ..> ReporteProductoDTO : Exporta
```

---

## 2. Diagrama de Secuencia — Generación de Reportes

```mermaid
sequenceDiagram
    autonumber
    actor Usuario
    participant Program
    participant Pipeline as PipelineProcessor
    participant LF as LectorFactory
    participant Lector as IImportarDatos
    participant Dominio as Dominio (Modelo)
    participant EF as EscritorFactory
    participant Escritor as IExportarDatos
    participant FS as Sistema de Archivos

    Usuario ->> Program: Ingresa rutas y formatos
    Program ->> Pipeline: Ejecutar(rutas, formatos)
    
    %% Fase de Carga
    rect rgb(240, 248, 255)
        note right of Pipeline: Fase 1: Carga de Datos Crudos
        Pipeline ->> LF: ObtenerLector(formatoClientes)
        LF -->> Pipeline: Instancia de Lector (Csv/Json)
        Pipeline ->> Lector: LeerClientes(rutaClientes)
        Lector -->> Pipeline: List<ClienteDTO>
        Pipeline ->> LF: ObtenerLector(formatoPedidos)
        LF -->> Pipeline: Instancia de Lector (Csv/Json)
        Pipeline ->> Lector: LeerPedidos(rutaPedidos)
        Lector -->> Pipeline: List<PedidoItemDTO>
    end

    %% Fase de Mapeo y Validación
    rect rgb(255, 250, 240)
        note right of Pipeline: Fase 2: Validación y Mapeo de Clientes
        loop Por cada ClienteDTO
            Pipeline ->> Pipeline: MapearCliente(dto, clientesMap)
            alt Datos Inválidos o Duplicados
                Pipeline -->> Pipeline: Lanza ProcesamientoPipelineException
                note over Pipeline: Se registra advertencia en consola y se omite
            else Datos Válidos
                Pipeline ->> Dominio: Instancia ClienteNatural/Empresarial
                Dominio -->> Pipeline: Objeto Cliente
            end
        end
    end

    rect rgb(255, 240, 245)
        note right of Pipeline: Fase 3: Validación y Agrupación de Pedidos
        loop Por cada Grupo de Pedidos (IdPedido)
            Pipeline ->> Pipeline: MapearPedidoCabecera(id, primeraFila)
            alt Cabecera Inválida
                Pipeline -->> Pipeline: Lanza ProcesamientoPipelineException
                note over Pipeline: Se registra advertencia en consola y se omite
            else Cabecera Válida
                Pipeline ->> Dominio: Instancia PedidoNacional/Internacional
                Dominio -->> Pipeline: Objeto Pedido
            end
            
            loop Por cada Item de Pedido en el Grupo
                Pipeline ->> Pipeline: MapearPedidoItem(id, fila, productosCatalog)
                alt Item Inválido
                    Pipeline -->> Pipeline: Lanza ProcesamientoPipelineException
                    note over Pipeline: Se omite el ítem e incrementa advertencias
                else Item Válido
                    Pipeline ->> Dominio: Instancia PedidoItem y Producto
                    Dominio -->> Pipeline: Objeto PedidoItem
                    Pipeline ->> Dominio: Agregar item a Pedido.Items
                end
            end
            
            alt Pedido sin Items Válidos
                note over Pipeline: Se ignora el pedido
            else Pedido con Items Válidos
                Pipeline ->> Pipeline: Relacionar Pedido con Cliente (email)
                alt Cliente no existe
                    note over Pipeline: Se añade a pedidosHuerfanos
                else Cliente existe
                    Pipeline ->> Dominio: Agregar pedido a Cliente.Pedidos
                end
            end
        end
    end

    %% Fase de Reportes
    rect rgb(240, 255, 240)
        note right of Pipeline: Fase 4: Mapeo y Escritura de Reportes
        Pipeline ->> Pipeline: Mapea a ReporteProductoDTO y ReporteClienteDTO
        Pipeline ->> EF: ObtenerEscritor(formatoSalida)
        EF -->> Pipeline: Instancia de Escritor (Json/Xml)
        Pipeline ->> Escritor: EscribirReporteProductos(productos, ruta)
        Escritor ->> FS: Guardar archivo productos
        Pipeline ->> Escritor: EscribirReporteClientes(clientes, ruta)
        Escritor ->> FS: Guardar archivo clientes
    end

    Pipeline -->> Program: Completado
    Program -->> Usuario: Muestra resumen en consola
```
