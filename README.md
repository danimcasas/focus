# Focus

Temporizador de escritorio para Windows: Concentración (25 minutos), Estudio
(50 minutos) y Descanso (10 minutos), con inicio, pausa, reanudación y reinicio.

Creadores: **danimcasas.com curso codex 2026**.

## Descargar y usar

La [versión 1.0.2](https://github.com/danimcasas/focus/releases/tag/v1.0.2)
está disponible en GitHub Releases. Extrae el paquete
`Focus-Windows11-EXE-X.Y.Z.zip` y abre `Focus.exe`. El ejecutable requiere
.NET Framework 4.x y no tiene firma digital. El paquete no instala un runtime.
Comprueba el archivo `.sha256` para verificar el ZIP.

Focus funciona sin conexión. El progreso se pierde al cerrar la app. También
se incluye una alternativa web local en `web/index.html` dentro del paquete.

## Estructura

- `Focus/src/Focus.cs`: fuente de escritorio WinForms.
- `Focus/index.html`: fuente web, con CSS y JavaScript integrados.
- `Focus/VERSION`: número de la versión actual en formato `X.Y.Z`.
- `Focus/CHANGELOG.md`: cambios de cada versión.
- `Focus/scripts/`: compilación, pruebas y empaquetado.
- `Focus/tests/`: comprobaciones funcionales y renderizado del pie.
- `Focus/landing/`: landing local; editarla no publica un sitio web.
- `Focus/release/` y `Focus/.build/`: artefactos locales excluidos de Git.
- `hola.html`: ejercicio inicial independiente, en catalán.

## Compilar y comprobar

Usa Windows y PowerShell 7. El proceso utiliza el compilador de .NET Framework
4.x instalado en Windows, sin descargar dependencias.

```powershell
& .\Focus\scripts\Build.ps1 -Test
& .\Focus\scripts\Package.ps1
```

El segundo comando compila, ejecuta las pruebas y crea el ZIP, su suma SHA-256
y el manifiesto de archivos. Se detiene si la entrega ya existe. Las imágenes
de revisión visual se generan en una subcarpeta de `Focus/.build/X.Y.Z/` por cada
compilación. Así se evita sobrescribir un ejecutable que esté en uso.

## Versiones y publicación

1. Trabaja en una rama `codex/descripcion-del-cambio` y conserva commits concretos.
2. Incrementa `Focus/VERSION`: parche para correcciones, menor para funciones
   compatibles y mayor para cambios incompatibles. Actualiza la versión visible
   de la web y el historial. La versión del EXE se genera desde `VERSION`.
3. Compila, prueba y revisa visualmente la interfaz y el paquete final.
4. Integra en `main`, crea la etiqueta `vX.Y.Z` sobre el commit validado y sube
   la rama y la etiqueta a GitHub.
5. Crea la GitHub Release de esa etiqueta con el ZIP y su `.sha256`. Verifica
   los archivos remotos antes de enlazar la descarga desde la landing.

Los fuentes y las instrucciones se guardan en Git; los ejecutables se publican
como archivos de una Release. Publicar la app y publicar la landing son acciones
distintas. Las versiones anteriores se conservan.
