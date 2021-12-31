# Título del Proyecto

_Acá va un párrafo que describa lo que es el proyecto_

## Índice

- [Título del Proyecto](#título-del-proyecto)
  - [Índice](#índice)
  - [Get started 🚀](#get-started-)
    - [Consideraciones de uso](#consideraciones-de-uso)
      - [En proyectos .NET Core](#en-proyectos-net-core)
      - [En proyectos Full Framework](#en-proyectos-full-framework)
  - [Despliegue 📦](#despliegue-)
    - [API](#api)
    - [Paquete NuGet](#paquete-nuget)
    - [Analytics](#analytics)
    - [Monitorización 🤓](#monitorización-)
  - [Contribuyendo 🖇️](#contribuyendo-️)
  - [Versionado 📌](#versionado-)
  - [FAQ ❔](#faq-)
  - [Expresiones de Gratitud 🎁](#expresiones-de-gratitud-)

## Get started 🚀

_Estas instrucciones te permitirán tener unas nociones básicas sobre cómo utilizar el API._

El API dispone de un cliente autogenerado. Para utilizarlo hay que instalarse el paquete nuget

* Aida.AidaTemplate.Api.Client

### Consideraciones de uso
#### En proyectos .NET Core

Estos clientes autogenerados tienen la particularidad que pueden hacer uso directo del HttpClientFactory de .NET Core.

Por defecto, no integran la gestión de la autenticación, por lo que para usarlos en nuestras APIs securizadas con OAuth2, debemos registrarlos haciendo uso de los extensions methods habilitados en el paquete nuget Aida.Core.Http, y especificando el ClientCredentialsTokenRequest correspondiente

```c
    services.AddHttpClient<IAidaTemplate.Api.Client, AidaTemplate.Api.Client>(client => {
        client.BaseAddress = new Uri(apiAddress);
    }, new ClientCredentialsTokenRequest {
        Address = "identityServerUrl",
        ClientId = "clientId",
        ClientSecret = "clientSecret",
        Scope = "netcore31-api"
    });
```

#### En proyectos Full Framework
En este tipo de proyectos, aunque podríamos hacer uso del inyector de dependencias, la realidad es que los productos no lo están usando, por lo que de cara a hacer uso de estos clientes, no nos quedaría más remedio que construirlos manualmente

```c
    var clientCredentialsTokenRequest = new ClientCredentialsTokenRequest {
        Address = "identityServerUrl",
        ClientId = "clientId",
        ClientSecret = "clientSecret",
        Scope = "netcore31-api"
    };

    var oauth2AuthorizationHandler = new AuthorizationTokenHandler(new OAuth2AuthorizationClient(clientCredentialsTokenRequest));
    var httpClient = new HttpClient(oauth2AuthorizationHandler) {
        BaseAddress = new Uri(apiAddress)
    };

    return new AidaTemplateApiClient(httpClient);
```

## Despliegue 📦
_Agrega notas adicionales sobre como hacer deploy_

###  API

- AidaTemplate.Api

Para desplegar:

1. Crear una nueva Release en [Azure DevOps](url al release pipeline del proyecto).
2. Configurar el entorno del API en los [ApplicationSettings](https://aidacanarias.visualstudio.com/Market-Configurations/_git/ApplicationSettings). Busca el fichero `AidaTemplate.Api.xml`.
3. Obtener las Releases desde [este job de Jenkins](http://jenkins-aida:8080/view/...).
4. Desplegar la Release deseada para el entorno configurado desde [este job de Jenkins](http://jenkins-aida:8080/view/.../).

### Paquete NuGet

Se despliega en este **[![Build Status](url pipeline)](url pipeline)**.

Paquete NuGet con Cliente AutoGenerado:

- Aida.AidaTemplate.Api.Client

El despliegue de estos paquetes se realizará al subir la versión en el fichero [Directory.Build.props](./Directory.Build.props) y llevar el cambio a la rama `master`.

### Analytics
La API dispone de un proyecto propio de Application Insights dónde hacer seguimiento a las peticiones y errores que se van produciendo.

* [QA / Dev](url al application insights)

### Monitorización 🤓

Puedes seguir esta sección de la [Guía de Backend](https://aidacanarias.visualstudio.com/CrossCutting%20and%20Innovation/_wiki/wikis/Backend%20Guidelines/17/Diagn%C3%B3stico?anchor=monitorizaci%C3%B3n), para comprobar cómo se debe incluir la monitorzación en el API.

## Contribuyendo 🖇️

Si quieres contribuir con este proyecto te invitamos a que leas el documento [CONTRIBUTING.md](./doc/CONTRIBUTING.md)

## Versionado 📌

Usamos [SemVer](http://semver.org/) para el versionado. En el [CHANGELOG.md](./doc/CHANGELOG.md) podrás ver las distintas versiones así como sus changelogs.

## FAQ ❔

**¿Es este el formato que tengo que seguir para las preguntas?**

*Sí, es este, preguntas en negrita y respuesta en cursiva.*

## Expresiones de Gratitud 🎁

* Comenta a otros sobre este proyecto 📢
* Invita una cerveza 🍺 o un café ☕ a alguien del equipo.
* etc.


Template de README.md ❤️ por [Villanuevand](https://gist.github.com/Villanuevand/6386899f70346d4580c723232524d35a) 😊