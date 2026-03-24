using AwesomeAssertions;
using AwesomeAssertions.Execution;
using NetTopologySuite.Geometries;
using NMPlot.Parser;

namespace NMPlot.ParserTests;

[TestClass]
public class SimpleGridParserTest
{
    [DeploymentItem("example.grd")]
    [TestMethod]
    public void SimpleGridParserReaderTest()
    {
        string filename = "example.grd";

        SimpleGridParser simpleGridParser = new SimpleGridParser();
        List<CoordinateM> coordinates = simpleGridParser.Read(filename);

        coordinates.Should().NotBeEmpty();
        coordinates.Count.Should().Be(9604);

        coordinates.All(x => !double.IsNaN(x.M)).Should().BeTrue();
        coordinates.Min(x => x.M).Should().BeApproximately(6.0599999, 0.00001);
        coordinates.Max(x => x.M).Should().BeApproximately(83.400002, 0.00001);
    }
}