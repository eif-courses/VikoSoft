using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace VikoSoft.Data;

public class UlidValueConverter : ValueConverter<Ulid, string>
{
    public UlidValueConverter() 
        : base(
            ulid => ulid.ToString(),
            str => Ulid.Parse(str))
    { }
}