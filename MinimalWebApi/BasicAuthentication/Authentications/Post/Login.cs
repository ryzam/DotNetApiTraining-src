using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BasicAuthentication.Authentication;

    public record User
    {
        public string Username { get; set;}
        public string Password { get; set;}
    }

    public static class LoginEndPoints
    {
        public static void Features(WebApplication app)
        {
            app.MapPost("/login",([FromBody]User user)=>{
                Console.WriteLine(user.UserName);
                //return TypedResults.Ok(user.UserName);
                
                if (user.Username == "test" && user.Password == "password")
                {
                    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Your_Secret_Key_12345678910111213"));

                    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                    var claims = new[]
                        {
                            new Claim(JwtRegisteredClaimNames.Sub, "test token"),
                            new Claim(JwtRegisteredClaimNames.Name, "test"),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                        };

                    var token = new JwtSecurityToken(
                    issuer: "http://localhost:5231",
                    audience: "test-aud",
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(120),
                    signingCredentials: credentials);

                    string tokenString = new JwtSecurityTokenHandler().WriteToken(token);
                    //return TypedResults.Ok(new { Token = tokenString });
                    return Results.Ok(new {Token = tokenString});
                   
                }

            return TypedResults.Ok();
            
            });
        }
    }
