using System.Security;

namespace WpfAppСourseWork.Model
{
    public interface IAuthenticationService
    {
        bool AuthenticateUser(SecureString password);
    }

}
