# Dukas.Net  ![CI Build](https://github.com/tomas-rampas/Dukas.Net/actions/workflows/ci-code.yml/badge.svg) ![Test Coverage](https://img.shields.io/endpoint?url=https://gist.githubusercontent.com/tomas-rampas/240a53fad3b4d85a4f79ab772e84cb6a/raw/code-coverage.json) [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
Download tick data from Dukascopy and convert it to desired time frame resolution. Dukas.Net is a service wrapper of Bi5.Net nuget package.

#### Command Line Help

All available verbs and parameters can be enumerated by running help command: 

```
dukas.net --help
```

#### Command Line Verbs
|Verb | Description |
|:---|:---|
|fetch|Fetch and resample tick data for specified products|
|resample|Perform resampling of existing tick data without downloading from Dukascopy servers|

#### Command Line Arguments
Below is a list of all arguments common to all verbs. 

|Short | Long | Description                                                                                                                                                                                                                                                                                                                                                                  |
|:---|:---|:-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
|-s|--start-date| Required. Tick data start date                                                                                                                                                                                                                                                                                                                                               |
|-e|--end-date| Tick data end date. Current date and time is used when not provided                                                                                                                                                                                                                                                                                                     |
|-p|--products| Required. Comma-separated list of products (e.g. -p EURUSD,BRENTCMDUSD,DEUIDXEUR,XAUSUD)                                                                                                                                                                                                                                                                |
|-q|--quote-side| (Default: Bid) Requested quote side. Possible values: Bid, Ask, Both                                                                                                                                                                                                                                                                                                         |
| |--major-scale| (Default: Day) Time frame major scale. Possible values: Tick, Second, Minute, Hour, Day, Month, Year                                                                                                                                                                                                                                                                        |
| |--minor-scale| (Default: 1) Time frame minor scale                                                                                                                                                                                                                                                                                                                                          |
|-o|--output-dir| Required. Output data directory                                                                                                                                                                                                                                                                                                                                              |
|-f|--file-scale| (Default: Day). File scale defines the time frequency for data in each file. For example, when FileScale is Day, data will be grouped by day with one file per day. When using --major-scale Day and --file-scale Day, the resulting file will contain one record. Possible values: Hour, Day, Month, Year |
|-h|--header| (Default: false) Include header in resulting data file                                                                                                                                                                                                                                                                                                                       |
|-g|--gzip| (Default: false) Compress result                                                                                                                                                                                                                                                                                                                                             |
| |--help| Display detailed help screen including all possible verbs                                                                                                                                                                                                                                                                                                                   |
| |--version| Display version information                                                                                                                                                                                                                                                                                                                                                 |

#### Examples

#### Fetch
Get 1-minute OHLCV Bid and Ask quotes data between January 1, 2020, and December 31, 2020, for the specified products; store gzipped Bid and Ask data in the e:\temp directory. 
This command also creates a tick data sub-directory for each downloaded product, allowing for later resampling to other time frames.
```
dukas.net fetch -s "2020-01-01 00:00:00" -e "2020-12-31 23:59:59" -p DEUIDXEUR,GBRIDXGBP,USA30IDXUSD,AUSIDXAUD,LIGHTCMDUSD -o "e:\temp" -q Both --major-scale Minute --minor-scale 1 --file-scale Day --header -g
```
#### Resample
The resample command reuses previously fetched tick data to create new time frame resolutions without accessing Dukascopy servers. The following example shows how to generate 10-minute OHLCV Bid and Ask bars. Other settings match the fetch example above.
```
dukas.net resample -s "2020-01-01 00:00:00" -e "2020-12-31 23:59:59" -p DEUIDXEUR,GBRIDXGBP,USA30IDXUSD,AUSIDXAUD,LIGHTCMDUSD -o "e:\temp" -q Both --major-scale Minute --minor-scale 10 --file-scale Day --header -g
```

TBC

# Bi5.Net
[Nuget package](https://www.nuget.org/packages/Bi5.Net/) for requesting and converting Dukascopy tick data. 

I'm aware of lacking proper documentation. However, using the NuGet is quite simple. The NuGet package expects settings passed from command line arguments above through the [LoaderConfig](https://github.com/tomas-rampas/Dukas.Net/blob/main/Bi5.Net/Models/LoaderConfig.cs).

Simply put, NuGet package requires config to be passed from calling process. the [LoaderConfig](https://github.com/tomas-rampas/Dukas.Net/blob/main/Bi5.Net/Models/LoaderConfig.cs) class replicates all command line arguments.

-----------------------------
TBC
