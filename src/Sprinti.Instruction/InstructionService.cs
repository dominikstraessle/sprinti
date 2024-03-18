using Sprinti.Domain;

namespace Sprinti.Instruction;

public class InstructionService
{
    private int[] _actualPositions =
    [
        (int)Color.None,
        (int)Color.Yellow,
        (int)Color.Blue,
        (int)Color.Red
    ];

    private const int QuarterToDegree = 90;

    private int GetMovementToColorPosition(Color color, int position)
    {
        var numberOfQuarterRotations = _actualPositions[(int)color] - position;
        return numberOfQuarterRotations;
    }

    public IList<ISerialCommand> GetInstructionSequence(SortedDictionary<int, Color> config)
    {
        var sequence = InitSequence();

        foreach (var (index, color) in config)
        {
            if (color == Color.None)
            {
                continue;
            }

            var position = IndexToPosition(index);
            var rotation = GetMovementToColorPosition(color, position);
            if (rotation != 0)
            {
                var rotateCommand = GetRotateCommandAndUpdateActualPosition(rotation);
                sequence.Add(rotateCommand);
            }

            sequence.Add(new EjectCommand(color));
        }

        return FinishSequence(sequence);
    }

    private RotateCommand GetRotateCommandAndUpdateActualPosition(int rotation)
    {
        _actualPositions = _actualPositions.Select(colorPos => (colorPos - rotation) % 4).ToArray();
        return new RotateCommand(rotation * QuarterToDegree);
    }

    private static int IndexToPosition(int index)
    {
        return (index - 1) % 4;
    }

    private static IList<ISerialCommand> FinishSequence(IList<ISerialCommand> sequence)
    {
        sequence.Add(new LiftCommand(Direction.Down));
        sequence.Add(new FinishCommand());
        return sequence;
    }

    private static List<ISerialCommand> InitSequence()
    {
        return [new ResetCommand()];
    }
}