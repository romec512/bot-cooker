using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Bot_Application1
{
    public class Parser
    {
        private static string GetPage(string site, Activity message)
        {

            var str = HttpUtility.UrlEncode(message.Text, Encoding.GetEncoding(1251));
            site = site + str;
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(site);
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

            using (StreamReader stream = new StreamReader(
                 resp.GetResponseStream(), Encoding.GetEncoding(1251)))
            {
                str = stream.ReadToEnd();
            }
            return str;
        }

        public static string ParseListReciepts(string reciept, Activity message)
        {
            string site = "http://www.povarenok.ru/recipes/search/?name=";
            Regex reg = new Regex(@"<a href=""http://www.povarenok.ru/recipes/show/[\d]+/"" title=""посмотреть рецепт [^>]+"">[^<]+</a>");
            Regex reg1 = new Regex(@">[^<]+<");
            Regex NameOfRec = new Regex(@"[^<]");
            string page = GetPage(site, message);
        }
    }
}