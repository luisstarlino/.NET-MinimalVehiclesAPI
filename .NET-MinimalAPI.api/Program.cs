using _NET_MinimalAPI;
using _NET_MinimalAPI.Domain.DTOs;
using _NET_MinimalAPI.Domain.Entities;
using _NET_MinimalAPI.Domain.Enuns;
using _NET_MinimalAPI.Domain.Interfaces;
using _NET_MinimalAPI.Domain.ModelViews;
using _NET_MinimalAPI.Domain.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
Setup.ConfigureServices(builder);
var app = builder.Build();
Setup.ConfigureApp(app);



