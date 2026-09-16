using System;

namespace Assets.Up2Unity.Runtime
{
    public class Up2UnityException : Exception
    {
        internal Up2UnityException(ErrorData err) : base(err.Message)
        {
            Code = err.Code;
        }

        public string Code { set; get; }
    }
}
