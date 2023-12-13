global using CryptoCoinsParser.Domain.DbEntities;
global using CryptoCoinsParser.Persistence.Schemas;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Metadata.Builders;
global using JetBrains.Annotations;
global using System.Reflection;
global using CryptoCoinsParser.Persistence.Context;
global using Microsoft.EntityFrameworkCore.Design;
global using Microsoft.Extensions.Configuration;
global using Shared;
global using EntityFramework.Exceptions.PostgreSQL;
global using CryptoCoinsParser.Persistence.Repositories.Interfaces;
global using AutoMapper;
global using CryptoCoinsParser.Persistence.Cache;
global using CryptoCoinsParser.Persistence.Repositories.Implementation;
global using CryptoCoinsParser.Persistence.Services;
global using Microsoft.EntityFrameworkCore.Diagnostics;
global using Microsoft.Extensions.DependencyInjection;
global using Shared.Extensions;
global using Z.EntityFramework.Extensions;