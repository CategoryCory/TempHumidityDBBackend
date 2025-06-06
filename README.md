# TempHumidityBackend

`TempHumidityBackend` is a .NET 9 background service that listens for UDP packets from an ESP32-based sensor device, deserializes the CBOR payload into structured temperature and humidity data, and forwards this data to a configurable REST API endpoint via HTTP POST method.

## Features

- Listens for UDP datagrams on a configurable port
- Decodes CBOR payloads into structured sensor data
- Forwards data to a configurable RESTful API endpoint
- Runs as a long-lived background service

## Installation and setup

1. Ensure that you have [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0) installed
2. Clone the repository and change directory
    ```bash
    git clone https://github.com/CategoryCory/TempHumidityDBBackend.git
    cd TempHumidityDBBackend
    ```
3. Restore solution
    ```bash
    dotnet restore
    ```
4. Create a copy of `appsettings.Example.json` called `appsettings.json`
    ```bash
    cp appsettings.Example.json appsettings.json
    ```
5. Add settings to `appsettings.json` that correspond to your setup

## Incoming UDP Data Format

The ESP32 sends a binary UDP payload encoded in CBOR (Concise Binary Object Representation). After decoding, the payload structure is as follows:

```json
{
    "temperature_celsius": 28.0,
    "relative_humidity": 53.7
}
```

## Outgoing REST request

Each decoded payload is forwarded as a JSON object via HTTP `POST` to the configured endpoint.

### Request

```
POST {RestEndpointUrl}
Content-Type: application/json
```

### Body

```json
{
    "temperature_celsius": 28.0,
    "relative_humidity": 53.7
}
```
_The field names and structure are preserved from the decoded CBOR message._

## How to Run

Ensure you are using *.NET 9*.

You can run the service directly by using the .NET CLI:
```bash
dotnet run
```

Or configure it as a service for persistent background execution. For example, in Linux:
```ini
# /etc/systemd/system/temphumiditybackend.service
[Unit]
Description=TempHumidityBackend UDP to REST Relay
After=network.target

[Service]
ExecStart=/usr/bin/dotnet /path/to/TempHumidityBackend.dll
WorkingDirectory=/path/to/
Restart=always
User=your-user

[Install]
WantedBy=multi-user.target
```

Then:
```bash
sudo systemctl enable temphumiditybackend
sudo systemctl start temphumiditybackend
```

## Dependencies

- [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
- [System.Formats.Cbor](https://www.nuget.org/packages/System.Formats.Cbor/)
- `Microsoft.Extensions.Hosting` for `BackgroundService` support

## License

MIT license
