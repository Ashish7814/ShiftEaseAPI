using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiftEase.Core.Interface
{
    public interface IEmailService
    {
        Task SendAsync(string to, string subject, string htmlBody);
    }
}
