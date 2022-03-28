using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebApi.Shared.Extensions;

namespace WebApi.Domain.Notifications
{
    public class Notification : INotification
    {
        private readonly List<Message> _messages = new();

        public string ErrorCode { get; protected set; }

        public void AddMessage(string errorCode, string content)
        {
            _messages.Add(new Message
            {
                Code = errorCode,
                Content = content,
            });

            SetErrorCode(errorCode);
        }

        public void AddException(Exception exception, string errorCode = default) => AddMessage(
            errorCode,
            exception
                .GetAllMessage(Environment.NewLine)
                .Trim());

        public bool Any() => _messages.Count > 0;

        public string StringifyMessages() => _messages
            .Aggregate(new StringBuilder(), (sb, message) => sb.AppendLine(message.Content))
            .ToString()
            .Trim();

        public IEnumerable<(string Code, string Content)> All() => _messages
            .Select(message => (message.Code, message.Content));

        private void SetErrorCode(string errorCode)
        {
            var shouldKeepFirstErrorCode = ErrorCode == default;

            if (shouldKeepFirstErrorCode)
            {
                ErrorCode = errorCode;
            }
        }
    }
}
