# ***Creature Wars AR***

Documentación de la actividad 1 AR Foundation


# ***Documentación***

Creature Wars AR es un juego de combates de criaturas elementales en realidad aumentada. El juego combina las cartas físicas del jugador con monstruos virtuales.

## **Cómo jugar**
Al apuntar a una superficie (por ejemplo, una mesa), podremos colocar el tablero en el que, cuando inicie el combate, aparecerá el monstruo rival y, una vez puesto, la partida dará comienzo.

Al iniciar la partida, el jugador debe elegir una de sus cartas físicas y colocarla frente al tablero del enemigo. Al hacer eso aparecerá un monstruo de la carta seleccionada
Y encima del tablero el monstruo enemigo.

Si el tipo de monstruo del jugador gana al del rival, este gana la ronda y el juego continúa; si no, el juego se acaba. En caso de empate, la ronda se repite, es decir, no se pierde ni se avanza: se juega otra vez esa ronda con la carta que se quiera.

El objetivo del juego es llegar a superar el mayor número de rondas posibles.


## **Monstruos**
Hay 5 monstruos, uno por cada elemento.
| Monstruo | Tipo | 
| ------------- | ------------- |
| Cactoro  | Planta  |
| Tribal  |  Fuego  |
| Fish  | Agua  |
| Frog  | Eléctrico |
| Armabee  | Volador|

Cada tipo gana y pierde contra 2 tipos diferentes, como en piedra, papel, tijera, lagarto, spock; una versión más compleja del piedra, papel, tijera clásico.

<img width="600" height="600" alt="LogoApp" src="https://github.com/user-attachments/assets/27bdbe0d-f191-483b-bcb5-0a3afc62d975" />

## **Funciones AR utilizadas**
La app está desarrollada en Unity 6, la versión 6000.4.9ft, con el paquete AR Foundation y el Universal Render Pipeline. Las funcionalidades AR utilizadas son:

* **Plane tracing:** La aplicación busca y detecta superficies del mundo real; al hacer clic en la pantalla, se coloca el tablero enemigo sobre la superficie elegida.
* **Image tracking:** La aplicación reconoce cartas físicas; al detectar una, hace aparecer encima de ella el monstruo conectado a la carta.

### *Interfaz y extras*
#### *inicio app*
Al iniciar la app, saldrá una interfaz en mitad de la pantalla para comenzar o cerrar la app.

Al continuar, el jugador verá un pequeño tutorial de cómo funciona el juego, los tipos de monstruos y a qué tipos ganan.

#### *Durante el juego*
Durante el juego, el jugador verá por pantalla las rondas que lleva ganadas y un botón de información con la imagen de los tipos y contra quién ganan y pierden a modo de recordatorio. Además de música de fondo en bucle.
#### *Final del combate*
Al acabar el combate aparecerá el mensaje de victoria, derrota o empate. Un recordatorio de que retire la carta jugada y un botón para continuar jugando o reiniciar el juego en caso de derrota.
Además del mensaje, el jugador oirá diferentes efectos de sonido cuando gane o pierda.

## **Aspecto innovador**
El juego podría ser el típico juego de combate por rondas, pero al implementar cartas físicas, de las cuales aparecen monstruos virtuales, mezcladas con elementos virtuales como la zona de enemigos o las interfaces del juego, crea una mezcla original entre juegos de cartas físicos tcg y juegos de combate virtuales.

Además de eso, lleva el típico juego de piedra, papel o tijera, sistema típico triangular en el que uno gana a otro, pero pierde contra el tercero, a un nivel extra al añadir 2 tipos más, pasando a un sistema pentagonal.

## **Resumen del proceso de desarrollo**
El proyecto inició con la propuesta de uno de nosotros de hacer un juego de combates donde el jugador saque una carta que invoque un monstruo contra el monstruo para acabar con los enemigos que haya delante de él en el tablero. Con esa idea en mente se habló de qué tan factible era hacerla o cómo la haríamos en el tiempo que había, y a partir de ahí se desarrolló en distintas fases:

1- Colocación del tablero: Primeras versiones del tablero y pequeñas pruebas con AR
Foundation para comprobar la colocación de objetos sobre superficies.

2- Elección de monstruos y definición de los tipos que formarían la tabla de elementos.

3- Primera iteración loop jugable: primera versión del combate, qué tipos ganaban a cuáles.

4- Reconocimiento de cartas. Se empezó por reconocer una y luego se expandió a las 5 con sus 5 monstruos.

5- Mejora y finalización del loop de juego

6- Creación de interfaz

7- Pulido de bugs y sistemas

8- Sistema de audio y efectos de sonido

9- Versión final de tablero, sistema de combate, medida monstruos

10- Pruebas, pulido y arreglo de bugs.

### *Organización del equipo*
Sin duda, lo que más nos costó del trabajo fue la organización. Una vez se habló de la idea del juego, no se habló de cuándo empezar o cómo repartir el trabajo que ya se había empezado a hacer. Y aunque al darnos cuenta de que esto traería un problema, se habló de lo que estaba haciendo cada uno; algunos trabajos se pisaron o se hicieron 2 veces.

Aunque para la parte final sí conseguimos organizar un poco lo que estaba hecho, quién estaba haciendo qué, qué estaba a medias y qué faltaba por hacer.
## **Problemas encontrados y soluciones aplicadas**
Estos son algunos de los problemas que tuvimos y cómo los solucionamos.
### La carta física quedaba tapada por el tablero virtual
**Problema:** Se pensó en hacer un tablero completo con hueco para la carta real, pero vimos que daba ciertos problemas para reconocer bien la carta.

**Solución:** Para solucionarlo, decidimos cambiar el enfoque: en vez de que fuera un tablero entero, que solo fuera la zona del enemigo, como una zona decorada en la que aparecen.
### El escáner de cartas no funcionaba al tener la detección de superficies activado
**Problema:** Nos dimos cuenta de que al tener los dos sistemas juntos, no funcionaban bien porque en vez de identificar la carta, la veía como una superficie.

**Solución:** Hacer que, una vez que se coloque el tablero, se desactive el buscar superficies y se active el reconocer cartas.
### Diferentes ramas y errores de sistemas
**Problema:** Al trabajar a la vez, hubo momentos en los que, al subir un sistema funcional, se rompía otro, y para poder avanzar, mientras se arreglaba algo, se trabajaba en otra rama, lo cual daba problemas luego al juntar los sistemas de diferentes ramas.

**Solución:** Pasar los sistemas a mano de una rama a otra y, en cuanto a los errores, mirar que todo siga funcionando, no solo el nuevo sistema añadido.

### Coordinación del equipo
**Problema:** La comunicación y el hacer el trabajo dos veces fueron el problema más difícil y complicado.

**Solución:** Para las próximas entregas, organizar antes qué se va a hacer, tener todas las conversaciones por el grupo, avisar de que se está haciendo y preguntar si alguien estaba haciendo algo de eso antes de empezar a hacerlo o continuarlo.

## **Contribución de cada miembro del equipo**
| **Miembro** | **Tareas realizadas** | 
| ------------- | ------------- |
| *Arnau Pascual*  | - Música y efectos de sonido: selección de los audios e integración en el juego (música en bucle y sonidos de victoria y derrota) <br> - Primeras versiones del tablero y pruebas iniciales, incluida la zona transparente para ver la carta real. <br> - Sistema de combate y gestión de rondas, junto con Jana (participación secundaria). <br> - Colocación del tablero u objeto al tocar la pantalla y adaptación del menú de objetos con su imagen, junto con Jana <br> - Inicio Documentación |
| *Jana Puig* | - Núcleo principal del juego: sistema de combate por tipos y gestión de rondas(responsable principal). <br> - Colocación del tablero u objeto al tocar la pantalla y adaptación del menú de objetos con su imagen, junto con Arnau. <br> Primera implementación mensaje de victoria/derrota por pantalla  |
| *Claudia Ruiz*  | - Buscar assets enemigos junto a Víctor <br> - Crear tabla de tipos junto a Víctor <br> - Ampliación a 5 cartas y 5 monstruos diferentes del escáner de cartas <br>Creación del tablero enemigo final <br> - Corrección de bugs de combate <br> - Unificación de las diferentes ramas del proyecto <br> -  Corrección de errores al unir diferentes ramas <br>- Activar animaciones de los monstruos al aparecer <br> - Ajustar el tamaño de los monstruos para su visibilidad <br> - Ajustar la posición del tablero <br> - Crear y ajustar posición aparición enemigo al tablero enemigo <br> Botón pasar siguiente ronda y reiniciar partida |
| *Víctor González*  |- Idea principal del juego <br>- Buscar assets enemigos junto a Claudia <br> - Crear tabla de tipos junto a Claudia<br>- Primera iteración escáner de cartas: reconocimiento de las cartas físicas <br> - Aparición del monstruo sobre la carta escaneada <br> - Tutorial para explicar al jugador como jugar <br> - Toda la interfaz de usuario: pantalla de inicio, tutoriales partida, contador de rondas, modificaciones mensajes  fin de partida y boton pasar ronda / reinicio <br>- Documentación <br> - Readme |
## Authors

- ***Víctor González*** [@TheWolfG145](https://www.github.com/TheWolfG145)
- ***Jana Puig*** [@JanaPuig](https://www.github.com/JanaPuig)
- ***Claudia Ruiz*** [@Claurm12](https://www.github.com/Claurm12)
- ***Arnau Pascual*** [@Pascra](https://www.github.com/Pascra)

