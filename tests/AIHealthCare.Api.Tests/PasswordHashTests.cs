namespace AIHealthCare.Api.Tests;
using AIHealthCare.Infrastructure.Data;

public sealed class PasswordHashTests
{
    [Fact]
    public void HashAndVerify_ShouldRoundTrip()
    {
        const string password = "CorrectHorseBatteryStaple123!";
        var hash = DbSeeder.HashPassword(password);

        Assert.True(DbSeeder.VerifyPassword(password, hash));
        Assert.False(DbSeeder.VerifyPassword("WrongPassword", hash));
    }
}
