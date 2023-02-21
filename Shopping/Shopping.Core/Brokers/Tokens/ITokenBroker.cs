using Shopping.Core.Models.Users;

namespace Shopping.Core.Brokers.Tokens
{
    public interface ITokenBroker
    {
        string GenerateJWT(User user);
    }
}
