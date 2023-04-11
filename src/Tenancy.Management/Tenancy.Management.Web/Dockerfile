#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

# creates a working dir in the sdk folder
WORKDIR /src

# copy csproj and Tenancy.Management.Web folder into src
COPY ["Tenancy.Management.Web/Tenancy.Management.Web.csproj", "Tenancy.Management.Web/"]
RUN dotnet restore "Tenancy.Management.Web/Tenancy.Management.Web.csproj"

#copy everything from Tenancy.Management folder into src
COPY . .
WORKDIR "/src/Tenancy.Management.Web"
RUN dotnet build "Tenancy.Management.Web.csproj" -c Release -o /app/build

FROM build AS publish

# publish dotnet project into src/app/publish
RUN dotnet publish "Tenancy.Management.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
# change folder to src/app
WORKDIR /app

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tenancy.Management.Web.dll"]