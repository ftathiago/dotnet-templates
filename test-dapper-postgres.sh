cd ./src/Content/WebApi/eng/docker
docker-compose down -v --remove-orphans

docker-compose up -d postgres
sleep 10s
cd ../../../../../
sleep 10s

find ./Test -delete

dotnet new atwebapi -o Test -n TestUnpack --database postgres --orm dapper --allow-scripts yes
cd ./Test
dotnet test
dotnet user-secrets --project ./src/TestUnpack.Api set "ConnectionStrings:Default" "Server=localhost;Port=15432;Database=WebApiDB;User Id=postgres;Password=Postgres2021!;"
dotnet build
dotnet fm list migrations --processor PostgreSQL11_0 --assembly ./src/TestUnpack.DapperInfraData/bin/Debug/net5.0/TestUnpack.DapperInfraData.dll -c "Server=localhost;Port=15432;Database=WebApiDB;User Id=postgres;Password=Postgres2021!;"
dotnet fm migrate --processor PostgreSQL11_0 --assembly ./src/TestUnpack.DapperInfraData/bin/Debug/net5.0/TestUnpack.DapperInfraData.dll -c "Server=localhost;Port=15432;Database=WebApiDB;User Id=postgres;Password=Postgres2021!;" up
dotnet run --project ./src/TestUnpack.Api
cd ..

cd ./src/Content/WebApi/eng/docker
docker-compose down -v --remove-orphans
cd ../../../../../