using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebApi.Shared.Extensions;

namespace WebApi.Business.Notifications
{
    public class Notification : INotification
    {
        private readonly List<Message> _messages = new();

        public int ErrorCode { get; protected set; }

        public void AddMessage(int errorCode, string content)
        {
            _messages.Add(new Message
            {
                Code = errorCode,
                Content = content,
            });

            SetErrorCode(errorCode);
        }

        public void AddException(Exception exception, int errorCode = default) => AddMessage(
            errorCode,
            exception.GetAllMessage(Environment.NewLine)
                .ToString()
                .Trim());

        public bool Any() => _messages.Count > 0;

        public string StringifyMessages() => _messages
            .Aggregate(new StringBuilder(), (sb, message) => sb.AppendLine(message.Content))
            .ToString()
            .Trim();

        public IEnumerable<(int Code, string Content)> All() => _messages
            .Select(message => (message.Code, message.Content));

        private void SetErrorCode(int errorCode)
        {
            var shouldKeepFirstErrorCode = ErrorCode == default;

            if (shouldKeepFirstErrorCode)
            {
                ErrorCode = errorCode;
            }
        }
    }
}
