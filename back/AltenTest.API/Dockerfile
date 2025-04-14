# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ["AltenTest.sln", "./"]
COPY ["AltenTest.API/AltenTest.API.csproj", "AltenTest.API/"]
COPY ["AltenTest.Application/AltenTest.Application.csproj", "AltenTest.Application/"]
COPY ["AltenTest.Domain/AltenTest.Domain.csproj", "AltenTest.Domain/"]
COPY ["AltenTest.Infrastructure/AltenTest.Infrastructure.csproj", "AltenTest.Infrastructure/"]

# Restore dependencies
RUN dotnet restore

# Copy the rest of the code
COPY . .

# Build and publish
RUN dotnet build "AltenTest.API/AltenTest.API.csproj" -c Release -o /app/build
RUN dotnet publish "AltenTest.API/AltenTest.API.csproj" -c Release -o /app/publish

# Ensure the products.json file is copied to the publish directory
RUN mkdir -p /app/publish/Persistence/Seeding
RUN cp AltenTest.Infrastructure/Persistence/Seeding/products.json /app/publish/Persistence/Seeding/

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 80
EXPOSE 443

ENTRYPOINT ["dotnet", "AltenTest.API.dll"] 