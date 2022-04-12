using MyRentalWebService.Data.Dtos;
using MyRentalWebService.Data.Interfaces;
using MyRentalWebService.Data.Providers;
using MyRentalWebService.Infrastructure.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyRentalWebService.Data.Repository
{
    public class OptionRepository : IOptionRepository
    {
        private readonly AppDbContext _db;

        public OptionRepository(AppDbContext context)
        {
            this._db = context;
        }
        enum options
        {
            theme, language, languages, currency
        }
        public RepositoryResult<OptionDto> GetOptions()
        {
            var theme = _db.Options.FirstOrDefault(x => x.Key == GetOptionName(options.theme));
            var language = _db.Options.FirstOrDefault(x => x.Key == GetOptionName(options.language));
            var languages = _db.Options.FirstOrDefault(x => x.Key == GetOptionName(options.languages));
            var currency = _db.Options.FirstOrDefault(x => x.Key == GetOptionName(options.currency));

            return new RepositoryResult<OptionDto>(200, new OptionDto
            {
                Currency = currency != null ? currency.Value : "usd",
                Language = language != null ? language.Value : "en",
                Theme = theme != null ? theme.Value : "dark",
                Languages = languages != null ? languages.Value : "en,fa"
            }, null);
        }

        public RepositoryResult UpdateOptions(OptionDto option)
        {
            try
            {
                var theme = _db.Options.FirstOrDefault(x => x.Key == GetOptionName(options.theme));
                var language = _db.Options.FirstOrDefault(x => x.Key == GetOptionName(options.language));
                var currency = _db.Options.FirstOrDefault(x => x.Key == GetOptionName(options.currency));

                if (theme != null)
                    theme.Value = option.Theme;
                if (language != null)
                    language.Value = option.Language;
                if (currency != null)
                    currency.Value = option.Currency;
                _db.SaveChanges();

                return new RepositoryResult(204, null); ;
            }
            catch (Exception e)
            {
                return new RepositoryResult(500, e.ToMessageResult());
            }
        }

        private string GetOptionName(options Opt) => Opt switch
        {
            options.theme => "use-theme",
            options.language => "use-language",
            options.languages => "languages",
            options.currency => "use-currency",
            _ => string.Empty,
        };
    }
}
