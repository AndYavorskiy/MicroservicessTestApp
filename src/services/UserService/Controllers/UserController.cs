using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Entities;
using UserService.Models;
using UserService.Repositories;

namespace UserService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private const int PasswordMinLength = 8;

        private readonly ILogger<UserController> logger;
        private readonly IUserRepository userRepository;

        public UserController(IUserRepository medicamentsRepository, ILogger<UserController> logger)
        {
            this.userRepository = medicamentsRepository;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<UserModel>>> Get()
        {
            return new ObjectResult((await userRepository.GetAll())
                .Select(MapToModel)
                .ToList());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserModel>> Get(string id)
        {
            var food = await userRepository.Get(id);
            if (food == null)
            {
                return new NotFoundResult();
            }

            return new ObjectResult(MapToModel(food));
        }

        [HttpPost]
        public async Task<ActionResult<UserDetailsModel>> Create(UserCreateModel createModel)
        {
            if (string.IsNullOrEmpty(createModel.Password) || createModel.Password.Length < PasswordMinLength)
            {
                return new BadRequestResult();
            }

            var user = new User
            {
                FirstName = createModel.FirstName,
                LastName = createModel.LastName,
                Email = createModel.Email,
                Password = createModel.Password,
                Birthday = createModel.Birthday
            };

            await userRepository.Create(user);

            //TODO: notify another services

            return new ObjectResult(MapToModel(user));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UserDetailsModel>> Update(string id, [FromBody] UserModel userModel)
        {
            var user = await userRepository.Get(id);
            if (user == null)
            {
                return new NotFoundResult();
            }

            user.FirstName = userModel.FirstName;
            user.LastName = userModel.LastName;
            user.Email = userModel.Email;
            user.Birthday = userModel.Birthday;

            await userRepository.Update(user);

            //TODO: notify another services

            return new ObjectResult(MapToModel(user));
        }

        [HttpPost("{id}/status")]
        public async Task<IActionResult> ChangeStatus(string id, [FromBody]bool isActive)
        {
            var user = await userRepository.Get(id);
            if (user == null)
            {
                return new NotFoundResult();
            }

            user.IsActive = isActive;

            await userRepository.Update(user);

            return Ok();
        }

        [HttpPut("{id}/credentials")]
        public async Task<ActionResult> ChangePassword(string id, [FromBody] ChangePasswordModel passwordModel)
        {
            var user = await userRepository.Get(id);
            if (user == null)
            {
                return new NotFoundResult();
            }

            if (user.Password != passwordModel.OldPassword || string.IsNullOrEmpty(passwordModel.NewPassword) || passwordModel.NewPassword.Length < PasswordMinLength)
            {
                return new BadRequestResult();
            }

            user.Password = passwordModel.NewPassword;

            await userRepository.Update(user);

            //TODO: notify another services (Authorization)

            return Ok();
        }

        private static UserDetailsModel MapToModel(User user) => new UserDetailsModel
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Birthday = user.Birthday,
            IsActive = user.IsActive
        };
    }
}
