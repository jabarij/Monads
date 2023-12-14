namespace Monads;

public struct ResultOk<T>
{
    public ResultOk(T result)
    {
        Result = result;
    }

    public T Result { get; }
}