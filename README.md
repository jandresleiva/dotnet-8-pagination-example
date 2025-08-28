# .NET 8 Pagination Example

This repository contains a minimal example of implementing **pagination** in a .NET 8 Web API using controllers, a service layer, and a lightweight **record DTO** for the response envelope.

It uses the default **WeatherForecast** template, extended to generate 100 sample entries in memory. Pagination is applied in the service layer with support for an optional `totalCount`.

Here you can find the full article [Minimalistic Pagination in .NET 8 (LTS) Using a Record DTO](https://medium.com/@andresleiva.4/minimalistic-pagination-in-net-8-lts-using-a-record-dto-836f3d3be6fb).

Feel free to navigate my other articles at [Medium](https://medium.com/@andresleiva.4).

---

## Features

- ✅ ASP.NET Core 8 Web API with controllers  
- ✅ `record` DTO for clean, immutable pagination responses  
- ✅ Service layer encapsulating paging logic  
- ✅ Optional `totalCount` for clients that need to calculate pages  
- ✅ OpenAPI/Swagger for interactive API exploration  

---

## Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

### Run the project

```bash
dotnet run --project dotnet_pagination_basics
```

The API will be available at:
```
https://localhost:5293
```

Explore the contract

Swagger/OpenAPI definition is exposed at:
```
https://localhost:5293/swagger/v1/swagger.json
```

Swagger UI is available at:
```
https://localhost:5293/swagger
```

## Example Usage

Endpoint:
```
GET /WeatherForecast?pageNumber=2&getTotalCount=true
```

Response:
```
{
  "items": [
    { "date": "2025-09-03", "temperatureC": 17, "summary": "Mild" },
    { "date": "2025-09-04", "temperatureC": 6, "summary": "Cool" }
    // ...
  ],
  "page": 2,
  "pageSize": 15,
  "totalCount": 100
}
```
Project Structure
```
dotnet-8-pagination-example/
├── Controllers/
│   └── WeatherForecastController.cs
├── Services/
│   ├── DTOs/
│   │   └── PaginatedResults.cs
│   ├── IWeatherForecastService.cs
│   └── WeatherForecastService.cs
├── Program.cs
├── WeatherForecast.cs
└── ...
```

## Notes

In this sample, data is pre-generated in memory (100 entries).

In a real-world app:

WeatherForecastService would query a database.

totalCount would be computed with CountAsync().

Pagination should always be applied on a sorted query to keep page boundaries consistent.

## License

This repository is provided for educational purposes and is licensed under the MIT License.

