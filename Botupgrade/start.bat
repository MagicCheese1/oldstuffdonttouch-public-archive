cd /root/app/Roberto
git pull 
dotnet publish -c release -r ubuntu.18.04-x64
./bin/release/netcoreapp2.1/ubuntu.18.04-x64/publish/Botupgrade
pause