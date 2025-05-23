FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copiar todo el contenido del proyecto
COPY . . 

# Restaurar dependencias usando la solución
RUN dotnet restore ExamenUsuarios.sln

# Publicar la aplicación
RUN dotnet publish ExamenUsuarios.sln -c Release -o /app/out

# Imagen final
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .

# Cambia esto si el nombre del DLL es diferente
ENV APP_NET_CORE=registrousuaou.dll

CMD ASPNETCORE_URLS=http://*:$PORT dotnet $APP_NET_CORE
