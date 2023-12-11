# Proyecto de Servicio Social [TUTORIAL Mochilas Por Favor]

## Introducción
El juego Mochilas Por Favor se basa en la mecánica básica del popular juego Papers Please. El jugador buscará entre los artículos de diversas personas que quieren tomar el transporte público del trolebús y de acuerdo a dos listas de objetos requeridos y baneados, discernirá si debe dejar pasar al pasajero o no. Este juego busca fortalecer las habilidades de reconocimiento de elementos.

## Dinámica de Juego
El ciclo de juego comienza presentando al pasajero con una imagen de una persona y los objetos que trae consigo. El jugador juzgará si el pasajero puede pasar o no de acuerdo a las listas de objetos requeridos y baneados, dependiendo de su respuesta, se contará como un acierto cuando permita el paso a personas que tienen los objetos requeridos y ningún objeto baneado o no dejar pasar a aquellos que no tienen los objetos requeridos y/o portan un objeto baneado. De otra forma, se considerará un error.

Desde el punto de vista técnico, el jugador tiene tres fuentes de información principales a las cuales considerar, la lista de objetos requeridos que deberá buscar en la mochila del pasajero, los objetos baneados que también deberá identificar y la mochila del pasajero con los objetos a ser juzgados.

## Consideraciones
Para que un pasajero pueda pasar, debe:
* Portar con TODOS los objetos requeridos.
* No tener ningún objeto baneado.
Cualquiera de estos puntos que no se cumpla, no se debe dejar pasar.

El juego cuenta con un sistema dinámico de niveles, en el que tras una racha de victorias, el jugador subirá de nivel, aumentando el reto. De igual forma, si acumula una racha de derrotas, bajará de nivel a un reto más sencillo.

El juego no cuenta con límites de tiempo.

## Estructura del juego
El juego se compone de un total de 10 niveles, cada uno modifica la cantidad de objetos requeridos y objetos baneados, además de que los pasajeros traen consigo más objetos.

1. Mochila: 3 Objetos, Requeridos: 1, Baneados: 0
2. Mochila: 3 Objetos, Requeridos: 2, Baneados: 0
3. Mochila: 4 Objetos, Requeridos: 2, Baneados: 1
4. Mochila: 4 Objetos, Requeridos: 2, Baneados: 2
5. Mochila: 5 Objetos, Requeridos: 3, Baneados: 3
6. Mochila: 7 Objetos, Requeridos: 3, Baneados: 5
7. Mochila: 9 Objetos, Requeridos: 5, Baneados: 7
8. Mochila: 11 Objetos, Requeridos: 6, Baneados: 9
9. Mochila: 13 Objetos, Requeridos: 7, Baneados: 12
10. Mochila: 14 Objetos, Requeridos: 8, Baneados: 15

## Tutorial
El tutorial también consistirá en la explicación general de las mecánicas principales. Al ser un juego con una interfaz un poco cargada, se buscará explicar cada elemento importante antes de las mecánicas de juego. También será necesario redistribuir elementos en el juego y en el tutorial.

## Registro de Actividades

### 11-12-23
Se analizaron las características y estructura del juego, se documentaron los puntos más importantes de las mecánicas principales y se distinguieron los objetos y condiciones de victoria y derrota.
