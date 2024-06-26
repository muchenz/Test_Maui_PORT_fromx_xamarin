﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Maui;
using Test_MauiApp1.Models;


namespace Test_MauiApp1.Helpers
{
    public class Message
    {

        public static void MessageDontHavePermission(Application application)
        {
            var message = new DisplayAlertMessage();
            message.Title = "Message";
            message.Message = $"You don't have perrmision\n to do this operation.";
            message.Cancel = "Ok";

            MessagingCenter.Send<Application, DisplayAlertMessage>(application, "ShowAlert", message);


        }


        public static void SimpleMessage(Application application, string txt)
        {
            var message = new DisplayAlertMessage();

            message.Title = "Information";
            message.Message = txt;
            message.Cancel = "Ok";

            MessagingCenter.Send<Application, DisplayAlertMessage>(Application.Current, "ShowAlert", message);


        }
    }
}
