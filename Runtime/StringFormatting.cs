using UnityEngine;


    public static class StringFormatting
    {
        const string backingFieldPost = "k__BackingField";
        public static string GetBackingFieldName(string propName) => string.Format("<{1}>{0}", backingFieldPost, propName);
    }
