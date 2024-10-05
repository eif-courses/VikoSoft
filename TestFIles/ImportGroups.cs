using FastEndpoints;
using StudyPlannerSoft.Data;

namespace StudyPlannerSoft.Features.Groups;

internal sealed class ImportPlannedGroupsRequest
{
    public IFormFile File { get; set; }
}

internal sealed class ImportGroups : Endpoint<ImportPlannedGroupsRequest>
{
    private readonly MyDatabaseContext _dbContext;
    private readonly ImportGroupsService _importGroupsService;
    private readonly ILogger<ImportGroups> _logger;

    public ImportGroups(MyDatabaseContext dbContext, ImportGroupsService importGroupsService,
        ILogger<ImportGroups> logger)
    {
        _dbContext = dbContext;
        _importGroupsService = importGroupsService;
        _logger = logger;
    }

    public override void Configure()
    {
        Post("planned-groups/import");
        AllowFileUploads();
        AllowAnonymous();
    }

    public override async Task HandleAsync(ImportPlannedGroupsRequest req, CancellationToken ct)
    {
        var groups = _importGroupsService.ImportFromExcel(req.File.OpenReadStream());
        _dbContext.PlannedGroups.AddRange(groups);

        await _dbContext.SaveChangesAsync(ct);
        await SendOkAsync(ct);
    }
}