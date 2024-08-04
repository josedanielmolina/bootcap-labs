namespace ApiAdmin.Exceptions
{
    public class ClientErrorException : Exception
    {
        public ClientErrorException() { }

        public ClientErrorException(string message)
            : base(message) { }

    }
}
