using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace WorkAppReactAPI.Configuration
{
    // A handler that can determine whether a MaximumOfficeNumberRequirement is satisfied
    // internal class MaximumOfficeNumberAuthorizationHandler : AuthorizationHandler<MaximumOfficeNumberRequirement>
    // {
    //     protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MaximumOfficeNumberRequirement requirement)
    //     {
    //         // Bail out if the office number claim isn't present
    //         if (!context.User.HasClaim(c => c.Issuer == "http://localhost:5000/" && c.Type == "office" ))
    //         {
    //             return Task.CompletedTask;
    //         }
    //         else
    //         {
    //             context.Succeed(requirement);
    //             return Task.CompletedTask;
    //         }

    //         // // Bail out if we can't read an int from the 'office' claim
    //         // int officeNumber;
    //         // if (!int.TryParse(context.User.FindFirst(c => c.Issuer == "http://localhost:5000/" && c.Type == "office").Value, out officeNumber))
    //         // {
    //         //     return Task.CompletedTask;
    //         // }

    //         // // Finally, validate that the office number from the claim is not greater
    //         // // than the requirement's maximum
    //         // if (officeNumber <= requirement.MaximumOfficeNumber)
    //         // {
    //         //     // Mark the requirement as satisfied
    //         //     context.Succeed(requirement);
    //         // }

    //         // return Task.CompletedTask;
    //     }
    // }

    // // A custom authorization requirement which requires office number to be below a certain value
    // internal class MaximumOfficeNumberRequirement : IAuthorizationRequirement
    // {
    //     public MaximumOfficeNumberRequirement(int officeNumber)
    //     {
    //         MaximumOfficeNumber = officeNumber;
    //     }

    //     public int MaximumOfficeNumber { get; private set; }
    // }



    
    // public class MyAuthorizationAuthorize : AuthorizeAttribute
    // {
    //     private readonly string allowfunction;
    //     private readonly string fucntion;

    //     public MyAuthorizationAuthorize(string allowfunction, string fucntion)
    //     {
    //         this.allowfunction = fucntion;
    //         this.fucntion = fucntion;

    //     } 
         
    // }
    // public class MyAuthorizationRequirement : IAuthorizationRequirement
    // {
    //     public MyAuthorizationRequirement(int noOfDays)
    //     {
    //         TimeSpendInDays = noOfDays;
    //     }

    //     public int TimeSpendInDays { get; private set; }
    // }
    // public class MyAuthorizationHandler : AuthorizationHandler<MyAuthorizationRequirement>
    // {
    //     protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MyAuthorizationRequirement requirement)
    //     {
    //         if (!context.User.HasClaim(c => c.Type == "DateOfJoining"))
    //         {
    //             return Task.FromResult(0);
    //         }

    //         var dateOfJoining = Convert.ToDateTime(context.User.FindFirst(
    //             c => c.Type == "DateOfJoining").Value);

    //         double calculatedTimeSpend = (DateTime.Now.Date - dateOfJoining.Date).TotalDays;

    //         if (calculatedTimeSpend >= requirement.TimeSpendInDays)
    //         {
    //             context.Succeed(requirement);
    //         }
    //         return Task.FromResult(0);
    //     }
    // }
    // public class MyAuthorizationPolicy : IAuthorizationPolicyProvider
    // {
    //     public DefaultAuthorizationPolicyProvider defaultPolicyProvider { get; }
    //     public MyAuthorizationPolicy(IOptions<AuthorizationOptions> options)
    //     {
    //         defaultPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
    //     }
    //     public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
    //     {
    //         return defaultPolicyProvider.GetDefaultPolicyAsync();
    //     }

    //     public Task<AuthorizationPolicy> GetFallbackPolicyAsync()
    //     {
    //         return defaultPolicyProvider.GetFallbackPolicyAsync();
    //     }
        
    //     public Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
    //     {
    //         string[] subStringPolicy = policyName.Split(new char[] { '.' });
    //         if (subStringPolicy.Length > 1 && subStringPolicy[0].Equals("MyAuthorization", StringComparison.OrdinalIgnoreCase) && int.TryParse(subStringPolicy[1], out var days))
    //         {
    //             var policy = new AuthorizationPolicyBuilder();
    //             policy.AddRequirements(new MyAuthorizationRequirement(days));
    //             return Task.FromResult(policy.Build());
    //         }
    //         return defaultPolicyProvider.GetPolicyAsync(policyName);
    //     }
    // }
}