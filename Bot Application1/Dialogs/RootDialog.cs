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
            var str = HttpUtility.UrlEncode(message.Text, Encoding.GetEncoding(1251));
            string site = "http://www.povarenok.ru/recipes/search/?name=" + str;

            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(site);
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

            using (StreamReader stream = new StreamReader(
                 resp.GetResponseStream(), Encoding.GetEncoding(1251)))
            {
                str = stream.ReadToEnd();
            }

            using (StreamWriter sw = new StreamWriter(@"E:\output.html", false, System.Text.Encoding.Default))
            {
                sw.WriteLine(str);
            }
            Regex reg = new Regex(@"http://www.povarenok.ru/recipes/show/[0-9]+/");
            foreach (Match match in reg.Matches(str))
            {
                await context.PostAsync(match.ToString());
                break;
            }
            context.Wait(MessageReceivedAsync);
        }
    }
}
