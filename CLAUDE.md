# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Common Commands

### Building the Solution
```bash
dotnet build Dukas.Net.sln
```

### Running Tests
```bash
# Run all tests
dotnet test

# Run tests with coverage
dotnet test -p:CollectCoverage=true -p:CoverletOutput=./TestResults/coverage.opencover.xml -p:CoverletOutputFormat=opencover Bi5.Net.Tests/

# Run a specific test class
dotnet test --filter "FullyQualifiedName~Bi5.Net.Tests.ArrayUtilsTest"

# Run a specific test method
dotnet test --filter "FullyQualifiedName~Bi5.Net.Tests.ArrayUtilsTest.ToTickArray_ShouldWork"
```

### Code Quality
```bash
# Restore NuGet packages
dotnet restore Dukas.Net.sln
```

### Publishing
```bash
# Package the Bi5.Net library as a NuGet package
dotnet pack Bi5.Net/Bi5.Net.csproj --configuration Release
```

## Project Architecture

Dukas.Net is a .NET solution for downloading and processing financial tick data from Dukascopy. It consists of three projects:

1. **Dukas.Net** - Console application that provides a command-line interface to fetch and resample tick data
2. **Bi5.Net** - Core library (published as a NuGet package) that handles the actual data fetching, parsing, and conversion
3. **Bi5.Net.Tests** - Unit tests for the Bi5.Net library

### Key Components

#### Dukas.Net (CLI)
- **Program.cs** - Entry point for the CLI app
- **CmdOptions.cs** - Base class for command-line options
- **FetchOptions.cs** - Options for the fetch command
- **ResampleOptions.cs** - Options for the resample command

#### Bi5.Net (Core Library)
- **Loader.cs** - Main engine that handles fetching and processing of tick data
- **IO/** - File handling components for reading/writing tick data
   - **FileWriter.cs** - Base abstract file writer
   - **TickDataFileWriter.cs** - Writer for tick data files
   - **OhlcvFileWriter.cs** - Writer for OHLCV (candle) data
- **Models/** - Data models for financial instruments and configuration
- **Net/** - Network components for communicating with Dukascopy servers
- **Products/** - Definitions of financial products available from Dukascopy
- **Utils/** - Utility classes for time manipulation, compression, etc.

### Data Flow

1. User executes the CLI with either "fetch" or "resample" command
2. Command-line arguments are parsed into a `LoaderConfig` object
3. The `Loader` class uses this configuration to:
   - For fetch: Download tick data from Dukascopy and store locally
   - For resample: Process existing tick data to generate new time frames
4. The data is processed and saved to the specified output directory

### Supported Operations

- **Fetch**: Download tick data from Dukascopy and convert to desired timeframe
- **Resample**: Convert existing tick data to different timeframes without re-downloading

### Data Formats

- Input: Proprietary Bi5 format from Dukascopy
- Output: CSV files with OHLCV (Open, High, Low, Close, Volume) data
- Optional compression with GZip

### Timeframe Support

The library supports various timeframes for data processing:
- Tick
- Second
- Minute
- Hour
- Day
- Month
- Year

Files can be organized by different time scales (Minute, Hour, Day, Month, Year) as specified by the FileScale parameter.