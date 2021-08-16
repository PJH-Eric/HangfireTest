#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["HangfireDemo.csproj", "."]
COPY ["Service/Service.csproj", "Service/"]
COPY ["Repository/Repository.csproj", "Repository/"]
COPY ["ModelDTO/ModelDTO.csproj", "ModelDTO/"]
RUN dotnet restore "./HangfireDemo.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "HangfireDemo.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HangfireDemo.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HangfireDemo.dll"]