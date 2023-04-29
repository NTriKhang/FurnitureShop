using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using ShopOnline.Api.Data.Repository.Contracts;
using ShopOnline.API.Entities;
using ShopOnline.API.Extensions;
using ShopOnline.Models.dtos;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;

namespace ShopOnline.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserManagementController : ControllerBase
	{
		private readonly IUserRepository _userRepository;
		private readonly IWebHostEnvironment _webHostEnvironment;
		public UserManagementController(IUserRepository userRepository, IWebHostEnvironment webHostEnvironment)
		{
			_webHostEnvironment = webHostEnvironment;
			_userRepository = userRepository;
		}
		[HttpPost("UploadImage")]
		[Authorize]
		public async Task<ActionResult<List<UploadResultDto>>> UploadImage([FromForm] List<IFormFile> files)
		{
			try
			{
				List<UploadResultDto> uploadResults = new List<UploadResultDto>();

				foreach (var file in files)
				{
					UploadResultDto uploadResultDto = new UploadResultDto();
					string trustedFileNameForFileStorage;
					var untrustedFileName = file.FileName;
					uploadResultDto.FileName = untrustedFileName;
					var trustedFileNameForDisplay =
						WebUtility.HtmlEncode(untrustedFileName);
					trustedFileNameForFileStorage = Path.GetRandomFileName();
					var path = Path.Combine(_webHostEnvironment.WebRootPath, "Image",
						"unsafe_uploads",
						trustedFileNameForFileStorage);
					using (FileStream fs = new(path, FileMode.Create))
					{
						await file.CopyToAsync(fs);
					}

					var userName = User.Identity.Name;
					if (userName == null)
						throw new Exception("User not found");
					var user = await _userRepository.GetUserByName(x => x.UserName == userName);
					await _userRepository.UploadImage("/Image/unsage_uploads/" + trustedFileNameForFileStorage, user);
					uploadResultDto.StoredFileName = trustedFileNameForFileStorage;
					uploadResults.Add(uploadResultDto);
				}
				return Ok(uploadResults);

				//if (ImageUrl == string.Empty)
				//	throw new Exception("Image url is requested");
				//var userName = User.Identity.Name;
				//if (userName == null)
				//	throw new Exception("User identity is not occur");
				//var user = await _userRepository.GetUserByName(x => x.UserName == userName);
				//if (user == null)
				//	throw new Exception("User not found");
				//var result = await _userRepository.UploadImage(ImageUrl, user);
				//return Ok(result);
			}
			catch (Exception ex)
			{
				return BadRequest("It go into but not occur");
			}
		}
		[HttpGet("{name}", Name = "getUser")]
		public async Task<ActionResult<UserDto>> GetUser(string name)
		{
			try
			{
				if (name == string.Empty)
					throw new Exception("name is required");
				var user = await _userRepository.GetUserByName(x => x.UserName == name, includeProperty: "userRole");
				if (user == null)
					return null;
				return user.ConvertToDto();
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}
		[HttpPost("register")]
		public async Task<ActionResult<UserDto>> Register([FromBody] UserRegisterDto userRegisterDto)
		{
			try
			{
				if (ModelState.IsValid)
				{
					if (userRegisterDto != null)
					{
						ScriptPassword(userRegisterDto.Password, out byte[] passwordHash, out byte[] passwordSalt);
						var userToAdd = new User()
						{
							UserName = userRegisterDto.UserName,
							PasswordHash = passwordHash,
							PasswordSalt = passwordSalt,
							Email = userRegisterDto.Email,
							userRoleId = 2
						};
						var userAdded = await _userRepository.AddUser(userToAdd);
						if (userAdded == null)
							return default(UserDto);
						var user = userAdded.ConvertToDto();
						return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
					}
				}
				var message = new Exception("Something is not valid");
				throw message;
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}
		private void ScriptPassword(string passwordRequest, out byte[] passwordHash, out byte[] passwordSalt)
		{
			using (var hmac = new HMACSHA512())
			{
				passwordSalt = hmac.Key;
				passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(passwordRequest));
			}
		}

		[HttpPost("login")]
		public async Task<ActionResult<string>> Login([FromBody] UserLoginDto userLoginDto)
		{
			try
			{
				var user = await _userRepository.GetUserByName(x => x.UserName == userLoginDto.UserName, includeProperty: "userRole");
				if (user != null)
				{
					if (!ValidatePassword(userLoginDto.Password, user.PasswordHash, user.PasswordSalt))
					{
						return BadRequest("wrong password");
					}
					string jwtToken = CreateToken(user);
					await _userRepository.RefreshToken(jwtToken, user);
					return Ok(jwtToken);
				}
				return BadRequest("wrong user name");
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status404NotFound, ex.Message);
			}
		}
		private string CreateToken(User user)
		{
			List<Claim> claims = new List<Claim>()
			{
				new Claim(ClaimTypes.Name, user.UserName),
				new Claim(ClaimTypes.Role, user.userRole.Name),
			};
			var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("My top secret key"));
			var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

			var token = new JwtSecurityToken(
				claims: claims,
				expires: DateAndTime.Now.AddDays(1),
				signingCredentials: cred
				);
			var jwt = new JwtSecurityTokenHandler().WriteToken(token);
			return jwt;
		}
		private bool ValidatePassword(string password, byte[] passwordHash, byte[] passwordSalt)
		{
			using (var hmac = new HMACSHA512(passwordSalt))
			{
				var passwordHashCheck = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
				return passwordHashCheck.SequenceEqual(passwordHash);
			}
		}
	}
}
