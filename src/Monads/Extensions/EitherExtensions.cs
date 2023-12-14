using System;
using System.Diagnostics;
using DotNetExtensions;

namespace Monads.Extensions;

[DebuggerStepThrough]
public static class EitherExtensions
{
    public static void Act<TLeft, TRight>(this Either<TLeft, TRight> maybe, Action<TLeft> left, Action<TRight> right) =>
        maybe.Match(
            left: e => left.ToVoidFunc().Invoke(e),
            right: e => right.ToVoidFunc().Invoke(e));

    public static Either<TLeftProjection, TRightProjection> Map<TLeft, TRight, TLeftProjection, TRightProjection>(
        this Either<TLeft, TRight> either,
        Func<TLeft, TLeftProjection> left,
        Func<TRight, TRightProjection> right) =>
        either.Match(
            left: e => Either<TLeftProjection, TRightProjection>.Left(left(e)),
            right: e => Either<TLeftProjection, TRightProjection>.Right(right(e)));

    public static Either<TProjection, TProjection> Map<T, TProjection>(
        this Either<T, T> either,
        Func<T, TProjection> map) =>
        either.Match(
            left: e => Either<TProjection, TProjection>.Left(map(e)),
            right: e => Either<TProjection, TProjection>.Right(map(e)));

    public static Either<TLeftProjection, TRight> MapLeft<TLeft, TRight, TLeftProjection>(
        this Either<TLeft, TRight> either,
        Func<TLeft, TLeftProjection> map) =>
        either.Map(
            leftMapping: map,
            rightMapping: Functions.Id);

    public static Either<TLeft, TRightProjection> MapRight<TLeft, TRight, TRightProjection>(
        this Either<TLeft, TRight> either,
        Func<TRight, TRightProjection> map) =>
        either.Map(
            leftMapping: Functions.Id,
            rightMapping: map);

    public static TResult Match<TLeft, TMiddle, TRight, TResult>(
        this Either<TLeft, Either<TMiddle, TRight>> either,
        Func<TLeft, TResult> left,
        Func<TMiddle, TResult> middle,
        Func<TRight, TResult> right) =>
        either.Match(
            left: left,
            right: e => e.Match(
                left: middle,
                right: right));

    public static TResult Match<TLeft, TMiddle, TRight, TResult>(
        this Either<Either<TLeft, TMiddle>, TRight> either,
        Func<TLeft, TResult> left,
        Func<TMiddle, TResult> middle,
        Func<TRight, TResult> right) =>
        either.Match(
            left: e => e.Match(
                left: left,
                right: middle),
            right: right);

    public static TResult Match<TFirst, TSecond, TThird, TFourth, TResult>(
        this Either<TFirst, Either<TSecond, Either<TThird, TFourth>>> either,
        Func<TFirst, TResult> first,
        Func<TSecond, TResult> second,
        Func<TThird, TResult> third,
        Func<TFourth, TResult> fourth) =>
        either.Match(
            left: first,
            right: e => e.Match(
                left: second,
                right: f => f.Match(
                    left: third,
                    right: fourth)));

    public static TResult Match<TFirst, TSecond, TThird, TFourth, TResult>(
        this Either<Either<Either<TFirst, TSecond>, TThird>, TFourth> either,
        Func<TFirst, TResult> first,
        Func<TSecond, TResult> second,
        Func<TThird, TResult> third,
        Func<TFourth, TResult> fourth) =>
        either.Match(
            left: e => e.Match(
                left: f => f.Match(
                    left: first,
                    right: second),
                right: third),
            right: fourth);

    public static TResult Match<TFirst, TSecond, TThird, TFourth, TFifth, TResult>(
        this Either<TFirst, Either<TSecond, Either<TThird, Either<TFourth, TFifth>>>> either,
        Func<TFirst, TResult> first,
        Func<TSecond, TResult> second,
        Func<TThird, TResult> third,
        Func<TFourth, TResult> fourth,
        Func<TFifth, TResult> fifth) =>
        either.Match(
            left: first,
            right: e => e.Match(
                left: second,
                right: f => f.Match(
                    left: third,
                    right: g => g.Match(
                        left: fourth,
                        right: fifth))));

    public static TResult Match<TFirst, TSecond, TThird, TFourth, TFifth, TResult>(
        this Either<Either<Either<Either<TFirst, TSecond>, TThird>, TFourth>, TFifth> either,
        Func<TFirst, TResult> first,
        Func<TSecond, TResult> second,
        Func<TThird, TResult> third,
        Func<TFourth, TResult> fourth,
        Func<TFifth, TResult> fifth) =>
        either.Match(
            left: e => e.Match(
                left: f => f.Match(
                    left: g => g.Match(
                        left: first,
                        right: second),
                    right: third),
                right: fourth),
            right: fifth);

    public static TResult Match<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TResult>(
        this Either<TFirst, Either<TSecond, Either<TThird, Either<TFourth, Either<TFifth, TSixth>>>>> either,
        Func<TFirst, TResult> first,
        Func<TSecond, TResult> second,
        Func<TThird, TResult> third,
        Func<TFourth, TResult> fourth,
        Func<TFifth, TResult> fifth,
        Func<TSixth, TResult> sixth) =>
        either.Match(
            left: first,
            right: e => e.Match(
                left: second,
                right: f => f.Match(
                    left: third,
                    right: g => g.Match(
                        left: fourth,
                        right: h => h.Match(
                            left: fifth,
                            right: sixth)))));

    public static TResult Match<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TResult>(
        this Either<Either<Either<Either<Either<TFirst, TSecond>, TThird>, TFourth>, TFifth>, TSixth> either,
        Func<TFirst, TResult> first,
        Func<TSecond, TResult> second,
        Func<TThird, TResult> third,
        Func<TFourth, TResult> fourth,
        Func<TFifth, TResult> fifth,
        Func<TSixth, TResult> sixth) =>
        either.Match(
            left: e => e.Match(
                left: f => f.Match(
                    left: g => g.Match(
                        left: h => h.Match(
                            left: first,
                            right: second),
                        right: third),
                    right: fourth),
                right: fifth),
            right: sixth);

    public static TResult Match<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TResult>(
        this Either<TFirst, Either<TSecond, Either<TThird, Either<TFourth, Either<TFifth, Either<TSixth, TSeventh>>>>>> either,
        Func<TFirst, TResult> first,
        Func<TSecond, TResult> second,
        Func<TThird, TResult> third,
        Func<TFourth, TResult> fourth,
        Func<TFifth, TResult> fifth,
        Func<TSixth, TResult> sixth,
        Func<TSeventh, TResult> seventh) =>
        either.Match(
            left: first,
            right: e => e.Match(
                left: second,
                right: f => f.Match(
                    left: third,
                    right: g => g.Match(
                        left: fourth,
                        right: h => h.Match(
                            left: fifth,
                            right: i => i.Match(
                                left: sixth,
                                right: seventh))))));

    public static TResult Match<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TResult>(
        this Either<Either<Either<Either<Either<Either<TFirst, TSecond>, TThird>, TFourth>, TFifth>, TSixth>, TSeventh> either,
        Func<TFirst, TResult> first,
        Func<TSecond, TResult> second,
        Func<TThird, TResult> third,
        Func<TFourth, TResult> fourth,
        Func<TFifth, TResult> fifth,
        Func<TSixth, TResult> sixth,
        Func<TSeventh, TResult> seventh) =>
        either.Match(
            left: e => e.Match(
                left: f => f.Match(
                    left: g => g.Match(
                        left: h => h.Match(
                            left: i => i.Match(
                                left: first,
                                right: second),
                            right: third),
                        right: fourth),
                    right: fifth),
                right: sixth),
            right: seventh);

    public static Either<TLeft, TRight> Flatten<TLeft, TRight>(this Either<Either<TLeft, TRight>, TRight> either) =>
        either.Match(
            left: Functions.Id,
            right: Either<TLeft, TRight>.Right);

    public static Either<TLeft, TRight> Flatten<TLeft, TRight>(this Either<TLeft, Either<TLeft, TRight>> either) =>
        either.Match(
            left: Either<TLeft, TRight>.Left,
            right: Functions.Id);

    public static Either<TLeft, TRight> Flatten<TLeft, TRight>(this Either<Either<TLeft, TRight>, Either<TLeft, TRight>> either) =>
        either.Match(
            left: Functions.Id,
            right: Functions.Id);

    public static Type GetLeftType<TLeft, TRight>(this Either<TLeft, TRight> _) =>
        typeof(TLeft);

    public static Type GetRightType<TLeft, TRight>(this Either<TLeft, TRight> _) =>
        typeof(TRight);
}