﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PatientPortalService.Api.Infrastructure.Adaptors
{
    public class SMSService : IMessageSystem
    {
        public void Send(Message msg)
        {
            //notify user via sms
        }
    }
}