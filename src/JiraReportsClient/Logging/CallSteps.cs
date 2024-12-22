namespace JiraReportsClient.Logging;

public enum CallSteps
{
    Undefined = -1,
    BeforeCall = 1,
    AfterCall = 2,
    AfterDeserialization = 3
}