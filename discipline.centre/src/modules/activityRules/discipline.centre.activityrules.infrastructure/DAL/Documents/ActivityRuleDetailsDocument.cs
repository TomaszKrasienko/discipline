using discipline.centre.shared.infrastructure.DAL;

namespace discipline.centre.activity_rules.infrastructure.DAL.Documents;

public sealed record ActivityRuleDetailsDocument(string Title, string? Note) : IDocument;