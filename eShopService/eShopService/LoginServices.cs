using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace eShopService
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public partial class eShopServices : eShopService.ILoginService
    {
        public void Login(string username, string password)

        {
            eShopDBDataContext dc = new eShopDBDataContext();
            if (CheckIfUserOnline(username))
            {
                OperationContext.Current.GetCallbackChannel<ILoginServiceCallBack>().ResponseToLogin(-2);
            }
            else
            {
                var query = from a in dc.Users where a.Username == username & a.Password == password select a.Username;
                if (query.Count() == 0)
                {
                    OperationContext.Current.GetCallbackChannel<ILoginServiceCallBack>().ResponseToLogin(-1);
                }
                else
                {
                    OnlineUser newOnlineUser = new OnlineUser()
                    {
                        Username = username
                    };
                    // Save query to log
                    using (StreamWriter sw = File.AppendText(path))
                    {
                        sw.WriteLine(DateTime.Now + " - " + "Adding user " + username + " as online user");
                    }
                    dc.OnlineUsers.InsertOnSubmit(newOnlineUser);
                    dc.SubmitChanges();
                    OperationContext.Current.GetCallbackChannel<ILoginServiceCallBack>().ResponseToLogin(1);
                }
            }
        }

        private bool CheckIfUserOnline(string username)
        {
            eShopDBDataContext dc = new eShopDBDataContext();
            if (dc.OnlineUsers.Any(u => u.Username == username))
                return true;
            return false;
        }


    }
}
