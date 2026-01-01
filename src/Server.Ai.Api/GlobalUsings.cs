global using System;
global using MediatR;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Metadata.Builders;
global using FluentValidation;
global using MassTransit;

global using Server.Common.Types;
global using Server.Common.Messaging;
global using Server.Ai.Api.Models.Requests;
global using Server.Ai.Api.Models.Entities;
global using Server.Ai.Api.Models.Responses;
global using Server.Ai.Api.Application.Commands;
global using Server.Ai.Api.Application.Services;
global using Server.Ai.Api.Infrastructure.Contexts;
global using Server.Messages.Identities;
global using Server.Messages.Ais;