namespace DotNetExtensions
{
    internal static class HashCodeExtensions
    {
        public static HashCode Append<T>(this HashCode hashCode, T value)
        {
            hashCode.Add(value);
            return hashCode;
        }

        public static HashCode Append(this HashCode hashCode, params object[] values)
        {
            for (int valueIndex = 0; valueIndex < values.Length; valueIndex++)
                hashCode.Add(values[valueIndex]);
            return hashCode;
        }
    }
}
