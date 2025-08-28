# .NET 8 Pagination Strategies Example

This repository demonstrates **three different pagination strategies** in a .NET 8 Web API: **Offset-based**, **Keyset**, and **Cursor-based** pagination. Each strategy is implemented with controllers, a service layer, and dedicated **record DTOs** for clean response envelopes.

It uses the default **WeatherForecast** template, extended to generate 100 sample entries in memory with unique IDs. All pagination strategies are applied in the service layer with support for optional `totalCount`.

Here you can find the full article [Minimalistic Pagination in .NET 8 (LTS) Using a Record DTO](https://medium.com/@andresleiva.4/minimalistic-pagination-in-net-8-lts-using-a-record-dto-836f3d3be6fb).

Feel free to navigate my other articles at [Medium](https://medium.com/@andresleiva.4).

---

## Features

- ✅ ASP.NET Core 8 Web API with controllers  
- ✅ **Three pagination strategies implemented:**
  - **Offset-based pagination** - Traditional page/pageSize approach
  - **Keyset pagination** - Uses ID-based filtering for better performance
  - **Cursor-based pagination** - Base64-encoded cursors for stateless navigation
- ✅ Dedicated `record` DTOs for each pagination strategy  
- ✅ Service layer encapsulating all pagination logic  
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

### 1. Offset-based Pagination (Traditional)
```
GET /WeatherForecast?pageNumber=2&getTotalCount=true
```

Response:
```json
{
  "items": [
    { "id": 31, "date": "2025-09-03", "temperatureC": 17, "summary": "Mild" },
    { "id": 32, "date": "2025-09-04", "temperatureC": 6, "summary": "Cool" }
    // ...
  ],
  "page": 2,
  "pageSize": 15,
  "totalCount": 100
}
```

### 2. Keyset Pagination (Performance-optimized)
```
GET /WeatherForecast/keyset?pageSize=10&getTotalCount=true
GET /WeatherForecast/keyset?lastId=15&pageSize=10
```

Response:
```json
{
  "items": [
    { "id": 16, "date": "2025-09-18", "temperatureC": 22, "summary": "Warm" },
    { "id": 17, "date": "2025-09-19", "temperatureC": 8, "summary": "Cool" }
    // ...
  ],
  "pageSize": 10,
  "lastId": 25,
  "hasNextPage": true,
  "totalCount": 100
}
```

### 3. Cursor-based Pagination (Stateless)
```
GET /WeatherForecast/cursor?pageSize=10&getTotalCount=true
GET /WeatherForecast/cursor?cursor=MTU%3D&pageSize=10
```

Response:
```json
{
  "items": [
    { "id": 16, "date": "2025-09-18", "temperatureC": 22, "summary": "Warm" },
    { "id": 17, "date": "2025-09-19", "temperatureC": 8, "summary": "Cool" }
    // ...
  ],
  "pageSize": 10,
  "nextCursor": "MjU=",
  "previousCursor": "NQ==",
  "hasNextPage": true,
  "hasPreviousPage": true,
  "totalCount": 100
}
```
## Project Structure
```
dotnet-pagination-basics/
├── Controllers/
│   └── WeatherForecastController.cs
├── Services/
│   ├── DTOs/
│   │   ├── PaginatedResults.cs          # Offset-based pagination DTO
│   │   ├── KeysetPaginatedResults.cs    # Keyset pagination DTO
│   │   └── CursorPaginatedResults.cs    # Cursor-based pagination DTO
│   ├── IWeatherForecastService.cs
│   └── WeatherForecastService.cs
├── Program.cs
├── WeatherForecast.cs                   # Enhanced with Id property
└── ...
```

## Pagination Strategies Comparison

| Strategy | Performance | Use Case | Pros | Cons |
|----------|-------------|----------|------|------|
| **Offset-based** | Poor for large offsets | Simple UI pagination | Easy to implement, jump to any page | Performance degrades with large datasets |
| **Keyset** | Excellent | Real-time feeds, APIs | Fast, consistent performance | Cannot jump to arbitrary pages |
| **Cursor-based** | Excellent | Stateless APIs, mobile apps | Stateless, bidirectional navigation | More complex implementation |

## Implementation Notes

### Data Generation
- Sample data is pre-generated in memory (100 entries with sequential IDs)
- Each `WeatherForecast` includes an `Id` property for keyset/cursor pagination
- Data is sorted by `Id` to ensure consistent pagination boundaries

### Real-world Considerations
- **Database Integration**: `WeatherForecastService` would query a database using Entity Framework or similar
- **Performance**: `totalCount` would be computed with `CountAsync()` for large datasets
- **Indexing**: Ensure database indexes on pagination keys (e.g., `Id`, `CreatedDate`)
- **Consistency**: Always apply pagination on sorted queries to maintain stable page boundaries

### Cursor Encoding
- Cursors are Base64-encoded for URL safety
- Invalid cursors are gracefully handled (fallback to first page)
- `null` cursor means "start from beginning"

## License

This repository is provided for educational purposes and is licensed under the MIT License.

