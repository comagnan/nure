FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
COPY *.sln .
COPY Nure/ ./Nure/
ENV PATH="/root/.dotnet/tools:${PATH}"
RUN apt-get update && \
apt-get install -y software-properties-common && \
wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb && \
dpkg -i packages-microsoft-prod.deb && \
add-apt-repository universe && \
apt-get update && \
apt-get install -y apt-transport-https && \
dotnet build /Nure/Nure.csproj
ENV LD_LIBRARY_PATH=/Nure/bin/Debug/net6.0/runtimes/ubuntu.18.04-x64/native/
ENTRYPOINT ["dotnet", "run", "--project", "/Nure", "--"]
