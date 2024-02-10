using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Time2PartB
{
    private char[] hour;
    private char[] minute;
    private char[] second;


    public Time2PartB()
    {
        Hour = new char[2];
        Minute = new char[2];
        Second = new char[2];
    }


    public Time2PartB(char time1, char time2, char time3, char time4, char time5, char time6)

    {

        SetTime(time1, time2, time3, time4, time5, time6);

    }


    public Time2PartB(Time2PartB time)
            : this(time.Hour[0], time.Hour[1], time.Minute[0], time.Minute[1], time.Second[0], time.Second[1]) { }


    public void SetTime(char time1, char time2, char time3, char time4, char time5, char time6)
    {
         
        Hour = new[] { time1, time2 };
        Minute = new[] { time3, time4 };
        Second = new[] { time5, time6 };

    }

    public char[] Hour
    {
        get
        {
            return hour;
        }
        set
        {
            if (value.Length != 2)
            {
                throw new ArgumentOutOfRangeException(nameof(value),
                    value, $"{nameof(Hour)} must be 0-23");
            }
            hour = value;
        }
    }

    public char[] Minute
    {
        get
        {
            return minute;
        }
        set
        {
            if (value.Length != 2)
            {
                throw new ArgumentOutOfRangeException(nameof(value),
                    value, $"{nameof(Minute)} must be 0-59");
            }
            minute = value;
        }

    }

    public char[] Second
    {
        get
        {
            return second;
        }
        set
        {
            if (value.Length != 2)
            {
                throw new ArgumentOutOfRangeException(nameof(value),
                    value, $"{nameof(Second)} must be 0-59");
            }
            second = value;
        }
    }

    public virtual string ToUniversalString() =>
        $"Time2sw: {new string(Hour)}:{new string(Minute)}:{new string(Second)}";


    public override string ToString()
    {
        int hour = Convert.ToInt32(new string(Hour));
        string period = (hour < 12) ? "AM" : "PM";
        hour = (hour == 0 || hour == 12) ? 12 : hour % 12;
        return $"Time2sw: {hour}:{new string(Minute)}:{new string(Second)} {period}";
    }

}
