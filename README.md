# SalesPlatformBackend

## Overview

This project is an example of a comprehensive application service that includes an API, unit tests, integration tests, and several libraries for domain models, infrastructure, and application services. The project connects to both SQL Server and CosmosDB.

### Prerequisites
To run this project, you will need the following tools installed:
 - Visual Studio 2019 or later / Visual Studio Code
 - Git
 - .NET SDK (version 8.0, can be found [here](https://dotnet.microsoft.com/en-us/download))
 - CosmosDB Emulator (Windows, can be found [here](https://learn.microsoft.com/en-us/azure/cosmos-db/how-to-develop-emulator?tabs=windows,csharp&pivots=api-nosql))
 - SQL Server Management Studio (SSMS)
 - Docker

## Project Structure

The project is organized into the following libraries:
 - API: Exposes endpoints for the application.
 - Unit Tests: Contains unit tests for testing individual components.
 - Integration Tests: Contains integration tests for testing interactions between components.
 - Domain Library: Contains domain models and business logic.
 - Infrastructure Library: Contains repositories and database contexts (SQL Server and CosmosDB).
 - Application Service Library: Contains application services for coordinating domain and infrastructure layers.

## Setup Instructions

To run the application
 - Clone the repository:
`git clone  https://github.com/your-username/school-project.git`
 - Run CosmosDB Emulator: Follow the instructions to install and start the CosmosDB Emulator on Windows. You can find the emulator [here](https://learn.microsoft.com/en-us/azure/cosmos-db/how-to-develop-emulator?tabs=windows,csharp&pivots=api-nosql#tabpanel_1_windows)
 - Setup SQL Server:
	 - Open a powershell in the same folder as the .git of the repository
	 -  `docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Suits0811" -e "MSSQL_PID=Evaluation" -p 1433:1433 --name sqlpreview --hostname sqlpreview -d mcr.microsoft.com/mssql/server:2022-preview-ubuntu-22.04`
	 - `cd .\Infrastructure\`
	 - `dotnet tool install --global dotnet-ef`
	 - `$env:sqlconn="Server=localhost,1433;Database=SalesPlatformDB;User Id=sa;Password=Suits0811;TrustServerCertificate=True;"`
		 - This is to ensure the environment id for the context is set correctly
	 - `dotnet ef database update`
 - Build and Run the Application: Open the solution in Visual Studio or Visual Studio Code and build the solution.
	 - `dotnet build`
	 - `cd .\Api.Service\`
	 - `dotnet run`
 - Open the browser and find the `http://localhost:5088/swagger/index.html`
 - Send a `/SeedDataEndpoint` to get test data in the database.

To Run the tests
- Use the following command `dotnet test` in the main folder
- Or use visual studio build in test manager
