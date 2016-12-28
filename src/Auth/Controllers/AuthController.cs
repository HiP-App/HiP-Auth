using Auth.Models;
using Auth.Models.AuthViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Auth.Utility;

namespace Auth.Controllers
{
    [Authorize]
    public class AuthController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private EmailSender emailSender;
        private readonly ILogger _logger;
        private PasswordGenerator passwordGenerator;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            EmailSender emailSender,
            ILoggerFactory loggerFactory)
        {
            _userManager = userManager;
            this.emailSender = emailSender;
            _logger = loggerFactory.CreateLogger<AuthController>();
            passwordGenerator = new PasswordGenerator();
        }

        // POST: /Auth/Register
        [HttpPost]
        [AllowAnonymous]
        [Route("auth/register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=532713
                    // Send an email with this link
                    //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    //var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
                    //await _emailSender.SendEmailAsync(model.Email, "Confirm your account",
                    //    $"Please confirm your account by clicking this link: <a href='{callbackUrl}'>link</a>");

                    _logger.LogInformation(3, "User created a new account with password.");

                    return Ok();
                }
                AddErrors(result);
            }

            return BadRequest(ModelState);
        }

        // GET: /Auth/ConfirmEmail
        [HttpGet]
        [AllowAnonymous]
        [Route("auth/ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
                return BadRequest();
            
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound();
            
            var result = await _userManager.ConfirmEmailAsync(user, code);
            return Ok(result.Succeeded);
        }

        // POST: /Auth/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [Route("auth/ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Email);

                if (user != null)
                {
                    var password = passwordGenerator.Generate();
                    if(password != null)
                    {
                        var removePassword = await _userManager.RemovePasswordAsync(user);
                        var result = await _userManager.AddPasswordAsync(user, password);
                        if (result.Succeeded)
                        {
                            try
                            {
                                await emailSender.InviteAsync(model.Email, password);
                            }

                            //Error while sending email                        
                            catch (MailKit.Net.Smtp.SmtpCommandException SmtpError)
                            {
                                _logger.LogDebug(SmtpError.ToString());
                                // 503 - Service Unavailable
                                return new StatusCodeResult(503);
                            }

                            // 202 - Accepted (Emails get send async, so we cannot guarantee that emails are send)
                            return new StatusCodeResult(202);
                        }
                        AddErrors(result);
                    }                                        
                }
            }
            
            return BadRequest(ModelState);
        }

        // GET: /Auth/ResetPassword
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code = null)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Auth/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return NotFound();
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return Ok();
            }
            AddErrors(result);

            return BadRequest(ModelState);
        }

        //
        // PUT: /Auth/ChangePassword
        [HttpPut]
        [Route("auth/changePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await GetCurrentUserAsync();
                if (user != null)
                {
                    var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                
                    if (result.Succeeded)
                    {
                        _logger.LogInformation(3, "User changed their password successfully.");
                        return Ok();
                    }

                    AddErrors(result);
                }
            }

            return BadRequest(ModelState);
        }


        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }

        #endregion

    }
}
