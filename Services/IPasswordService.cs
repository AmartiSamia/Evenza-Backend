namespace Project.API.Services
{
    public interface IPasswordService
    {
        List<string> ValidatePassword(string password);
        bool IsPasswordValid(string password);
    }
}
