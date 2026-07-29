# -----------------------------
# 1. Build stage (SDK)
# -----------------------------
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copiamos solo el csproj para cache
COPY ["Plantilla_Base.csproj", "./"]
RUN dotnet restore "Plantilla_Base.csproj"

# Copiamos el resto del código
COPY . .
WORKDIR "/src"

# Publicación estable (sin trimming agresivo)
RUN dotnet publish "Plantilla_Base.csproj" \
    -c Release \
    -o /app/publish \
    --self-contained true \
    -r linux-x64 \
    /p:PublishSingleFile=true \
    /p:DebugType=None \
    /p:EnableCompressionInSingleFile=true \
    /p:IncludeNativeLibrariesForSelfExtract=true \
    /p:PublishTrimmed=false

# -----------------------------
# 2. Imagen final (Distroless)
# -----------------------------
FROM gcr.io/distroless/base-debian12 AS final

# Usuario no root
USER nonroot

WORKDIR /app

# Copiamos el ejecutable único
COPY --from=build /app/publish .

# Healthcheck (opcional)
HEALTHCHECK --interval=30s --timeout=5s --start-period=10s \
  CMD ["./Plantilla_Base", "--healthcheck"]

# Ejecutamos la app
ENTRYPOINT ["./Plantilla_Base"]
