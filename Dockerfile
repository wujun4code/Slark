FROM microsoft/dotnet:2.0-sdk
WORKDIR /app

# copy csproj and restore as distinct layers
COPY ./src/Slark/ ./
# ENV https http://192.168.1.149:8118
# ENV http http://192.168.1.149:8118
RUN dotnet restore Slark.sln

EXPOSE 60000

# copy and build everything else
COPY . ./
RUN dotnet publish -c Release
ENTRYPOINT ["dotnet", "./Slark.Server.ConsoleApp.NETCore/bin/Release/netcoreapp2.0/Slark.Server.ConsoleApp.NETCore.dll","--server.urls", "http://0.0.0.0:60000"]