FROM docker.io/bachm44/ubuntu-dotnet3.1-node:stable as server

ENV ASPNETCORE_Environment=Production

WORKDIR /server

COPY . ./

RUN dotnet publish "ZPI/ZPI.csproj" -c Release -o publish

CMD ["dotnet","publish/ZPI.dll"]