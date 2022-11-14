using System;

namespace Sherlog
{
    public class SherlogAssertException : Exception
    {
        public SherlogAssertException(string message) : base(message) { }
    }
}
