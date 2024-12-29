using System.Text.Json.Serialization;
using JiraReportsClient.Utils.Json.Converters;

namespace JiraReportsClient.Utils.Structs;

public struct FixedDouble : IEquatable<FixedDouble>
{
    [JsonConverter(typeof(FixedDoubleJsonConverter))]
    public double Value { get; set; }

    public FixedDouble(double value)
    {
        Value = Math.Round(value, 2); 
    }

    public static implicit operator FixedDouble(double value) => new (value);
    public static implicit operator double(FixedDouble fixedDouble) => fixedDouble.Value;
    
    public override string ToString()
    {
        return Value.ToString("F2");
    }

    public static FixedDouble operator +(FixedDouble a, FixedDouble b) => new FixedDouble(a.Value + b.Value);
    public static FixedDouble operator -(FixedDouble a, FixedDouble b) => new FixedDouble(a.Value - b.Value);
    public static FixedDouble operator *(FixedDouble a, FixedDouble b) => new FixedDouble(a.Value * b.Value);
    public static FixedDouble operator /(FixedDouble a, FixedDouble b) => new FixedDouble(a.Value / b.Value);
    public static bool operator >(FixedDouble a, FixedDouble b) => a.Value > b.Value;
    public static bool operator <(FixedDouble a, FixedDouble b) => a.Value < b.Value;
    public static bool operator >=(FixedDouble a, FixedDouble b) => a.Value >= b.Value;
    public static bool operator <=(FixedDouble a, FixedDouble b) => a.Value <= b.Value;
    public static bool operator ==(FixedDouble a, FixedDouble b) => a.Value == b.Value;
    public static bool operator !=(FixedDouble a, FixedDouble b) => a.Value != b.Value;
    


    
    public override bool Equals(object? obj)
    {
        return obj is FixedDouble other && Value.Equals(other.Value);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public bool Equals(FixedDouble other)
    {
        return Value.Equals(other.Value);
    }
}