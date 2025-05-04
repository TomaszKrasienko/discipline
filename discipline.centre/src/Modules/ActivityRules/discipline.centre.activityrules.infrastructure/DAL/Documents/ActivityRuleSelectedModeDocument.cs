using discipline.centre.shared.infrastructure.DAL;

namespace discipline.centre.activityrules.infrastructure.DAL.Documents;

public sealed record ActivityRuleSelectedModeDocument(string Mode, IReadOnlySet<int>? DaysOfWeek) : IDocument;