using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace JiraReportsClient.Utils.SchemaGenerators;

public static class PlainTextSchemaGenerator
{
    private static readonly HashSet<Type> ProcessedTypes = [];

    private static readonly HashSet<Type> PrimitiveTypes =
    [
        typeof(string),
        typeof(int),
        typeof(long),
        typeof(float),
        typeof(double),
        typeof(decimal),
        typeof(bool),
        typeof(DateTime),
        typeof(Guid),
        typeof(TimeSpan),
        typeof(DateTimeOffset)
    ];

    public static string GenerateSchema<T>()
    {
        ProcessedTypes.Clear();
        var sb = new StringBuilder();
        PrintType(typeof(T), sb, 0);
        return sb.ToString();
    }

    private static void PrintType(Type type, StringBuilder sb, int indent)
    {
        if (ProcessedTypes.Contains(type))
            return;

        ProcessedTypes.Add(type);

        // Print type name
        sb.AppendLine($"{new string(' ', indent)}{GetFriendlyTypeName(type)}");

        // Get all properties
        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        foreach (var prop in properties)
        {
            string propertyType = GetPropertyTypeDescription(prop);
            sb.AppendLine($"{new string(' ', indent + 2)}- {prop.Name} ({propertyType})");

            // If it's a complex type that we haven't processed yet, process it
            Type typeToProcess = GetTypeToProcess(prop.PropertyType);
            if (typeToProcess != null && !PrimitiveTypes.Contains(typeToProcess))
            {
                sb.AppendLine();
                PrintType(typeToProcess, sb, indent + 4);
            }
        }
    }

    private static string GetPropertyTypeDescription(PropertyInfo prop)
    {
        Type type = prop.PropertyType;

        // Handle nullable types
        if (Nullable.GetUnderlyingType(type) != null)
        {
            return $"nullable {GetFriendlyTypeName(Nullable.GetUnderlyingType(type))}";
        }

        // Handle collections
        if (type.IsGenericType && typeof(IEnumerable).IsAssignableFrom(type))
        {
            Type itemType = type.GetGenericArguments().FirstOrDefault();
            if (itemType != null)
            {
                return $"list of {GetFriendlyTypeName(itemType)}";
            }
        }

        return GetFriendlyTypeName(type);
    }

    private static string GetFriendlyTypeName(Type type)
    {
        if (type == typeof(string)) return "string";
        if (type == typeof(int)) return "integer";
        if (type == typeof(long)) return "long";
        if (type == typeof(float)) return "float";
        if (type == typeof(double)) return "double";
        if (type == typeof(decimal)) return "decimal";
        if (type == typeof(bool)) return "boolean";
        if (type == typeof(DateTime)) return "datetime";
        if (type == typeof(Guid)) return "guid";
        if (type == typeof(TimeSpan)) return "timespan";
        if (type == typeof(DateTimeOffset)) return "datetimeoffset";

        return type.Name;
    }

    private static Type GetTypeToProcess(Type type)
    {
        if (Nullable.GetUnderlyingType(type) != null)
        {
            type = Nullable.GetUnderlyingType(type);
        }

        if (type.IsGenericType && typeof(IEnumerable).IsAssignableFrom(type))
        {
            type = type.GetGenericArguments().FirstOrDefault();
        }

        if (type?.IsClass == true && type != typeof(string))
        {
            return type;
        }

        return null;
    }
}