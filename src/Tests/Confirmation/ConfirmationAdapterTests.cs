using Sprinti.Confirmation;
using Sprinti.Domain;

namespace Sprinti.Tests.Confirmation;

public class ConfirmationAdapterTests
{
    [Fact]
    public void TestConfirmationSerialization()
    {
        var config = new CubeConfig
        {
            Time = new DateTime(2023, 10, 10, 17, 10, 5, DateTimeKind.Utc),
            Config = new SortedDictionary<int, Color>
            {
                { 1, Color.Red },
                { 2, Color.Blue },
                { 3, Color.Red },
                { 4, Color.Yellow },
                { 5, Color.None },
                { 6, Color.None },
                { 7, Color.Yellow },
                { 8, Color.Red }
            }
        };
        const string expected = """
                                {"time":"2023-10-10 17:10:05","config":{"1":"red","2":"blue","3":"red","4":"yellow","5":"","6":"","7":"yellow","8":"red"}}
                                """;


        var result = ConfirmationAdapter.SerializeCubeConfig(config);
        Assert.Equal(expected, result);
    }
}