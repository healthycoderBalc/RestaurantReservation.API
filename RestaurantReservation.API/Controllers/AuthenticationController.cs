using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RestaurantReservation.API.Contracts.Responses;
using RestaurantReservation.API.Models.Authentication;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace RestaurantReservation.API.Controllers
{
  
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {

        private readonly IEmployeeRepository _employeeRepository;
        private readonly JwtTokenGenerator _jwtTokenGenerator;

        public AuthenticationController(IEmployeeRepository employeeRepository, JwtTokenGenerator tokenGenerator)
        {
            _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
            _jwtTokenGenerator = tokenGenerator ?? throw new ArgumentNullException(nameof(tokenGenerator));

        }

        /// <summary>
        /// Authenticate user
        /// </summary>
        /// <param name="authenticationRequestBody">UserName, LastName and password (id)</param>
        /// <response code="200">Returns the token</response>
        /// <returns>A valid token</returns>
        [HttpPost("authenticate")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(NotOkResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(AuthorizedResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult<string>> Authenticate(AuthenticationRequestBody authenticationRequestBody)
        {
            var user = await ValidateUserCredentials(
                authenticationRequestBody.UserName, authenticationRequestBody.LastName, authenticationRequestBody.Password);
            if (user == null)
            {
                return Unauthorized();
            }
            var token = _jwtTokenGenerator.GenerateToken(user);

            return Ok(new { Token = token });
        }

        private async Task<Employee?> ValidateUserCredentials(string? firstName, string? lastName, int password)
        {
            var user = await _employeeRepository.ValidateUserCredentials(firstName, lastName, password);

            return user;

        }
    }
}
