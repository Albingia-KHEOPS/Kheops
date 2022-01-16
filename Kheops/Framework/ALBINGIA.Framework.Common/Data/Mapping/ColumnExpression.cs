using System;
using System.Data;
using System.Data.Linq.Mapping;
using System.Reflection;
using System.Reflection.Emit;

namespace ALBINGIA.Framework.Common.Data.Mapping
{
  /// <summary>
  /// columns informations
  /// </summary>
  public sealed class ColumnExpression
  {
    #region Constants
    private const string METHOD_NAME_ISDBNULL = "IsDBNull"; 
    #endregion
    #region Fields
    internal string Name;
    internal bool IsPrimaryKey;
    internal string Expression;
    internal bool IsDbGenerated;
    internal bool CanBeNull;
    internal int Ordinal;
    internal string PropertyName;
    internal PropertyInfo PropInfo;
    internal Type TypeInfo;
    internal string TableName = string.Empty;
    internal string PropertyRef = string.Empty;
    internal string ColumnRef = string.Empty;

        public string ColTypeInfo { get; internal set; }
        #endregion
        #region Constructors, Destructors
        /// <summary>
        /// Create an instance
        /// </summary>
        internal ColumnExpression()
    {
      Ordinal = -1;
    }
    #endregion
    #region Properties
    /// <summary>
    /// Renvoie une instance de logger pour la librairie.
    /// </summary>
    //private static TraceSource Logger
    //{
    //  get
    //  {
    //    return TraceManager.LoggerArcData;
        
    //  }
    //}
    #endregion
    #region Methods

    internal static ColumnExpression GetLinqAttribute(ColumnAttribute p_attribute)
    {
      ColumnExpression v_column = new ColumnExpression();
      v_column.CanBeNull = p_attribute.CanBeNull;
      v_column.IsPrimaryKey = p_attribute.IsPrimaryKey;
      v_column.IsDbGenerated = p_attribute.IsDbGenerated;
      v_column.Name = p_attribute.Name;
      v_column.Expression = p_attribute.Expression;
      return v_column;
    }

    internal void GenerateIL(
      ILGenerator p_ilout,
      int p_index,
      LocalBuilder p_objectToMap,
      ref LocalBuilder p_memberIndex,
      ColumnExpression[] p_maps)
    {
      if ((TypeInfo.IsGenericType))
      {
        if (TypeInfo.GetGenericTypeDefinition().FullName == typeof(Nullable<>).FullName)
          GenerateILForNullableMember(p_ilout, p_index, p_objectToMap, ref p_memberIndex, TypeInfo.GetGenericArguments()[0], p_maps);
        else
          throw new OnlyNullableMappingException("Only Nullable generic type is allowed");
      }
      else
        if (CanBeNull)
          GenerateILForMemberWithIgnoringNullValue(p_ilout, p_index, p_objectToMap, ref p_memberIndex, p_maps);
        else
          GenerateILForMember(p_ilout, p_index, p_objectToMap, p_memberIndex, p_maps);
    }

    internal void GenerateILForMember(
      ILGenerator p_ilout, int p_index,
      LocalBuilder p_objectToMap,
      LocalBuilder p_fieldIndex,
      ColumnExpression[] p_maps)
    {
      // pushing the object to fill on the stack
      p_ilout.Emit(OpCodes.Ldloc, p_objectToMap);

      // pushing the datareader on the stack
      p_ilout.Emit(OpCodes.Ldarg_1);

      // Getting the ordinal value for the method/field
      //p_ilout.Emit(OpCodes.Ldarg_2);
      p_ilout.Emit(OpCodes.Ldc_I4, p_index);
      //p_ilout.Emit(OpCodes.Ldelem_I);

      //GenerateILToGetDataFromDataReader(p_ilout, p_index, TypeInfo, p_maps);
      p_ilout.Emit(OpCodes.Callvirt, GetDataReaderMethod(TypeInfo));
      p_ilout.Emit(OpCodes.Callvirt, PropInfo.GetSetMethod());
    }

    /// <summary>
    /// Renvoie la methode correspondant au type de données pour une lecture dans le DataReader.
    /// </summary>
    /// <param name="p_type"></param>
    /// <returns></returns>
    public static MethodInfo GetDataReaderMethod(Type p_type)
    {
      Type v_dataReaderType = typeof(IDataRecord);
      switch (p_type.FullName)
      {
        case "System.Guid": return v_dataReaderType.GetMethod("GetGuid");//1
        case "System.Byte": return v_dataReaderType.GetMethod("GetByte");//2
        case "System.Int16": return v_dataReaderType.GetMethod("GetInt16");//3
        case "System.Int32": return v_dataReaderType.GetMethod("GetInt32");//4
        case "System.Int64": return v_dataReaderType.GetMethod("GetInt64");//5
        case "System.Double": return v_dataReaderType.GetMethod("GetDouble");//6
        case "System.Single": return v_dataReaderType.GetMethod("GetFloat"); //7
        case "System.Decimal": return v_dataReaderType.GetMethod("GetDecimal");//8
        case "System.DateTime": return v_dataReaderType.GetMethod("GetDateTime");//9
        case "System.Boolean": return v_dataReaderType.GetMethod("GetBoolean");//10
        case "System.Object": return v_dataReaderType.GetMethod("GetValue");//11
        case "System.Byte[]": return v_dataReaderType.GetMethod("GetBytes");//12
        case "System.String": return v_dataReaderType.GetMethod("GetString");//13
        default:
          throw new TypeNotHandledMappingException(p_type.FullName);
      }
    }

    private void GenerateILForMemberWithIgnoringNullValue(
      ILGenerator p_ilout,
      int p_index,
      LocalBuilder p_objectToMap,
      ref LocalBuilder p_memberIndex,
      ColumnExpression[] p_maps)
    {
      MethodInfo v_dataReaderIsDBNull = typeof(IDataRecord).GetMethod(METHOD_NAME_ISDBNULL);
      if (p_memberIndex == null)
        p_memberIndex = p_ilout.DeclareLocal(typeof(int));

      Label v_canBeNullLabel = p_ilout.DefineLabel();

      // pushing the datareader on the stack
      p_ilout.Emit(OpCodes.Ldarg_1);

      //Retrieving the field p_index					
      //p_ilout.Emit(OpCodes.Ldarg_2);
      p_ilout.Emit(OpCodes.Ldc_I4, p_index);
      //p_ilout.Emit(OpCodes.Ldelem_I);

      //Storing it in the p_fieldIndex local variable
      p_ilout.Emit(OpCodes.Dup);
      p_ilout.Emit(OpCodes.Stloc, p_memberIndex);

      // Is NULL?
      p_ilout.Emit(OpCodes.Callvirt, v_dataReaderIsDBNull);

      // If yes go to v_canBeNullLabel
      p_ilout.Emit(OpCodes.Brtrue, v_canBeNullLabel);

      // pushing the object to fill on the stack
      p_ilout.Emit(OpCodes.Ldloc, p_objectToMap);

      // pushing the datareader on the stack
      p_ilout.Emit(OpCodes.Ldarg_1);

      // Getting the ordinal value for the method/field
      p_ilout.Emit(OpCodes.Ldloc, p_memberIndex);

      //GenerateILToGetDataFromDataReader(p_ilout, p_index, TypeInfo, p_maps);
      p_ilout.Emit(OpCodes.Callvirt, GetDataReaderMethod(TypeInfo));

      p_ilout.Emit(OpCodes.Callvirt, PropInfo.GetSetMethod());

      p_ilout.MarkLabel(v_canBeNullLabel);
    }

    internal void GenerateILForNullableMember(
      ILGenerator p_ilout,
      int p_index,
      LocalBuilder p_objectToMap,
      ref LocalBuilder p_memberIndex,
      Type p_nullableType,
      ColumnExpression[] p_maps)
    {
      MethodInfo v_dataReaderIsDBNull = typeof(IDataRecord).GetMethod(METHOD_NAME_ISDBNULL);
      ConstructorInfo v_nullableConstructor = TypeInfo.GetConstructor(new Type[] { p_nullableType });
      LocalBuilder v_nullLocal = p_ilout.DeclareLocal(TypeInfo);
      if (p_memberIndex == null)
      {
        p_memberIndex = p_ilout.DeclareLocal(typeof(int));
      }

      Label v_lblBeginIsNullBlock = p_ilout.DefineLabel();
      Label v_lblEndIsNullBlock = p_ilout.DefineLabel();

      #region Is Null?
      // pushing the datareader on the stack
      p_ilout.Emit(OpCodes.Ldarg_1);

      //Retrieving the field p_index					
      //p_ilout.Emit(OpCodes.Ldarg_2);
      p_ilout.Emit(OpCodes.Ldc_I4, p_index);
      //p_ilout.Emit(OpCodes.Ldelem_I);

      //Storing it in the p_fieldIndex local variable
      p_ilout.Emit(OpCodes.Dup);
      p_ilout.Emit(OpCodes.Stloc, p_memberIndex);

      // Is NULL?
      p_ilout.Emit(OpCodes.Callvirt, v_dataReaderIsDBNull);
      #endregion

      // If yes go to v_lblBeginIsNullBlock
      p_ilout.Emit(OpCodes.Brtrue, v_lblBeginIsNullBlock);

      #region Is Not Null
      // pushing the object to fill on the stack
      p_ilout.Emit(OpCodes.Ldloc, p_objectToMap);

      // pushing the datareader on the stack
      p_ilout.Emit(OpCodes.Ldarg_1);

      // Getting the ordinal value for the method/field
      p_ilout.Emit(OpCodes.Ldloc, p_memberIndex);

      //GenerateILToGetDataFromDataReader(p_ilout, p_index, p_nullableType, p_maps);
      p_ilout.Emit(OpCodes.Callvirt, GetDataReaderMethod(p_nullableType));

      p_ilout.Emit(OpCodes.Newobj, v_nullableConstructor);

      // To branch after the null instruction block
      p_ilout.Emit(OpCodes.Br, v_lblEndIsNullBlock);

      #endregion

      #region Is Null
      p_ilout.MarkLabel(v_lblBeginIsNullBlock);

      // pushing the object to fill on the stack
      p_ilout.Emit(OpCodes.Ldloc, p_objectToMap);

      p_ilout.Emit(OpCodes.Ldloca, v_nullLocal.LocalIndex);
      // Instantiate a Nullable<T> with no value
      p_ilout.Emit(OpCodes.Initobj, TypeInfo);
      p_ilout.Emit(OpCodes.Ldloc, v_nullLocal);

      #endregion

      p_ilout.MarkLabel(v_lblEndIsNullBlock);
      p_ilout.Emit(OpCodes.Callvirt, PropInfo.GetSetMethod());
    }
    #endregion
  }
}
