namespace Monads
{
    public readonly struct ResultError<T>
    {
        public ResultError(T result)
        {
            Result = result;
        }

        public T Result { get; }
    }
}
