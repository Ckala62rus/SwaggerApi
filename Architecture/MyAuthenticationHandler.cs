using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Architecture
{
    public class MyAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public MyAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder, 
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
            //
        }
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            return await AuthenticateAsync();
        }
    }
}