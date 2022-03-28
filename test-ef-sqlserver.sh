cd ./src/Content/WebApi/eng/docker
docker-compose down -v --remove-orphans

docker-compose up -d sqlserver
sleep 10s
cd ../../../../../
echo Waiting database startup
sleep 30s


find ./Test -delete

dotnet new atwebapi -o Test -n TestUnpack --database sqlserver --orm ef --allow-scripts yes
cd ./Test
dotnet user-secrets --project ./src/TestUnpack.Api set "ConnectionStrings:Default" "Server=localhost;Database=WebApiDB; User=sa;Password=MyP4ssw0rd_;Pooling=True;Enlist=False; Min Pool Size=30; Max Pool Size=300;"
dotnet build
dotnet test
dotnet ef migrations add InitialMigrations --startup-project ./src/TestUnpack.Api --project ./src/TestUnpack.EfInfraData
dotnet ef database update --startup-project ./src/TestUnpack.Api --project ./src/TestUnpack.EfInfraData
dotnet run --project ./src/TestUnpack.Api
cd ..

cd ./src/Content/WebApi/eng/docker
docker-compose down -v --remove-orphans
cd ../../../../../