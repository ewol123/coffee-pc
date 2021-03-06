﻿using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coffee_pc.Services
{
    public class SignalRMasterClient
    {
        public string Url { get; set; }
        public HubConnection Connection { get; set; }
        public IHubProxy Hub { get; set; }
        public Action MethodToCall { get; set; }

        public SignalRMasterClient(string url, Action method)
        {
            Connect(url,method);
        }

        public void Stop()
        {
            Connection.Stop();
        }

        public void Connect(string url, Action method)
        {
            try
            {
                Url = url;
                Connection = new HubConnection(url, useDefaultUrl: false);
                Hub = Connection.CreateHubProxy("ServiceStatusHub");
                Connection.Start().Wait();
                Hub.On<string>("ordersPlaced", (message) =>
                {
                    MethodToCall = method;
                    MethodToCall();
                });

                Hub.On<string>("ordersFinalize", (message) =>
                {
                    MethodToCall = method;
                    MethodToCall();
                });
            }
            catch (Exception e)
            {
                Connect(url,method);
            }

        }

       
    }
}
