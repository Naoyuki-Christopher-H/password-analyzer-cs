# Password Analyzer Documentation

## Table of Contents

- [Overview](#overview)
- [System Requirements](#system-requirements)
- [Installation](#installation)
- [Project Structure](#project-structure)
- [Configuration](#configuration)
- [Usage](#usage)
- [Password Analysis Algorithm](#password-analysis-algorithm)
- [API Endpoints](#api-endpoints)
- [Testing](#testing)
- [Deployment](#deployment)
- [Security](#security)
- [Troubleshooting](#troubleshooting)
- [License](#license)

## Overview

The Password Analyzer is an ASP.NET Core MVC web application that evaluates password 
strength based on security criteria. It provides real-time feedback and recommendations 
to help users create secure passwords.

### Key Features
- Password strength analysis and scoring
- Real-time requirements checking
- No password storage or logging
- Apple-inspired user interface
- Responsive web design

## System Requirements

### Development
- .NET 7.0 SDK or higher
- Visual Studio 2022 or Visual Studio Code
- Git for version control

### Production
- .NET 7.0 Runtime
- Web server (IIS, Nginx, Apache, or Kestrel)
- SSL certificate for HTTPS

## Installation

### Clone and Build
```bash
git clone <repository-url>
cd password-analyzer-cs
dotnet restore
dotnet build
dotnet run
```

### Access Application
Open browser and navigate to:
- Development: `https://localhost:5001`
- Production: Your configured domain

## Project Structure

```
password-analyzer-cs/
├── Controllers/
│   ├── HomeController.cs
│   └── PasswordController.cs
├── Models/
│   ├── ErrorViewModel.cs
│   └── PasswordAnalysisResult.cs
├── Views/
│   ├── Home/
│   │   ├── Index.cshtml
│   │   └── Privacy.cshtml
│   ├── Password/
│   │   └── Analyze.cshtml
│   └── Shared/
│       └── _Layout.cshtml
├── wwwroot/
│   ├── css/
│   │   ├── apple-style.css
│   │   └── site.css
│   └── js/
│   │   ├── password-analyzer.js
│       └── site.js
├── Program.cs
└── appsettings.json
```

## Configuration

### Application Settings
Modify `appsettings.json`:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

### Environment Configuration
Create environment-specific files:
- `appsettings.Development.json`
- `appsettings.Production.json`

### HTTPS Configuration
HTTPS is enabled by default. Configure certificates in:
- Development: `dotnet dev-certs https --trust`
- Production: Configure in web server or `appsettings.Production.json`

## Usage

### Basic Flow
1. Navigate to the application
2. Click "Analyzer" in navigation
3. Enter password in the input field
4. Click "Analyze Password"
5. Review results and recommendations

### Password Analysis Results
- **Strength**: Weak, Medium, or Strong
- **Score**: 0-100 points
- **Requirements Met**: List of satisfied criteria
- **Requirements Missing**: List of unsatisfied criteria
- **Statistics**: Length and character type analysis

### Features
- Password visibility toggle (Show/Hide)
- Real-time strength indicator
- Detailed security recommendations
- Mobile-responsive design

## Password Analysis Algorithm

### Scoring System (0-100 points)

#### Length Scoring
- 8-11 characters: 2 points per character
- 12+ characters: 2 points per character + 10 bonus points
- Maximum: 40 points

#### Character Variety (15 points each)
- Uppercase letters
- Lowercase letters
- Numbers
- Special characters

#### Penalties
- Common passwords: -30 points
- Total score capped at 0-100 range

### Strength Classification
- **Weak**: 0-49 points
- **Medium**: 50-79 points
- **Strong**: 80-100 points

### Security Criteria
1. Minimum 8 characters
2. At least one uppercase letter
3. At least one lowercase letter
4. At least one number
5. At least one special character
6. Not a common password

### Common Passwords List
The system checks against these common passwords:
- "password", "123456", "12345678", "123456789"
- "qwerty", "abc123", "password1", "admin", "welcome"

## API Endpoints

### Home Controller
- `GET /` - Home page
- `GET /Home/Privacy` - Privacy policy page
- `GET /Home/Error` - Error page

### Password Controller
- `GET /Password/Analyze` - Password analysis form
- `POST /Password/Analyze` - Submit password for analysis

### Request/Response Format

#### POST /Password/Analyze
**Request:**
```http
POST /Password/Analyze
Content-Type: application/x-www-form-urlencoded

password=YourPassword123!
```

**Response:**
- Returns HTML view with analysis results
- Model: `PasswordAnalysisResult`

#### PasswordAnalysisResult Model
```csharp
public class PasswordAnalysisResult
{
    public string Password { get; set; }
    public string Strength { get; set; } // Weak, Medium, Strong
    public int Score { get; set; } // 0-100
    public int MaxScore { get; set; } = 100;
    public List<string> RequirementsMet { get; set; }
    public List<string> RequirementsMissing { get; set; }
    public int Length { get; set; }
    public bool HasUpperCase { get; set; }
    public bool HasLowerCase { get; set; }
    public bool HasNumbers { get; set; }
    public bool HasSpecialChars { get; set; }
    public bool IsCommonPassword { get; set; }
}
```

## Testing

### Run Tests
```bash
dotnet test
```

### Test Categories
1. **Unit Tests**: Individual component testing
2. **Integration Tests**: Component interaction testing
3. **Functional Tests**: User workflow testing

### Example Test
```csharp
[Fact]
public void Analyze_StrongPassword_ReturnsHighScore()
{
    var controller = new PasswordController();
    string password = "StrongP@ssw0rd123!";
    
    var result = controller.Analyze(password) as ViewResult;
    var model = result.Model as PasswordAnalysisResult;
    
    Assert.True(model.Score >= 80);
    Assert.Equal("Strong", model.Strength);
}
```

## Deployment

### Self-Hosted Deployment

#### Step 1: Publish Application
```bash
dotnet publish -c Release -o ./publish
```

#### Step 2: Configure Web Server

**IIS Configuration:**
1. Install .NET Hosting Bundle
2. Create application pool (No Managed Code)
3. Deploy to site directory
4. Configure SSL certificate

**Nginx Configuration:**
```nginx
server {
    listen 80;
    server_name yourdomain.com;
    return 301 https://$server_name$request_uri;
}

server {
    listen 443 ssl;
    server_name yourdomain.com;
    
    ssl_certificate /path/to/cert.pem;
    ssl_certificate_key /path/to/key.pem;
    
    location / {
        proxy_pass http://localhost:5000;
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection keep-alive;
        proxy_set_header Host $host;
        proxy_cache_bypass $http_upgrade;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
    }
}
```

#### Step 3: Configure Service
Create systemd service:
```ini
[Unit]
Description=Password Analyzer
After=network.target

[Service]
Type=exec
WorkingDirectory=/var/www/password-analyzer
ExecStart=/usr/bin/dotnet /var/www/password-analyzer/password-analyzer-cs.dll
Restart=always
Environment=ASPNETCORE_ENVIRONMENT=Production

[Install]
WantedBy=multi-user.target
```

### Docker Deployment

#### Dockerfile
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY . .
RUN dotnet build "password-analyzer-cs.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "password-analyzer-cs.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "password-analyzer-cs.dll"]
```

#### Docker Commands
```bash
docker build -t password-analyzer .
docker run -d -p 8080:80 --name password-analyzer-app password-analyzer
```

### Cloud Deployment

#### Azure App Service
```bash
az webapp up --name password-analyzer --runtime "DOTNET|7.0"
```

#### AWS Elastic Beanstalk
```bash
eb init -p "dotnet-7.0" password-analyzer
eb create password-analyzer-env
eb deploy
```

## Security

### Data Protection
- No password storage in databases or logs
- In-memory processing only
- HTTPS encryption required
- Secure session management

### Input Validation
- Server-side validation
- Regex pattern matching
- Length constraints
- Character encoding validation

### Security Headers
The application includes these security headers:
- Content-Security-Policy
- X-Content-Type-Options: nosniff
- X-Frame-Options: DENY
- X-XSS-Protection: 1; mode=block
- Strict-Transport-Security

### Privacy Features
- No user tracking
- No analytics data collection
- Transparent privacy policy
- Local analysis only

## Troubleshooting

### Common Issues

#### Application Won't Start
1. Check .NET SDK version: `dotnet --version`
2. Verify dependencies: `dotnet restore`
3. Check port availability
4. Review application logs

#### HTTPS Certificate Issues
```bash
# Development certificate
dotnet dev-certs https --clean
dotnet dev-certs https --trust

# Check certificate
dotnet dev-certs https --check
```

#### Performance Issues
1. Check server resources
2. Review application logs
3. Monitor network traffic
4. Verify database connections (if applicable)

### Logging
Application logs are written to:
- Console (development)
- Default logging providers (production)
- Configure in `appsettings.json`

### Error Handling
- Development: Detailed error pages
- Production: Generic error messages
- Custom error page at `/Home/Error`

## License

This project is licensed under the MIT License.

### Third-Party Licenses
- **Bootstrap 5**: MIT License
- **Bootstrap Icons**: MIT License
- **.NET 7.0**: MIT License

### Attribution
When using this software, please include:
- Link to original repository
- Copyright notice
- MIT License text

---
