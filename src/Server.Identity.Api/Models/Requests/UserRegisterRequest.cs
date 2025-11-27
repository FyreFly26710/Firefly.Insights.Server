using FluentValidation;

namespace Server.Identity.Api.Models.Requests;
public class UserRegisterRequest
{
    public string UserAccount { get; set; } = string.Empty;

    public string UserPassword { get; set; } = string.Empty;

    public string ConfirmPassword { get; set; } = string.Empty;
}

public class UserRegisterRequestValidator : AbstractValidator<UserRegisterRequest>
{
    public UserRegisterRequestValidator()
    {
        RuleFor(x => x.UserAccount)
            .NotEmpty().WithMessage("User account is required")
            .MinimumLength(4)
            .MaximumLength(20);

        RuleFor(x => x.UserPassword)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(4)
            .MaximumLength(20);

        RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.UserPassword).WithMessage("Passwords do not match");
    }
}