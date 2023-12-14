using System;
using System.Diagnostics;

namespace DotNetExtensions;

[DebuggerStepThrough]
internal static class ActionExtensions
{
    public static Func<Void> ToVoidFunc(this Action action) =>
        () =>
        {
            action();
            return Void.Instance;
        };

    public static Func<Void> ToVoidFuncOrNull(this Action action) =>
        action != null
            ? action.ToVoidFunc()
            : null;

    public static Func<Void> ToVoidFuncOrEmpty(this Action action) =>
        action != null
            ? action.ToVoidFunc()
            : (() => Void.Instance);

    public static Func<T, Void> ToVoidFunc<T>(this Action<T> action) =>
        arg =>
        {
            action(arg);
            return Void.Instance;
        };

    public static Func<T, Void> ToVoidFuncOrNull<T>(this Action<T> action) =>
        action != null
            ? action.ToVoidFunc()
            : null;

    public static Func<T, Void> ToVoidFuncOrEmpty<T>(this Action<T> action) =>
        action != null
            ? action.ToVoidFunc()
            : (e => Void.Instance);

    public static Func<T1, T2, Void> ToVoidFunc<T1, T2>(this Action<T1, T2> action) =>
        (arg1, arg2) =>
        {
            action(arg1, arg2);
            return Void.Instance;
        };

    public static Func<T1, T2, Void> ToVoidFuncOrNull<T1, T2>(this Action<T1, T2> action) =>
        action != null
            ? action.ToVoidFunc()
            : null;

    public static Func<T1, T2, Void> ToVoidFuncOrEmpty<T1, T2>(this Action<T1, T2> action) =>
        action != null
            ? action.ToVoidFunc()
            : ((arg1, arg2) => Void.Instance);

    public static Func<T1, T2, T3, Void> ToVoidFunc<T1, T2, T3>(this Action<T1, T2, T3> action) =>
        (arg1, arg2, arg3) =>
        {
            action(arg1, arg2, arg3);
            return Void.Instance;
        };

    public static Func<T1, T2, T3, Void> ToVoidFuncOrNull<T1, T2, T3>(this Action<T1, T2, T3> action) =>
        action != null
            ? action.ToVoidFunc()
            : null;

    public static Func<T1, T2, T3, Void> ToVoidFuncOrEmpty<T1, T2, T3>(this Action<T1, T2, T3> action) =>
        action != null
            ? action.ToVoidFunc()
            : ((arg1, arg2, arg3) => Void.Instance);

    public static Func<T> DoAndReturnParam<T>(this Action<T> action, T param) =>
        () =>
        {
            action(param);
            return param;
        };
}