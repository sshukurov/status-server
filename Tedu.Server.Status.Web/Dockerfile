FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /publish
ENV ASPNETCORE_URLS http://+:5000
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY . .
WORKDIR /src/Tedu.Server.Status.Web
RUN dotnet build Tedu.Server.Status.Web.csproj -c Release -o /publish

FROM build AS publish
RUN dotnet publish Tedu.Server.Status.Web.csproj -c Release -o /publish

FROM base AS final
WORKDIR /publish
COPY --from=publish /publish .
ENTRYPOINT ["dotnet", "Tedu.Server.Status.Web.dll"]