using System;

namespace Monads;

public static class Functions
{
    public static T Id<T>(T arg) =>
        arg;

    public static T? NullableId<T>(T arg) where T : struct =>
        arg;

    public static T SideEffect<T>(T arg, Action<T> sideEffect)
    {
        sideEffect(arg);
        return arg;
    }

    public static T ReturnDefault<T>() =>
        default;

    public static TOut ReturnDefault<TIn, TOut>(TIn _) =>
        default;
}