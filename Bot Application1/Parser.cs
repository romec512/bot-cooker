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
        public static string GetPage(string site, IMessageActivity message)
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

        private static string GetPage(string site)
        {
            string str;
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(site);
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

            using (StreamReader stream = new StreamReader(
                 resp.GetResponseStream(), Encoding.GetEncoding(1251)))
            {
                str = stream.ReadToEnd();
            }
            return str;
        }

        public static string[,] ParseListReciepts(string reciept, IMessageActivity message)//метод для парсинга странички с 10 рецептами
        {
            string site = "http://www.povarenok.ru/recipes/search/?name=";
            Regex reg = new Regex(@"<a href=""http://www.povarenok.ru/recipes/show/[\d]+/"" title=""посмотреть рецепт [^\>]+"">[^\<]+</a>");
            Regex reg1 = new Regex(@">[^\<]+<");
            Regex link = new Regex(@"http://www.povarenok.ru/recipes/show/[\d]+/");
            Regex NameOfRec = new Regex(@"[^\>\<]+");
            string page = GetPage(site, message);
            int i = 0;
            page = page.Replace("&quot;", "\"");
            string[,] result = new string[2, 10];//массив с результатами парсинга, в 0 строке названия рецептов, в 1 строке ссылки на рецепт
            foreach (Match match in reg.Matches(page))
            {
                result[0, i] = i + 1 + ")"+NameOfRec.Match(reg1.Match(match.ToString()).ToString()).ToString();
                result[1, i] = link.Match(match.ToString()).ToString();
                i++;
            }
            return result;
        }

        public static List<string> ParseReciept(string site)//метод для парсинга странички с рецептом, site - ссылка на нужную страничку
        {
            Regex reciept = new Regex(@"<td valign=""top"" style=""padding: 0px 0px 0px 6px;"">[^\>\<]+</td>");
            string str = Parser.GetPage(site);
            str = str.Replace("&quot;", "\"");
            str = str.Replace("<br />", "");
            List<string> result = new List<string>();
            foreach (Match match in reciept.Matches(str))//парсим ответ с принт странички на сам рецепт.
                                                         //сначала находим по нужному тегу, дальше откидываем тег, далее откидываем знаки > <
                                                         //ничего умнее не придумал, увы :/
            {

                string point = match.ToString();
                point = Regex.Match(point, @">[^\>\<]+<").ToString();
                point = Regex.Match(point, @"[^\>\<]+").ToString();
                result.Add(point);
            }
            return result;
        }

        public static string ParseTime(string site)//метод для парсинка времени приготовления
        {
            Regex time = new Regex(@"itemprop=""totalTime"">[^<]+</time>");
            string page = Parser.GetPage(site);
            string result = Regex.Match(time.Match(page).ToString(), @">[^<]+<").ToString();
            result ="Время приготовления:" + Regex.Match(result, @"[^<>]+").ToString();
            return result;
        }

        public static string ParseIngredient(string site)//метод для парсинга странички с рецептом(для нахождения игредиентов)
        {
            Regex reg = new Regex(@"<li class=""cat"">[^\>\<]+</li>");
            Regex reg1 = new Regex(@">[^\>\<]+<");
            Regex reg2 = new Regex(@"[^\>\<]+");
            site = site.Replace("show", "print");
            string page = GetPage(site);
            page = page.Replace("&quot;", "\"");
            page = page.Replace("&mdash;", "-");
            page = page.Replace("\t", "");
            page = page.Replace("\n", "");
            string result = "";
            foreach(Match match in  reg.Matches(page))
            {
                result += reg2.Match(reg1.Match(match.ToString()).ToString()).ToString() + '\n';
            }
            return result;
        }
    }
}