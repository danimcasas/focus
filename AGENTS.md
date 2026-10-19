# Instrucciones del proyecto

## Alcance y materiales

Este directorio reúne ejercicios locales y el temporizador **Focus**. Trabaja en español, salvo que una tarea pida otro idioma. Antes de modificar algo, identifica la variante afectada y comprueba los archivos y el estado actuales.

- `hola.html`: prueba mínima en catalán; es independiente de Focus. Conserva su idioma y su alcance salvo petición expresa.
- `Focus/index.html`: fuente editable de la versión web. Contiene HTML, CSS y JavaScript en un solo archivo, sin dependencias externas.
- `Focus/release/Focus-Windows11-1.0.0.zip` y su carpeta: entrega histórica de la versión web portable. `LEEME-Windows11.txt`, `VERSION.txt`, `SHA256SUMS.txt` y el archivo `.zip.sha256` documentan esa entrega.
- `Focus/src/Focus.cs`: fuente editable de la aplicación de escritorio WinForms. `Focus/VERSION` centraliza el número actual; `Focus/CHANGELOG.md` registra sus cambios. Las carpetas versionadas de `Focus/release/` contienen entregas históricas y artefactos generados.
- `Focus/release/QA-Windows11.txt`: evidencia de pruebas del paquete web 1.0.0 y de una revisión visual pendiente en aquel momento. `Focus/release/.qa-run-*/FocusSmokeTest.cs` es un test de la variante nativa; su mera presencia no acredita una ejecución correcta. `Focus-Windows11.lnk` es un acceso directo local, no un artefacto portable.

La carpeta tiene un repositorio Git. La compilación y el empaquetado están en `Focus/scripts/`; consulta `README.md`. El destino autorizado es el repositorio público `danimcasas/focus` en GitHub. Comprueba otra vez este inventario si la estructura cambia.

## Edición y versiones

Toda entrega de una nueva versión de Focus debe incluir una aplicación de escritorio para Windows con un ejecutable real `.exe`, compilado y probado. La firma digital es opcional. La versión web puede acompañar la entrega; el HTML, un acceso directo o un ZIP sin ejecutable no cumplen por sí solos este requisito. Conserva el código fuente correspondiente y documenta los requisitos de ejecución.

Cada entrega debe tener una versión numérica `X.Y.Z`, registrada en `Focus/VERSION`, en los metadatos y el pie del EXE, en la web incluida y en la documentación. Incrementa parche para correcciones, menor para funciones compatibles y mayor para cambios incompatibles. El pie de la app debe mostrar literalmente **danimcasas.com curso codex 2026**, además de la versión; conserva este crédito en ambas variantes.

1. Para la web, edita `Focus/index.html`; para escritorio, `Focus/src/Focus.cs`. No alteres entregas anteriores ni sus hashes para presentar cambios nuevos.
2. Mantén separadas las variantes HTML y EXE. Comparten los modos y su comportamiento básico, pero cada una tiene código, empaquetado y validación propios. Si una petición afecta a ambas, revisa ambas expresamente; no presupongas que un cambio en una actualiza la otra.
3. Conserva las funciones observadas salvo petición contraria: Concentración 25 minutos, Estudio 50, Descanso 10; iniciar o reanudar, pausar conservando el tiempo, reiniciar el modo seleccionado, detenerse al llegar a cero y cambiar de modo reiniciando la sesión. La web no conserva el progreso tras recargar.
4. Mantén la interfaz y los textos en español de Focus y el enfoque local y sin conexión. No añadas servicios, cuentas, telemetría, instalaciones ni dependencias sin necesidad justificada y autorización cuando impliquen coste, cambios globales o acciones externas.
5. Preserva los originales. Si preparas una entrega, usa un nombre y una versión nuevos, sincroniza la versión del código y la documentación, y genera hashes SHA-256 de los archivos finales. Distingue fuente, copia empaquetada, ejecutable y archivo comprimido.

## Validación según el cambio

- Ejecuta `& .\Focus\scripts\Build.ps1 -Test` en Windows con PowerShell 7. Revisa las imágenes generadas y el crédito a tamaño normal y mínimo. `& .\Focus\scripts\Package.ps1` compila, prueba y genera una entrega nueva con manifiesto y ZIP; rechaza sobrescribir versiones existentes.
- Comprueba las tres duraciones, los estados de los botones y los flujos de inicio, pausa, reanudación, reinicio, cambio de modo y finalización. Para temporización, verifica que la pausa conserva el tiempo y que el reloj calcula el restante a partir de una marca de tiempo, también tras suspensiones o retrasos de la interfaz.
- En la web, revisa consola y funcionamiento en un navegador real, teclado y foco visible, accesibilidad básica y tamaños de pantalla estrechos. Valida el HTML/JavaScript y el contenido empaquetado. Si la política del entorno impide abrir `file://` o una vista previa, indícalo como revisión visual pendiente; no sortees el bloqueo.
- En la versión WinForms, compila y prueba en Windows con las herramientas disponibles; revisa controles, redimensionado, escala DPI, teclado, texto accesible y cierre de la aplicación. El test C# existente puede orientar la prueba funcional, pero verifica su resultado en la versión que entregas.
- Antes de dar por lista una distribución, abre o inspecciona el artefacto final, comprueba que contiene los archivos previstos y contrasta sus hashes con el manifiesto. Distingue compilación, prueba automatizada, revisión visual y aceptación del usuario.

## Forma de trabajar y cierre

Usa Git desde la raíz: comprueba el estado, trabaja en una rama `codex/descripcion`, guarda commits claros e integra los cambios validados en `main`. Etiqueta cada entrega como `vX.Y.Z`. Sube los fuentes y etiquetas al repositorio autorizado, y publica el ZIP con su `.sha256` en una GitHub Release cuando la tarea incluya el despliegue. Excluye credenciales, compilaciones temporales y ejecutables del historial de fuentes. Comprueba el commit remoto y los archivos de la Release; actualiza la landing sólo con enlaces de descarga verificados. Publicar la landing requiere su propio alcance autorizado.

Busca primero en la variante y versión afectadas. Usa PowerShell y rutas literales en Windows; comprueba destinos antes de mover o borrar. Evita instalaciones y cambios destructivos innecesarios. Resuelve las decisiones rutinarias y solicita datos sólo cuando alteren el resultado o requieran autorización. No publiques, distribuyas ni ejecutes acciones externas por el hecho de haber creado un paquete.

Al terminar, indica qué archivos cambiaste, dónde está la entrega, qué comprobaciones pasaron y qué queda pendiente. No conviertas informes antiguos, archivos de test o hashes de transferencia en pruebas de una versión nueva.
