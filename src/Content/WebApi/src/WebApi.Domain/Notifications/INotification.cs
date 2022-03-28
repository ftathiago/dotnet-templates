using System;
using System.Collections.Generic;

namespace WebApi.Domain.Notifications
{
    public interface INotification
    {
        string ErrorCode { get; }

        void AddMessage(string errorCode, string content);

        void AddException(Exception exception, string errorCode = default);

        bool Any();

        string StringifyMessages();

        IEnumerable<(string Code, string Content)> All();
    }
}
