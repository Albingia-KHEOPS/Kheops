namespace ALBINGIA.Framework.Common.IOFile
{
    public static class ManageProperty
    {
        public static object GetPropertyValue(object monObjet, string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
                return null;
            System.Reflection.PropertyInfo propertyInfo = monObjet.GetType().GetProperty(propertyName);
            return propertyInfo.GetValue(monObjet, null);
        }
    }
}
