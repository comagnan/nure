FROM mcr.microsoft.com/dotnet/sdk:5.0-focal AS build
COPY *.sln .
COPY Nure/ ./Nure/
ENV PATH="/root/.dotnet/tools:${PATH}"
RUN apt-get update && \
apt-get install -y software-properties-common && \
wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb && \
dpkg -i packages-microsoft-prod.deb && \
add-apt-repository universe && \
apt-get update && \
apt-get install -y apt-transport-https
ENTRYPOINT ["dotnet", "run", "--project", "/Nure", "--"]

