cd C:\Users\Илья\source\repos\WordOfTheDayApp
dotnet-ef database update -s WordOfTheDay.Api -p WordOfTheDay.Repository
docker-compose  -f "docker-compose.yml" build