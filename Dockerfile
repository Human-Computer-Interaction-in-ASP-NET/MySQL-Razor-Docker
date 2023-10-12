FROM mcr.microsoft.com/dotnet/aspnet:7.0
COPY dist /app
WORKDIR /app
EXPOSE 80/tcp
ENTRYPOINT ["dotnet", "SeedIdentity.dll"]
