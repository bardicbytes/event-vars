
namespace BardicBytes.EventVars
{
    public static class StringFormatting
    {
        const string backingFieldSuffix = "k__BackingField";
        public static string GetBackingFieldName(string propName) => string.Format("<{1}>{0}", backingFieldSuffix, propName);
    }
}