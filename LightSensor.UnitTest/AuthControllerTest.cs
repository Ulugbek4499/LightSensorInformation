using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using Newtonsoft.Json;
using Server.Controllers;
using Server.DataBase;
using Server.Entities.Identity;

namespace LightSensor.UnitTest;

public class AuthControllerTest
{
    private readonly AuthController _authController;
    private static readonly List<User> UsersData = new List<User>
    {
     new User
     {
         Id = 1,
         Username = "Jamshid",
         PasswordHash = BCrypt.Net.BCrypt.HashPassword("Jamshid123")
     },
     new User
     {
         Id = 2,
         Username = "Bahrombek",
         PasswordHash = BCrypt.Net.BCrypt.HashPassword("Bahrombek123")
     },
      new User
     {
         Id = 3,
        Username = "Elyor",
        PasswordHash = BCrypt.Net.BCrypt.HashPassword("Elyor123")
        }
    };

    public AuthControllerTest()
    {
        var configurationMock = new Mock<IConfiguration>();
        configurationMock.Setup(x => x.GetSection("Jwt:Token").Value).Returns("ThisIsSecretKey MySecretKey Secret Key");

        var usersDbSetMock = SetUpUsersDbSet(UsersData);

        var dbContextMock = new Mock<IApplicationDbContext>();
        dbContextMock.Setup(x => x.Users).Returns(usersDbSetMock.Object);

        _authController = new AuthController(configurationMock.Object, dbContextMock.Object);
    }


    private static Mock<DbSet<User>> SetUpUsersDbSet(List<User> users)
    {
        var usersQueryable = users.AsQueryable();
        var usersDbSetMock = new Mock<DbSet<User>>();
        usersDbSetMock.As<IQueryable<User>>().Setup(m => m.Provider).Returns(usersQueryable.Provider);
        usersDbSetMock.As<IQueryable<User>>().Setup(m => m.Expression).Returns(usersQueryable.Expression);
        usersDbSetMock.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(usersQueryable.ElementType);
        usersDbSetMock.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(() => usersQueryable.GetEnumerator());

        return usersDbSetMock;
    }

    [Theory]
    [InlineData("NewUser", "NewUser123")]       //Here is the new username which database does not have that
    [InlineData("NewUser2", "NewUser1234")]
    public async Task UserRegister_Success(string username, string password)
    {
        // Arrange
        var userDto = new UserDto
        {
            Username = username,
            Password = password
        };

        // Act
        var result = await _authController.Register(userDto);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result.Result);

        var okObjectResult = (OkObjectResult)result.Result;
        Assert.Equal(200, okObjectResult.StatusCode);

        // Ensure that the result value is not null
        Assert.NotNull(okObjectResult.Value);
    }

    [Theory]
    [InlineData("Jamshid", "Jamshid123")]      // User with this username already exists in the database which is here in our array
    [InlineData("Bahrombek", "Bahrombek123")] 
    public async Task UserRegister_Fail(string username, string password)
    {
        // Arrange
        var userDto = new UserDto
        {
            Username = username,
            Password = password
        };

        // Act
        var result = await _authController.Register(userDto);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<BadRequestObjectResult>(result.Result);

        var badRequestResult = (BadRequestObjectResult)result.Result;
        Assert.Equal(400, badRequestResult.StatusCode);
        Assert.Equal("Username already exists. Please choose a different username.", badRequestResult.Value);
    }

    [Theory]
    [InlineData("Jamshid", "Jamshid123")]
    [InlineData("Bahrombek", "Bahrombek123")]
    [InlineData("Elyor", "Elyor123")]
    public void UserLogin_Success(string username, string password)
    {
        // Arrange
        var userDto = new UserDto
        {
            Username = username,
            Password = password
        };

        // Act
        var result = _authController.Login(userDto);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result.Result);

        var okObjectResult = (OkObjectResult)result.Result;
        Assert.Equal(200, okObjectResult.StatusCode);
        Assert.NotNull(okObjectResult.Value);

        var token = okObjectResult.Value.ToString();
        Assert.NotEmpty(token);
    }

    [Theory]
    [InlineData("Jamshid", "IncorrectPaswordHere")]
    [InlineData("Bahrombek", "IncorrectPaswordHere")]
    [InlineData("Elyor", "IncorrectPaswordHere")]
    public void UserLogin_Fail(string username, string password)
    {
        // Arrange
        var userDto = new UserDto
        {
            Username = username,
            Password = password
        };

        // Act
        var result = _authController.Login(userDto);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<BadRequestObjectResult>(result.Result);

        var badRequestResult = (BadRequestObjectResult)result.Result;
        Assert.Equal(400, badRequestResult.StatusCode);
        Assert.Equal("Invalid password", badRequestResult.Value);
    }
}
