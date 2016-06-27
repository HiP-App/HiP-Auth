HiP-Auth
======
HiP-Auth is an Authentication and Authorization service built and maintained by [HiPCMS](https://github.com/HiP-App/HiP-CmsWebApi) which is a content management system developed by the project group [History in 
Paderborn](http://is.uni-paderborn.de/fachgebiete/fg-engels/lehre/ss15/hip-app/pg-hip-app.html).

HiP-Auth is currently acting as a microservice for HiPCMS. Nevertheless it can be consumed by any application which requires token based authentication.
We also develop a [REST API](https://github.com/HiP-App/HiP-CmsWebApi) and a [Client app](https://github.com/HiP-App/HiP-CmsAngularApp) which uses this service. The REST API is built on .NET Core 1.0 and client app is developed on Angular2.

See the LICENSE file for licensing information.

See [the graphs page](https://github.com/HiP-App/HiP-Auth/graphs/contributors) 
for a list of code contributions.

## Requirements:

 * [Visual Studio 2015](https://www.visualstudio.com/en-us/products/vs-2015-product-editions.aspx) and make sure you have [.NET Core](https://www.microsoft.com/net/core#windows) installed
 * ASP.NET 5 for [Windows](http://docs.asp.net/en/latest/getting-started/installing-on-windows.html) or [Linux](http://docs.asp.net/en/latest/getting-started/installing-on-linux.html).
 * [PostgreSQL](http://www.postgresql.org/download/)
 * [NuGet Package Manager](https://www.nuget.org/), an extension of Visual Studio.
 

## Technolgies and Frameworks

HiP-Auth support token authentication built on .NET Core 1.0 with C# 6.0 using [openiddict](https://github.com/openiddict/openiddict-core). Below are the runtime specifications we use

 * For Windows : [dotnet cli for windows](https://www.microsoft.com/net/core#windows)
 * For Linux	: [dotnet cli for ubuntu](https://www.microsoft.com/net/core#ubuntu)

## Getting started

 * Clone the repository.
 * Create a new file "appsettings.Development.json" at "scr/Auth". (See "src/Auth/appsettings.Development.json.example")
 * Update the new appsettings.Development.json file to match your needs.
 * Navigate to `src\Auth` then run `dotnet ef database update` to update your database.

## How to develop

 * The latest code is available on [the project's Github-page](https://github.com/HiP-App/HiP-CmsWebApi/)
 * You can [fork the repo](https://help.github.com/articles/fork-a-repo/) or [clone our repo](https://help.github.com/articles/cloning-a-repository/)
   * To submit patches you should fork and then [create a Pull Request](https://help.github.com/articles/using-pull-requests/)
   * If you are part of the project group, you can create new branches on the main repo as described [in our internal
     Confluence](http://atlassian-hip.cs.upb.de:8090/display/DCS/Conventions+for+git)

We are using [Visual Studio 2015](https://www.visualstudio.com/en-us/products/vs-2015-product-editions.aspx). 


## How to test
 * Create a new file "appsettings.Test.json" at "test/Auth.Tests". (See "test/Auth.Tests/appsettings.Test.json.example")
 * Update the new appsettings.Test.json file to match your needs.
 * Navigate to `test\Auth.Tests` then run `dotnet test` to update your database.


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
