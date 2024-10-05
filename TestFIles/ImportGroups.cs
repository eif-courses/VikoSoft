using VikoSoft.Data;
using VikoSoft.Services;

namespace VikoSoft.TestFIles;

internal sealed class ImportPlannedGroupsRequest
{
    public IFormFile File { get; set; }
}

internal sealed class ImportGroups
{
    private readonly VikoDbContext _dbContext;
    private readonly ImportGroupsService _importGroupsService;
    private readonly ILogger<ImportGroups> _logger;

    public ImportGroups(VikoDbContext dbContext, ImportGroupsService importGroupsService,
        ILogger<ImportGroups> logger)
    {
        _dbContext = dbContext;
        _importGroupsService = importGroupsService;
        _logger = logger;
    }
    
    public async Task HandleAsync(ImportPlannedGroupsRequest req, CancellationToken ct)
    {
        var groups = _importGroupsService.ImportFromExcel(req.File.OpenReadStream());
        _dbContext.PlannedGroups.AddRange(groups);

        await _dbContext.SaveChangesAsync(ct);
    }
}