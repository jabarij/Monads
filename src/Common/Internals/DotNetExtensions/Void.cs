using System;

namespace DotNetExtensions
{
    internal sealed class Void
    {
        private Void() { if (Instance != null) throw new InvalidOperationException(); }
        public static readonly Void Instance = new Void();
    }
}
