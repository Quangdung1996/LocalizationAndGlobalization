using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Domain.Resources
{
    public class CustomerDataTransferObjectShared
    {
    }

    public interface ICustomerDataTransferObjectShared : IStringLocalizer
    {
    }

    public class CustomerDataTransferObjectLocalizer : ICustomerDataTransferObjectShared
    {
        private readonly IStringLocalizer _localizer;

        public CustomerDataTransferObjectLocalizer(IStringLocalizer<CustomerDataTransferObjectShared> localizer)
        {
            _localizer = localizer;
        }

        public LocalizedString this[string name, params object[] arguments] => _localizer[name, arguments];
        public LocalizedString this[string name] => _localizer[name];

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            return _localizer.GetAllStrings(includeParentCultures);
        }

        [Obsolete("This method is obsolete. Use `CurrentCulture` and `CurrentUICulture` instead.")]
        public IStringLocalizer WithCulture(CultureInfo culture)
        {
            return _localizer.WithCulture(culture);
        }
    }
}