using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.FluentValidation;

namespace Medcenter.Service.Local
{
    public class CustomRegistrationValidator : AbstractValidator<Register>
    {
        public IAuthRepository UserAuthRepo { get; set; }

        public CustomRegistrationValidator()
        {
            RuleSet(ApplyTo.Post, () =>
            {
                //RuleFor(x => x.UserName).NotEmpty().When(x => x.Email.IsNullOrEmpty());
                RuleFor(x => x.UserName).NotEmpty();
                RuleFor(x => x.UserName)
                    .Must(x => UserAuthRepo.GetUserAuthByUserName(x) == null)
                    .WithErrorCode("AlreadyExists")
                    .WithMessage("UserName already exists")
                    .When(x => !x.UserName.IsNullOrEmpty());
                //RuleFor(x => x.Email)
                //    .Must(x => x.IsNullOrEmpty() || UserAuthRepo.GetUserAuthByUserName(x) == null)
                //    .WithErrorCode("AlreadyExists")
                //    .WithMessage("Email already exists")
                //    .When(x => !x.Email.IsNullOrEmpty());

                RuleFor(x => x.FirstName).NotEmpty();
                RuleFor(x => x.LastName).NotEmpty();
                //RuleFor(x => x.Email).NotEmpty();
                RuleFor(x => x.Password).NotEmpty();
                // add your own rules here...
            });

            RuleSet(
                ApplyTo.Put,
                () =>
                {
                    RuleFor(x => x.UserName).NotEmpty();
                    //RuleFor(x => x.Email).NotEmpty();
                    // add your own rules here...
                });
        }

        public class CustomRegistrationFeature : IPlugin
        {
            public string AtRestPath { get; set; }

            public CustomRegistrationFeature()
            {
                this.AtRestPath = "/register";
            }

            public void Register(IAppHost appHost)
            {
                appHost.RegisterService<RegisterService>(AtRestPath);
                appHost.RegisterAs<CustomRegistrationValidator, IValidator<Register>>();
            }
        }
    }
}