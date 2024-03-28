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

public record FinishedResponse(int PowerInWattHours, ResponseState ResponseState);

public record CompletedResponse(ResponseState ResponseState);