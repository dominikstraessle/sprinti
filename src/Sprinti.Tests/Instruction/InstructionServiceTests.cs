using Sprinti.Domain;
using Sprinti.Instruction;
using Sprinti.Tests.Serial;

namespace Sprinti.Tests.Instruction;

public class InstructionServiceTests
{
    private readonly InstructionService _instructionService = new();

    public static TheoryData2<SortedDictionary<int, Color>, IEnumerable<ISerialCommand>> TestData =>
        new()
        {
            {
                new SortedDictionary<int, Color>
                {
                    { 1, Color.None },
                    { 2, Color.None },
                    { 3, Color.None },
                    { 4, Color.None },
                    { 5, Color.None },
                    { 6, Color.None },
                    { 7, Color.None },
                    { 8, Color.None }
                },
                [
                    new ResetCommand(),
                    new LiftCommand(Direction.Down),
                    new FinishCommand()
                ]
            },
            {
                new SortedDictionary<int, Color>
                {
                    { 1, Color.None },
                    { 2, Color.Yellow },
                    { 3, Color.None },
                    { 4, Color.None },
                    { 5, Color.None },
                    { 6, Color.None },
                    { 7, Color.None },
                    { 8, Color.None }
                },
                [
                    new ResetCommand(),
                    new EjectCommand(Color.Yellow),
                    new LiftCommand(Direction.Down),
                    new FinishCommand()
                ]
            },
            {
                new SortedDictionary<int, Color>
                {
                    { 1, Color.Yellow },
                    { 2, Color.Yellow },
                    { 3, Color.None },
                    { 4, Color.None },
                    { 5, Color.None },
                    { 6, Color.None },
                    { 7, Color.None },
                    { 8, Color.None }
                },
                [
                    new ResetCommand(),
                    new RotateCommand(90),
                    new EjectCommand(Color.Yellow),
                    new RotateCommand(-90),
                    new EjectCommand(Color.Yellow),
                    new LiftCommand(Direction.Down),
                    new FinishCommand()
                ]
            },
            {
                new SortedDictionary<int, Color>
                {
                    { 1, Color.Yellow },
                    { 2, Color.Yellow },
                    { 3, Color.Yellow },
                    { 4, Color.Yellow },
                    { 5, Color.None },
                    { 6, Color.None },
                    { 7, Color.None },
                    { 8, Color.None }
                },
                [
                    new ResetCommand(),
                    new RotateCommand(90),
                    new EjectCommand(Color.Yellow),
                    new RotateCommand(-90),
                    new EjectCommand(Color.Yellow),
                    new RotateCommand(-90),
                    new EjectCommand(Color.Yellow),
                    new RotateCommand(-90),
                    new EjectCommand(Color.Yellow),
                    new LiftCommand(Direction.Down),
                    new FinishCommand()
                ]
            },
            {
                new SortedDictionary<int, Color>
                {
                    { 1, Color.None },
                    { 2, Color.Yellow },
                    { 3, Color.Blue },
                    { 4, Color.Red },
                    { 5, Color.None },
                    { 6, Color.None },
                    { 7, Color.None },
                    { 8, Color.None }
                },
                [
                    new ResetCommand(),
                    new EjectCommand(Color.Yellow),
                    new EjectCommand(Color.Blue),
                    new EjectCommand(Color.Red),
                    new LiftCommand(Direction.Down),
                    new FinishCommand()
                ]
            },
            {
                new SortedDictionary<int, Color>
                {
                    { 1, Color.None },
                    { 2, Color.Yellow },
                    { 3, Color.Blue },
                    { 4, Color.Red },
                    { 5, Color.None },
                    { 6, Color.Yellow },
                    { 7, Color.Blue },
                    { 8, Color.Red }
                },
                [
                    new ResetCommand(),
                    new EjectCommand(Color.Yellow),
                    new EjectCommand(Color.Blue),
                    new EjectCommand(Color.Red),
                    new EjectCommand(Color.Yellow),
                    new EjectCommand(Color.Blue),
                    new EjectCommand(Color.Red),
                    new LiftCommand(Direction.Down),
                    new FinishCommand()
                ]
            },
            {
                new SortedDictionary<int, Color>
                {
                    { 1, Color.None },
                    { 2, Color.Red },
                    { 3, Color.Red },
                    { 4, Color.None },
                    { 5, Color.None },
                    { 6, Color.Red },
                    { 7, Color.Red },
                    { 8, Color.None }
                },
                [
                    new ResetCommand(),
                    new RotateCommand(180),
                    new EjectCommand(Color.Red),
                    new RotateCommand(-90),
                    new EjectCommand(Color.Red),
                    new RotateCommand(90),
                    new EjectCommand(Color.Red),
                    new RotateCommand(-90),
                    new EjectCommand(Color.Red),
                    new LiftCommand(Direction.Down),
                    new FinishCommand()
                ]
            },
        };

    [Theory]
    [MemberData(nameof(TestData))]
    public void TestGetInstructionSequence(SortedDictionary<int, Color> config,
        IList<ISerialCommand> expectedCommands)
    {
        var sequence = _instructionService.GetInstructionSequence(config);
        Assert.NotNull(sequence);
        Assert.Equal(expectedCommands, sequence);
        for (var i = 0; i < expectedCommands.Count; i++)
        {
            Assert.Equal(expectedCommands[i], sequence[i]);
        }
    }
}