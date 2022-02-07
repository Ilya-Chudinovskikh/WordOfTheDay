FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["WordOfTheDay.Api/WordOfTheDay.Api.csproj", "WordOfTheDay.Api/"]
COPY ["WordOfTheDay.Domain/WordOfTheDay.Domain.csproj", "WordOfTheDay.Domain/"]
COPY ["WordOfTheDay.Repository/WordOfTheDay.Repository.csproj", "WordOfTheDay.Repository/"]
RUN dotnet restore "WordOfTheDay.Api/WordOfTheDay.Api.csproj"
COPY . .
WORKDIR "/src/WordOfTheDay.Api"
RUN dotnet build "WordOfTheDay.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WordOfTheDay.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WordOfTheDay.Api.dll"]