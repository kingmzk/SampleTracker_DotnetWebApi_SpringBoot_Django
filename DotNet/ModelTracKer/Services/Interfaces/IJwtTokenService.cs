namespace ModelTracKer.Services.Interfaces
{
    public interface IJwtTokenService
    {
        string GenerateToken(string username, string role);
    }
}
