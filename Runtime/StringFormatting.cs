
namespace BardicBytes.EventVars
{
    public static class StringFormatting
    {
        const string backingFieldSuffix = "k__BackingField";
        /// <summary>
        /// adds a the prefix k__BackingField. used for editor scripts mostly
        /// </summary>
        /// <param name="propName"></param>
        /// <returns></returns>
        public static string GetBackingFieldName(string propName) => string.Format("<{1}>{0}", backingFieldSuffix, propName);
    }
}