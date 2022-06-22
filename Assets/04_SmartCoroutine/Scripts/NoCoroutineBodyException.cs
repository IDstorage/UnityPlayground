namespace UP04 
{
    public class NoCoroutineBodyException : System.Exception
    {
        public override string Message => "This instance is body-less coroutine. You cannot reuse it.";
    }
}