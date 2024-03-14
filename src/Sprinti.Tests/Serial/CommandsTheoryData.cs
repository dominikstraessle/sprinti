using Sprinti.Serial;
using static Sprinti.Serial.EnumMapper;
using static Sprinti.Serial.EnumMapper.Color;
using static Sprinti.Serial.EnumMapper.Direction;
using static Sprinti.Serial.EnumMapper.ResponseState;

namespace Sprinti.Tests.Serial;

public class TheoryData<T1, T2, T3> : TheoryData
{
    /// <summary>
    /// Adds data to the theory data set.
    /// </summary>
    /// <param name="p1">The first data value.</param>
    /// <param name="p2">The second data value.</param>
    /// <param name="p3">The third data value.</param>
    public void Add(T1 p1, T2 p2, T3 p3)
    {
        AddRow(p1, p2, p3);
    }
}

public class CommandsTheoryData : TheoryData<ISerialCommand, string, ResponseState>
{
    public CommandsTheoryData()
    {
        Add(new RotateCommand(90), "complete", Complete);
        Add(new EjectCommand(Red), "complete", Complete);
        Add(new LiftCommand(Down), "complete", Complete);
        Add(new ResetCommand(), "complete", Complete);
        Add(new ResetCommand(), "error invalid_argument", InvalidArgument);
        Add(new ResetCommand(), "error not_implemented", NotImplemented);
        Add(new ResetCommand(), "error machine_error", MachineError);
        Add(new ResetCommand(), "error error", Error);
        Add(new ResetCommand(), "some other response", Unknown);
        Add(new FinishCommand(), "finish 10", Finished);
    }
}