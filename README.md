<img src="./art/repo_header.png" alt="Mapbox for Xamarin.Forms" width="800" />

# Mapbox for Xamarin.Forms
This library provides the controls/renderers for using Mapbox SDKs inside your Xamarin.Forms app.

## About
This project is maintained by Naxam Co.,Ltd.<br>
We specialize in developing mobile applications using Xamarin and native technology stack.<br>

**Looking for developers for your project?**<br>

<a href="mailto:tuyen@naxam.net"> 
<img src="https://github.com/NAXAM/naxam.github.io/blob/master/assets/img/hire_button.png?raw=true" height="40"></a> <br>

## Usage

### Install Nuget package

```
Install-Package Naxam.Mapbox.Forms -pre
```

### Add the Mapbox Service to your Manifest
```xml
<service android:name="com.mapbox.mapboxsdk.telemetry.TelemetryService" />
```

### Permisisons
Mapbox requires location and internet permissions on the device in order to run.

Android
```
<uses-permission android:name="android.permission.ACCESS_NETWORK_STATE" />
<uses-permission android:name="android.permission.ACCESS_COARSE_LOCATION" />
<uses-permission android:name="android.permission.ACCESS_FINE_LOCATION" />
<uses-permission android:name="android.permission.INTERNET" />
```

iOS (info.plist)
```
<key>NSLocationAlwaysUsageDescription</key>
<string>This app needs to use your location</string>
<key>NSLocationWhenInUseUsageDescription</key>
<string>This app needs to use your location</string>
<key>MGLMapboxAccessToken</key>
<string>YOUR_MAPBOX_ACCESS_TOKEN</string>
```

### Your XAML
```xml
<local:MapView 
    x:Name="map" 
    VerticalOptions="FillAndExpand" 
    MapStyle="{Binding CurrentMapStyle}" 
    ZoomLevel="{Binding ZoomLevel}"
/>
```

### Your code behind

```c#
map.DidTapOnMapCommand = new Command<Tuple<Position, Point>>((Tuple<Position, Point> obj) =>
{
    var features = map.GetFeaturesAroundPoint.Invoke(obj.Item2, 6, null);
    var filtered = features.Where((arg) => arg.Attributes != null);
    foreach (IFeature feat in filtered) {
        var str = JsonConvert.SerializeObject(feat);
        System.Diagnostics.Debug.WriteLine(str);
    }

});
map.DidFinishLoadingStyleCommand = new Command<MapStyle>((obj) =>
{
    map.ResetPositionFunc.Execute(null);
    foreach (Layer layer in obj.OriginalLayers)
    {
        System.Diagnostics.Debug.WriteLine(layer.Id);
    }

});
map.ZoomLevel = Device.RuntimePlatform == Device.Android ? 4 : 10;
```

> Detailed documentation is coming soon.

### Using Vector Tiles (WMTS)

For example, using a GeoServer WMTS url, and having a workspace named "Ski" and a layer named "planet_osm_line".

After `DidFinishLoadingStyle` :

```c#
var TILESET_URL = "http://localhost:8080/geoserver/gwc/service/wmts?REQUEST=GetTile&SERVICE=WMTS&VERSION=1.0.0&LAYER=Ski:planet_osm_line&STYLE=&TILEMATRIX=EPSG:900913:{z}&TILEMATRIXSET=EPSG:900913&FORMAT=application/vnd.mapbox-vector-tile&TILECOL={x}&TILEROW={y}";

var myTiles = new TileSet("1.0.0", TILESET_URL);
VectorSource vectorSource = new VectorSource("vector-source", myTiles);
map.Functions.AddSource(vectorSource);

var myLayer = new LineLayer("tile-data", "vector-source")
{
    LineColor = Expression.Color(Color.FromHex("#F13C6E")),
    LineWidth = Expression.Literal(4.0f),
    LineCap = Expression.Literal("round"),
    LineJoin = Expression.Literal("round"),
    SourceLayer = "planet_osm_line"
};
map.Functions.AddLayer(myLayer);
```

## License

Mapbox for Xamarin.Forms is released under the Apache License license.
See [LICENSE](./LICENSE) for details.
