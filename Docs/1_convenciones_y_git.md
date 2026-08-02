# Convenciones de Código, Estilo C# y Guía de Git

Para garantizar la legibilidad y cohesión del código escrito por los tres integrantes del equipo, se siguen las convenciones oficiales de Microsoft y las directrices internas definidas para el proyecto.

---

## 1. Convenciones de Nomenclatura

| Convención | Uso o Destino | Ejemplo |
| :--- | :--- | :--- |
| **PascalCase** | Clases, Interfaces, Métodos, Propiedades públicas, Constantes, Namespaces | `ClienteEmpresarial`, `CalcularTotal()`, `PrecioUnitario`, `IExportarDatos` |
| **camelCase** | Parámetros de métodos, Variables locales | `rutaArchivo`, `pedidoId`, `comprasRealizadas` |
| **_camelCase** | Campos privados de una clase | `_clientes`, `_fecha`, `_exportador` |
| **snake_case** | Columnas o claves en archivos CSV/JSON de entrada | `id_cliente`, `tipo_cliente`, `email_cliente` |

### Reglas Adicionales

1. **Nombres descriptivos**: Evitar abreviaturas confusas (`decimal totalConImpuesto`, no `decimal tci`).
2. **Interfaces con "I"**: Toda interfaz empieza con `I` seguido de PascalCase (`IImportarDatos`, `IExportarDatos`).
3. **Clases en singular**: Entidades del dominio en singular (`Cliente`, `Pedido`, no `Clientes`, `Pedidos`).
4. **Sufijo DTO**: Todo Data Transfer Object lleva el sufijo `DTO` (`ClienteDTO`, `PedidoItemDTO`, `ReporteClienteDTO`).

---

## 2. Convenciones de Sintaxis y Estilo

### A. Uso de Llaves — Estilo Allman

En C#, las llaves de apertura `{` y cierre `}` siempre van en su propia línea, alineadas con la declaración que las abre:

```csharp
// ✅ Correcto (Allman)
public class Cliente
{
    public void Guardar()
    {
        if (esValido)
        {
            // Lógica
        }
    }
}

// ❌ Incorrecto (Egyptian / K&R)
public class Cliente {
    public void Guardar() {
```

### B. Una Clase por Archivo

Cada clase, interfaz o enum vive en su propio archivo `.cs` con el mismo nombre de la entidad:
`Cliente.cs`, `IExportarDatos.cs`, `ReporteClienteDTO.cs`.

### C. Propiedades Auto-Implementadas vs. Campos Privados

```csharp
// Para propiedades simples sin lógica adicional:
public string Ciudad { get; set; }

// Para propiedades que necesitan validación en el setter:
private string _email;
public string Email
{
    get { return _email; }
    set
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("El email no puede estar vacío.");
        }
        _email = value;
    }
}
```

### D. Nullabilidad Intencional en DTOs

- **DTOs de entrada** (`ClienteDTO`, `PedidoItemDTO`): todos los campos llevan `?` porque reflejan datos crudos aún sin validar.
- **DTOs de reporte** (`ReporteClienteDTO`, `ReporteProductoDTO`, `PedidoReporteDTO`): los campos **no** llevan `?` porque los datos ya fueron validados y procesados. Solo lleva `?` el campo `PedidoMasCostoso` en `ReporteClienteDTO`, porque un cliente puede no tener pedidos válidos (tercer estado real).

---

## 3. Manejo de Errores — Regla Clave

El proyecto distingue dos categorías de errores con comportamiento opuesto:

| Tipo de error | Ejemplo | Comportamiento | Implementación |
| :--- | :--- | :--- | :--- |
| **Técnico / I/O** | Archivo no encontrado, sin permisos | **Detiene** la ejecución | Se propaga (`throw`) hacia `Program.cs` |
| **De dominio / datos** | Fila con columnas faltantes, email inválido, pedido huérfano | **No detiene** la ejecución | `try/catch` **dentro** del `foreach`, no alrededor |

```csharp
// ✅ Correcto: error de dominio capturado por fila
foreach (string linea in File.ReadLines(ruta).Skip(1))
{
    try
    {
        // Parseo y validación de la fila
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[ADVERTENCIA] Fila ignorada: {ex.Message}");
        // El foreach continúa con la siguiente línea
    }
}

// ❌ Incorrecto: el try/catch alrededor del foreach detiene todo si falla una fila
try
{
    foreach (string linea in File.ReadLines(ruta).Skip(1))
    {
        // ...
    }
}
catch (Exception ex) { /* Para todo el proceso */ }
```

> **Pedidos huérfanos** no son un error: son un caso de negocio. Se tratan con un `if`, no con una excepción.

---

## 4. Guía de Trabajo con Git y GitHub

### Flujo Estándar (GitHub Flow)

```
main  ←─── siempre con código que funciona
  │
  └─── feature/lector-csv        ← trabajo del integrante
         │
         └─── commits pequeños y descriptivos
                │
                └─── Pull Request → revisión → Merge a main
```

### Comandos Esenciales

```bash
# Antes de empezar a trabajar: actualizar main
git checkout main
git pull origin main

# Crear rama de trabajo (nunca programar directamente en main)
git checkout -b feature/nombre-de-tarea

# Guardar progreso en commits pequeños y frecuentes
git status
git add NombreArchivo.cs
git commit -m "feat(lectores): implementar LeerClientes en LectorCSV"

# Subir la rama a GitHub
git push -u origin feature/nombre-de-tarea
```

### Prefijos de Commits (Conventional Commits)

| Prefijo | Cuándo usarlo | Ejemplo |
| :--- | :--- | :--- |
| `feat` | Nueva funcionalidad | `feat(dtos): agregar PedidoReporteDTO` |
| `fix` | Corrección de bug | `fix(lectores): corregir parseo de fecha en LectorCSV` |
| `docs` | Solo documentación | `docs: actualizar convenciones y arquitectura` |
| `refactor` | Mejora sin cambio de comportamiento | `refactor(cliente): extraer validación de email` |
| `style` | Formato, indentación, llaves | `style: aplicar Allman style en clases de dominio` |
| `chore` | Limpieza, usings, renombres menores | `chore: eliminar usings innecesarios en LectorCSV` |

### Tipos de Ramas

- `feature/`: nuevas funcionalidades (`feature/lector-json`, `feature/exportador-xml`)
- `fix/`: correcciones de bug (`fix/namespace-cliente-empresarial`)
- `docs/`: cambios solo de documentación (`docs/actualizar-readme`)

### Resolución de Conflictos

Si Git marca un conflicto al hacer `pull` o fusionar:
```
<<<<<<< HEAD
// Tu código local
=======
// Código del compañero en GitHub
>>>>>>> branch-compañero
```
Habla con tu compañero, decide cuál es el correcto, borra las marcas, guarda y haz commit:
```bash
git add ArchivoCorregido.cs
git commit -m "fix: resolver conflicto en cálculo de total de pedido"
git push
```
