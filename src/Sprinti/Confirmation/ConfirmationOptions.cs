namespace Sprinti.Confirmation;

public class ConfirmationOptions
{
    public const string Confirmation = "Connection";
    public Uri BaseAddress { get; set; } = new("http://52.58.217.104:5000");
    public string TeamName { get; set; } = "team29";
    public string Password { get; set; } = "noauth";

    public string CubesPath => $"{BaseAddress}cubes";
    private string CubesTeamPath => $"{CubesPath}/{TeamName}";
    public string CubesTeamStartPath => $"{CubesTeamPath}/start";
    public string CubesTeamEndPath => $"{CubesTeamPath}/end";
    public string CubesTeamConfigPath => $"{CubesTeamPath}/config";
}