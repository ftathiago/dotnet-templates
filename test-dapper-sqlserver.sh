cd ./src/Content/WebApi/eng/docker
docker-compose down -v --remove-orphans

docker-compose up -d sqlserver
sleep 10s
cd ../../../../../
echo Waiting database startup
sleep 30s

find ./Test -delete

dotnet new atwebapi -o Test -n TestUnpack --database sqlserver --orm dapper --allow-scripts yes
cd ./Test
dotnet test
dotnet user-secrets --project ./src/TestUnpack.Api set "ConnectionStrings:Default" "Server=localhost;Database=WebApiDB; User=sa;Password=MyP4ssw0rd_;Pooling=True;Enlist=False; Min Pool Size=3; Max Pool Size=300;"
dotnet build
dotnet fm list migrations --processor SqlServer2016 --assembly ./src/TestUnpack.DapperInfraData/bin/Debug/net5.0/TestUnpack.DapperInfraData.dll -c "Server=localhost;Database=WebApiDB; User=sa;Password=MyP4ssw0rd_;Pooling=True;Enlist=False; Min Pool Size=101; Max Pool Size=300;"
dotnet fm migrate --processor SqlServer2016 --assembly ./src/TestUnpack.DapperInfraData/bin/Debug/net5.0/TestUnpack.DapperInfraData.dll -c "Server=localhost;Database=WebApiDB; User=sa;Password=MyP4ssw0rd_;Pooling=True;Enlist=False; Min Pool Size=101; Max Pool Size=300;" up
dotnet run --project ./src/TestUnpack.Api
cd ..

cd ./src/Content/WebApi/eng/docker
docker-compose down -v --remove-orphans
cd ../../../../../