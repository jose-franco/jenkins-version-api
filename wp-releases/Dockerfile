FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["wp-releases/wp-releases.csproj", "wp-releases/"]
RUN dotnet restore "wp-releases/wp-releases.csproj"
COPY . .
WORKDIR "/src/wp-releases"
RUN dotnet build "wp-releases.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "wp-releases.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "wp-releases.dll"]