using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace webapiserver.Controllers
{
    public class CurrentUserServices: ICurrentUserService {
        private readonly IHttpContextAccessor httpContextAccessor;

        public CurrentUserServices(IHttpContextAccessor httpContextAccessor){
            this.httpContextAccessor =  httpContextAccessor;
        }
        public  string? UserId => httpContextAccessor?.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        public  string? IpAddress => httpContextAccessor?.HttpContext?.Connection?.LocalIpAddress?.ToString();

    }
}