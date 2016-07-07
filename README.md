HiP-Auth
======
HiP-Auth is an Authentication and Authorization service built and maintained by [HiPCMS](https://github.com/HiP-App/HiP-CmsWebApi) which is a content management system developed by the project group [History in 
Paderborn](http://is.uni-paderborn.de/fachgebiete/fg-engels/lehre/ss15/hip-app/pg-hip-app.html).

HiP-Auth is currently acting as a microservice for HiPCMS. Nevertheless it can be consumed by any application which requires token based authentication.
We also develop a [REST API](https://github.com/HiP-App/HiP-CmsWebApi) and a [Client app](https://github.com/HiP-App/HiP-CmsAngularApp) which uses this service. The REST API is built on .NET Core 1.0 and client app is developed on Angular2.

See the LICENSE file for licensing information.

See [the graphs page](https://github.com/HiP-App/HiP-Auth/graphs/contributors) 
for a list of code contributions.

## Technolgies and Requirements:
HiP-CmsWebApi is a REST API built on .NET Core 1.0 with C# 6.0. Below are the requirements needed to build and develop this project,
 * [.NET Core](https://www.microsoft.com/net/core#windows) for windows or Linux.
 * [PostgreSQL](http://www.postgresql.org/download/)

## IDE Options
 * Visual Studio 2015 with Update 3 and [NuGet Package Manager](https://www.nuget.org/). 
 * Visual Studio Code with [C# extention](https://marketplace.visualstudio.com/items?itemName=ms-vscode.csharp).
 
## Getting started

 * Clone the repository.
 * Create a new file `appsettings.Development.json` at `scr/Auth`. (See `src/Auth/appsettings.Development.json.example`).
 * Update the new `appsettings.Development.json` file to match your needs.
 * To run using Visual Studio, just start the app with/without debugging.
 * To run through terminal,
  * Navigate to `src\Auth`
  * Set Environment Varriable `ASPNETCORE_ENVIRONMENT=Development`
  * Execute `dotnet run`

## How to develop

 * You can [fork](https://help.github.com/articles/fork-a-repo/) or [clone](https://help.github.com/articles/cloning-a-repository/) the repo.
   * To submit patches you should fork and then [create a Pull Request](https://help.github.com/articles/using-pull-requests/)
   * If you are part of the project group, you can create new branches on the main repo as described [in our internal
     Confluence](http://atlassian-hip.cs.upb.de:8090/display/DCS/Conventions+for+git)

## How to test
 * Create a new file "appsettings.Test.json" at "test/Auth.Tests". (See "test/Auth.Tests/appsettings.Test.json.example")
 * Update the new appsettings.Test.json file to match your needs.
 * Navigate to `test\Auth.Tests` then run `dotnet test`.


## How to submit Defects and Feature Proposals

Please write an email to [hip-app@campus.upb.de](mailto:hip-app@campus.upb.de).

## Documentation

Documentation is currently collected in our [internal Confluence](http://atlassian-hip.cs.upb.de:8090/dashboard.action). If something is missing in 
this README, just [send an email](mailto:hip-app@campus.upb.de).


## Contact

> HiP (History in Paderborn) ist eine Plattform der:
> Universität Paderborn
> Warburger Str. 100
> 33098 Paderborn
> http://www.uni-paderborn.de
> Tel.: +49 5251 60-0

You can also [write an email](mailto:hip-app@campus.upb.de).
