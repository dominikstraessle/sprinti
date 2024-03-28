namespace Sprinti.Serial;

public enum ResponseState
{
    Complete,
    InvalidArgument,
    NotImplemented,
    MachineError,
    Error,
    Unknown,
    Finished
}

/**
 * Sendet die verbrauchte Energie in Watt-Stunden (Wh).
 */
public record FinishedResponse(int PowerInWattHours, ResponseState ResponseState);

/**
 * Bestätigt dass die angegebene Transaktion erfolgreich ausgeführt wurde.
 */
public record CompletedResponse(ResponseState ResponseState);