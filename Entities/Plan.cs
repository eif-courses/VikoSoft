using Microsoft.EntityFrameworkCore;

namespace StudyPlannerSoft.Entities;

public class Plan
{
    public Ulid Id { get; set; } = Ulid.NewUlid();
    public string Name { get; set; } = string.Empty; // Plan name (could be related to the group name)
    public string? Label { get; set; } = string.Empty; // Could be associated with the group label
    public List<PlannedSubject> PlanSubjects { get; set; } = new List<PlannedSubject>(); // Associated subjects
}

public static class PlanEntityConfiguration
{
    public static void Configure(ModelBuilder modelBuilder)
    {
    }
}