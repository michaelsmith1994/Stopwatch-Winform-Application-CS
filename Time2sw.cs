using System;
public class Time2sw : Time2PartB
{
    public int Hours { get; set; }
    public int Minutes { get; set; }
    public int Seconds { get; set; }
    private int milliseconds;

    public int Milliseconds
    {
        get { return milliseconds; }
        set
        {
            if (value < 0 || value > 999)
            {
                throw new ArgumentOutOfRangeException(nameof(value), value, $"{nameof(Milliseconds)} value is outside the range in requirements.");
            }
            milliseconds = value;
        }
    }

    public void SetTimeExtend(int milliseconds)
    {
        Milliseconds = milliseconds;
    }

    public Time2sw(int hour, int minute, int seconds, int milliseconds) : base()
    {
        Console.WriteLine($"Setting Time2sw: {hour}h {minute}m {seconds}s {milliseconds}ms");
        Hours = hour;
        Minutes = minute;
        Seconds = seconds;
        Milliseconds = milliseconds;
    }

    public Time2sw(Time2PartB time) : base(time)
    {
        SetTimeExtend(0);
    }

    public Time2sw(Time2sw time)
            : base(time)
    {
        SetTimeExtend(time.Milliseconds);
    }
    public override string ToUniversalString()
    {
        return $"{base.ToUniversalString()}:{Milliseconds:D3}";
    }

    public override string ToString()
    {
        return $"{base.ToString()}:{Milliseconds:D3}";
    }

}
