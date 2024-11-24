namespace Business.Abstract
{
    public interface IJwtTokenService
    {
        string GenerateToken(string username);
    }
}
