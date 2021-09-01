FROM docker.io/bachm44/ubuntu-dotnet3.1-node:stable as server

ENV ASPNETCORE_Environment=Production
ENV ASPNETCORE_URLS http://+:5001

WORKDIR /server
VOLUME ./wwwroot/Repository
COPY . ./

RUN dotnet publish "ZPI/ZPI.csproj"-c Release -o publish

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim

COPY --from=server /publish .

EXPOSE 5001/tcp

ENTRYPOINT ["dotnet","ZPI.dll"]