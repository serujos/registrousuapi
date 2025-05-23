FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

COPY *.csproj ./
RUN dotnet restore ExamenUsuarios.sln

COPY . ./
RUN dotnet publish ExamenUsuarios.sln -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .

#CAMBIAR AQUI EL NOMBRE DEL APLICATIVO
#nombre de tu app busca en bin\Release****\netcore5.0\plantitas.exe
ENV APP_NET_CORE registrousuapi.dll 

CMD ASPNETCORE_URLS=http://*:$PORT dotnet $APP_NET_CORE