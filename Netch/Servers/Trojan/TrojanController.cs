﻿using System.Collections.Generic;
using System.IO;
using Netch.Controllers;
using Netch.Models;
using Netch.Servers.Trojan.Models;
using Netch.Utils;
using Newtonsoft.Json;

namespace Netch.Servers.Trojan
{
    public class TrojanController : Guard, IServerController
    {
        public TrojanController()
        {
            StartedKeywords.Add("started");
            StoppedKeywords.Add("exiting");
        }

        public override string MainFile { get; protected set; } = "Trojan.exe";
        public override string Name { get; protected set; } = "Trojan";
        public int? Socks5LocalPort { get; set; }
        public string LocalAddress { get; set; }


        public bool Start(Server s, Mode mode)
        {
            var server = (Trojan) s;
            File.WriteAllText("data\\last.json", JsonConvert.SerializeObject(new TrojanConfig
            {
                local_addr = LocalAddress ?? Global.Settings.LocalAddress,
                local_port = Socks5LocalPort ?? Global.Settings.Socks5LocalPort,
                remote_addr = DNS.Lookup(server.Hostname).ToString(),
                remote_port = server.Port,
                password = new List<string>
                {
                    server.Password
                }
            }));

            return StartInstanceAuto("-c ..\\data\\last.json");
        }

        public override void Stop()
        {
            StopInstance();
        }
    }
}