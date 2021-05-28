namespace Monads
{
    public readonly struct ResultOk<T>
    {
        public ResultOk(T result)
        {
            Result = result;
        }

        public T Result { get; }
    }
}
