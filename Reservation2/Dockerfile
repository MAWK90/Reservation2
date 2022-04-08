#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.



FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Reservation2/Reservation2.csproj", "Reservation2/"]
RUN dotnet restore "Reservation2/Reservation2.csproj"
COPY . .
WORKDIR "/src/Reservation2"
RUN dotnet build "Reservation2.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Reservation2.csproj" -c Release -o /app/publish


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_ENVIRONMENT Development
ENTRYPOINT ["dotnet", "Reservation2.dll"]