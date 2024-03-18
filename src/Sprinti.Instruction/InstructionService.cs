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
            var numberOfRequiredRotations = GetNumberOfRequiredRotations(color, position);
            if (numberOfRequiredRotations != 0)
            {
                var rotateCommand = GetOptimizedRotateCommand(numberOfRequiredRotations);
                UpdateActualPosition(numberOfRequiredRotations);
                sequence.Add(rotateCommand);
            }

            sequence.Add(new EjectCommand(color));
        }

        return FinishSequence(sequence);
    }

    private void UpdateActualPosition(int numberOfRequiredRotations)
    {
        _actualPositions = _actualPositions.Select(colorPos => (colorPos - numberOfRequiredRotations) % 4).ToArray();
    }

    private int GetNumberOfRequiredRotations(Color color, int position)
    {
        var numberOfQuarterRotations = _actualPositions[(int)color] - position;
        return numberOfQuarterRotations;
    }

    private static RotateCommand GetOptimizedRotateCommand(int rotation) => rotation switch
    {
        // 270 should become -90
        > 2 => new RotateCommand((rotation - 4) * QuarterToDegree),
        // -270 should become 90
        // -180 should become 180 -> always prefer positive
        <= -2 => new RotateCommand((rotation + 4) * QuarterToDegree),
        // +-90 is already optimal
        _ => new RotateCommand(rotation * QuarterToDegree)
    };

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