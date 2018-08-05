using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PatientPortalService.Api.Infrastructure.Adaptors
{
    public interface IMessageSystem
    {
        void Send(Message msg);
    }
}