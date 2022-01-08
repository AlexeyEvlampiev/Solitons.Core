#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["samples/Solitons.Samples.RestApi/Solitons.Samples.RestApi.csproj", "samples/Solitons.Samples.RestApi/"]
RUN dotnet restore "samples/Solitons.Samples.RestApi/Solitons.Samples.RestApi.csproj"
COPY . .
WORKDIR "/src/samples/Solitons.Samples.RestApi"
RUN dotnet build "Solitons.Samples.RestApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Solitons.Samples.RestApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Solitons.Samples.RestApi.dll"]