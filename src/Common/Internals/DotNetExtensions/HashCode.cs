using System.Collections;

namespace DotNetExtensions
{
    internal class HashCode
    {
        public const int DefaultInitialHash = -2128831035;
        public const int DefaultOffset = 16777619;
        public const int DefaultNullHash = 0;

        public HashCode(int initialHash = DefaultInitialHash, int offset = DefaultOffset, int nullHash = DefaultNullHash)
        {
            InitialHash = initialHash;
            Offset = offset;
            NullHash = nullHash;
            CurrentHash = initialHash;
        }

        public int InitialHash { get; }
        public int Offset { get; }
        public int NullHash { get; }
        public int CurrentHash { get; private set; }

        public int ToHashCode() => CurrentHash;

        public void Add(object obj)
        {
            unchecked
            {
                CurrentHash = CurrentHash * Offset ^ ComputeHashCode(obj);
            }
        }

        private int ComputeHashCode(object obj) =>
            (obj is IEnumerable enumerable && !(obj is string))
            ? ComputeHashCode(enumerable)
            : GetHashCode(obj);

        private int ComputeHashCode(IEnumerable enumerable)
        {
            if (enumerable == null)
                return NullHash;

            int hash = InitialHash;
            unchecked
            {
                foreach (var element in enumerable)
                    hash *= Offset ^ GetHashCode(element);
            }

            return hash;
        }

        private int GetHashCode(object obj) =>
            obj?.GetHashCode() ?? NullHash;
    }
}
