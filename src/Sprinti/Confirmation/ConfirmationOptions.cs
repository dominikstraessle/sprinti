﻿namespace Sprinti.Confirmation;

public class ConfirmationOptions : ISprintiOptions
{
    public const string Confirmation = "Confirmation";
    public string BaseAddress { get; set; } = "https://oawz3wjih1.execute-api.eu-central-1.amazonaws.com";
    public string TeamName { get; set; } = "team00";
    public string Password { get; set; } = "aTdpCRIrI9CLS1";
    public string CubesPath => $"{BaseAddress}cubes";
    private string CubesTeamPath => $"{CubesPath}/{TeamName}";
    public string CubesTeamStartPath => $"{CubesTeamPath}/start";
    public string CubesTeamEndPath => $"{CubesTeamPath}/end";
    public string CubesTeamConfigPath => $"{CubesTeamPath}/config";
    public bool Enabled { get; set; } = true;
}