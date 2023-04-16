using System;
using System.Net.Http;

namespace BLZ.Common.Extensions
{
    public static class CloneOrNullExtensions
    {
        public static Uri? CloneOrNull(this Uri? obj)
            => obj == null ? null : new Uri(obj.ToString());

        public static T? CloneOrNull<T>(this T? obj) where T : ICloneable
            => obj == null ? obj : (T)obj.Clone();
    }
}

