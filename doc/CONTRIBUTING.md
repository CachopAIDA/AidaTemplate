# Contributing

Si has llegado hasta aquí es que quieres contribuir en el proyecto. Este documento te cuenta cómo hacerlo y qué cosas debes tener en cuenta.

## Comenzando 🚀

_Estas instrucciones te permitirán tener el proyecto en funcionamiento en tu máquina local para propósitos de desarrollo y pruebas. Mira **Despliegue** para conocer como desplegar el proyecto._

- Clona este repositorio

### Pre-requisitos 📋

_Que cosas necesitas para instalar el software y como instalarlas_

- [DotNetCore 3.1 sdk](https://dotnet.microsoft.com/download/dotnet-core/3.1)
- [Docker For Windows](https://docs.docker.com/docker-for-windows/install/)
- Base de datos SqlServer corriendo localmente en el puerto `1433`. Puedes levantar el contenedor ejecutando `docker-compose -f docker-compose-dev.yml up`

### Instalación 🔧

_Una serie de ejemplos paso a paso que te dice lo que debes ejecutar para tener un entorno de desarrollo ejecutandose_

Levanta la base de datos.

```powershell
docker-compose up sima-database -d
```

## Documentación de desarrollo 👨‍💻👩‍💻

_Recursos que pueden ayudarte a entrar en contexto y comprender mejor nuestras decisiones y diseño_

- [Diagramas de Secuencia](./doc/Diagrams/)
- [Architecture Decision Record (ADR)](./doc/ADRs/)

## Ejecutando las pruebas ⚙️

_Explica como ejecutar las pruebas automatizadas para este sistema_

Encontrarás los tests en los proyectos:

- `NetCore31Api.Tests` -> Tests unitarios y de integración del API.

Ejecuta los tests:

```powershell
dotnet test
```

Ejecuta el API:

```powershell
cd ./src/Api
dotnet run
```

## Contribuyendo 🖇️

Para contribuir, deberás sacar una nueva rama de `master` para hacer tus cambios y posteriormente una Pull Request de esa rama hacia `master`.

Antes de hacer una Pull Request, asegúrate de:

- Actualizar el [CHANGELOG.md](./doc/CHANGELOG.md).
- Revisar si hay que actualizar el [README.md](../README.md).
- Documentar posibles decisiones en un ADR.
- En caso de haber deuda técnica, regístrala.
- Los tests pasan.

## Integración Continua ✅

[Pipeline de Azure DevOps](poner url pipeline del proyecto).

[![Build Status](poner badge status del proyecto)]

Si falla la integración continua, llega una alerta al canal de slack **#producto-pipeline**.

## Despliegue 📦

_Agrega notas adicionales sobre como hacer deploy_

### API

- Generación de la Release en [Azure DevOps](ruta al pipeline de release correspondiente).
- Distribución en [Jenkins](http://jenkins-aida:8080/view/).

## Construido con 🛠️

_Menciona las herramientas que utilizaste para crear tu proyecto_

- [AspNetCore 3.1](https://docs.microsoft.com/es-es/aspnet/core/?view=aspnetcore-3.1) - El framework web usado
- [NuGet](https://www.nuget.org/) - Manejador de dependencias
