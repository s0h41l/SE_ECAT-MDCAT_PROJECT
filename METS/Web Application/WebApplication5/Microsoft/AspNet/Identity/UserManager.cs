using System;
using System.Threading.Tasks;

namespace Microsoft.AspNet.Identity
{
    internal class UserManager
    {
        internal static Task FindByNameAsync(string email)
        {
            throw new NotImplementedException();
        }

        internal static Task ResetPasswordAsync(object id, string code, string password)
        {
            throw new NotImplementedException();
        }
    }
}