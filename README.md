# campus-api-client-javascript
PoC for fetching Obsevations from the Campus API
This is a public SDK for interacting with the Campus API. It allows you to easily fetch observations and other data from the Campus API using JavaScript.

## Features

- Fetch observations from the Campus API
- Simple and easy-to-use interface
- Suitable for beginners and experienced developers alike


## Table of Contents

- [Installation](#installation)
- [Configuration](#configuration)
- [Usage](#usage)
  - [Authentication](#authentication)
  - [Fetching Observations](#fetching-observations)
- [Running from Command Line](#running-from-command-line)
  - [Expected Output](#expected-output)
- [Running from Command Line](#running-from-command-line)
- [Error Handling](#error-handling)
- [License](#license)

## Installation

First, you need to install the necessary dependencies. You can do this using npm:

```sh
dotnet add package Microsoft.Extensions.Configuration
dotnet add package Microsoft.Extensions.Configuration.Json
dotnet add package System.Text.Json
```

## Configuration

Please copy the [appsettings_template.json](./appsettings_template.json) to appsettings.json and update the values with 
your own credentials, and SensorId.

```json
{
  "Azure": {
    "ClientId": "your_client_id",
    "ClientSecret": "your_client_secret",
    "TenantId": "your_tenant_id",
    "CampusApiUrl": "https://campus_url"
  },
  "Observations": {
    "SensorId": "your_sensor_id format:Sensor-12345678-abcd-1234-cdef-123456789abc",
    "SensorType": "your_sensor_type format:dtmi:org:brickschema:schema:Brick:Energy_Usage_Sensor;1"
  }
}
```

## Usage
Here's a basic example of how to use the SDK to fetch observations:

### Authentication
To authenticate with Azure, you need to provide your client_id, client_secret, and tenant_id. The SDK will use these credentials to obtain a JWT token.

### Fetching Observations
Once authenticated, you can use the SDK to fetch observations from your API.

*Example*
Here is an example of how to use the SDK:

Authenticate:
```dotnet
var apiClient = new ApiClient(
                azureSettings.ClientId,
                azureSettings.ClientSecret,
                azureSettings.TenantId,
                azureSettings.CampusApiUrl
            );

  ```
Fetch Observations:
```dotnet
var observations = await apiClient.GetObservationsAsync(
                    observationSettings.SensorId,
                    "2023-01-01T00:00:00Z",
                    "2023-01-02T00:00:00Z",
                    "dtmi:org:brickschema:schema:Brick:Energy_Usage_Sensor;1"
                );
                Console.WriteLine("Observations: " + observations);
```

  Please see Brick Schema ,eg [Energy_Usage_Sensor](https://ontology.brickschema.org/brick/Energy_Usage_Sensor.html) for how the sensor type should be formatted.


## Running from Command Line
To run the SDK from the command line:
```sh
dotnet build
dotnet run
```

### Expected Output

```json
[
    {
      "sensorId": "Sensor-12345678-abcd-1234-cdef-123456789abc",
      "sensorType": "dtmi:org:brickschema:schema:Brick:Energy_Usage_Sensor;1",
      "timestamp": "2023-01-01T00:00:00Z",
      "value": 10.03
    },
    {
      "sensorId": "Sensor-12345678-abcd-1234-cdef-123456789abc",
      "sensorType": "dtmi:org:brickschema:schema:Brick:Energy_Usage_Sensor;1",
      "timestamp": "2023-01-01T01:00:00Z",
      "value": 8.0
    }
  ]
```

## Error Handling
The SDK includes basic error handling. If authentication fails or fetching observations fails, an error message will be logged to the console.  

## License
This project is licensed under the Apache 2.0 License.
