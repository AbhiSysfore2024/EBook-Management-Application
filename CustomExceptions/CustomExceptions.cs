namespace Exceptions
{
    public class EmptOrNullSpaceUsrnmePsswrd : Exception
    {
        public EmptOrNullSpaceUsrnmePsswrd(string message) : base(message) { }

    }

    public class EqualUserNameAndPassword : Exception
    {
        public EqualUserNameAndPassword(string message) : base(message) { }
    }

    public class InvalidUserNamePassword : Exception
    {
        public InvalidUserNamePassword(string message) : base(message) { }
    }
}
