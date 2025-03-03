using EventManagement.Application.Interfaces;
namespace EventManagement.Infrastructure.Services;
using BCrypt.Net;


    public class PasswordHasher : IPasswordHasher
    {
        public string Generate(string password) =>
            BCrypt.EnhancedHashPassword(password, HashType.SHA512);
        
        public bool Verify(string password, string hashedPassword) =>
            BCrypt.EnhancedVerify(password, hashedPassword, HashType.SHA512);

    }
