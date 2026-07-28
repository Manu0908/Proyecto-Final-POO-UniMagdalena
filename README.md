# Pipeline de Análisis de Clientes y Compras — Proyecto Final POO

Aplicación de consola en C# (.NET) que procesa, limpia, relaciona y analiza datos transaccionales de un e-commerce a partir de archivos externos (CSV o JSON), generando reportes analíticos exportables en JSON o XML.

El desarrollo se fundamenta en los principios de la Programación Orientada a Objetos (POO) y la aplicación de los patrones de diseño **Strategy** y **Factory**.

---

## Stack Tecnológico

- **Lenguaje**: C# (.NET 10.0)
- **Paradigma**: POO — Herencia, Polimorfismo, Encapsulamiento, Abstracción
- **Serialización**: `System.Text.Json` (JSON) y `System.Xml.Serialization` (XML)
- **Control de Versiones**: Git y GitHub

---

## Estructura del Proyecto

```text
Proyecto Final POO/
│
├── Docs/
│   ├── 1_convenciones_y_git.md          # Convenciones de código C# y guía de Git
│   └── 2_arquitectura_y_requerimientos.md # Arquitectura, patrones, reglas de negocio y estado de implementación
│
├── Source/
│   ├── DTOs/
│   │   ├── ClienteDTO.cs               # DTO entrada: fila cruda del archivo de clientes
│   │   ├── PedidoItemDTO.cs            # DTO entrada: fila cruda del archivo de pedidos (un ítem)
│   │   ├── PedidoDTO.cs                # DTO reporte: pedido agrupado con ítems y totales (PedidoReporteDTO)
│   │   ├── ReporteClienteDTO.cs        # DTO reporte: cliente con totales y pedido más costoso
│   │   └── ReporteProductoDTO.cs       # DTO reporte: producto con total de ventas
│   │
│   ├── Interfaces/
│   │   ├── IImportarDatos.cs           # Contrato Strategy para leer archivos de entrada
│   │   └── IExportarDatos.cs           # Contrato Strategy para escribir reportes de salida
│   │
│   ├── Lectores/
│   │   ├── LectorCSV.cs                # Estrategia concreta: lectura de archivos CSV
│   │   └── LectorJSON.cs               # Estrategia concreta: lectura de archivos JSON (pendiente)
│   │
│   ├── Escritores/                      # Pendiente: EscritorJSON y EscritorXML
│   │
│   ├── Cliente.cs                       # Clase abstracta del dominio
│   ├── ClienteNatural.cs                # Hereda de Cliente — frecuente si > 5 compras
│   ├── ClienteEmpresarial.cs            # Hereda de Cliente — frecuente si > $50M acumulado
│   ├── Pedido.cs                        # Clase abstracta del dominio
│   ├── PedidoNacional.cs                # Hereda de Pedido — impuesto 19%
│   ├── PedidoInternacional.cs           # Hereda de Pedido — impuesto 30%
│   ├── PedidoItem.cs                    # Línea de un pedido con producto, cantidad y precio
│   ├── Producto.cs                      # Entidad de catálogo con número de ventas
│   └── Program.cs                       # Punto de entrada (en construcción)
│
├── Proyecto Final POO C#.csproj
└── Proyecto Final POO C#.slnx
```

---

## Reglas de Negocio Principales

| Regla | Detalle |
| :--- | :--- |
| **Pedido Nacional** | Impuesto del **19%** sobre el subtotal |
| **Pedido Internacional** | Impuesto del **30%** sobre el subtotal |
| **Cliente Natural frecuente** | Más de **5 compras** realizadas |
| **Cliente Empresarial frecuente** | Total acumulado > **$50.000.000 COP** |
| **Pedido huérfano** | Email de cliente no encontrado → se guarda en lista separada, no detiene el proceso |
| **Error de I/O** | Archivo no encontrado o sin permisos → **detiene** la ejecución |
| **Error de datos** | Fila corrupta o inválida → se registra en consola, **no detiene** el proceso |

---

## Patrones de Diseño Aplicados

### Strategy
Desacopla el pipeline del formato físico de los archivos:
- **`IImportarDatos`** → `LectorCsv`, `LectorJson`
- **`IExportarDatos`** → `EscritorJson`, `EscritorXml` *(pendientes)*

### Factory
Encapsula la instanciación de la estrategia correcta en tiempo de ejecución:
- **`LectorFactory`** *(pendiente)* — devuelve `IImportarDatos` según `"CSV"` o `"JSON"`
- **`EscritorFactory`** *(pendiente)* — devuelve `IExportarDatos` según `"JSON"` o `"XML"`

---

## Ejecución

```bash
dotnet run
```

El programa solicitará por consola la ruta del archivo de clientes, la ruta del archivo de pedidos, el formato de cada uno (CSV/JSON) y el formato del reporte de salida (JSON/XML).

---

## Integrantes y Declaración de IA

Este proyecto es desarrollado de manera colaborativa. Declaramos el uso de herramientas de Inteligencia Artificial para la estructuración de conceptos y organización de documentación. Las decisiones de diseño, arquitectura y negocio son tomadas y revisadas por los integrantes del equipo.
