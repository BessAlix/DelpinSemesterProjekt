﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using DelpinBooking.Migrations;
using DelpinBooking.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace DelpinBooking.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterCompanyModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterCompanyModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterCompanyModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterCompanyModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "Din {0} skal være minimum {2} og max {1} karaktere.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Adgangskode")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Bekræft adgangskode")]
            [Compare("Password", ErrorMessage = "Adgangskoderne stemmer ikke overens")]
            public string ConfirmPassword { get; set; }

            [Required]
            [Display(Name = "Firmanavn")]
            public string CompanyName { get; set; }

            [Required]
            [Display(Name = "Ansvarlig leder")]
            public string LeaderName { get; set; }

            [Required]
            [Display(Name = "CVR")]
            public string CVR { get; set; }

            [Required]
            [Display(Name = "Selskabsform")]
            public string CompanyForm { get; set; }

            [Required]
            [Display(Name = "Telefonnummer")]
            public string PhoneNumber { get; set; }

            [Required]
            [Display(Name = "Adresse")]
            public string Address { get; set; }

            [Required]
            [Display(Name = "Postnummer")]
            public int PostCode { get; set; }

            [Required]
            [Display(Name = "By")]
            public string City { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = Input.Email,
                    Email = Input.Email,
                    CompanyName = Input.CompanyName,
                    LeaderName = Input.LeaderName,
                    CVR = Input.CVR,
                    CompanyForm = Input.CompanyForm,
                    Address = Input.Address,
                    PhoneNumber = Input.PhoneNumber,
                    PostCode = Input.PostCode,
                    City = Input.City
                };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}