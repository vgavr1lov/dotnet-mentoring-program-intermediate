namespace SqlQueryProviderLib
{
   public static class TypeConverter
   {
      public static object? ConvertValueToTargetType(object sourceProperty, Type targetType)
      {
         if (targetType.IsEnum)
         {
            Enum.TryParse(targetType, sourceProperty.ToString(), out object? parsedEnum);
            return parsedEnum;
         }

         if (targetType == typeof(DateOnly))
            return DateOnly.FromDateTime((DateTime)sourceProperty);

         return Convert.ChangeType(sourceProperty, targetType);
      }
   }
}
