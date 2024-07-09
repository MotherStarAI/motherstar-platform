using FluentValidation;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RCommon;
using MotherStar.Platform.Application.Contracts.SEO.Lighthouse.Commands;

namespace MotherStar.Platform.Application.SEO.Lighthouse.Validations
{


    public class CreateProfileCommandValidator : AbstractValidator<CreateLighthouseProfileCommand>
    {
        public CreateProfileCommandValidator(ILogger<CreateProfileCommandValidator> logger)
        {
            RuleFor(command => command.CreatedByEmail).NotEmpty().WithMessage("No CreatedByEmail found");
            RuleFor(command => command.WebsiteUrl).NotEmpty().Must(x => x.IsWebUrl()).WithMessage("No WebsiteUrl found");

            logger.LogTrace("----- INSTANCE CREATED - {ClassName}", GetType().Name);
        }
    }
}
