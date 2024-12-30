using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace JiraReportsClient.Utils.SchemaGenerators;

public static class CSharpSchemaGenerator
{
    private static readonly HashSet<Type> _processedTypes = new();
    
    public static string GenerateSchema<T>()
    {
        _processedTypes.Clear();
        var sb = new StringBuilder();
        ProcessType(typeof(T), sb, 0);
        return sb.ToString();
    }

    private static void ProcessType(Type type, StringBuilder sb, int indent)
    {
        if (_processedTypes.Contains(type)) return;
        _processedTypes.Add(type);

        if (type.IsEnum)
        {
            GenerateEnum(type, sb, indent);
            return;
        }

        GenerateClass(type, sb, indent);
        
        foreach (var property in type.GetProperties())
        {
            var propertyType = GetUnderlyingType(property.PropertyType);
            if (ShouldProcessType(propertyType))
            {
                sb.AppendLine();
                ProcessType(propertyType, sb, indent);
            }
        }
    }

    private static void GenerateConstructors(Type type, StringBuilder sb, int indent)
    {
        var indentStr = new string(' ', indent);
        var constructors = type.GetConstructors(BindingFlags.Public | BindingFlags.Instance)
            .Where(c => c.GetParameters().Length > 0)
            .ToList();

        foreach (var ctor in constructors)
        {
            var parameters = ctor.GetParameters();
            var paramList = string.Join(", ", parameters.Select(p => $"{GetTypeName(p.ParameterType)} {p.Name}"));
            
            // If it's the primary constructor (C# 12+)
            if (type.GetCustomAttributes().Any(attr => attr.GetType().Name == "PrimaryConstructorAttribute"))
            {
                sb.AppendLine($"{indentStr}public class {type.Name}({paramList})");
                return;
            }

            sb.AppendLine($"{indentStr}    public {type.Name.Split('`')[0]}({paramList})");
            sb.AppendLine($"{indentStr}    {{");
            foreach (var param in parameters)
            {
                sb.AppendLine($"{indentStr}        this.{char.ToUpper(param.Name[0]) + param.Name.Substring(1)} = {param.Name};");
            }
            sb.AppendLine($"{indentStr}    }}");
            sb.AppendLine();
        }
    }

    private static void GenerateEnum(Type type, StringBuilder sb, int indent)
    {
        var indentStr = new string(' ', indent);
        sb.AppendLine($"{indentStr}public enum {type.Name}");
        sb.AppendLine($"{indentStr}{{");
        
        var values = Enum.GetValues(type);
        foreach (var value in values)
        {
            sb.AppendLine($"{indentStr}    {Enum.GetName(type, value)} = {(int)value},");
        }
        
        sb.AppendLine($"{indentStr}}}");
    }

    private static void GenerateClass(Type type, StringBuilder sb, int indent)
    {
        var indentStr = new string(' ', indent);
        var isAbstract = type.IsAbstract ? "abstract " : "";
        var baseTypes = GetBaseTypes(type);
        var inheritance = baseTypes.Any() ? $" : {string.Join(", ", baseTypes)}" : "";

        sb.AppendLine($"{indentStr}public {isAbstract}class {GetCleanTypeName(type)}{inheritance}");
        sb.AppendLine($"{indentStr}{{");

        GenerateConstructors(type, sb, indent);

        foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
        {
            var typeName = GetTypeName(prop.PropertyType);
            var getter = prop.GetMethod?.IsAbstract == true ? ";" : " { get; set; }";
            var abstModifier = prop.GetMethod?.IsAbstract == true ? "abstract " : "";
            sb.AppendLine($"{indentStr}    public {abstModifier}{typeName} {prop.Name}{getter}");
        }

        sb.AppendLine($"{indentStr}}}");
    }

    private static string GetCleanTypeName(Type type)
    {
        if (!type.IsGenericType) return type.Name;
        
        var baseName = type.Name.Split('`')[0];
        var genericArgs = string.Join(", ", type.GetGenericArguments().Select(GetTypeName));
        return $"{baseName}<{genericArgs}>";
    }

    private static string GetTypeName(Type type)
    {
        if (type == typeof(string)) return "string";
        if (type == typeof(int)) return "int";
        if (type == typeof(long)) return "long";
        if (type == typeof(double)) return "double";
        if (type == typeof(bool)) return "bool";
        if (type == typeof(DateTime)) return "DateTime";

        if (type.IsGenericType)
        {
            var genericType = type.GetGenericTypeDefinition();
            if (genericType == typeof(Nullable<>))
                return $"{GetTypeName(Nullable.GetUnderlyingType(type))}?";
                
            var typeName = type.GetGenericTypeDefinition().Name.Split('`')[0];
            var genericArgs = string.Join(", ", type.GetGenericArguments().Select(GetTypeName));
            
            if (genericType == typeof(List<>) || genericType == typeof(IReadOnlyList<>) || genericType == typeof(IEnumerable<>))
                return $"List<{genericArgs}>";
            if (genericType == typeof(Dictionary<,>) || genericType == typeof(IDictionary<,>) || genericType == typeof(IReadOnlyDictionary<,>))
                return $"Dictionary<{genericArgs}>";
                
            return $"{typeName}<{genericArgs}>";
        }

        return type.Name;
    }

    private static IEnumerable<string> GetBaseTypes(Type type)
    {
        var types = new List<string>();
        if (type.BaseType != null && type.BaseType != typeof(object))
            types.Add(GetCleanTypeName(type.BaseType));
        types.AddRange(type.GetInterfaces().Select(GetCleanTypeName));
        return types;
    }

    private static bool ShouldProcessType(Type type)
    {
        return (type.IsClass || type.IsEnum) 
            && type != typeof(string) 
            && !type.IsPrimitive 
            && !_processedTypes.Contains(type);
    }

    private static Type GetUnderlyingType(Type type)
    {
        if (type.IsGenericType)
        {
            var genericTypeDef = type.GetGenericTypeDefinition();
            if (genericTypeDef == typeof(Nullable<>))
                return Nullable.GetUnderlyingType(type);
                
            if (typeof(IEnumerable).IsAssignableFrom(type))
                return type.GetGenericArguments()[0];
                
            if (genericTypeDef == typeof(IDictionary<,>) || 
                genericTypeDef == typeof(Dictionary<,>) || 
                genericTypeDef == typeof(IReadOnlyDictionary<,>))
                return type.GetGenericArguments()[1];
        }
        
        return type;
    }
}