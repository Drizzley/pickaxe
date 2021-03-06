﻿using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;

namespace Pickaxe.Console
{
    public class Program
    {
        /*usage: pickaxe [options]
         *usage: pickaxe [path-to-source]
         * 
         * Options:
         * -i   interactive prompt
         * 
         * 
         * 
         */
        public static void Main(string[] args)
        {
            string location = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string log4netPath = Path.Combine(Path.GetDirectoryName(location), "Log4Net.config");
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            log4net.Config.XmlConfigurator.Configure(logRepository, new FileInfo(log4netPath));

            if (args.Length != 0)
            {
                var sources = new List<string>();

                if (args[0].StartsWith("http") || args[0].StartsWith("https"))
                {
                    using (var client = new WebClient())
                    {
                        sources.Add(client.DownloadString(args[0]));
                    }
                }
                else
                {
                    if (!File.Exists(args[0]))
                    {
                        System.Console.WriteLine(string.Format("File {0} not found.", args[0]));
                        return;
                    }

                    using (var reader = new StreamReader(args[0]))
                    {
                        sources.Add(reader.ReadToEnd());
                    }
                }

                Runner.Run(sources.ToArray(), new string[] { });
            }
            else
            {
                Interactive.Prompt();
            }
        }
    }
}
