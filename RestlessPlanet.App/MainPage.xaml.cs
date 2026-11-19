using Esri.ArcGISRuntime.Mapping;
using Map = Esri.ArcGISRuntime.Mapping.Map;
namespace RestlessPlanet.App;

public partial class MainPage : ContentPage {
    public MainPage() {
        InitializeComponent();

        var map = new Map(BasemapStyle.ArcGISTopographic) {
            // центр США: широта, долгота, масштаб
            InitialViewpoint = new Viewpoint(39.8, -98.6, 30_000_000)
        };

        MainMapView.Map = map;
    }
}
