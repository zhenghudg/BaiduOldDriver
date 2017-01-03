﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetDisk;
using System.Threading;

namespace GoodSamaritan
{
    class MyHttpServer : HttpServer
    {
        private Credential cred;
        private Dictionary<string, DateTime> lasttime = new Dictionary<string, DateTime>();
        public MyHttpServer(int port, Credential cred)
        : base(port)
        {
            this.cred = cred;
            new Thread(new ThreadStart(GC)).Start();
        }
        public override void handleGETRequest(HttpProcessor p)
        {
            var ip = p.socket.Client.RemoteEndPoint.ToString().Split(':')[0];
            lock (this)
            {
                if (lasttime.ContainsKey(ip) && (DateTime.Now - lasttime[ip]).TotalSeconds < 10.0)
                {
                    p.writeFailure();
                    p.outputStream.WriteLine("Wait 10 seconds before next request");
                    return;
                }
                lasttime[ip] = DateTime.Now;
            }
            try
            {
                var args = p.http_url.Split('/');
                var tres = Operation.Transfer(Uri.UnescapeDataString(args[1]), "/", cred, args[2]);
                if (tres.success == false || tres.errno != 0) throw tres.exception;
                var dres = Operation.GetDownload(tres.info[0].path, cred);
                p.writeSuccess();
                dres.urls.ToList().ForEach(u => p.outputStream.WriteLine(u.url));
            }
            catch (Exception ex)
            {
                p.writeFailure(); p.outputStream.WriteLine(ex);
            }
        }
        public override void handlePOSTRequest(HttpProcessor p, StreamReader inputData)
        {
            p.writeFailure();
            p.outputStream.WriteLine("Use GET");
        }
        private void GC()
        {
            while (true)
            {
                lock (this)
                    lasttime.Clear();
                Thread.Sleep(60 * 1000);
            }
        }
    }
}
