namespace Monads.FluentAssertions
{
    public static class EitherAssertionExtensions
    {
        public static EitherAssertions<TLeft, TRight> Should<TLeft, TRight>(this Either<TLeft, TRight> actualValue) => new EitherAssertions<TLeft, TRight>(actualValue);
        public static NullableEitherAssertions<TLeft, TRight> Should<TLeft, TRight>(this Either<TLeft, TRight>? actualValue) => new NullableEitherAssertions<TLeft, TRight>(actualValue);
    }
}
