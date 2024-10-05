using VikoSoft.Entities;

namespace VikoSoft.Dto;

public class PlannedGroupDto
{
    public Ulid Id { get; set; } = Ulid.NewUlid();
    public string Name { get; set; } = string.Empty;
    public Semester Semester { get; set; }
    public string? LabelName { get; set; } 

    public int? Vf { get; set; } = 0;
    public int? Vnf { get; set; } = 0;
    public string? SubGroupCount { get; set; } = "1";
    public Ulid StudyProgramId { get; set; }
    public Ulid? LecturerId { get; set; }

    public List<SubjectDto> Subjects { get; set; } = new List<SubjectDto>();
}