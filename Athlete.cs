using System;
using System.Collections.Generic;

public class Athlete
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public List<Time2sw> Laps { get; }

    public Athlete(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
        Laps = new List<Time2sw>();
    }

    public void AddLapTime(int hour, int minute, int seconds, int milliseconds)
    {
        var lapTime = new Time2sw(hour, minute, seconds, milliseconds);
        Laps.Add(lapTime);
    }

    public void AddLapTime(Time2sw lapTime)
    {
        Laps.Add(lapTime);
    }

    public void ClearLaps()
    {
        Laps.Clear();
    }

    public override string ToString()
    {
        return $"{FirstName} {LastName}";
    }
}
