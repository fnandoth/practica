# Microservicios

Proyecto creado con el fin de poner en practica lo estudiado sobre .Net y microservicios

## Servicios del proyecto
- gateway - Puerta de entrada encargada de redirigir solicitudes (YARP)
- user-service - Servicio de usuarios (API)
- product-service - Servicio de productos (API)
- Order-service - Servicio de ordenes (API)


## Requisitos para ejecutar el Docker y levantar los servicios
Instalar Docker si no lo tienes
verifica que tengas instalada la imagen de postgres

- [Docker](https://www.docker.com/get-started)
- [Docker Postgres](https://hub.docker.com/_/postgres)

## Instrucciones para el levantamiento

### Clona el repositorio
```bash
git clone https://github.com/fnandoth/practica
cd practica
```
### Levantar los servicios
```bash
docker-compose up --build
```
si quieres limpiar los contenedores y volumenes
```bash
docker-compose down -v
```
para reiniciar los servicios 
```bash
docker-compose restar
```
por ultimo si lo quieres levantar en segundo plano
```bash
docker-compose up -d --build
```