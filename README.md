# Password Analyzer

## Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Quick Start](#quick-start)
- [Installation](#installation)
- [Usage](#usage)
- [Project Structure](#project-structure)
- [Password Analysis Criteria](#password-analysis-criteria)
- [API Reference](#api-reference)
- [Testing](#testing)
- [Deployment](#deployment)
- [Security](#security)
- [Contributing](#contributing)
- [License](#license)

## Overview

Password Analyzer is a web application built with ASP.NET Core MVC that evaluates password 
strength based on industry-standard security criteria. The application provides real-time 
feedback, detailed analysis, and actionable recommendations to help users create secure passwords.

## Features

- **Real-time Password Analysis**: Instant feedback on password strength
- **Security Scoring**: 0-100 point scoring system with clear categorization
- **Detailed Requirements**: Lists met and missing security criteria
- **Privacy-Focused**: No password storage or logging
- **Apple-Inspired UI**: Clean, modern interface with intuitive design
- **Responsive Design**: Works on desktop, tablet, and mobile devices
- **No Dependencies**: Self-contained application with minimal external requirements

## Quick Start

### Prerequisites
- .NET 7.0 SDK or higher
- Git (for cloning repository)

### Clone and Run
```bash
# Clone the repository
git clone https://github.com/Naoyuki-Christopher-H/password-analyzer-cs.git

# Navigate to project directory
cd password-analyzer-cs

# Restore dependencies
dotnet restore

# Run the application
dotnet run
```

Access the application at: `https://localhost:5001`

## Installation

### Detailed Installation Steps

1. **Clone the Repository**
   ```bash
   git clone https://github.com/Naoyuki-Christopher-H/password-analyzer-cs.git
   cd password-analyzer-cs
   ```

2. **Build the Project**
   ```bash
   dotnet build
   ```

3. **Trust HTTPS Development Certificate**
   ```bash
   dotnet dev-certs https --trust
   ```

4. **Run the Application**
   ```bash
   dotnet run
   ```

### Docker Installation
```bash
# Build Docker image
docker build -t password-analyzer .

# Run container
docker run -d -p 8080:80 --name password-analyzer-app password-analyzer
```

## Usage

### Basic Usage

1. **Navigate to the Application**
   Open your browser and go to `https://localhost:5001`

2. **Access Password Analyzer**
   - Click "Analyzer" in the navigation menu
   - Or click "Start Analyzing" on the homepage

3. **Analyze Your Password**
   - Enter your password in the input field
   - Click "Analyze Password" button
   - Review the analysis results

### Password Analysis Results

The analyzer provides:
- **Strength Rating**: Weak, Medium, or Strong
- **Numerical Score**: 0-100 points
- **Requirements Met**: List of satisfied security criteria
- **Requirements Missing**: Areas needing improvement
- **Statistics**: Password length and character composition
- **Recommendations**: Specific suggestions for enhancement

### Features

- **Show/Hide Password**: Toggle password visibility
- **Real-time Feedback**: Strength meter updates as you type
- **Mobile Responsive**: Works on all device sizes
- **Privacy Assurance**: No data storage or transmission

## Project Structure

```
password-analyzer-cs/
├── Controllers/
│   ├── HomeController.cs        # Home and privacy pages
│   └── PasswordController.cs    # Password analysis logic
├── Models/
│   ├── ErrorViewModel.cs        # Error handling model
│   └── PasswordAnalysisResult.cs # Analysis results model
├── Views/
│   ├── Home/
│   │   ├── Index.cshtml         # Homepage
│   │   └── Privacy.cshtml       # Privacy policy
│   ├── Password/
│   │   └── Analyze.cshtml       # Analysis interface
│   └── Shared/
│       ├── _Layout.cshtml       # Main layout
│       ├── _ViewImports.cshtml  # View imports
│       ├── _ViewStart.cshtml    # View initialization
│       └── Error.cshtml         # Error page
├── wwwroot/
│   ├── css/
│   │   ├── apple-style.css      # Apple-inspired styles
│   │   └── site.css            # Base styles
│   ├── js/
│   │   └── site.js             # Client-side scripts
│   └── lib/                    # Third-party libraries
├── Documentation/
│   └── DOCUMENTATION.md        # Complete documentation
├── Program.cs                  # Application entry point
├── appsettings.json           # Configuration
└── password-analyzer-cs.csproj # Project file
```

## Password Analysis Criteria

### Scoring System (0-100 points)

#### Length Requirements
- **0-7 characters**: 0 points
- **8-11 characters**: 2 points per character (max 22)
- **12+ characters**: 2 points per character + 10 bonus points
- **Maximum**: 40 points

#### Character Variety (15 points each)
- Uppercase letters (A-Z)
- Lowercase letters (a-z)
- Numbers (0-9)
- Special characters (!@#$%^&*, etc.)

#### Penalties
- **Common passwords**: -30 points
- Score is capped between 0-100

### Strength Classification

- **Weak**: 0-49 points
- **Medium**: 50-79 points
- **Strong**: 80-100 points

### Security Requirements

The analyzer checks for these criteria:

1. **Minimum 8 characters**
2. **At least one uppercase letter**
3. **At least one lowercase letter**
4. **At least one number**
5. **At least one special character**
6. **Not a common password**

### Common Password Detection

The system checks against a list of common passwords including:
- "password", "123456", "12345678", "123456789"
- "qwerty", "abc123", "password1", "admin", "welcome"

## API Reference

### Endpoints

#### Home Controller
- `GET /` - Application homepage
- `GET /Home/Privacy` - Privacy policy page
- `GET /Home/Error` - Error handling page

#### Password Controller
- `GET /Password/Analyze` - Display password analysis form
- `POST /Password/Analyze` - Submit password for analysis

### Request Format

#### Password Analysis Request
```http
POST /Password/Analyze
Content-Type: application/x-www-form-urlencoded

password=YourPassword123!
```

### Response Model

```csharp
public class PasswordAnalysisResult
{
    public string Password { get; set; }           // Analyzed password
    public string Strength { get; set; }           // Weak, Medium, Strong
    public int Score { get; set; }                // 0-100 points
    public int MaxScore { get; set; } = 100;      // Maximum possible score
    public List<string> RequirementsMet { get; set; }    // Satisfied criteria
    public List<string> RequirementsMissing { get; set; } // Missing criteria
    public int Length { get; set; }               // Password length
    public bool HasUpperCase { get; set; }        // Contains uppercase
    public bool HasLowerCase { get; set; }        // Contains lowercase
    public bool HasNumbers { get; set; }          // Contains numbers
    public bool HasSpecialChars { get; set; }     // Contains special chars
    public bool IsCommonPassword { get; set; }    // Is common password
}
```

## Testing

### Running Tests

```bash
# Run all tests
dotnet test

# Run with coverage
dotnet test --collect:"XPlat Code Coverage"
```

### Test Categories

1. **Unit Tests**
   - Password analysis logic
   - Model validation
   - Controller methods

2. **Integration Tests**
   - API endpoint testing
   - Database integration (if applicable)
   - External service integration

3. **Functional Tests**
   - User workflow testing
   - UI interaction testing
   - Cross-browser compatibility

### Example Test

```csharp
[Fact]
public void AnalyzePassword_StrongPassword_ReturnsHighScore()
{
    // Arrange
    var controller = new PasswordController();
    string password = "StrongP@ssw0rd123!";
    
    // Act
    var result = controller.Analyze(password) as ViewResult;
    var model = result.Model as PasswordAnalysisResult;
    
    // Assert
    Assert.True(model.Score >= 80);
    Assert.Equal("Strong", model.Strength);
    Assert.Contains("Uppercase letter", model.RequirementsMet);
    Assert.Contains("Special character", model.RequirementsMet);
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
2. Create IIS site with application pool (.NET CLR: No Managed Code)
3. Configure SSL certificate
4. Deploy published files to site directory

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
    
    ssl_certificate /path/to/certificate.pem;
    ssl_certificate_key /path/to/private.key;
    
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

#### Step 3: Configure System Service
```ini
[Unit]
Description=Password Analyzer Web Application
After=network.target

[Service]
Type=exec
WorkingDirectory=/var/www/password-analyzer
ExecStart=/usr/bin/dotnet /var/www/password-analyzer/password-analyzer-cs.dll
Restart=always
RestartSec=10
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
RUN dotnet restore
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "password-analyzer-cs.dll"]
```

#### Docker Commands
```bash
# Build image
docker build -t password-analyzer .

# Run container
docker run -d -p 8080:80 -p 8443:443 --name password-analyzer password-analyzer

# View logs
docker logs password-analyzer
```

### Cloud Deployment

#### Azure App Service
```bash
# Create and deploy
az webapp up --name password-analyzer --runtime "DOTNET|7.0"
```

#### AWS Elastic Beanstalk
```bash
# Initialize
eb init -p "dotnet-7.0" password-analyzer

# Create environment
eb create password-analyzer-env

# Deploy
eb deploy
```

## Security

### Data Protection

- **No Password Storage**: Passwords are never written to disk or databases
- **In-Memory Processing**: Analysis occurs in server memory only
- **Secure Disposal**: Memory cleared after analysis completion
- **HTTPS Enforcement**: All communications encrypted with TLS

### Input Validation

- **Server-Side Validation**: All input validated on server
- **Regex Pattern Matching**: Character type validation using regular expressions
- **Length Constraints**: Maximum password length enforcement
- **Encoding Validation**: UTF-8 character encoding verification

### Security Headers

The application implements these security headers:
- `Content-Security-Policy`: Restricts resource loading
- `X-Content-Type-Options: nosniff`: Prevents MIME type sniffing
- `X-Frame-Options: DENY`: Prevents clickjacking
- `X-XSS-Protection: 1; mode=block`: Enables XSS protection
- `Strict-Transport-Security`: Enforces HTTPS

### Privacy Features

- **No Data Collection**: No analytics or user tracking
- **Local Processing**: All analysis occurs on server
- **Transparent Policy**: Clear privacy policy available
- **User Control**: No persistent user data

## Contributing

### Development Setup

1. **Fork the Repository**
   ```bash
   fork https://github.com/Naoyuki-Christopher-H/password-analyzer-cs.git
   ```

2. **Create Feature Branch**
   ```bash
   git checkout -b feature/your-feature-name
   ```

3. **Make Changes**
   - Follow existing code style
   - Add tests for new functionality
   - Update documentation as needed

4. **Test Your Changes**
   ```bash
   dotnet test
   ```

5. **Commit and Push**
   ```bash
   git commit -m "Add your feature description"
   git push origin feature/your-feature-name
   ```

6. **Create Pull Request**
   - Describe changes clearly
   - Reference related issues
   - Ensure all tests pass

### Code Standards

- **C#**: Follow Allman style braces
- **Naming**: Use meaningful, descriptive names
- **Comments**: Add comments for complex logic
- **Testing**: Maintain high test coverage
- **Documentation**: Update docs for new features

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
