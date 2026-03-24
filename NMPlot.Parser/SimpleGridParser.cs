using System.Globalization;
using NetTopologySuite.Geometries;

namespace NMPlot.Parser;

public sealed class SimpleGridParser
{
    public List<CoordinateM> Read(string filePath)
    {
        var points = new List<CoordinateM>();
        using var reader = new StreamReader(filePath);

        int row = 0;

        while (reader.ReadLine() is { } line)
        {
            if (row < 3)
            {
                // skip first lines.
                row++;
                continue;
            }

            var result = ReadLine(line);

            if (result == null)
            {
                // Skip empty or invalid lines
                continue;
            }

            // You can associate the value with the point here (e.g., store in a dictionary, or attach attributes)
            // For now, we just add the point to the list
            points.Add(result);

            row++;
        }

        return points;
    }

    /// <summary>
    /// Reads a line from the .grd file and extracts the latitude, longitude, and value.
    /// </summary>
    /// <param name="line"></param>
    /// <returns>longitude, latitude and value</returns>
    public static CoordinateM? ReadLine(string line)
    {
        // Parse each line of the .grd file
        // Assuming each line contains values corresponding to a grid row
        string[] values = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (values.Length > 1)
        {
            // Convert value to double, assuming it represents the associated value at this grid point

            // double is similar to NMPLOT parser type.
            string tempValue = values[1].Replace("}", "").Trim(); // remove trailing '}' if present.

            double value = double.Parse(tempValue, CultureInfo.InvariantCulture);

            string[] latLong = values[0].Substring(1, values[0].Length - 2).Split(',', StringSplitOptions.RemoveEmptyEntries);

            double longitude = double.Parse(latLong[0], CultureInfo.InvariantCulture);
            double latitude = double.Parse(latLong[1], CultureInfo.InvariantCulture);

            var result = new CoordinateM()
            {
                X = longitude,
                Y = latitude,
                M = value,
            };

            return result;
        }

        return null;
    }
}
