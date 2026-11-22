using System;
using System.Collections.Generic;
using System.Text;

namespace RestlessPlanet.Core;

public interface IEarthquakeService {
    Task<IReadOnlyList<Earthquake>> GetRecentAsync(string feed = "all_day", CancellationToken ct = default);
}

public sealed class UsgsEarthquakeService(HttpClient http) : IEarthquakeService {
    // feed: all_hour, all_day, all_week, all_month, 4.5_week, significant_month ...
    public async Task<IReadOnlyList<Earthquake>> GetRecentAsync(string feed = "all_day", CancellationToken ct = default) {
        var url = $"https://earthquake.usgs.gov/earthquakes/feed/v1.0/summary/{feed}.geojson";
        var json = await http.GetStringAsync(url, ct);
        return UsgsGeoJsonParser.Parse(json);
    }
}
