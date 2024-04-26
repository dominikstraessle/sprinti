namespace Sprinti.Workflow;

public class WorkflowOptions : ISprintiOptions
{
    public const string Workflow = "Workflow";
    public bool Enabled { get; set; } = true;
}