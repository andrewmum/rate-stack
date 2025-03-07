namespace rate_app.Core.Entities
{
    public interface ICurrentUserService
    {
        string GetUserId();
        string GetEmail();
        string GetName();
        bool IsAuthenticated();
    }
}
