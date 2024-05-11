using BusinessLayer.Models.User;
using BusinessLayer.Services;
using BusinessLayer.Services.Abstraction;
using DataAccessLayer.Data;
using TestUtilities.Data;
using TestUtilities.MockedObjects;

namespace BusinessLayer.Tests.Services
{
    public class UserServiceTest
    {
        private DbContextOptions<BookHubDbContext> _dbContextOptions;
        private BookHubDbContext _dbContext;
        private IUserService _userService;

        public UserServiceTest()
        {
            _dbContextOptions = MockedDbContext.GenerateNewInMemoryDbContextOptions();
            _dbContext = MockedDbContext.CreateFromOptions(_dbContextOptions);
            _userService = new UserService(_dbContext);
        }

        [Fact]
        public async Task CreateUserAsync_Succes()
        {
            var createUserModel = TestDataHelperUser.GetCreateUser1();

            var createdResult = await _userService.CreateUserAsync(createUserModel);
            var savedUser = await _userService.GetUserByIdAsync(createdResult.Id);

            Assert.NotNull(createdResult);
            Assert.NotNull(savedUser);

            Assert.Equal(createUserModel.Name, createdResult.Name);
            Assert.Equal(createUserModel.UserName, createdResult.UserName);
            Assert.Equal(createUserModel.Email, createdResult.Email);

            Assert.Equal(createUserModel.Name, savedUser.Name);
            Assert.Equal(createUserModel.UserName, savedUser.UserName);
            Assert.Equal(createUserModel.Email, savedUser.Email);
        }

        [Fact]
        public async Task EditUserAsync_NoUser()
        {
            var editUserModel = new EditUserModel();

            var editResult = await _userService.EditUserAsync("4", editUserModel);

            Assert.Null(editResult);
        }

        [Fact]
        public async Task EditUserAsync_Changed()
        {
            var user = TestDataHelperUser.GetUser1();
            var editUserModel = new EditUserModel()
            {
                UserName = "Palko",
                Name = "Pavol"
            };

            var editResult = await _userService.EditUserAsync(user.Id, editUserModel);
            var editedUser = await _userService.GetUserByIdAsync(user.Id);

            Assert.NotNull(editResult);
            Assert.NotNull(editedUser);
            Assert.Equal(editUserModel.Name, editResult.Name);
            Assert.Equal(editUserModel.UserName, editResult.UserName);
            Assert.Equal(editUserModel.Name, editedUser.Name);
            Assert.Equal(editUserModel.UserName, editedUser.UserName);
        }

        [Fact]
        public async Task GetUserByIdAsync_NoUser()
        {
            var user = await _userService.GetUserByIdAsync("4");
            Assert.Null(user);
        }

        [Fact]
        public async Task GetUserByIdAsync_FindUser()
        {
            var user = TestDataHelperUser.GetUser2();
            var user2 = TestDataHelperUser.GetUser3();
            await _dbContext.AppUsers.AddAsync(user);
            await _dbContext.AppUsers.AddAsync(user2);
            await _dbContext.SaveChangesAsync();

            var foundUser = await _userService.GetUserByIdAsync(user.Id);

            Assert.NotNull(foundUser);
            Assert.Equal(user.Id, foundUser.Id);
            Assert.Equal(user.UserName, foundUser.UserName);
            Assert.Equal(user.Name, foundUser.Name);
            Assert.Equal(user.Email, foundUser.Email);
        }

        [Fact]
        public async Task GetUsersAsync_MultipleUsers()
        {
            var user = TestDataHelperUser.GetUser3();
            var user2 = TestDataHelperUser.GetUser1();
            var user3 = TestDataHelperUser.GetUser2();

            var foundUsers = await _userService.GetUsersAsync();
            var storedUsers = await _dbContext.AppUsers.ToListAsync();

            Assert.NotNull(foundUsers);
            Assert.NotNull(storedUsers);
            Assert.NotEmpty(foundUsers);
            Assert.NotEmpty(storedUsers);
        }

        [Fact]
        public async Task DeleteUserAsync_BadUserId()
        {
            var user = TestDataHelperUser.GetUser3();
            await _dbContext.AppUsers.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            var deleteResult = await _userService.DeleteUserAsync("4");
            Assert.False(deleteResult);
        }

        [Fact]
        public async Task DeleteUserAsync_Success()
        {
            var user = TestDataHelperUser.GetUser3();
            await _dbContext.AppUsers.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            var deleteResult = await _userService.DeleteUserAsync(user.Id);
            var userDeleted = await _userService.GetUserByIdAsync(user.Id);

            Assert.True(deleteResult);
            Assert.Null(userDeleted);
        }
    }
}
