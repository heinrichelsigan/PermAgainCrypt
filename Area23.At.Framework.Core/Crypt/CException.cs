using Area23.At.Framework.Core.Crypt;
using Area23.At.Framework.Core.Util;

namespace Area23.At.Framework.Core.Crypt
{
    public class CException : ApplicationException
    {
        public static CException LastException
        {
            get => (CException)AppDomain.CurrentDomain.GetData(Constants.LAST_EXCEPTION);
            protected set => AppDomain.CurrentDomain.SetData(Constants.LAST_EXCEPTION, value);
        }

        public CException Previous { get; protected set; }

        public DateTime TimeStampException { get; set; }



        public CException(string message) : base(message)
        {
            TimeStampException = DateTime.UtcNow;
            CException lastButNotLeast = LastException;
            Previous = lastButNotLeast != null ? lastButNotLeast : null;
            AppDomain.CurrentDomain.SetData(Constants.LAST_EXCEPTION, this);
        }

        public CException(string message, Exception innerException) : base(message, innerException)
        {
            TimeStampException = DateTime.UtcNow;
            CException lastButNotLeast = LastException;
            Previous = lastButNotLeast != null ? lastButNotLeast : null;
            AppDomain.CurrentDomain.SetData(Constants.LAST_EXCEPTION, this);
        }

        public static void SetLastException(Exception exc)
        {
            CException cqrLastEx = exc != null && exc is CException ? (CException)exc :
                exc != null && exc.InnerException != null ? new CException(exc.Message, exc.InnerException) :
                    exc != null && exc.Message != null ? new CException(exc.Message) : null;

            cqrLastEx.Source = exc.Source;
            cqrLastEx.HelpLink = exc.HelpLink;
            cqrLastEx.HResult = exc.HResult;
            cqrLastEx.Previous = LastException;

            AppDomain.CurrentDomain.SetData(Constants.LAST_EXCEPTION, cqrLastEx);
        }
    }

}
