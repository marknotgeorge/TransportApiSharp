# TransportApiSharp
A C# Class Library to make use of [TransportAPI](http://www.transportapi.com), a UK public transport data platform. It's very much work in (slow) progress. At the present time all of the Bus endpoints have been implemented, but the only sample app that's more than a bare template is the Windows 10 UWP sample. I plan to use Xamarin Forms to create an Android sample in due course, but not an iOs sample (I neither own nor want an iOs device, but if you wish to create a sample for me, feel free).

## Usage
To use the library, you first need to sign up with TransportAPI and get a key. You can get a free key which allows for 1000 hits per day, or there are various paid options. 

The easiest way of getting the library should be by NuGet:

[![NuGet Pre Release](https://img.shields.io/nuget/vpre/TransportApiSharp.svg)](https://www.nuget.org/packages/TransportApiSharp/)

Alternatively, you can fork it.

Each method will return a C# class based on the JSON response from the endpoint. If there's an error, the method will return `null`. In this case, the error message will be available in the `LastError` property of the client.

Here's a bit of sample code which will retrieve and print a list of the 25 bus stops nearest Arnos Grove tube station:

```C#
  public async void BusStopsNearArnosGrove()
    {
      var appId = "abc123"; // This is fake...
      var appKey = "gdfkgndkjgndfkgndfklgdk" //... so is this.
            
      var client = new TransportApiClient(appId, appKey);

      var response = await client.BusStopsNear(51.6164, 0.1330);
      
      if (response != null)
      {
        foreach (var stop in response.stops)
        {
          Console.WriteLine(stop.name);
        }
      }
      else
        Console.WriteLine($"TransportAPI error: {client.LastError}"); // C# 6.0 string interpolation!
    }
```



  
