using System;

namespace Unickq.Sitecore.BeAutomation.Utils
{
    public class NotASiteCoreException : ScException
    {
        public NotASiteCoreException(string message)
            : base(message)
        {
        }
    }

    public class ScException : Exception
    {
        public ScException(string message)
            : base(message)
        {
        }

        public ScException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }

    public class ScContentEditorException : ScException
    {
        public ScContentEditorException(string message)
            : base(message)
        {
        }

        public ScContentEditorException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
