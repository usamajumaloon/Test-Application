using System;
using System.Collections.Generic;
using System.Text;

namespace TestApplication.Services.Constants
{
    public struct ExceptionType
    {
        public const string ArgumentException = "ArgumentException";
        public const string UnauthorizedAccessException = "UnauthorizedAccessException";
        public const string AuthenticationException = "AuthenticationException";
        public const string NullReferenceException = "NullReferenceException";

        public const string SqlException = "SqlException";
    }
}