# TransportApiSharp
A C# Class Library to make use of [TransportAPI](http://www.transportapi.com), a UK public transport data platform. It's very much work in (slow) progress. At the present time only two resources have been implemented, and the only sample app that's more than a bare template is the Windows 10 UWP sample. I plan to use Xamarin Forms to create an Android sample in due course, but not an iOs sample (I neither own nor want an iOs device, but if you wish to create a sample for me, feel free).

## Usage
To use the library, you first need to sign up with TransportAPI and get a key. You can get a free key which allows for 1000 hits per day, or there are various paid options. Then clone the library, and add a reference to the <code>TransportApiSharp</code> project to your own project. I'll create a NuGet library sometime...

Here's a bit of sample code which will retrieve and print a list of the 25 bus stops nearest Arnos Grove tube station:

```C#
  public async void BusStopsNearArnosGrove()
    {
      var appId = "abc123"; // This is fake...
      var appKey = "gdfkgndkjgndfkgndfklgdk" //... so is this.
            
      var client = new TransportApiClient(ApiCredentials.appId, ApiCredentials.appKey);

      var response = await client.BusStopsNear(51.6164, 0.1330);
            
      foreach (var stop in response.stops)
      {
        Console.WriteLine(stop.name);
      }
    }
```



  
