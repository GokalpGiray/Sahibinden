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
            Practise();
        }
        private static void Practise()
        {
            //Site içerisinden ilgili verilerin çekilebilmesi için bağlantı kurulumu
            string mainUrl = "https://www.sahibinden.com";
            HtmlDocument doc = new HtmlDocument();
            WebClient client = new WebClient();

            client.Encoding = Encoding.UTF8;
            client.Headers.Add("accept", "application / json, text / plain, */*");
            client.Headers.Add("origin", "https://www.sahibinden.com");
            client.Headers.Add("user-agent", "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/15.0 Safari/605.1.15");
            
            string downloadString = client.DownloadString(mainUrl);
            doc.LoadHtml(downloadString);

            //Ana Sayfa vitrinindeki eleman sayısının belirledik
            string rootPath = "/html/body/div[5]/div[3]/div/div[3]/div[3]/ul";
            int itemCount = doc.DocumentNode.SelectSingleNode(rootPath).ChildNodes.Count;

            int liCount = (itemCount - 1) / 2;

            //Sitenin güvenlik uygulaması tek user-agent kullanıldığı takdirde istekleri banlıyor. Bunun olmaması için çalıştığından test ederek emin olduğumuz user-agent'lar kullanıyoruz.
            string[] userAgents = {
                "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_6_1 rv:6.0; sl-SI) AppleWebKit/534.8.2 (KHTML, like Gecko) Version/5.0 Safari/534.8.2",
                "Mozilla/5.0 (X11; Linux i686; rv:5.0) Gecko/20200217 Firefox/35.0",
                "Mozilla/5.0 (Macintosh; U; PPC Mac OS X 10_7_9) AppleWebKit/5310 (KHTML, like Gecko) Chrome/40.0.865.0 Mobile Safari/5310",
                "Mozilla/5.0 (iPad; CPU OS 8_0_2 like Mac OS X; sl-SI) AppleWebKit/535.7.3 (KHTML, like Gecko) Version/4.0.5 Mobile/8B113 Safari/6535.7.3",
                "Mozilla/5.0 (compatible; MSIE 5.0; Windows NT 6.0; Trident/4.0)",
                "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_6_1 rv:6.0; sl-SI) AppleWebKit/534.8.2 (KHTML, like Gecko) Version/5.0 Safari/534.8.2",
                "Mozilla/5.0 (compatible; MSIE 8.0; Windows NT 6.2; Trident/3.1)",
                "Mozilla/5.0 (Windows; U; Windows NT 5.1) AppleWebKit/531.12.5 (KHTML, like Gecko) Version/4.0.4 Safari/531.12.5",
                "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)",
                "Opera/8.28 (X11; Linux x86_64; en-US) Presto/2.8.282 Version/10.00",
                "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/15.0 Safari/605.1.15",
                "Opera/9.33 (Windows NT 6.2; en-US) Presto/2.12.276 Version/12.00",
                "Mozilla/5.0 (compatible; MSIE 5.0; Windows NT 5.01; Trident/4.0)",
                "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_8_5 rv:5.0) Gecko/20170813 Firefox/37.0",
                "Mozilla/5.0 (Windows NT 6.0; en-US; rv:1.9.1.20) Gecko/20200822 Firefox/37.0",
                "Mozilla/5.0 (compatible; MSIE 9.0; Windows 95; Trident/3.0)",
                "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/101.0.4951.64 Safari/537.36",
                "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/5360 (KHTML, like Gecko) Chrome/36.0.808.0 Mobile Safari/5360",
                "Opera/8.22 (Windows 95; sl-SI) Presto/2.10.255 Version/12.00",
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/99.0.4844.84 Safari/537.36"
            };

            //Öğeleri içine yazmak için liste oluşturduk
            List<Advert> adverts = new List<Advert>();

            //Her bir li'ye ayrı ayrı erişmek için döngü kullandık
            for (int i = 0; i < liCount ; i++)
            {
                string mainUrlHref = "https://www.sahibinden.com";
                HtmlDocument docHref = new HtmlDocument();
                WebClient clientHref = new WebClient();

                clientHref.Encoding = Encoding.UTF8;
                clientHref.Headers.Add("accept", "application / json, text / plain, */*");
                clientHref.Headers.Add("origin", "https://www.sahibinden.com");

                clientHref.Headers.Add("user-agent", userAgents[i % 20]);
                
                string downloadStringHref = clientHref.DownloadString(mainUrlHref);
                docHref.LoadHtml(downloadStringHref);

                string hrefXpath = "/html/body/div[5]/div[3]/div/div[3]/div[3]/ul/li[" + (i+1) + "]/a";
                //Bir sonraki adıma geçmeden verilerin düzgün şekilde çekilebildiğinden emin olmak için if kullandık
                if (docHref.DocumentNode.SelectSingleNode(hrefXpath) != null)
                {
                    string resultHref = docHref.DocumentNode.SelectSingleNode(hrefXpath).Attributes["href"].Value;

                    //Bir sonraki adıma geçmeden verilerin düzgün şekilde çekilebildiğinden emin olmak için if kullandık
                    if (resultHref != null)
                    {
                        string mainUrlTitle = ("https://www.sahibinden.com" + resultHref);
                        HtmlDocument docTitle = new HtmlDocument();
                        WebClient clientTitle = new WebClient();

                        clientTitle.Encoding = Encoding.UTF8;
                        clientTitle.Headers.Add("accept", "application / json, text / plain, */*");
                        clientTitle.Headers.Add("origin", "https://www.sahibinden.com");

                        clientTitle.Headers.Add("user-agent", userAgents[i%20]);

                        string downloadStringTitle = clientTitle.DownloadString(mainUrlTitle);
                        docTitle.LoadHtml(downloadStringTitle);

                        string titleXpath = "/html/body/div[5]/div[4]/div[1]/div/div[1]/h1";
                        string priceXpath = "/html/body/div[5]/div[4]/div[1]/div/div[2]/div[2]/input";

                        //Sitenin reklamlarının kodun çalışmasını olumsuz etkilemesini engellemek amacı ile if-else kullanılmıştır
                        if (docTitle.DocumentNode.SelectSingleNode(titleXpath) != null && docTitle.DocumentNode.SelectSingleNode(priceXpath) != null)
                        {
                            //Her seferinde yeni bir obje oluşturulacak ve bu objeler listenin içerisine aktarılacak
                            Advert advert = new Advert();

                            advert.Title = docTitle.DocumentNode.SelectSingleNode(titleXpath).InnerText;

                            string resultPrice = docTitle.DocumentNode.SelectSingleNode(priceXpath).Attributes["value"].Value;
                            string trimPrice = resultPrice.TrimStart();
                            string[] price = trimPrice.Split(' ');
                            advert.Price = Convert.ToInt64(price[0].Replace(".", ""));
                            
                            advert.Currency = price[1];

                            adverts.Add(advert);
                            Console.WriteLine((i+1) + ") " + advert.Title + " - " + price[0] + " " + advert.Currency);
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
            }
            Console.WriteLine("Average Price = " + Math.Round(GetAverage(adverts), 3).ToString());
            WriteToFile(adverts);
        }
        //Ortalama alma hesabı
        private static double GetAverage(List<Advert> advert)
        {
            List<long> prices = advert.Select(x => x.Price).ToList();

            return prices.Average();
        }
        //Hazırlanan liste .txt uzantılı dosya içerisine yazdırılır
        private static void WriteToFile(List<Advert> advert)
        {
            FileStream fs = new FileStream("C:\\Users\\gökalp giray\\Desktop\\adverts.txt", FileMode.Append, FileAccess.Write, FileShare.Write);
            StreamWriter sw = new StreamWriter(fs);

            foreach (var ad in advert)
            {
                sw.WriteLine(ad.Title + " - " + ad.Price + " " + ad.Currency);
            }
            sw.Close();
            Console.WriteLine("İlanlar C:\\Users\\gökalp giray\\Desktop\\adverts.txt dosya yoluna kayıt edildi.");
        }
    }
}
