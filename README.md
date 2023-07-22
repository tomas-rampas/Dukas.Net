# Dukas.Net  ![CI Build](https://github.com/tomas-rampas/Dukas.Net/actions/workflows/ci_build.yml/badge.svg) ![Test Coverage](https://img.shields.io/endpoint?url=https://gist.githubusercontent.com/tomas-rampas/240a53fad3b4d85a4f79ab772e84cb6a/raw/code-coverage.json) [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
Download tick data from Dukascopy and convert it to desired time frame resolution. Dukas.Net is a service wrapper of Bi5.Net nuget package.

#### Prerequisite
[.NET Runtime 7.X](https://dotnet.microsoft.com/en-us/download/dotnet/7.0) installed

#### Command Line Help

All available verbs and parameters can be enumerated by running help command: 

```
dukas.net --help
```

#### Command Line Verbs
|Verb | Description |
|:---|:---|
|fetch|Fetch and Resample tick data of given products|
|resample|Perform resampling of existing Tick data; there is not any data downloaded from Dukascopy servers; existing fetched tick data are being used for resampling |

#### Command Line Arguments
Below is a list of all arguments mutual for all verbs. 

|Short | Long | Description |
|:---|:---|:---|
|-s|--start-date|Required. Tick data start date|
|-e|--end-date|Tick data end date. Current date time is used when argument not provided|
|-p|--products|Required. Comma seprated list of tick data of demanded products (e.g. -p EURUSD,BRENTCMDUSD,DEUIDXEUR,XAUSUD)  |
|-q|--quote-side|(Default: Bid) Requested Quote side. Possible values: Bid, Ask, Both|
| |--major-scale|(Default: Day) Time Frame major scale. Possible values: Tick, Sec, Min, Hour, Day, Month, Year|
| |--minor-scale|(Default: 1) Time Frame minor scale|
|-o|--output-dir|Required. Output data directory|
|-f|--file-scale|(Default Day). File Scale defines time frequency for the data in file. E.g. when FileScale is FileScale.Day data will be grouped by Day and store in the file with frequency where one file represents one day of data. I.e. consideriderin --major-scale Day and --file-scale Day then resulting file will contain one record; Possible values: Min, Hour, Day, Month, Year |
|-h|--header|(Default: false) Include header in resulting data file|
|-g|--gzip|(Default: false) Compress result|
| |--help|Display detailed help screen including all possible verbs.|
| |--version|Display version information.|

#### All arguments sample

#### Fetch
Get 1 Minute OHLCV Bid and Ask quotes data between 1st of Jan. 2020 and 31st of Dec. 2020 for given list of products; store gzipped Bid and Ask data into the e:\temp directory. 
This command also creates Tick data sub-directory for every donwloded product data so it can be used for resampling to other time frames later. 

```
dukas.net fetch -s "2020-01-01 00:00:00" -e "2020-12-31 23:59:59" -p DEUIDXEUR,GBRIDXGBP,USA30IDXUSD,AUSIDXAUD,LIGHTCMDUSD -o "e:\temp" -q Both --major-scale Min --minor-scale 1 --file-scale Day --header -g
```
#### Resample
Reample command is reusing fetched tick data and resampling them to another time frame resultions without touching Dukascopy servers. Folowing sample shows how to get 10 Minute OHLCV Bid and Ask bars. Other settings are same as in sample for fetch verb above. 
```
dukas.net resample -s "2020-01-01 00:00:00" -e "2020-12-31 23:59:59" -p DEUIDXEUR,GBRIDXGBP,USA30IDXUSD,AUSIDXAUD,LIGHTCMDUSD -o "e:\temp" -q Both --major-scale Min --minor-scale 10 --file-scale Day --header -g
```

TBC

# Bi5.Net
[Nuget package](https://www.nuget.org/packages/Bi5.Net/) for requesting and converting Dukascopy tick data. 
I;m aware of lacking proper documentation. However, using the NuGet is quite simple. The NuGet package expects settings passed from command line arguments above through the [LoaderConfig](https://github.com/tomas-rampas/Dukas.Net/blob/main/Bi5.Net/Models/LoaderConfig.cs).
Simply put, NuGet package requires config to be passed from calling process. Config class replicates all command line arguments.

-----------------------------
TBC
