using System;
using System.Collections.Generic;
using System.Text;

namespace RestlessPlanet.Core;


public sealed record Earthquake(
    string Id,
    double Magnitude,
    string Place,
    DateTimeOffset Time,
    double Latitude,
    double Longitude,
    double DepthKm,
    string Url);