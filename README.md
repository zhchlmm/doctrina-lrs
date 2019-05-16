## Getting Started
Use these instructions to get the project up and running.

### Prerequisites
You will need the following tools:

* [Visual Studio Code or 2017](https://www.visualstudio.com/downloads/)
* [.NET Core SDK 2.2](https://www.microsoft.com/net/download/dotnet-core/2.2)

### Setup
Follow these steps to get your development environment set up:

  1. Clone the repository
  2. At the root directory, restore required packages by running:
     ```
     dotnet restore
     ```
  3. Next, build the solution by running:
     ```
     dotnet build
     ```
  4. Next, within the `Doctrina.WebUI\ClientApp` directory, launch the front end by running:
     ```
     npm start
     ```
  5. Once the front end has started, within the `Doctrina.WebUI` directory, launch the back end by running:
     ```
	 dotnet run
	 ```
  5. Launch [http://localhost:52468/](http://localhost:52468/) in your browser to view the Web UI

  6. Launch [http://localhost:52468/api](http://localhost:52468/api) in your browser to view the API

## Technologies
* .NET Core 2.2
* ASP.NET Core 2.2
* Entity Framework Core 2.2

## License

This project is licensed under the GPL-3.0 License - see the [LICENSE.md](https://github.com/doctrina-solutions/doctrina-lrs/blob/master/LICENSE.md) file for details.