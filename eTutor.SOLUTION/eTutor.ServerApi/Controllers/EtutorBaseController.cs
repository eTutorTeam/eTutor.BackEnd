using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using static System.Int32;

namespace eTutor.ServerApi.Controllers
{
    public class EtutorBaseController : ControllerBase
    {
        protected int GetUserId()
        {
            try
            {
                var claims = HttpContext.User.Claims;
                Claim idClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
                int value = 0;
                value = Parse(idClaim.Value);
                return value;
            }
            catch (Exception e)
            {
                return -1;
            }
        }

        protected string GetUserEmail()
        {
            try
            {
                IEnumerable<Claim> claims = HttpContext.User.Claims;
                Claim emailCalim = claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);
                return emailCalim.Value;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        
    }
}
