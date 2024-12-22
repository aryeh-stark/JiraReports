namespace JiraReportsClient.Exceptions;

public enum ExceptionTypes
{
    Undefined = 0,
    
    ConfigurationGeneralError = 10,
    ConfigurationMissingFile,
    ConfigurationMissingJsonJiraSection,
    ConfigurationMissingProperties,
    ConfigurationPropertyNotNumber,
    ConfigurationPropertyEmpty,
    
    
    HttpGetBoardsError = 20,
    HttpGetBoardByIdError,
    HttpGetBoardByIdDeserializationError,
    HttpGetBoardsForProjectsError,
    
    
    HttpGetSprintError = 30,
    HttpGetSprintByIdError,
    HttpGetSprintDeserializationError,
    HttpGetSprintsForBoardError,
    HttpGetSprintsForBoardDeserializationError,
    HttpGetIssuesForSprintError,
    HttpGetIssuesForSprintDeserializationError,
    
    HttpGetProjectsError = 40,
    HttpGetProjectsDeserializationError,
    
    HttpGetIssuesError = 50,
    HttpGetIssueByIdError,
    HttpGetIssuesByIdsError,
    
    HttpGetSprintReportError = 60,
    HttpGetSprintReportForBoardAndSprintError,
    HttpGetSprintReportForBoardAndSprintDeserializationError,
    
    NotScrumBoardError = 201,
}