using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace StudyPlannerSoft.Data;

public class UlidValueConverter : ValueConverter<Ulid, string>
{
    public UlidValueConverter() 
        : base(
            ulid => ulid.ToString(),
            str => Ulid.Parse(str))
    { }
}