namespace Exam.App.Services.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(int id) : base($"Item with id {id} could not be found.")
    {
    }
}