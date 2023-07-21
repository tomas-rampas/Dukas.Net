# Dukas.Net  [![Build Status](https://app.travis-ci.com/tomas-rampas/Dukas.Net.svg?branch=main)](https://app.travis-ci.com/tomas-rampas/Dukas.Net)  [![Coverage Status](https://coveralls.io/repos/github/tomas-rampas/Dukas.Net/badge.svg?branch=main)](https://coveralls.io/github/tomas-rampas/Dukas.Net?branch=main)
Download tick data from Dukascopy and convert it to desired time frame resolution. Dukas.Net is a service wrapper of Bi5.Net nuget package.

#### Prerequisite
.NET Core 6/7 installed

#### Command Line Arguments

All available parameters can be enumerated by running help command: 

```
dukas.net --help
```

Below is a list of all arguments.

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
| |--help|Display this help screen.|
| |--version|Display version information.|

#### All arguments sample

Get 1 Minute OHLCV Bid and Ask quotes data between 1st of Jan. 2020 and 31st of Dec. 2020 for given list of products; store gzipped Bid and Ask data into the e:\temp directory. 
This command also creates Tick data sub-directory for every donwloded product data so it can be used for resampling to other time frames later. It's considered for future enhancmenet to resample Ticks to another time frame rosultion without touching Dukascopy servers.

```
dukas.net fetch -s "2020-01-01 00:00:00" -e "2020-12-31 23:59:59" -p DEUIDXEUR,GBRIDXGBP,USA30IDXUSD,AUSIDXAUD,LIGHTCMDUSD -o "e:\temp" -q Both --major-scale Min --minor-scale 1 --file-scale Day --header -g
```

Get 10 Minute OHLCV Bid and Ask bars. Other settings are same as in sample above. 
```
dukas.net resample -s "2020-01-01 00:00:00" -e "2020-12-31 23:59:59" -p DEUIDXEUR,GBRIDXGBP,USA30IDXUSD,AUSIDXAUD,LIGHTCMDUSD -o "e:\temp" -q Both --major-scale Min --minor-scale 10 --file-scale Day --header -g
```

TBC

# Bi5.Net
Nuget package for requesting and converting Dukascopy tick data 

-----------------------------
TBC
