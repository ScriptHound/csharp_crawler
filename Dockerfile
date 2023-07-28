FROM ubuntu:23.04

WORKDIR /crawler
RUN apt-get update && apt-get install -y dotnet-sdk-6.0
RUN apt install -y ca-certificates && update-ca-certificates
COPY . .
RUN dotnet dev-certs https --trust
RUN dotnet build
CMD ["dotnet", "run"]
