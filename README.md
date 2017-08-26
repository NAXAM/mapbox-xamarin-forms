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

> Detail documentation is coming soon.

## License

Mapbox for Xamarin.Forms is released under the Apache License license.
See [LICENSE](./LICENSE) for details.
