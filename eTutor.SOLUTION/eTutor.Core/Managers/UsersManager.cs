using System.Threading.Tasks;
using eTutor.Core.Contracts;
using eTutor.Core.Models;
using Microsoft.AspNetCore.Identity;

namespace eTutor.Core.Managers
{
    public class UsersManager
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public UsersManager(SignInManager<User> signInManager, UserManager<User> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }


        public async Task<IOperationResult<User>> AuthenticateUser(string email, string password)
        {
            var signInResult = await _signInManager.PasswordSignInAsync(email, password, false, false);

            if (!signInResult.Succeeded)
            {
                if (signInResult.IsNotAllowed)
                {
                    return BasicOperationResult<User>.Fail("User is not allowed");
                }

                if (signInResult.IsLockedOut)
                {
                    return BasicOperationResult<User>.Fail("User is locked out");
                }

                if (signInResult.RequiresTwoFactor)
                {
                    return  BasicOperationResult<User>.Fail("User requires two factor authentication");
                }
                
                return BasicOperationResult<User>.Fail("User may not exist, check your email and password");
            }

        }
    }
}