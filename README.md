# Dukas.Net  [![Build Status](https://app.travis-ci.com/tomas-rampas/Dukas.Net.svg?branch=main)](https://app.travis-ci.com/tomas-rampas/Dukas.Net)
Download tick data from Dukascopy and convert it to desired time frame resolution. Dukas.Net is a service wrapper of Bi5.Net nuget package.

## How To

### Command Line Arguments

Get 1 Minute OHLCV Bid-side data between 1st of Jan. 2020 and 31st of Dec. 2020
for given list of products; store the data into the C:\temp

`dukas.net -s 2020-01-01 -e 2020-12-31 -p "EURUSD,GBPUSD,BTCUSD,DEUIDXEUR"
--major-scale Min --minor-scale 1 -o "c:\temp" -q Bid`

TBC

# Bi5.Net
Nuget package for requesting and converting Dukascopy tick data 

-----------------------------
TBC
