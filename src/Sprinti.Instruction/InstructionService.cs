using Sprinti.Domain;

namespace Sprinti.Instruction;

public class InstructionService
{
    private int _actualPosition;
    private const int QuarterToDegree = 90;

    private int GetMovementToPosition(int position)
    {
        var numberOfQuarterRotations = position - _actualPosition;
        return numberOfQuarterRotations * QuarterToDegree;
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
            var rotation = GetMovementToPosition(position);
            if (rotation != 0)
            {
                _actualPosition = position;
                sequence.Add(new RotateCommand(rotation));
            }

            sequence.Add(new EjectCommand(color));
        }

        return FinishSequence(sequence);
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