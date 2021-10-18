using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Service.Common.Interfaces;

namespace Users.Service.Common.Models
{
    public class RequestResult : IRequestResult
    {
        public string FailureReason { get; }

        public bool IsSuccess => this.FailureReason == null;

        private RequestResult()
        {
        }

        public static RequestResult Success()
        {
            return new RequestResult();
        }
        
        private RequestResult(string failureReason)
        {
            this.FailureReason = failureReason;
        }

        public static RequestResult Failure(string failureReason)
        {
            return new RequestResult(failureReason);
        }
    }

    public class RequestResult<T> : IRequestResult<T>
    {
        public string FailureReason { get; }

        public bool IsSuccess => this.FailureReason == null;

        public T Data   {get; }

        private RequestResult(T data)
        {
            this.Data = data;
        }

        public static RequestResult<T> Success(T data)
        {
            return new RequestResult<T>(data);
        }
        
        private RequestResult(string failureReason)
        {
            this.FailureReason = failureReason;
        }

        public static RequestResult<T> Failure(string failureReason)
        {
            return new RequestResult<T>(failureReason);
        }
    }
}
