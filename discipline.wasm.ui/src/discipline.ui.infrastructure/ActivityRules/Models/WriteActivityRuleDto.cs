namespace discipline.ui.infrastructure.ActivityRules.Models;

public class WriteActivityRuleDto
{
    public string? Title { get; set; }
    public string? Note { get; set; }
    public string? Mode { get; set; }
    public List<int>? Days { get; set; }
}