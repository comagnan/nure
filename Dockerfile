FROM mcr.microsoft.com/dotnet/core/sdk:3.1-bionic AS build
COPY *.sln .
COPY Nure/ ./Nure/
ENV PATH="/root/.dotnet/tools:${PATH}"
RUN apt-get update && \
apt-get install -y software-properties-common && \
wget https://packages.microsoft.com/config/ubuntu/18.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb && \
dpkg -i packages-microsoft-prod.deb && \
add-apt-repository universe && \
apt-get update && \
apt-get install -y apt-transport-https && \
apt-get update && \
apt-get install -y dotnet-sdk-2.1 && \
dotnet restore
ENTRYPOINT ["dotnet", "run", "--project", "/Nure", "--"]

