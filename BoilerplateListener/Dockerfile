From mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src

COPY ["DAL/DAL.csproj", "DAL/"]
COPY ["Logic/Logic.csproj", "Logic/"]
COPY ["KafkaManager/KafkaManager.csproj", "KafkaManager/"]
COPY ["Tests/Tests.csproj", "Tests/"]
COPY ["BoilerplateListener/BoilerplateListener.csproj", "BoilerplateListener/"]

RUN dotnet restore "BoilerplateListener/BoilerplateListener.csproj"
COPY . .
WORKDIR "/src/BoilerplateListener"

RUN dotnet build "BoilerplateListener.csproj" -c Release -o /app/build

FROM build as Publish
RUN dotnet publish "BoilerplateListener.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "BoilerplateListener.dll"]

