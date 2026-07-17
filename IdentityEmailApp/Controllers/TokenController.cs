using IdentityEmailApp.Models.JWTModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IdentityEmailApp.Controllers
{
    public class TokenController : Controller
    {
        private readonly JWTSettingsViewModel _settingsViewModel;

        public TokenController(IOptions<JWTSettingsViewModel> settingsViewModel)
        {
            _settingsViewModel = settingsViewModel.Value;
        }

        [HttpGet]
        public IActionResult Generate(SimpleUserViewModel model)
        {
            var claim = new[]
            {
                new Claim("name",model.Name),
                new Claim("surname",model.Surname),
                new Claim("city",model.City),
                new Claim("username",model.UserName),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settingsViewModel.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(

                issuer: _settingsViewModel.Issuer,
                audience: _settingsViewModel.Audience,
                claims: claim,
                expires: DateTime.UtcNow.AddMinutes(_settingsViewModel.ExpireMinutes),
                signingCredentials:creds
                
                );

            model.Token = new JwtSecurityTokenHandler().WriteToken(token);
            return View(model);
        }       
    }
}
