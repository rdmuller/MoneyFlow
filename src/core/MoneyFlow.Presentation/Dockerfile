FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
ENV ASPNETCORE_URLS=http://+:8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /app

COPY ["Directory.Build.props", "Directory.Build.props"]
COPY ["Directory.Packages.props", "Directory.Packages.props"]
COPY ["src/core/MoneyFlow.Presentation/MoneyFlow.Presentation.csproj", "src/core/MoneyFlow.Presentation/"]
COPY ["src/core/MoneyFlow.Application/MoneyFlow.Application.csproj", "src/core/MoneyFlow.Application/"]
COPY ["src/core/MoneyFlow.Domain/MoneyFlow.Domain.csproj", "src/core/MoneyFlow.Domain/"]
COPY ["src/infra/MoneyFlow.Infra/MoneyFlow.Infra.csproj", "src/infra/MoneyFlow.Infra/"]
COPY ["src/Shared/Shared.Application/Shared.Application.csproj", "src/Shared/Shared.Application/"]
COPY ["src/Shared/Shared.Domain/Shared.Domain.csproj", "src/Shared/Shared.Domain/"]
COPY ["src/Shared/Shared.Infra/Shared.Infra.csproj", "src/Shared/Shared.Infra/"]
COPY ["src/Shared/Shared.Presentation/Shared.Presentation.csproj", "src/Shared/Shared.Presentation/"]

RUN dotnet restore "src/core/MoneyFlow.Presentation/MoneyFlow.Presentation.csproj"

COPY . .
RUN dotnet build "src/core/MoneyFlow.Presentation/MoneyFlow.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "src/core/MoneyFlow.Presentation/MoneyFlow.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MoneyFlow.Presentation.dll"]