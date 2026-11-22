using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Symbology;
using Esri.ArcGISRuntime.UI;
using RestlessPlanet.Core;
using Map = Esri.ArcGISRuntime.Mapping.Map;
using Color = System.Drawing.Color;

namespace RestlessPlanet.App;

public partial class MainPage : ContentPage {
    private static readonly HttpClient Http = new();
    private readonly IEarthquakeService _quakes = new UsgsEarthquakeService(Http);
    private readonly GraphicsOverlay _quakeOverlay = new();
    private bool _loaded;

    public MainPage() {
        InitializeComponent();

        MainMapView.Map = new Map(BasemapStyle.ArcGISTopographic) {
            InitialViewpoint = new Viewpoint(39.8, -98.6, 30_000_000)
        };
        MainMapView.GraphicsOverlays!.Add(_quakeOverlay);
        MainMapView.GeoViewTapped += OnMapTapped;
    }

    protected override async void OnAppearing() {
        base.OnAppearing();
        if (_loaded) return;
        _loaded = true;

        try {
            var quakes = await _quakes.GetRecentAsync("all_day");
            foreach (var q in quakes)
                _quakeOverlay.Graphics.Add(ToGraphic(q));
            Title = $"RestlessPlanet — {quakes.Count} earthquakes in the last 24 hours";
        }
        catch (Exception ex) {
            await DisplayAlert("Could not load earthquakes", ex.Message, "OK");
        }
    }

    private static Graphic ToGraphic(Earthquake q) {
        var point = new MapPoint(q.Longitude, q.Latitude, SpatialReferences.Wgs84);

        // размер — по магнитуде, цвет — по глубине
        var size = 4 + q.Magnitude * 3;
        var color = q.DepthKm switch {
            < 70 => Color.FromArgb(220, 230, 80, 40),   // мелкофокусные — красно-оранжевые
            < 300 => Color.FromArgb(220, 240, 170, 30),  // промежуточные — жёлтые
            _ => Color.FromArgb(220, 60, 110, 220)   // глубокие — синие
        };

        var symbol = new SimpleMarkerSymbol(SimpleMarkerSymbolStyle.Circle, color, size) {
            Outline = new SimpleLineSymbol(SimpleLineSymbolStyle.Solid, Color.White, 1)
        };

        var graphic = new Graphic(point, symbol);
        graphic.Attributes["title"] = $"M {q.Magnitude:0.0} — {q.Place}";
        graphic.Attributes["detail"] = $"{q.Time.ToLocalTime():g} · depth {q.DepthKm:0} km";
        return graphic;
    }

    private async void OnMapTapped(object? sender, Esri.ArcGISRuntime.Maui.GeoViewInputEventArgs e) {
        var result = await MainMapView.IdentifyGraphicsOverlayAsync(_quakeOverlay, e.Position, 12, false);
        var graphic = result.Graphics.FirstOrDefault();

        if (graphic is null || e.Location is null) {
            MainMapView.DismissCallout();
            return;
        }

        var callout = new CalloutDefinition(
            graphic.Attributes["title"]?.ToString() ?? "",
            graphic.Attributes["detail"]?.ToString() ?? "");
        MainMapView.ShowCalloutAt(e.Location, callout);
    }
}