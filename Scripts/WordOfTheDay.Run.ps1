docker-compose  -f "..\docker-compose.yml" up --build
dotnet-ef database update -s ..\WordOfTheDay.Api -p ..\WordOfTheDay.Repository