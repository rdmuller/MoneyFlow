FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/core/MoneyFlow.Presentation/MoneyFlow.Presentation.csproj", "src/core/MoneyFlow.Presentation/"]
COPY ["src/core/MoneyFlow.Application/MoneyFlow.Application.csproj", "src/core/MoneyFlow.Application/"]
COPY ["src/mediator/Mediator.Abstractions/Mediator.Abstractions.csproj", "src/mediator/Mediator.Abstractions/"]
COPY ["src/core/MoneyFlow.Domain/MoneyFlow.Domain.csproj", "src/core/MoneyFlow.Domain/"]
COPY ["src/core/SharedKernel/SharedKernel.csproj", "src/core/SharedKernel/"]
COPY ["src/infra/MoneyFlow.Infra/MoneyFlow.Infra.csproj", "src/infra/MoneyFlow.Infra/"]
COPY ["src/mediator/Mediator/Mediator.csproj", "src/mediator/Mediator/"]
RUN dotnet restore "./src/core/MoneyFlow.Presentation/MoneyFlow.Presentation.csproj"
COPY . .
WORKDIR "/src/src/core/MoneyFlow.Presentation"
RUN dotnet build "./MoneyFlow.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MoneyFlow.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MoneyFlow.Presentation.dll"]