FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["src/Web/FaceIO.Api/FaceIO.Api.csproj", "src/Web/FaceIO.Api/"]
RUN dotnet restore "src/Web/FaceIO.Api/FaceIO.Api.csproj"
COPY . .
WORKDIR "/src/src/Web/FaceIO.Api"
RUN dotnet build "FaceIO.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FaceIO.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FaceIO.Api.dll"]