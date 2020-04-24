FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
COPY *.sln .
COPY Nure/ ./Nure/
ENV PATH="/root/.dotnet/tools:${PATH}"
RUN wget -O- https://packages.microsoft.com/keys/microsoft.asc | gpg --dearmor > microsoft.asc.gpg && \
mv microsoft.asc.gpg /etc/apt/trusted.gpg.d/ && \
wget https://packages.microsoft.com/config/debian/10/prod.list && \
mv prod.list /etc/apt/sources.list.d/microsoft-prod.list && \
chown root:root /etc/apt/trusted.gpg.d/microsoft.asc.gpg && \
chown root:root /etc/apt/sources.list.d/microsoft-prod.list && \
apt-get update && \
apt-get install -y apt-transport-https && \
apt-get update && \
apt-get install -y dotnet-sdk-2.1 && \
dotnet restore
ENTRYPOINT ["dotnet", "run", "--project", "/Nure", "--"]

