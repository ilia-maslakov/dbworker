using FluentValidation;
using dbworker.Data.EF;
namespace dbworker.Validators
{
    public class OrgValidator : AbstractValidator<Org>
    {
        public OrgValidator()
        {
            RuleFor(x => x.Name).NotNull();
        }
    }
}
