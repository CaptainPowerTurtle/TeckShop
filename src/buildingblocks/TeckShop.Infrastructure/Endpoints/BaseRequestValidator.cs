using FastEndpoints;
using FluentValidation;

namespace TeckShop.Infrastructure.Endpoints
{
    /// <summary>
    /// The base request validator.
    /// </summary>
    public class BaseRequestValidator : Validator<BaseRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseRequestValidator"/> class.
        /// </summary>
        public BaseRequestValidator()
        {
            RuleFor(baseRequest => baseRequest.Tenant)
                .NotEmpty().WithMessage("tenant is required!");
        }
    }
}
