using MyProject.Models;
using System.Threading.Tasks;

public interface ITokenService
{
    Task<string> GenerateTokenAsync(User user);
}
