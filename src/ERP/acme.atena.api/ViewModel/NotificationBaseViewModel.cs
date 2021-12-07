using acme.atena.api.ViewModel.Util;
using acme.atena.domain.DTO.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace acme.atena.api.ViewModel
{
    public class NotificationBaseViewModel
    {
        private readonly List<NotificationViewModel> _notifications;
        public IReadOnlyCollection<NotificationViewModel> Notifications => _notifications;
        public bool HasNotifications => _notifications.Any();

        public NotificationBaseViewModel()
        {
            _notifications = new List<NotificationViewModel>();
        }

        public void AddNotification(string key, string message)
        {
            _notifications.Add(new NotificationViewModel(key, message));
        }

        public void AddNotification(NotificationViewModel notification)
        {
            _notifications.Add(notification);
        }

        public void AddNotifications(IReadOnlyCollection<NotificationViewModel> notifications)
        {
            _notifications.AddRange(notifications);
        }

        public void AddNotifications(IList<NotificationViewModel> notifications)
        {
            _notifications.AddRange(notifications);
        }

        public void AddNotifications(ICollection<NotificationViewModel> notifications)
        {
            _notifications.AddRange(notifications);
        }


        public virtual bool IsValid()
        {
            return true;
        }
    }
}
