# Dukas.Net  [![Build Status](https://app.travis-ci.com/tomas-rampas/Dukas.Net.svg?branch=main)](https://app.travis-ci.com/tomas-rampas/Dukas.Net)  [![Coverage Status](https://coveralls.io/repos/github/tomas-rampas/Dukas.Net/badge.svg?branch=main)](https://coveralls.io/github/tomas-rampas/Dukas.Net?branch=main)
Download tick data from Dukascopy and convert it to desired time frame resolution. Dukas.Net is a service wrapper of Bi5.Net nuget package.

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
|-p|--products|Required. List of tick data demanded products|
|-q|--quote-side|(Default: Bid) Requested Quote side. Possible values: Bid, Ask, Both|
| |--major-scale|(Default: Day) Time Frame major scale. Possible values: Tick, Sec, Min, Hour, Day, Month, Year|
| |--minor-scale|(Default: 1) Time Frame minor scale|
|-o|--output-dir|Required. Output data directory|
|-f|--file-scale|(Default Day). File Scale defines time frequency for the data in file. E.g. when FileScale is FileScale.Day data will be grouped by Day and store in the file with frequency where one file represents one day of data. I.e. consideriderin --major-scale Day and --file-scale Day then resulting file will contain one record; Possible values: Min, Hour, Day, Month, Year |
|-h|--header|(Default: false) Include header in resulting data file|
| |--help|Display this help screen.|
| |--version|Display version information.|

#### All arguments sample

Get 1 Minute OHLCV Bid-side data between 1st of Jan. 2020 and 31st of Dec. 2020
for given list of products; store the data into the C:\temp

```
dukas.net -s 2020-01-01 -e 2020-12-31 -p "EURUSD,GBPUSD,BTCUSD,DEUIDXEUR" --major-scale Min --minor-scale 1 -o "c:\temp" -q Bid
```

TBC

# Bi5.Net
Nuget package for requesting and converting Dukascopy tick data 

-----------------------------
TBC
