namespace Sprinti.Display;

public interface IDisplayService
{
    void UpdateProgress(int stepNumber, string text);
    void Debug(string text);
}