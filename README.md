# Pipeline de Análisis de Clientes y Compras — Proyecto Final POO

Aplicación de consola en C# (.NET) que procesa, limpia, relaciona y analiza datos transaccionales de un e-commerce a partir de archivos externos (CSV o JSON), generando reportes analíticos exportables en JSON o XML.

El desarrollo se fundamenta en los principios de la Programación Orientada a Objetos (POO) y la aplicación de los patrones de diseño **Strategy** y **Factory**.

---

## Stack Tecnológico

- **Lenguaje**: C# (.NET 10.0)
- **Paradigma**: POO — Herencia, Polimorfismo, Encapsulamiento, Abstracción
- **Serialización**: `System.Text.Json` (JSON) y `System.Xml.Serialization` (XML)
- **Pruebas**: xUnit
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
├── MockData/
│   ├── clientes.csv                     # Archivo de clientes de prueba (CSV)
│   ├── pedidos.json                     # Archivo de pedidos de prueba (JSON)
│   ├── reporte_clientes.xml             # Reporte de clientes generado (XML)
│   └── reporte_productos.xml            # Reporte de productos generado (XML)
│
├── Proyecto_Final_POO_C_.Tests/
│   ├── Proyecto_Final_POO_C_.Tests.csproj
│   └── PruebasDominio.cs               # 5 pruebas unitarias xUnit (dominio y cálculos)
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
│   │   ├── LectorJSON.cs               # Estrategia concreta: lectura de archivos JSON
│   │   └── LectorFactory.cs            # Factory: retorna LectorCsv o LectorJson según formato
│   │
│   ├── Escritores/
│   │   ├── EscritorJson.cs             # Estrategia concreta: escritura de reportes en JSON
│   │   ├── EscritorXml.cs              # Estrategia concreta: escritura de reportes en XML
│   │   └── EscritorFactory.cs          # Factory: retorna EscritorJson o EscritorXml según formato
│   │
│   ├── Modelo/
│   │   ├── Cliente.cs                  # Clase abstracta del dominio
│   │   ├── ClienteNatural.cs           # Hereda de Cliente — frecuente si > 5 compras
│   │   ├── ClienteEmpresarial.cs       # Hereda de Cliente — frecuente si > $50M acumulado
│   │   ├── Pedido.cs                   # Clase abstracta del dominio
│   │   ├── PedidoNacional.cs           # Hereda de Pedido — impuesto 19%
│   │   ├── PedidoInternacional.cs      # Hereda de Pedido — impuesto 30%
│   │   ├── PedidoItem.cs               # Línea de un pedido con producto, cantidad y precio
│   │   ├── Producto.cs                 # Entidad de catálogo con número de ventas
│   │   └── Excepciones.cs              # Excepciones de dominio y pipeline personalizadas
│   │
│   ├── PipelineProcessor.cs            # Orquestador: carga → valida → relaciona → exporta
│   └── Program.cs                      # Punto de entrada y menú interactivo de consola
│
├── Proyecto Final POO C#.csproj
└── Proyecto Final POO C#.slnx
```

---

## Manejo de Errores y Excepciones Personalizadas

El sistema utiliza excepciones personalizadas para el control y validación de datos en la capa de dominio y el mapeo del pipeline:

- **`ClienteInvalidoException`**: Se lanza al detectar datos obligatorios vacíos o formatos de email incorrectos en un cliente.
- **`ProductoInvalidoException`**: Se lanza ante productos con precios o datos base inválidos.
- **`PedidoInvalidoException`**: Se lanza al validar la consistencia de los pedidos, precios o cantidades menores o iguales a cero.
- **`ProcesamientoPipelineException`**: Se utiliza durante el mapeo de DTOs a entidades de dominio y el flujo del pipeline para reportar y omitir registros corruptos sin detener la aplicación.

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
| **Error de datos / Dominio** | Datos inconsistentes detectados por excepciones personalizadas → se capturan en el pipeline y se omiten los registros, **no detiene** el proceso |


---

## Patrones de Diseño Aplicados

### Strategy
Desacopla el pipeline del formato físico de los archivos:
- **`IImportarDatos`** → `LectorCsv`, `LectorJson`
- **`IExportarDatos`** → `EscritorJson`, `EscritorXml`

### Factory
Encapsula la instanciación de la estrategia correcta en tiempo de ejecución:
- **`LectorFactory`** — devuelve `IImportarDatos` según `"CSV"` o `"JSON"`
- **`EscritorFactory`** — devuelve `IExportarDatos` según `"JSON"` o `"XML"`

---

## Ejecución

```bash
dotnet run
```

El programa solicita por consola:
1. Ruta del archivo de clientes
2. Formato del archivo de clientes (`CSV` o `JSON`)
3. Ruta del archivo de compras (pedidos)
4. Formato del archivo de compras (`CSV` o `JSON`)
5. Formato de salida de los reportes (`JSON` o `XML`)
6. Carpeta de destino para los archivos generados

Al finalizar, genera:
- `reporte_productos.<ext>` — listado de productos con total de ventas
- `reporte_clientes.<ext>` — listado de clientes con totales y pedido más costoso

Y muestra en consola un **resumen general**: ventas totales, pedidos nacionales vs. internacionales, clientes naturales vs. empresariales.

---

## Pruebas Unitarias

```bash
dotnet test
```

El proyecto `Proyecto_Final_POO_C_.Tests` contiene 5 pruebas unitarias con xUnit que cubren:
- Validación de email inválido en el dominio
- Cálculo de impuesto del 19% para pedido nacional
- Cálculo de impuesto del 30% para pedido internacional
- Comportamiento intercambiable: frecuencia en `ClienteNatural`
- Comportamiento intercambiable: frecuencia en `ClienteEmpresarial`

---

## Integrantes y Declaración de IA

Este proyecto es desarrollado de manera colaborativa. Declaramos el uso de herramientas de Inteligencia Artificial para la estructuración de conceptos y organización de documentación. Las decisiones de diseño, arquitectura y negocio son tomadas y revisadas por los integrantes del equipo.

