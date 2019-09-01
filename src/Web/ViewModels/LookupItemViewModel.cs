using FluentValidation;
using i2fam.DAL.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace i2fam.Web.ViewModels
{
    public class LookupItemViewModel : BaseViewModel
    {
        public int Id { get; set; }
        public LocaleCategory CategoryId { get; set; }
        public Language Lang { get; set; }
        public int LocaleId { get; set; }
        public string LocaleString { get; set; }
    }

    public class LookupItemViewModelValidator : AbstractValidator<LookupItemViewModel>
    {
        public LookupItemViewModelValidator()
        {
            RuleFor(register => register.CategoryId).NotEmpty().WithMessage("Category Id cannot be empty");
            RuleFor(register => register.Lang).NotEmpty().WithMessage("Language cannot be empty");
            RuleFor(register => register.LocaleString).NotEmpty().WithMessage("LocaleString Cannot be empty");
        }
    }
}
