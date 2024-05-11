namespace BusinessLayer.Exceptions;

public class EditingFailedException : Exception
{
    public EditingFailedException(string message) : base(message)
    {
    }
}