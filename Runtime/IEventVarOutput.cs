namespace BardicBytes.EventVars
{
    /// <summary>
    /// defines an EventVar as providing the particular type of output after being raised
    /// </summary>
    /// <typeparam name="TOutput"></typeparam>
    public interface IEventVarOutput<TOutput> : IEventVar
    {
        public TOutput Value { get; }
    }
}