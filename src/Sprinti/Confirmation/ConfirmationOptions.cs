namespace Sprinti.Confirmation;

public class ConfirmationOptions
{
    public const string Confirmation = "Connection";
    public string ConnectionString { get; set; } = "http://52.58.217.104:5000/cubes/team29";
    public string TestConnectionString { get; set; } = "http://52.58.217.104:5000/cubes";
}