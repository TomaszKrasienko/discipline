using discipline.centre.shared.infrastructure.DAL;

namespace discipline.centre.activity_rules.infrastructure.DAL.Documents;

public sealed record ActivityRuleSelectedModeDocument(string Mode, IEnumerable<int>? DaysOfWeek) : IDocument;