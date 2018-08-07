﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PatientPortalService.Api.Infrastructure.Adaptors
{
    public class SendMessageStrategyForEmail : ISendMessageStrategy
    {
        private readonly Message _msg;
        public SendMessageStrategyForEmail(Message msg)
        {
            _msg = msg;
        }

        public void SendMessages()
        {
            EmailService objemail = new EmailService();
            objemail.Send(_msg);
        }



    }
}