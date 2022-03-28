cd ./src/Content/WebApi/eng/docker
docker-compose down -v --remove-orphans

docker-compose up -d postgres
sleep 10s
cd ../../../../../
sleep 10s

find ./Test -delete

dotnet new atwebapi -o Test -n TestUnpack --database postgres --orm ef --allow-scripts yes
cd ./Test
dotnet user-secrets --project ./src/TestUnpack.Api set "ConnectionStrings:Default" "Server=localhost;Port=15432;Database=WebApiDB;User Id=postgres;Password=Postgres2021!;"
dotnet build
dotnet test
dotnet ef migrations add InitialMigrations --startup-project ./src/TestUnpack.Api --project ./src/TestUnpack.EfInfraData --no-build
dotnet ef database update --startup-project ./src/TestUnpack.Api --project ./src/TestUnpack.EfInfraData
dotnet run --project ./src/TestUnpack.Api
cd ..

cd ./src/Content/WebApi/eng/docker
docker-compose down -v --remove-orphans
cd ../../../../../