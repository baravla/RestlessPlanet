using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace RestlessPlanet.Core;

public static class UsgsGeoJsonParser {
    public static IReadOnlyList<Earthquake> Parse(string json) {
        using var doc = JsonDocument.Parse(json);
        var result = new List<Earthquake>();

        foreach (var feature in doc.RootElement.GetProperty("features").EnumerateArray()) {
            var props = feature.GetProperty("properties");
            var geometry = feature.GetProperty("geometry");

            // у некоторых событий магнитуды или координат нет — пропускаем
            if (props.GetProperty("mag").ValueKind == JsonValueKind.Null ||
                geometry.ValueKind == JsonValueKind.Null)
                continue;

            var coords = geometry.GetProperty("coordinates");

            result.Add(new Earthquake(
                Id: feature.GetProperty("id").GetString() ?? "",
                Magnitude: props.GetProperty("mag").GetDouble(),
                Place: props.GetProperty("place").GetString() ?? "Unknown location",
                Time: DateTimeOffset.FromUnixTimeMilliseconds(props.GetProperty("time").GetInt64()),
                Latitude: coords[1].GetDouble(),
                Longitude: coords[0].GetDouble(),
                DepthKm: coords[2].GetDouble(),
                Url: props.GetProperty("url").GetString() ?? ""));
        }

        return result;
    }
}
