# =========================
# 1. Build stage
# =========================
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /src

# Copy project files first
COPY ["LibrarySystem.PL/LibrarySystem.PL.csproj", "LibrarySystem.PL/"]
COPY ["LibrarySystem.BLL/LibrarySystem.BLL.csproj", "LibrarySystem.BLL/"]
COPY ["LibrarySystem.DAL/LibrarySystem.DAL.csproj", "LibrarySystem.DAL/"]

# Restore dependencies
RUN dotnet restore "LibrarySystem.PL/LibrarySystem.PL.csproj"

# Copy the rest of the source code
COPY . .

# Publish the API
RUN dotnet publish "LibrarySystem.PL/LibrarySystem.PL.csproj" \
    -c Release \
    -o /app/publish \
    --no-restore


# =========================
# 2. Runtime stage
# =========================
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final

WORKDIR /app

# Container listens on port 8080
ENV ASPNETCORE_HTTP_PORTS=8080

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "LibrarySystem.PL.dll"]