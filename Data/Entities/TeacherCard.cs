using Tarifikacija.Entities;

namespace VikoSoft.Data.Entities;

public enum TeacherCardSheetTypes
{
    FULLTIME = 0,
    HALFTIME = 1,
    OVERTIME = 2
}

public class TeacherCardSheetActivity : BaseEntity
{
    private int _hoursSpent;

    public TeacherCardSheet Sheet { get; set; }
    public Activity Activity { get; set; }

    public int HoursSpent
    {
        get => _hoursSpent;
        set
        {
            if (value > Activity.MaxHours)
            {
                throw new ArgumentException($"Hours spent ({value}) cannot exceed max allowed hours ({Activity.MaxHours}) for activity {Activity.Title}.");
            }
            _hoursSpent = value;
        }
    }
}

public class TeacherCardSheet : BaseEntity
{
    public TeacherCardSheetTypes SheetType { get; set; }
    public ICollection<TeacherCardSheetActivity> Activities { get; set; } = new List<TeacherCardSheetActivity>();
}

public class TeacherCard : BaseEntity
{
    public string Title { get; set; }
    public int Year { get; set; }
    public ICollection<TeacherCardSheet> Sheets { get; set; } = new List<TeacherCardSheet>();
}

public class ActivityCategory : BaseEntity
{
    public string Title { get; set; } = string.Empty;
}

public class Activity : BaseEntity
{
    public string Identifier { get; set; }
    public string Title { get; set; }
    public int MaxHours { get; set; }
    public string Comments { get; set; }
    public ActivityCategory Category { get; set; }
}