# Dukas.Net  [![Build Status](https://app.travis-ci.com/tomas-rampas/Dukas.Net.svg?branch=main)](https://app.travis-ci.com/tomas-rampas/Dukas.Net)
Download tick data from Dukascopy and convert it to desired time frame resolution. Dukas.Net is a service wrapper of Bi5.Net nuget package.

#### Command Line Arguments

Below is list of all available arguments. Similar list can be also shown by running help command: 

```
dukas.net --help
```

|Short | Long | Description |
|---|---|---|
|-s|--start-date|Required. Tick data start date|
|-e|--end-date|Tick data end date. Current date time is used when argument not provided|
|-p|--products|Required. List of tick data demanded products|
|-q|--quote-side|(Default: Bid) Requested Quote side. Possible values: Bid, Ask, Both|
| |--major-scale|(Default: Day) Time Frame major scale. Possible values: Sec, Min, Hour, Day, Week, Month, Year|
| |--minor-scale|(Default: 1) Time Frame minor scale|
|-o |--output-dir   |Required. Output data directory|
| |-help|Display this help screen.|
| |--version|Display version information.|

#### All arguments sample

Get 1 Minute OHLCV Bid-side data between 1st of Jan. 2020 and 31st of Dec. 2020
for given list of products; store the data into the C:\temp

```
dukas.net -s 2020-01-01 -e 2020-12-31 -p "EURUSD,GBPUSD,BTCUSD,DEUIDXEUR"
--major-scale Min --minor-scale 1 -o "c:\temp" -q Bid
```

TBC

# Bi5.Net
Nuget package for requesting and converting Dukascopy tick data 

-----------------------------
TBC
