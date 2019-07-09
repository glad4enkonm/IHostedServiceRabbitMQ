# Introduction 
A simple RabbitMQ client implemented in .net core 2.2 with IHostedService

# How it was done

1.	Install .net core 2.2 SDK https://dotnet.microsoft.com/download/dotnet-core/2.2
2.	dotnet new console <project name>
3.	dotnet add package Microsoft.Extensions.Hosting --version 2.2.0
4.	dotnet add package Microsoft.Extensions.DependencyInjection --version 2.2.0
5.	dotnet add package RabbitMQ.Client --version 5.1.0
6.	add `<LangVersion>latest</LangVersion>` to a .csproj file

# Build and Run
dotnet build
dotnet run
