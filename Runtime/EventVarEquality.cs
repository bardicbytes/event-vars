// alex@bardicbytes.com

using System.Collections.Generic;

namespace BardicBytes.EventVars
{
    public static class EventVarEquality
    {
        public static bool Equals<T>(T a, T b) => EqualityComparer<T>.Default.Equals(a, b);
    }
}
