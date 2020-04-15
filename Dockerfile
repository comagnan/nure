FROM mcr.microsoft.com/dotnet/core/sdk:2.1 AS build
WORKDIR /app

# copy csproj and restore as distinct layers
COPY *.sln .
COPY Nure/*.csproj ./Nure/
RUN dotnet tool install nukeeper --global && dotnet restore

# copy everything else and build app
COPY Nure/. ./Nure/
WORKDIR /app/Nure
RUN dotnet publish -c Release -o out


FROM mcr.microsoft.com/dotnet/core/aspnet:2.1 AS runtime
WORKDIR /app
ENV PATH="/root/.dotnet/tools:${PATH}"
COPY --from=build /root/.dotnet /root/.dotnet
COPY --from=build /app/Nure/out ./
ENTRYPOINT ["dotnet", "Nure.dll"]
