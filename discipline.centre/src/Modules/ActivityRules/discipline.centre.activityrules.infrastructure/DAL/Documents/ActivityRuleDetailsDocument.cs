using discipline.centre.shared.infrastructure.DAL;

namespace discipline.centre.activityrules.infrastructure.DAL.Documents;

public sealed record ActivityRuleDetailsDocument(string Title, string? Note) : IDocument;