# CSVUploaderApp

CSVUploaderApp is a small .NET Web App that uploads csv files to an AWS S3 Bucket.

### Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes.

### Prerequisites

- .NET Core 8.0 or later https://dotnet.microsoft.com/en-us/download
- AWS Account https://aws.amazon.com/
- Env.txt file for AWS Credentials

### Installing

Clone the repository:
```bash
git clone https://github.com/mrdavidrobison/CSVUploaderApp.git
```

Navigate to the project directory:
```bash
cd CSVUploaderApp
```

Restore the .NET packages:
```bash
dotnet restore CSVUploaderApp.csproj
```

### Running the application
You can run the application using the following command:
```bash
dotnet run
```

This will start the application and it will be accessible at http://localhost:5065