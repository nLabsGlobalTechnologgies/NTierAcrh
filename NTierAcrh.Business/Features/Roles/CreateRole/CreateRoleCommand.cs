﻿using ErrorOr;
using MediatR;

namespace NTierAcrh.Business.Features.Roles.CreateRole;
public sealed record CreateRoleCommand(string Name) : IRequest<ErrorOr<Unit>>;
