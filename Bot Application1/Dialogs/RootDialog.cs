using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Text;
using System.Web;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;

namespace Bot_Application1.Dialogs
{
    [Serializable]
    public class EchoDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }
        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;
            string site = "http://www.povarenok.ru/recipes/search/?name=";
            string str = Parser.GetPage(site, message);

            //HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(site);
            //HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

            //using (StreamReader stream = new StreamReader(
            //     resp.GetResponseStream(), Encoding.GetEncoding(1251)))
            //{
            //    str = stream.ReadToEnd();
            //}
            Regex reg = new Regex(@"http://www.povarenok.ru/recipes/show/[0-9]+/");
            Regex article = new Regex(@"/[0-9]+/");
            //Regex reciept = new Regex(@"<td valign=""top"" style=""padding: 0px 0px 0px 6px;"">[^\>\<]+</td>");
            //Regex NameRecieptReg = new Regex(@"<a href=""http://www.povarenok.ru/recipes/show/[0-9]+/"">[^\>\<]+</a>");
            Match art;
            //foreach (Match match in reg.Matches(str))
            //{
            //    art = article.Match(match.ToString());
            //    break;
            //}
            art = article.Match(reg.Match(str).ToString());//парсим ответ с полученными рецептами, находим номер статьи, чтобы получить принт версию
            site = "http://www.povarenok.ru/recipes/print" + art.ToString();
            //req = (HttpWebRequest)HttpWebRequest.Create(site);
            //resp = (HttpWebResponse)req.GetResponse();
            //using (StreamReader stream = new StreamReader(
            //     resp.GetResponseStream(), Encoding.GetEncoding(1251)))
            //{
            //    str = stream.ReadToEnd();
            //}
            //str = str.Replace("&quot;", "\"");
            //string NameReciept = NameRecieptReg.Match(str).ToString();
            //NameReciept = Regex.Match(NameReciept, @">[^\>\<]+<").ToString();
            //NameReciept = Regex.Match(NameReciept, @"[^\>\<]+").ToString();
            //await context.PostAsync(NameReciept);
            //foreach (Match match in reciept.Matches(str))//парсим ответ с принт странички на сам рецепт.
            //    //сначала находим по нужному тегу, дальше откидываем тег, далее откидываем знаки > <
            //    //ничего умнее не придумал, увы :/
            //{
            //    string point = match.ToString();
            //    point = Regex.Match(point, @">[^\>\<]+<").ToString();
            //    point = Regex.Match(point, @"[^\>\<]+").ToString();
            //    await context.PostAsync(point);
            //}
            string result = Parser.ParseReciept(site);
            context.PostAsync(result);
            context.Wait(MessageReceivedAsync);
        }
    }
}
