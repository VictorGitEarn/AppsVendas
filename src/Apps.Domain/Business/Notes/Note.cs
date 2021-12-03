using Apps.Domain.Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Domain.Business.Notes
{
    public class Note : INotes
    {
        private List<Notification> _notifications;

        public Note()
        {
            _notifications = new List<Notification>();
        }

        public List<Notification> GetNotes()
        {
            return _notifications;
        }

        public void Handle(Notification note)
        {
            _notifications.Add(note);
        }

        public bool HasNotes()
        {
            return _notifications.Any();
        }
    }
}
