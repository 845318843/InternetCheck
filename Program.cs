using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace InternetCheck
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("");
            if (args.Length < 1)
            {
                System.Console.WriteLine("example: InternetCheck.exe xx.dnslog.cn");
            }
            if (args.Length >= 1)
            {
                Console.WriteLine();
                http(args[0]);
                dns(args[0]);
                Console.WriteLine("DNS and HTTP all done!");

            }
        }

        public static void dns(string host)
        {
            Console.WriteLine("dnslog");
            string machineName = Environment.MachineName;
            string ip = "";
            string name = Dns.GetHostName();
            IPAddress[] ipadrlist = Dns.GetHostAddresses(name);
            foreach (IPAddress ipa in ipadrlist)
            {
                if (ipa.AddressFamily == AddressFamily.InterNetwork)
                {
                    ip = ip + ipa.ToString() + "-";
                }
            }
            string dnsstring = ip;//+ machineName
            Command c = new Command();
            string all = c.RunCmd("nslookup " + dnsstring + "." + host);
            Console.WriteLine("nslookup done!");
        }

        public static void http(string url)
        {
            string machineName = Environment.MachineName;
            int i = 1;
            NameValueCollection postValues = new NameValueCollection();
            postValues.Add("host", machineName);
            string name = Dns.GetHostName();
            IPAddress[] ipadrlist = Dns.GetHostAddresses(name);
            foreach (IPAddress ipa in ipadrlist)
            {
                if (ipa.AddressFamily == AddressFamily.InterNetwork)
                {
                    //组装数据  
                    postValues.Add("ip" + i.ToString(), ipa.ToString());
                    i++;
                }
            }
            Post(url, postValues);
        }

        public static void Post(string url, NameValueCollection postValues)
        {

            try
            {
                //定义webClient对象
                WebClient webClient = new WebClient();

                url = "http://" +postValues.Get("ip1")+ '.' + url + "/";
                Console.WriteLine(url);
                //向服务器发送POST数据
                byte[] responseArray = webClient.UploadValues(url, postValues);
                string data = Encoding.ASCII.GetString(responseArray);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }    
    
}
