namespace KnowledgeSpace.BackendServer.Service
{
    public interface ISequenceService
    {
        Task<int> GetKnowledgeBaseNewId(); 
    }
}
