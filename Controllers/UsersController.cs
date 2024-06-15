﻿using System.Security.Claims;
using BlogApp.Data.Abstract;
using BlogApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Controllers;

public class UsersController : Controller
{
    private readonly IUserRepository _userRepository;
    public UsersController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public IActionResult Login()
    {
        if(User.Identity!.IsAuthenticated)
        {
            return RedirectToAction("Index", "Posts");
        }
        return View();
    }
    public IActionResult Register()
    {
        
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if(ModelState.IsValid)
        {
            var user = await _userRepository.Users.FirstOrDefaultAsync(u => u.Email == model.Email || u.UserName == model.UserName);
            if(user == null)
            {
                _userRepository.CreateUser(new Entity.User {
                    UserName = model.UserName,
                    Name = model.Name,
                    Email = model.Email,
                    Password = model.Password,
                    Image = "avatar.png"
                });
                return RedirectToAction("Login");
            }else
            {
                ModelState.AddModelError("", "Username ve ya Email kullanımda");
            }
            
        }
        return View(model);
    }
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login");
    }
    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var isUser = await _userRepository.Users.FirstOrDefaultAsync(x => x.Email == model.Email && x.Password == model.Password);
            if (isUser != null)
            {
                var userClaims = new List<Claim>();

                userClaims.Add(new Claim(ClaimTypes.NameIdentifier, isUser.UserId.ToString()));
                userClaims.Add(new Claim(ClaimTypes.Name, isUser.UserName ?? ""));
                userClaims.Add(new Claim(ClaimTypes.GivenName, isUser.Name ?? ""));
                userClaims.Add(new Claim(ClaimTypes.UserData, isUser.Image ?? ""));

                if(isUser.Email == "info@ahmetaggoz.com")
                {
                    userClaims.Add(new Claim(ClaimTypes.Role, "admin"));
                }

                var claimsIdentity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties {
                    IsPersistent = true
                };

                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties
                );
                return RedirectToAction("Index", "Posts");
            }
            else
            {
                ModelState.AddModelError("", "Kullanıcı adı veya şifre yanlış");
            }
        }
        return View(model);
    }

    public IActionResult Profile(string username)
    {
        if(string.IsNullOrEmpty(username))
        {
            return NotFound();
        }
        var user = _userRepository
                    .Users
                    .Include(x => x.Posts)
                    .Include(x => x.Comments)
                    .ThenInclude(x => x.Post)
                    .FirstOrDefault(x => x.UserName == username);
        if(user == null)
        {
            return NotFound();
        }
        return View(user);
    }
}
