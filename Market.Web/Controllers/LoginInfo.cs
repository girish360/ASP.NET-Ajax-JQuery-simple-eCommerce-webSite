using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace Market.Web.Controllers
{
    public sealed class LoginInfo
    {
        
            private HttpSessionState _session;
            public LoginInfo(HttpSessionState session)
            {
                this._session = session;
            }

            public string Username
            {
                get { return (this._session["Username"] ?? string.Empty).ToString(); }
                set { this._session["Username"] = value; }
            }

            public string FullName
            {
                get { return (this._session["FullName"] ?? string.Empty).ToString(); }
                set { this._session["FullName"] = value; }
            }

            public int ID
            {
                get { return Convert.ToInt32((this._session["UID"] ?? -1)); }
                set { this._session["UID"] = value; }
            }

            

        }
    }
