using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TaskManagementApi.Data;
using TaskManagementApi.DTOs;
using TaskManagementApi.Services;

namespace TaskManagementApi.Tests;

public class AuthServiceTests
{
    private AppDbContext CreateInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    private IConfiguration CreateConfig()
    {
        var inMemorySettings = new Dictionary<string, string?>
        {
            { "Jwt:Key", "test-super-secret-key-minimum-32-characters!" },
            { "Jwt:Issuer", "test-issuer" },
            { "Jwt:Audience", "test-audience" }
        };
        return new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();
    }

    [Fact]
    public async Task Register_NewUser_ReturnsToken()
    {
        var context = CreateInMemoryContext();
        var service = new AuthService(context, CreateConfig());

        var result = await service.RegisterAsync(new RegisterDto
        {
            Email = "test@example.com",
            Password = "Test123!",
            FirstName = "Kalyani",
            LastName = "Deshpande"
        });

        Assert.NotNull(result);
        Assert.NotEmpty(result.Token);
        Assert.Equal("test@example.com", result.Email);
    }

    [Fact]
    public async Task Register_DuplicateEmail_ReturnsNull()
    {
        var context = CreateInMemoryContext();
        var service = new AuthService(context, CreateConfig());

        await service.RegisterAsync(new RegisterDto
        {
            Email = "dup@example.com",
            Password = "Test123!",
            FirstName = "Test",
            LastName = "User"
        });

        var result = await service.RegisterAsync(new RegisterDto
        {
            Email = "dup@example.com",
            Password = "Test123!",
            FirstName = "Test",
            LastName = "User"
        });

        Assert.Null(result);
    }

    [Fact]
    public async Task Login_ValidCredentials_ReturnsToken()
    {
        var context = CreateInMemoryContext();
        var service = new AuthService(context, CreateConfig());

        await service.RegisterAsync(new RegisterDto
        {
            Email = "login@example.com",
            Password = "Test123!",
            FirstName = "Test",
            LastName = "User"
        });

        var result = await service.LoginAsync(new LoginDto
        {
            Email = "login@example.com",
            Password = "Test123!"
        });

        Assert.NotNull(result);
        Assert.NotEmpty(result.Token);
    }

    [Fact]
    public async Task Login_WrongPassword_ReturnsNull()
    {
        var context = CreateInMemoryContext();
        var service = new AuthService(context, CreateConfig());

        await service.RegisterAsync(new RegisterDto
        {
            Email = "wrong@example.com",
            Password = "Correct123!",
            FirstName = "Test",
            LastName = "User"
        });

        var result = await service.LoginAsync(new LoginDto
        {
            Email = "wrong@example.com",
            Password = "WrongPassword!"
        });

        Assert.Null(result);
    }

    [Fact]
    public async Task Login_NonExistentUser_ReturnsNull()
    {
        var context = CreateInMemoryContext();
        var service = new AuthService(context, CreateConfig());

        var result = await service.LoginAsync(new LoginDto
        {
            Email = "nobody@example.com",
            Password = "Test123!"
        });

        Assert.Null(result);
    }
}