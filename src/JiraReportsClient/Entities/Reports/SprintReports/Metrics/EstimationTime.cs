using System.Text.Json.Serialization;
using JiraReportsClient.Utils.Structs;

namespace JiraReportsClient.Entities.Reports.SprintReports.Metrics;

[JsonConverter(typeof(EstimationTimeJsonConverter))]
public class EstimationTime
{
    private static readonly EstimationTime Zero = new(0);
    public static EstimationTime Empty => Zero;

    private readonly FixedDouble _timeInSeconds;

    public EstimationTime(double timeInSeconds)
    {
        if (timeInSeconds < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(timeInSeconds), "Time cannot be negative.");
        }

        _timeInSeconds = timeInSeconds;
    }

    public FixedDouble Seconds => _timeInSeconds;

    public FixedDouble Hours => _timeInSeconds / 3600; // Convert seconds to hours

    public FixedDouble Days => Math.Round(Hours / 8, 2); // Convert hours to days assuming 8 hours per day

    public static implicit operator EstimationTime(double seconds) => new(seconds);

    public static implicit operator FixedDouble(EstimationTime estimation) => estimation._timeInSeconds;
    public static implicit operator double(EstimationTime estimation) => estimation._timeInSeconds;
    public static implicit operator EstimationTime(FixedDouble fixedDouble) => new(fixedDouble);

    // Comparison operators with another EstimationTime
    public static bool operator >(EstimationTime a, EstimationTime b) => a._timeInSeconds > b._timeInSeconds;
    public static bool operator <(EstimationTime a, EstimationTime b) => a._timeInSeconds < b._timeInSeconds;
    public static bool operator >=(EstimationTime a, EstimationTime b) => a._timeInSeconds >= b._timeInSeconds;
    public static bool operator <=(EstimationTime a, EstimationTime b) => a._timeInSeconds <= b._timeInSeconds;

    // Comparison operators with double
    public static bool operator >(EstimationTime a, double b) => a._timeInSeconds > b;
    public static bool operator <(EstimationTime a, double b) => a._timeInSeconds < b;
    public static bool operator >=(EstimationTime a, double b) => a._timeInSeconds >= b;
    public static bool operator <=(EstimationTime a, double b) => a._timeInSeconds <= b;

    // Comparison operators with int
    public static bool operator >(EstimationTime a, int b) => a._timeInSeconds > b;
    public static bool operator <(EstimationTime a, int b) => a._timeInSeconds < b;
    public static bool operator >=(EstimationTime a, int b) => a._timeInSeconds >= b;
    public static bool operator <=(EstimationTime a, int b) => a._timeInSeconds <= b;

    public static double operator /(EstimationTime a, EstimationTime b)
    {
        if (b._timeInSeconds == 0)
        {
            throw new DivideByZeroException("Cannot divide by an EstimationTime with zero seconds.");
        }

        return a._timeInSeconds / b._timeInSeconds;
    }

    public static EstimationTime operator +(EstimationTime a, EstimationTime b) => a._timeInSeconds + b._timeInSeconds;
    public static EstimationTime operator +(EstimationTime a, int seconds) => a._timeInSeconds + seconds;
    
    public override string ToString() => $"Time: {Seconds} seconds ({Hours} hours, {Days} days)";
}