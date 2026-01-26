FROM mcr.microsoft.com/dotnet/sdk:10.0

WORKDIR /src

# Copy the main project
COPY api2spec-fixture-nancy.csproj ./
COPY Modules/ ./Modules/
COPY Program.cs ./

# Copy test project
COPY tests/Api.Tests/Api.Tests.csproj ./tests/Api.Tests/

# Restore all projects
RUN dotnet restore ./tests/Api.Tests/Api.Tests.csproj

# Copy test source files
COPY tests/Api.Tests/*.cs ./tests/Api.Tests/

# Run tests
ENTRYPOINT ["dotnet", "test", "./tests/Api.Tests/Api.Tests.csproj", "--no-restore", "-v", "normal"]
