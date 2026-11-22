using System;
using System.Collections.Generic;
using System.Text;

using RestlessPlanet.Core;

namespace RestlessPlanet.Tests;

public class UsgsGeoJsonParserTests {
    private const string Sample = """
    {
      "type": "FeatureCollection",
      "features": [
        {
          "type": "Feature",
          "id": "ak0001",
          "properties": { "mag": 4.7, "place": "50 km S of Anchorage, Alaska",
                          "time": 1700000000000, "url": "https://earthquake.usgs.gov/x" },
          "geometry": { "type": "Point", "coordinates": [-149.9, 60.7, 35.2] }
        },
        {
          "type": "Feature",
          "id": "nomag",
          "properties": { "mag": null, "place": "Somewhere", "time": 1700000000000, "url": "" },
          "geometry": { "type": "Point", "coordinates": [0, 0, 0] }
        }
      ]
    }
    """;

    [Fact]
    public void Parse_ReadsMagnitudeCoordinatesAndTime() {
        var quakes = UsgsGeoJsonParser.Parse(Sample);

        var q = Assert.Single(quakes);                 // второе событие без магнитуды пропущено
        Assert.Equal("ak0001", q.Id);
        Assert.Equal(4.7, q.Magnitude);
        Assert.Equal(60.7, q.Latitude);                // широта — второй элемент
        Assert.Equal(-149.9, q.Longitude);             // долгота — первый
        Assert.Equal(35.2, q.DepthKm);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1700000000000), q.Time);
    }
}