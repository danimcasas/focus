# Focus — copy de la landing

**Versión del borrador:** 0.3  
**Estado:** preparado en local; no publicado.  
**Producto que presenta:** Focus para Windows 11, en dos formatos.

## Mensaje central

**Tu tiempo, a tu ritmo.**  
Un temporizador sencillo para elegir un bloque de concentración, estudio o descanso y controlar la sesión desde una sola pantalla.

## Hero

**Eyebrow**  
App de escritorio para Windows 11

**Titular**  
Haz una cosa cada vez.

**Texto**  
Elige un bloque de concentración, estudio o descanso. Focus muestra el tiempo y te permite iniciar, pausar o reiniciar cuando lo necesites.

**CTA principal**  
Ver paquete en Google Drive  
https://drive.google.com/file/d/1g9_8SY8WLC7rnRP1Fu17lWxnuFvL3HFC/view?usp=drivesdk

**CTA secundario**  
Ver las dos versiones

**Microcopy**  
Aplicación nativa · Requiere .NET Framework 4.x.  
Acceso restringido; descarga pública pendiente.

## Los tres modos

**Concentración · 25 minutos**  
Un bloque breve para dedicarte a una tarea concreta.

**Estudio · 50 minutos**  
Un intervalo largo para leer, practicar o avanzar en una materia.

**Descanso · 10 minutos**  
Una pausa corta antes de elegir el siguiente bloque.

## Cómo se usa

1. **Elige un modo.** Concentración, estudio o descanso. Focus carga la duración correspondiente.
2. **Inicia o pausa.** Empieza cuando estés listo. Puedes pausar y reanudar con el tiempo restante.
3. **Decide el siguiente paso.** Al llegar a cero, la sesión termina. Tú eliges si descansas, cambias de modo o reinicias.

## Elige cómo abrir Focus

**App de escritorio · EXE 1.0.1**  
El paquete está en Google Drive: [Ver paquete en Google Drive](https://drive.google.com/file/d/1g9_8SY8WLC7rnRP1Fu17lWxnuFvL3HFC/view?usp=drivesdk). El acceso está restringido y la descarga pública sigue pendiente. Extrae el ZIP y abre Focus.exe. Funciona sin servidor y no realiza conexiones de red. Requiere .NET Framework 4.x; el paquete no incluye el runtime ni un instalador. El ZIP también incluye Focus.cs como fuente.

**Temporizador portable · HTML 1.0.0**  
Extrae el ZIP y abre index.html con el navegador predeterminado de Windows. No requiere instalación ni conexión a internet. La sesión no se guarda al cerrar o recargar.

## Preguntas frecuentes

**¿Los modos cambian automáticamente?**  
No. Al llegar a cero, el temporizador se detiene. Elige manualmente si quieres iniciar otro modo.

**¿Puedo ajustar la duración?**  
Esta versión incluye tres tiempos fijos: 25 minutos para Concentración, 50 para Estudio y 10 para Descanso.

**¿Focus guarda mi sesión?**  
No. Al cerrar la app o la página, la sesión termina. Cuando vuelvas a abrir Focus, empieza en Concentración, con 25 minutos.

**¿Qué necesita la versión de escritorio?**  
La versión EXE requiere .NET Framework 4.x. El ZIP no incluye ese runtime ni un instalador.

**¿El ejecutable está firmado?**  
No tiene firma Authenticode. El paquete ZIP incluye un archivo SHA-256 para comprobar su integridad.

## SEO

**Title**  
Focus · Temporizador para Windows 11

**Meta description**  
Focus es un temporizador para Windows 11 con modos de concentración, estudio y descanso. Elige entre la app de escritorio o la versión HTML portable.

## Criterios de contenido y publicación

- El CTA EXE apunta al paquete de Google Drive, con acceso restringido; no afirma que la descarga pública esté disponible. La alternativa HTML conserva su ruta relativa al ZIP local.
- La subida comunicada indica que el SHA-256 local del ZIP EXE coincide con el sidecar de Drive. El tamaño informado es 11.213 bytes y el ZIP incluye Focus.exe y documentación.
- El QA de la versión EXE informa que no se completó una revisión visual interactiva. Esta landing también queda pendiente de revisión visual porque el entorno bloqueó el acceso al servidor de vista previa local.
- No se añaden precio, licencia, testimonios, cifras de productividad, personalización del tiempo ni cambios automáticos de modo: la carpeta no documenta esas capacidades.
