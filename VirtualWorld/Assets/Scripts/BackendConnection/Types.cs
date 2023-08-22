using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Authentication
{
    public enum RequestType
    {
        GET,
        POST,
        PUT
    }

    public struct LoginUserData
    {
        public string username;
        public string password;
    }

    public struct LoggedUserData
    {
        public string username;
        public string token;
    }
}
