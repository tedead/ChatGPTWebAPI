﻿using System;
using System.Collections.Generic;
using System.Reflection;

namespace Csla.Data
{
  /// <summary>
  /// Defines a mapping between two sets of
  /// properties/fields for use by
  /// DataMapper.
  /// </summary>
  public class DataMap
  {
    #region MapElement

    internal class MemberMapping
    {
      private MemberInfo _from;
      public MemberInfo FromMember
      {
        get { return _from; }
      }

      private MemberInfo _to;
      public MemberInfo ToMember
      {
        get { return _to; }
      }

      public MemberMapping(MemberInfo fromMember, MemberInfo toMember)
      {
        _from = fromMember;
        _to = toMember;
      }
    }

    #endregion

    private Type _sourceType;
    private Type _targetType;
    private List<MemberMapping> _map = new List<MemberMapping>();
    private BindingFlags _fieldFlags = BindingFlags.Public | 
                                       BindingFlags.NonPublic | 
                                       BindingFlags.Instance;
    private BindingFlags _propertyFlags = BindingFlags.Public |
                                          BindingFlags.Instance |
                                          BindingFlags.FlattenHierarchy;

    /// <summary>
    /// Initializes an instance of the type.
    /// </summary>
    /// <param name="sourceType">
    /// Type of source object.
    /// </param>
    /// <param name="targetType">
    /// Type of target object.
    /// </param>
    public DataMap(Type sourceType, Type targetType)
    {
      _sourceType = sourceType;
      _targetType = targetType;
    }

    internal List<MemberMapping> GetMap()
    {
      return _map;
    }

    /// <summary>
    /// Adds a property to property
    /// mapping.
    /// </summary>
    /// <param name="sourceProperty">
    /// Name of source property.
    /// </param>
    /// <param name="targetProperty">
    /// Name of target property.
    /// </param>
    public void AddPropertyMapping(string sourceProperty, string targetProperty)
    {
      _map.Add(new MemberMapping(_sourceType.GetProperty(sourceProperty, _propertyFlags), _targetType.GetProperty(targetProperty, _propertyFlags)));
    }

    /// <summary>
    /// Adds a field to field mapping.
    /// </summary>
    /// <param name="sourceField">
    /// Name of source field.
    /// </param>
    /// <param name="targetField">
    /// Name of target field.
    /// </param>
    public void AddFieldMapping(string sourceField, string targetField)
    {
      _map.Add(new MemberMapping(_sourceType.GetField(sourceField, _fieldFlags), _targetType.GetField(targetField, _fieldFlags)));
    }

    /// <summary>
    /// Adds a field to property mapping.
    /// </summary>
    /// <param name="sourceField">
    /// Name of source field.
    /// </param>
    /// <param name="targetProperty">
    /// Name of target property.
    /// </param>
    public void AddFieldToPropertyMapping(string sourceField, string targetProperty)
    {
      _map.Add(new MemberMapping(_sourceType.GetField(sourceField, _fieldFlags), _targetType.GetProperty(targetProperty, _propertyFlags)));
    }

    /// <summary>
    /// Adds a property to field mapping.
    /// </summary>
    /// <param name="sourceProperty">
    /// Name of source property.
    /// </param>
    /// <param name="targetField">
    /// Name of target field.
    /// </param>
    public void AddPropertyToFieldMapping(string sourceProperty, string targetField)
    {
      _map.Add(new MemberMapping(_sourceType.GetProperty(sourceProperty, _propertyFlags), _targetType.GetField(targetField, _fieldFlags)));
    }
  }
}
