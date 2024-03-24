namespace Sprinti.Tests.Serial;

public class TheoryData3<T1, T2, T3> : TheoryData
{
    /// <summary>
    ///     Adds data to the theory data set.
    /// </summary>
    /// <param name="p1">The first data value.</param>
    /// <param name="p2">The second data value.</param>
    /// <param name="p3">The third data value.</param>
    public void Add(T1 p1, T2 p2, T3 p3)
    {
        AddRow(p1, p2, p3);
    }
}

public class TheoryData2<T1, T2> : TheoryData
{
    /// <summary>
    ///     Adds data to the theory data set.
    /// </summary>
    /// <param name="p1">The first data value.</param>
    /// <param name="p2">The second data value.</param>
    public void Add(T1 p1, T2 p2)
    {
        AddRow(p1, p2);
    }
}