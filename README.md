# Avalanche Comments!!!

A beautiful, avalanche-themed ASP.NET Core Blazor web application for collecting user comments. The application features a cool, refreshing interface inspired by mountain avalanches and winter themes.

## Features

- ❄️ **Avalanche Theme**: Cool blue gradients, snowflake icons, and mountain-inspired design
- 💬 **Comment System**: Users can submit comments with their name and message
- 📱 **Responsive Design**: Works on desktop and mobile devices
- 🎨 **Beautiful UI**: Modern, clean interface with smooth animations and hover effects
- 🏔️ **Mountain Aesthetics**: Mountain silhouette effects and winter color palette

## Getting Started

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later

### Running the Application

1. Navigate to the application directory:
   ```bash
   cd AvalancheComments
   ```

2. Run the application:
   ```bash
   dotnet run
   ```

3. Open your browser and navigate to `http://localhost:5000`

### Building the Application

To build the application:

```bash
cd AvalancheComments
dotnet build
```

Or build the entire solution:

```bash
dotnet build Avalanche.sln
```

## Testing

### Running Tests

To run all tests:

```bash
dotnet test
```

Or with verbose output:

```bash
dotnet test --verbosity normal
```

### Code Coverage

To run tests with code coverage collection:

```bash
dotnet test --collect:"XPlat Code Coverage"
```

This will generate a Cobertura XML coverage report in the `TestResults` directory. The coverage file can be found at:

```
AvalancheComments.Tests/TestResults/{guid}/coverage.cobertura.xml
```

You can use tools like [ReportGenerator](https://github.com/danielpalme/ReportGenerator) to convert the coverage report to HTML:

```bash
# Install ReportGenerator
dotnet tool install -g dotnet-reportgenerator-globaltool

# Generate HTML report
reportgenerator -reports:"AvalancheComments.Tests/TestResults/*/coverage.cobertura.xml" -targetdir:"coveragereport" -reporttypes:Html
```

## Project Structure

- `Avalanche.sln` - Solution file containing all projects
- `AvalancheComments/` - Main web application
  - `Components/Pages/Comments.razor` - Main comments page with form and display
  - `Components/Pages/Home.razor` - Welcome page
  - `Components/Layout/` - Layout components including navigation
  - `Models/CommentModel.cs` - Data model for comments
  - `wwwroot/app.css` - Custom CSS with avalanche theme styling
- `AvalancheComments.Tests/` - xUnit test project
  - `CommentModelTests.cs` - Unit tests for CommentModel validation

## Design Theme

The application uses an avalanche-inspired color scheme:
- Primary Blues: #0277bd, #01579b, #0288d1
- Light Blues: #e3f2fd, #b3e5fc, #e1f5fe
- Accents: Snowflake emojis (❄️) and mountain emojis (🏔️)

## Screenshots

### Home Page
![Home Page](https://github.com/user-attachments/assets/719f18aa-8e0e-4c68-ba75-86d2ba2ba7b9)

### Comments Page (Empty)
![Comments Page](https://github.com/user-attachments/assets/0b452093-1781-4822-8fd4-0334e85126e4)

### Comments Page (With Comments)
![Comments with Multiple Entries](https://github.com/user-attachments/assets/c8dd84e1-6e64-4b31-9c76-04dc5480ed86)

## Technologies Used

- ASP.NET Core 8.0
- Blazor Server
- C#
- HTML/CSS
- Bootstrap Icons

## Build Badge

[![CI Pipeline](https://github.com/rob-foulkrod/avalanche/actions/workflows/main.yml/badge.svg)](https://github.com/rob-foulkrod/avalanche/actions/workflows/main.yml)

## License

See [LICENSE](LICENSE) file for details.
