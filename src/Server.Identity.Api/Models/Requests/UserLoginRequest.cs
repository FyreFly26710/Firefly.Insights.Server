using FluentValidation;

namespace Server.Identity.Api.Models.Requests;
public class UserLoginRequest
{
    public String UserAccount { get; set; } = string.Empty;
    public String UserPassword { get; set; } = string.Empty;
}
public class UserLoginRequestValidator : AbstractValidator<UserLoginRequest>
{
    public UserLoginRequestValidator()
    {
        RuleFor(x => x.UserAccount)
            .NotEmpty().WithMessage("User account is required")
            .MinimumLength(4)
            .MaximumLength(20);

        RuleFor(x => x.UserPassword)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(4)
            .MaximumLength(20);
    }
}