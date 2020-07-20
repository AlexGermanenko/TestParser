using System;
using System.Text;
using System.Net;
using System.IO;
using HtmlAgilityPack;

namespace TestParser
{
    class Car
    {
        private string _vin { get; set; }
        private string _price { get; set; }
        private string _url { get; set; }

        public Car(string vin, string price, string url)
        {
            _vin = vin;
            _price = price;
            _url = url;
        }

        public override string ToString()
        {
            return $"Vin:   {_vin};\nPrice: {_price};\nUrl:   {_url}.";
        }
    }

    class Program
    {
        public static WebRequest request;
        public static WebResponse response;

        static void Main(string[] args)
        {
            HtmlDocument html = new HtmlDocument();
            html.LoadHtml(GetHtmlPage("https://www.kellysubaru.com/used-inventory/index.htm"));

            var vin = html.DocumentNode.SelectNodes("//dl[@class ='vin']/dd")[1].InnerText;
            var price = html.DocumentNode.SelectNodes("//span[starts-with(@class,'internetPrice ')]/span[@class='value']")[1].InnerText;
            var imgUrl = html.DocumentNode.SelectNodes("//div[@class ='media']/*/img")[1].Attributes["src"].Value;

            Console.WriteLine(new Car(vin, price, imgUrl).ToString());

            Console.ReadLine();
        }

        private static string GetHtmlPage(string requestUri)
        {
            HttpWebRequest request = HttpWebRequest.Create(requestUri) as HttpWebRequest;
            request.UserAgent = "UserAgent";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader myStream1 = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8"));
            return myStream1.ReadToEnd();
        }
    }
}
