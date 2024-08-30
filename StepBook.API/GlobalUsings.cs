global using AutoMapper;
global using Profile = AutoMapper.Profile;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Mvc;
global using StepBook.API.DTOs;
global using AutoMapper.QueryableExtensions;
global using Microsoft.EntityFrameworkCore;
global using StepBook.API.Data;
global using StepBook.API.Extensions;
global using StepBook.API.Models;
global using System.Text;
global using FluentValidation;
global using FluentValidation.AspNetCore;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.IdentityModel.Tokens;
global using Microsoft.OpenApi.Models;
global using StepBook.API.Auth;
global using StepBook.API.DTOs.Validation;
global using StepBook.API.Mappings;
global using StepBook.API.Providers;
global using StepBook.API.Helpers;
global using System.Security.Claims;
global using CloudinaryDotNet.Actions;
global using CloudinaryDotNet;
global using System.Security.Cryptography;
global using System.ComponentModel.DataAnnotations.Schema;
global using Microsoft.AspNetCore.Mvc.Filters;
global using StepBook.API.Filters;
global using System.IdentityModel.Tokens.Jwt;
global using System.Text.Json.Serialization;
global using Serilog;
global using StepBook.API.Middleware;
global using StepBook.API;
global using Amazon;
global using Amazon.Runtime;
global using Amazon.S3;
global using StepBook.API.Contracts.Classes;
global using StepBook.API.Contracts.Interfaces;
global using StepBook.API.Data.Configs;
global using StepBook.API.Hubs;
global using StepBook.API.Repositories.Classes;
global using StepBook.API.Repositories.Interfaces;
global using StepBook.API.Services;
global using Amazon.S3.Model;
global using Microsoft.AspNetCore.Authentication;
global using Microsoft.AspNetCore.Authentication.Cookies;
global using Microsoft.AspNetCore.Authentication.Google;