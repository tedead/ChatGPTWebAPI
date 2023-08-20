using System;
using System.Collections.Generic;
using System.Reflection;
using System.ComponentModel;
using Csla.Properties;

namespace Csla.Data
{
  /// <summary>
  /// Map data from a source into a target object
  /// by copying public property values.
  /// </summary>
  /// <remarks></remarks>
  public static class DataMapper
  {

    #region Map from IDictionary

    /// <summary>
    /// Copies values from the source into the
    /// properties of the target.
    /// </summary>
    /// <param name="source">A name/value dictionary containing the source values.</param>
    /// <param name="target">An object with properties to be set from the dictionary.</param>
    /// <remarks>
    /// The key names in the dictionary must match the property names on the target
    /// object. Target properties may not be readonly or indexed.
    /// </remarks>
    public static void Map(System.Collections.IDictionary source, object target)
    {
      Map(source, target, false);
    }

    /// <summary>
    /// Copies values from the source into the
    /// properties of the target.
    /// </summary>
    /// <param name="source">A name/value dictionary containing the source values.</param>
    /// <param name="target">An object with properties to be set from the dictionary.</param>
    /// <param name="ignoreList">A list of property names to ignore. 
    /// These properties will not be set on the target object.</param>
    /// <remarks>
    /// The key names in the dictionary must match the property names on the target
    /// object. Target properties may not be readonly or indexed.
    /// </remarks>
    public static void Map(System.Collections.IDictionary source, object target, params string[] ignoreList)
    {
      Map(source, target, false, ignoreList);
    }

    /// <summary>
    /// Copies values from the source into the
    /// properties of the target.
    /// </summary>
    /// <param name="source">A name/value dictionary containing the source values.</param>
    /// <param name="target">An object with properties to be set from the dictionary.</param>
    /// <param name="ignoreList">A list of property names to ignore. 
    /// These properties will not be set on the target object.</param>
    /// <param name="suppressExceptions">If <see langword="true" />, any exceptions will be supressed.</param>
    /// <remarks>
    /// The key names in the dictionary must match the property names on the target
    /// object. Target properties may not be readonly or indexed.
    /// </remarks>
    public static void Map(
      System.Collections.IDictionary source,
      object target, bool suppressExceptions,
      params string[] ignoreList)
    {
      List<string> ignore = new List<string>(ignoreList);
      foreach (string propertyName in source.Keys)
      {
        if (!ignore.Contains(propertyName))
        {
          try
          {
            SetPropertyValue(target, propertyName, source[propertyName]);
          }
          catch (Exception ex)
          {
            if (!suppressExceptions)
              throw new ArgumentException(
                String.Format("{0} ({1})",
                Resources.PropertyCopyFailed, propertyName), ex);
          }
        }
      }
    }

    #endregion

    #region Map to Dictionary

    /// <summary>
    /// Copies values from the source into the target.
    /// </summary>
    /// <param name="source">An object with properties to be loaded into the dictionary.</param>
    /// <param name="target">A name/value dictionary containing the source values.</param>
    public static void Map(object source, Dictionary<string, object> target)
    {
      Map(source, target, false);
    }

    /// <summary>
    /// Copies values from the source into the target.
    /// </summary>
    /// <param name="source">An object with properties to be loaded into the dictionary.</param>
    /// <param name="target">A name/value dictionary containing the source values.</param>
    /// <param name="ignoreList">A list of property names to ignore. 
    /// These properties will not be set on the target object.</param>
    public static void Map(object source, Dictionary<string, object> target, params string[] ignoreList)
    {
      Map(source, target, false, ignoreList);
    }

    /// <summary>
    /// Copies values from the source into the target.
    /// </summary>
    /// <param name="source">An object with properties to be loaded into the dictionary.</param>
    /// <param name="target">A name/value dictionary containing the source values.</param>
    /// <param name="ignoreList">A list of property names to ignore. 
    /// These properties will not be set on the target object.</param>
    /// <param name="suppressExceptions">If <see langword="true" />, any exceptions will be supressed.</param>
    public static void Map(
      object source, Dictionary<string, object> target,
      bool suppressExceptions,
      params string[] ignoreList)
    {
      List<string> ignore = new List<string>(ignoreList);
      PropertyInfo[] sourceProperties =
        GetSourceProperties(source.GetType());
      foreach (PropertyInfo prop in sourceProperties)
      {
        string propertyName = prop.Name;
        if (!ignore.Contains(propertyName))
        {
          try
          {
            target.Add(propertyName, prop.GetValue(source, null));
          }
          catch (Exception ex)
          {
            if (!suppressExceptions)
              throw new ArgumentException(
                String.Format("{0} ({1})",
                Resources.PropertyCopyFailed, propertyName), ex);
          }
        }
      }
    }

    #endregion

    #region Map from Object

    /// <summary>
    /// Copies values from the source into the
    /// properties of the target.
    /// </summary>
    /// <param name="source">An object containing the source values.</param>
    /// <param name="target">An object with properties to be set from the dictionary.</param>
    /// <remarks>
    /// The property names and types of the source object must match the property names and types
    /// on the target object. Source properties may not be indexed. 
    /// Target properties may not be readonly or indexed.
    /// </remarks>
    public static void Map(object source, object target)
    {
      Map(source, target, false);
    }

    /// <summary>
    /// Copies values from the source into the
    /// properties of the target.
    /// </summary>
    /// <param name="source">An object containing the source values.</param>
    /// <param name="target">An object with properties to be set from the dictionary.</param>
    /// <param name="ignoreList">A list of property names to ignore. 
    /// These properties will not be set on the target object.</param>
    /// <remarks>
    /// The property names and types of the source object must match the property names and types
    /// on the target object. Source properties may not be indexed. 
    /// Target properties may not be readonly or indexed.
    /// </remarks>
    public static void Map(object source, object target, params string[] ignoreList)
    {
      Map(source, target, false, ignoreList);
    }

    /// <summary>
    /// Copies values from the source into the
    /// properties of the target.
    /// </summary>
    /// <param name="source">An object containing the source values.</param>
    /// <param name="target">An object with properties to be set from the dictionary.</param>
    /// <param name="ignoreList">A list of property names to ignore. 
    /// These properties will not be set on the target object.</param>
    /// <param name="suppressExceptions">If <see langword="true" />, any exceptions will be supressed.</param>
    /// <remarks>
    /// <para>
    /// The property names and types of the source object must match the property names and types
    /// on the target object. Source properties may not be indexed. 
    /// Target properties may not be readonly or indexed.
    /// </para><para>
    /// Properties to copy are determined based on the source object. Any properties
    /// on the source object marked with the <see cref="BrowsableAttribute"/> equal
    /// to false are ignored.
    /// </para>
    /// </remarks>
    public static void Map(
      object source, object target,
      bool suppressExceptions,
      params string[] ignoreList)
    {
      List<string> ignore = new List<string>(ignoreList);
      PropertyInfo[] sourceProperties =
        GetSourceProperties(source.GetType());
      foreach (PropertyInfo sourceProperty in sourceProperties)
      {
        string propertyName = sourceProperty.Name;
        if (!ignore.Contains(propertyName))
        {
          try
          {
            SetPropertyValue(
              target, propertyName,
              sourceProperty.GetValue(source, null));
          }
          catch (Exception ex)
          {
            if (!suppressExceptions)
              throw new ArgumentException(
                String.Format("{0} ({1})",
                Resources.PropertyCopyFailed, propertyName), ex);
          }
        }
      }
    }

    /// <summary>
    /// Copies values from the source into the
    /// properties of the target.
    /// </summary>
    /// <param name="source">An object containing the source values.</param>
    /// <param name="target">An object with properties to be set from the dictionary.</param>
    /// <param name="map">A DataMap object containing the mappings to use during the copy process.</param>
    /// <remarks>
    /// The property names and types of the source object must match the property names and types
    /// on the target object. Source properties may not be indexed. 
    /// Target properties may not be readonly or indexed.
    /// </remarks>
    public static void Map(object source, object target, DataMap map)
    {
      Map(source, target, map, false);
    }

    /// <summary>
    /// Copies values from the source into the
    /// properties of the target.
    /// </summary>
    /// <param name="source">An object containing the source values.</param>
    /// <param name="target">An object with properties to be set from the dictionary.</param>
    /// <param name="suppressExceptions">If <see langword="true" />, any exceptions will be supressed.</param>
    /// <param name="map">A DataMap object containing the mappings to use during the copy process.</param>
    /// <remarks>
    /// The property names and types of the source object must match the property names and types
    /// on the target object. Source properties may not be indexed. 
    /// Target properties may not be readonly or indexed.
    /// </remarks>
    public static void Map(object source, object target, DataMap map, bool suppressExceptions)
    {
      foreach (DataMap.MemberMapping mapping in map.GetMap())
      {
        try
        {
          object value = GetValue(mapping.FromMember, source);
          SetValue(
            target, mapping.ToMember, value);
        }
        catch (Exception ex)
        {
          if (!suppressExceptions)
            throw new ArgumentException(
              String.Format("{0} ({1})",
              Resources.PropertyCopyFailed, mapping.FromMember.Name), ex);
        }
      }
    }

    private static PropertyInfo[] GetSourceProperties(Type sourceType)
    {
      List<PropertyInfo> result = new List<PropertyInfo>();
      PropertyDescriptorCollection props =
        TypeDescriptor.GetProperties(sourceType);
      foreach (PropertyDescriptor item in props)
        if (item.IsBrowsable)
          result.Add(sourceType.GetProperty(item.Name));
      return result.ToArray();
    }

    #endregion

    #region  Load from IDictionary

    /// <summary>
    /// Copies values from the source into the
    /// target.
    /// </summary>
    /// <param name="source">
    /// Dictionary containing the source values.
    /// </param>
    /// <param name="target">
    /// Business object with managed fields that
    /// will contain the copied values.
    /// </param>
    /// <param name="nameMapper">
    /// A function that translates the target
    /// property names into key values for the
    /// source dictionary.
    /// </param>
    public static void Load(System.Collections.IDictionary source, object target, Func<string, object> nameMapper)
    {
      var validTarget = target as Core.IManageProperties;
      if (validTarget == null)
        throw new NotSupportedException();
      var propertyList = validTarget.GetManagedProperties();
      foreach (var p in propertyList)
        validTarget.LoadProperty(p, source[nameMapper(p.Name)]);
    }

    #endregion

    #region  Load to IDictionary

    /// <summary>
    /// Copies values from the source into the
    /// target.
    /// </summary>
    /// <param name="source">
    /// Business object with managed fields that
    /// contain the source values.
    /// </param>
    /// <param name="target">
    /// Dictionary that will contain the resulting values.
    /// </param>
    /// <param name="nameMapper">
    /// A function that translates the source
    /// property names into key values for the
    /// target dictionary.
    /// </param>
    public static void Load(object source, System.Collections.IDictionary target, Func<string, object> nameMapper)
    {
      var validSource = source as Core.IManageProperties;
      if (validSource == null)
        throw new NotSupportedException();
      var propertyList = validSource.GetManagedProperties();
      foreach (var p in propertyList)
        target[nameMapper(p.Name)] = validSource.ReadProperty(p);
    }

    #endregion

    #region GetValue

    private static object GetValue(MemberInfo member, object source)
    {
      if (member.MemberType == MemberTypes.Property)
        return ((PropertyInfo)member).GetValue(source, null);
      else
        return ((FieldInfo)member).GetValue(source);
    }

    #endregion

    #region SetValue

    /// <summary>
    /// Sets an object's property with the specified value,
    /// coercing that value to the appropriate type if possible.
    /// </summary>
    /// <param name="target">Object containing the property to set.</param>
    /// <param name="propertyName">Name of the property to set.</param>
    /// <param name="value">Value to set into the property.</param>
    public static void SetPropertyValue(
      object target, string propertyName, object value)
    {
      PropertyInfo propertyInfo =
        target.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
      SetValue(target, propertyInfo, value);
    }

    /// <summary>
    /// Sets an object's field with the specified value,
    /// coercing that value to the appropriate type if possible.
    /// </summary>
    /// <param name="target">Object containing the field to set.</param>
    /// <param name="fieldName">Name of the field (public or non-public) to set.</param>
    /// <param name="value">Value to set into the field.</param>
    public static void SetFieldValue(
      object target, string fieldName, object value)
    {
      FieldInfo fieldInfo =
        target.GetType().GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
      SetValue(target, fieldInfo, value);
    }

    /// <summary>
    /// Sets an object's property or field with the specified value,
    /// coercing that value to the appropriate type if possible.
    /// </summary>
    /// <param name="target">Object containing the member to set.</param>
    /// <param name="memberInfo">MemberInfo object for the member to set.</param>
    /// <param name="value">Value to set into the member.</param>
    public static void SetValue(
      object target, MemberInfo memberInfo, object value)
    {
      if (value != null)
      {
        object oldValue;
        Type pType;
        if (memberInfo.MemberType == MemberTypes.Property)
        {
          PropertyInfo pInfo = (PropertyInfo)memberInfo;
          pType = pInfo.PropertyType;
          oldValue = pInfo.GetValue(target, null);
        }
        else
        {
          FieldInfo fInfo = (FieldInfo)memberInfo;
          pType = fInfo.FieldType;
          oldValue = fInfo.GetValue(target);
        }
        Type vType =
          Utilities.GetPropertyType(value.GetType());
        value = Utilities.CoerceValue(pType, vType, oldValue, value);
      }
      if (memberInfo.MemberType == MemberTypes.Property)
        ((PropertyInfo)memberInfo).SetValue(target, value, null);
      else
        ((FieldInfo)memberInfo).SetValue(target, value);
    }

    #endregion
  }
}

