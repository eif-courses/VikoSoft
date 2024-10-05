namespace VikoSoft.Entities;

public class Faculty
{
    public Ulid Id { get; set; } = Ulid.NewUlid();
    public string Name { get; set; } = string.Empty;
    public string ShortName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public ICollection<Department> Departments { get; set; } = new List<Department>();
}