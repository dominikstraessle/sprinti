namespace Sprinti.Domain;

public interface ISerialCommand
{
    string ToAsciiCommand();
}

/**
 * Rotiert die Platform.
 */
public record RotateCommand(int Degree) : ISerialCommand
{
    public string ToAsciiCommand()
    {
        return $"rotate {Degree}";
    }
}

/**
 * Auswerfen eines Würfels der angegebenen Farbe.
 */
public record EjectCommand(Color Color) : ISerialCommand
{
    public string ToAsciiCommand()
    {
        return $"eject {Color.Map()}";
    }
}

/**
 * Hebt oder senkt die Platform.
 */
public record LiftCommand(Direction Direction) : ISerialCommand
{
    public string ToAsciiCommand()
    {
        return $"lift {Direction.Map()}";
    }
}

/**
 * Markiert den Start des Wettbewerbs. Geht davon aus, dass Init bereits ausgeführt wurde.
 */
public record StartCommand : ISerialCommand
{
    public string ToAsciiCommand()
    {
        return "start";
    }
}

/**
 * Rotiert die Platform auf den Start-Zustand und hebt die Platform an.
 */
public record InitCommand : ISerialCommand
{
    public string ToAsciiCommand()
    {
        return "init";
    }
}

/**
 * Richtet Trichter auf Startposition aus.
 */
public record AlignCommand : ISerialCommand
{
    public string ToAsciiCommand()
    {
        return "align";
    }
}

/**
 * Dreht die Platform aus den Lichtschranken, damit korrekt kalibriert werden kann.
 */
public record MoveoutCommand : ISerialCommand
{
    public string ToAsciiCommand()
    {
        return "moveout";
    }
}

/**
 * Markiert das Ende aller Instruktionen und erwartet die verbrauchte Energie in Watt-Stunden als Antwort.
 */
public record FinishCommand : ISerialCommand
{
    public string ToAsciiCommand()
    {
        return "finish";
    }
}