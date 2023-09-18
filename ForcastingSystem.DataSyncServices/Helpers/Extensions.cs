namespace ForecastingSystem.DataSyncServices.Helpers
{
    public static class Extensions
    {
        public static string GetFinalMessage(this Exception ex)
        {
            while (ex.InnerException != null)
            {
                ex= ex.InnerException;
            }
            return ex.Message;
        }

        public static string ToJsonString<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            return "{" + string.Join(",", dictionary.Select(kv => kv.Key + "=" + kv.Value).ToArray()) + "}";
        }
    }

    internal static class EnumerableExtension
    {
        public static IEnumerable<IEnumerable<T>> Batch<T>(this IEnumerable<T> enumerator , int size)
        {
            var length = enumerator.Count();
            var pos = 0;
            do
            {
                yield return enumerator.Skip(pos).Take(size);
                pos = pos + size;
            } while (pos < length);
        }
    }
}
