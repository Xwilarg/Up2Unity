using System;

namespace Assets.Up2Unity.Runtime
{
    public class Up2JamException : Exception
    {
        internal Up2JamException(ErrorData err) : base(err.Message)
        {
            Code = err.Code;
        }

        public string Code { set; get; }
    }
}
