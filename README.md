# RestlessPlanet

A .NET MAUI app that shows our restless planet on an ArcGIS map:
live and historical earthquakes from USGS on top of tectonic plate
boundaries, with hurricanes and AI-powered queries planned next.

## Stack
- .NET MAUI (Windows, Android)
- ArcGIS Maps SDK for .NET
- C#, MVVM, xUnit

## Data sources
- USGS Earthquake Hazards Program (GeoJSON feeds and FDSN event API)
- Tectonic plate boundaries (PB2002 dataset)

## Getting started
1. Create a free ArcGIS Location Platform account and an API key with Basemaps access.
2. Copy `RestlessPlanet.App/Secrets.example.cs` to `Secrets.cs` and add your key.
3. Open `RestlessPlanet.sln` in Visual Studio and run.

## Roadmap
- [ ] Basemap in MAUI
- [ ] Recent earthquakes from USGS
- [ ] Tectonic plates, filters, historical mode
- [ ] Azure backend with infrastructure as code
- [ ] Natural-language queries and an MCP server
- [ ] NOAA hurricane tracks