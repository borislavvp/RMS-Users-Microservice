using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Users.Service.Common.Interfaces
{
    /// <summary>
    /// Represents the object responsible for keeping the result of an authentication request
    /// </summary>
    public interface IRequestResult
    {
        /// <summary>
        /// The reason that the action failed.
        /// </summary>
        string FailureReason { get; }

        /// <summary>
        /// Indication whether the action succeed.
        /// </summary>
        bool IsSuccess { get; }
    } 
    /// <summary>
    /// Represents the object responsible for keeping the result of an authentication request with additional data property
    /// </summary>
    public interface IRequestResult<T>
    {
        /// <summary>
        /// The data which is passed when the result is successfull
        /// </summary>
        T Data { get;}

        /// <summary>
        /// The reason that the action failed.
        /// </summary>
        string FailureReason { get; }

        /// <summary>
        /// Indication whether the action succeed.
        /// </summary>
        bool IsSuccess { get; }
    }
}
