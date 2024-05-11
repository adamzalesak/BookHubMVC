namespace BusinessLayer.Exceptions;

public class CreationFailedException : Exception
{
    public CreationFailedException(string message) : base(message)
    {
    }
}