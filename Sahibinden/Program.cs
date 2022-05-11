using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using HtmlAgilityPack;

namespace Sahibinden
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Deneme();

            
        }
        private static void Deneme()
        {
            string mainUrl = "https://www.sahibinden.com";
            HtmlDocument doc = new HtmlDocument();
            WebClient client = new WebClient();
            client.Encoding = Encoding.UTF8;
            client.Headers.Add("accept", "application / json, text / plain, */*");
            client.Headers.Add("origin", "https://www.sahibinden.com");

            client.Headers.Add("user-agent", "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/15.0 Safari/605.1.15");
            string downloadString = client.DownloadString(mainUrl);
            doc.LoadHtml(downloadString);

            string rootPath = "/html/body/div[5]/div[3]/div/div[3]/div[3]/ul";
            int itemCount = doc.DocumentNode.SelectSingleNode(rootPath).ChildNodes.Count;

            for (int i = 0; i < itemCount; i++)
            {
                string xpath = "/html/body/div[5]/div[3]/div/div[3]/div[3]/ul/li[" + i + "]/a/span";
                if (doc.DocumentNode.SelectSingleNode(xpath) != null)
                {
                    string resultText = doc.DocumentNode.SelectSingleNode(xpath).InnerText;
                    Console.WriteLine(i + " - " + resultText);
                }
            }
            //Tread
            //Asnyc Job
        }
    }
}
