﻿using System.Reflection;
using invensys.iserve.Application.Common.Exceptions;
using invensys.iserve.Application.Common.Interfaces;
using invensys.iserve.Application.Common.Security;

namespace invensys.iserve.Application.Common.Behaviours;
public class AuthorizationBehaviour<TRequest, TResponse>(
    IUser user,
    IIdentityRoleClaimService identityRoleClaimService,
    IIdentityAuthorizationService identityService) : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
   public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
   {
      var authorizeAttributes = request.GetType().GetCustomAttributes<AuthorizeAttribute>();

      if (authorizeAttributes.Any())
      {
         // Must be authenticated user
         if (user.Id == null)
         {
            throw new UnauthorizedAccessException();
         }

         // Role-based authorization
         var authorizeAttributesWithRoles = authorizeAttributes.Where(a => !string.IsNullOrWhiteSpace(a.Roles));

         if (authorizeAttributesWithRoles.Any())
         {
            var authorized = false;

            foreach (var roles in authorizeAttributesWithRoles.Select(a => a.Roles.Split(',')))
            {
               foreach (var role in roles)
               {
                  var isInRole = await identityRoleClaimService.HasRoleClaimAsync(user.Id, role.Trim());
                  if (isInRole)
                  {
                     authorized = true;
                     break;
                  }
               }
            }

            // Must be a member of at least one role in roles
            if (!authorized)
            {
               throw new ForbiddenAccessException();
            }
         }

         // Policy-based authorization
         var authorizeAttributesWithPolicies = authorizeAttributes.Where(a => !string.IsNullOrWhiteSpace(a.Policy));
         if (authorizeAttributesWithPolicies.Any())
         {
            foreach (var policy in authorizeAttributesWithPolicies.Select(a => a.Policy))
            {
               var authorized = await identityService.AuthorizeAsync(user.Id, policy);

               if (!authorized)
               {
                  throw new ForbiddenAccessException();
               }
            }
         }
      }

      // User is authorized / authorization not required
      return await next();
   }
}
