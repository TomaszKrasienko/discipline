using discipline.centre.shared.infrastructure.DAL;

namespace discipline.centre.activityrules.infrastructure.DAL.Documents;

internal sealed record StageDocument(string StageId, string Title, int Index) :  IDocument;