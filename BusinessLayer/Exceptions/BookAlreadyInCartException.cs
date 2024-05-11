namespace BusinessLayer.Exceptions;

public class BookAlreadyInCartException : Exception
{
    public BookAlreadyInCartException(string message) : base(message)
    {
    }
}