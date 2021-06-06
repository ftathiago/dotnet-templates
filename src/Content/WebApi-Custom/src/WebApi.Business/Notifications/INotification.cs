using System;
using System.Collections.Generic;

namespace WebApi.Business.Notifications
{
    public interface INotification
    {
        int ErrorCode { get; }

        void AddMessage(int errorCode, string content);

        void AddException(Exception exception, int errorCode = default);

        bool Any();

        string StringifyMessages();

        IEnumerable<(int Code, string Content)> All();
    }
}
