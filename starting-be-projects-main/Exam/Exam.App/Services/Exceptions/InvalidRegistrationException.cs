namespace Exam.App.Services.Exceptions;

public class InvalidRegistrationException : BadRequestException
{
    public InvalidRegistrationException(string message) : 
        base("Registration was unsuccessful. Following errors occured: " + message)
    {
    }
}