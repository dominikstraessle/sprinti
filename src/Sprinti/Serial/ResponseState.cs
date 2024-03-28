using System.Text.Json.Serialization;
using static Sprinti.Confirmation.JsonConverters;

namespace Sprinti.Serial;

[JsonConverter(typeof(ResponseStateJsonConverter))]
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