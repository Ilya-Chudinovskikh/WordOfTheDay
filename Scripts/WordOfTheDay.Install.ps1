cd C:\Users\Илья\source\repos\WordOfTheDayApp
dotnet-ef database update -s WordOfTheDay.Api -p WordOfTheDay.Repository
docker-compose  -f "C:\Users\Илья\source\repos\WordOfTheDayApp\docker-compose.yml" build