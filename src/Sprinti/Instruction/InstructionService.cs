using Sprinti.Domain;

namespace Sprinti.Instruction;

public interface IInstructionService
{
    IList<ISerialCommand> GetInstructionSequence(SortedDictionary<int, Color> config);
}

internal class InstructionService : IInstructionService
{
    private const int QuarterToDegree = 90;

    private int[] _actualPositions =
    [
        (int)Color.None,
        (int)Color.Yellow,
        (int)Color.Blue,
        (int)Color.Red
    ];


    public IList<ISerialCommand> GetInstructionSequence(SortedDictionary<int, Color> config)
    {
        var sequence = new List<ISerialCommand>();

        foreach (var (index, color) in config)
        {
            if (color == Color.None) continue;

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


        var backToInitRotations = GetNumberOfRequiredRotations(Color.None, 0);
        if (backToInitRotations != 0)
        {
            sequence.Add(GetOptimizedRotateCommand(backToInitRotations));
        }

        return sequence;
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

    private static RotateCommand GetOptimizedRotateCommand(int rotation)
    {
        return rotation switch
        {
            // 270 should become -90
            > 2 => new RotateCommand((rotation - 4) * QuarterToDegree),
            // -270 should become 90
            // -180 should become 180 -> always prefer positive
            <= -2 => new RotateCommand((rotation + 4) * QuarterToDegree),
            // +-90 is already optimal
            _ => new RotateCommand(rotation * QuarterToDegree)
        };
    }

    private static int IndexToPosition(int index)
    {
        return (index - 1) % 4;
    }
}