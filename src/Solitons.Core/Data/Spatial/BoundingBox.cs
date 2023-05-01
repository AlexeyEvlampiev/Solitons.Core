
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Solitons.Data.Spatial;

/// <summary>
/// 
/// </summary>
public readonly record struct BoundingBox
{
    private const string InvalidCsvFormatMessage = "Invalid bounding box CSV format";

    /// <summary>
    /// 
    /// </summary>
    /// <param name="xmin"></param>
    /// <param name="ymin"></param>
    /// <param name="xmax"></param>
    /// <param name="ymax"></param>
    public BoundingBox(float xmin, float ymin, float xmax, float ymax)
    {
        Xmin = xmin;
        Ymin = ymin;
        Xmax = xmax;
        Ymax = ymax;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="csv"></param>
    /// <returns></returns>
    /// <exception cref="FormatException"></exception>
    [DebuggerStepThrough]
    public static BoundingBox ParseCsv(string csv)
    {
        return TryParseCsv(csv, out var boundingBox)
            ? boundingBox
            : throw new FormatException(InvalidCsvFormatMessage);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="csv"></param>
    /// <param name="boundingBox"></param>
    /// <returns></returns>
    public static bool TryParseCsv(string csv, out BoundingBox boundingBox)
    {
        var parts = Regex
            .Split(csv, @"\s*,\s*")
            .Skip(String.IsNullOrWhiteSpace);
        var coordinates = new List<float>(4);
        foreach (var part in parts)
        {
            if (float.TryParse(part, NumberStyles.Float, CultureInfo.InvariantCulture, out var coordinate) &&
                coordinates.Count < 4)
            {
                coordinates.Add(coordinate);
            }
            else
            {
                boundingBox = default;
                return false;
            }
        }

        if (coordinates.Count == 4)
        {
            boundingBox = new BoundingBox(coordinates[0], coordinates[1], coordinates[2], coordinates[3]);
            return true;
        }

        boundingBox = default;
        return false;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="boundingBox"></param>
    /// <returns></returns>
    public static implicit operator float[](BoundingBox boundingBox)
    {
        return new float[] { boundingBox.Xmin, boundingBox.Ymin, boundingBox.Xmax, boundingBox.Ymax};
    }


    /// <summary>
    /// 
    /// </summary>
    public float Xmin { get; }

    /// <summary>
    /// 
    /// </summary>
    public float Ymin { get; }

    /// <summary>
    /// 
    /// </summary>
    public float Xmax { get; }

    /// <summary>
    /// 
    /// </summary>
    public float Ymax { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public override string ToString() =>
        string.Format(CultureInfo.InvariantCulture, "{0},{1},{2},{3}", Xmin, Ymin, Xmax, Ymax);
}