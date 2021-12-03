using Apps.Domain.Business.Notes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Domain.Business.Interfaces
{
    public interface INotes
    {
        bool HasNotes();
        List<Notification> GetNotes();
        void Handle(Notification note);
    }
}
