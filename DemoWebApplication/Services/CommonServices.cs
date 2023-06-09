﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DemoWebApplication.Enums;
using static DemoWebApplication.Enums.Alerts;

namespace DemoWebApplication.Services
{
    public class CommonServices
    {
        public static string ShowAlert(Alert obj,string message)
        {
            string alertDiv = null;
            switch (obj)
            {
                case Alert.Success:
                    alertDiv = "<div class='alert alert-success alert-dismissable' id='alert'>" +
                        "<button type='button' class='close' data-dismiss='alert'>×</button>" +
                        "<i class=bi - check - circle - fill'></i>" +
                        "<strong> Success!</ strong > " + message + "</a>.</div>";

                    break;
                case Alert.Danger:
                    alertDiv = "<div class='alert alert-danger alert-dismissible' id='alert'><button type='button' class='close' data-dismiss='alert'>×</button><i class='bi - exclamation - octagon - fill'></i><strong> Error!</ strong > " + message + "</a>.</div>";
                    break;
                case Alert.Info:
                    alertDiv = "<div class='alert alert-info alert-dismissable' id='alert'><button type='button' class='close' data-dismiss='alert'>×</button><i class='bi - info - circle - fill'></i><strong> Info!</ strong > " + message + "</a>.</div>";
                    break;
                case Alert.Warning:
                    alertDiv = "<div class='alert alert-warning alert-dismissable' id='alert'><button type='button' class='close' data-dismiss='alert'>×</button><i class='bi - exclamation - triangle - fill'></i><strong> Warning!</strong> " + message + "</a>.</div>";
                    break;
            }
            return alertDiv;
        }
    }
}