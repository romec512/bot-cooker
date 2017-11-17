using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Text;
using System.Web;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;

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
            //string site = "http://www.povarenok.ru/recipes/search/?name=";using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Linq;
using System.Web.Http;
using System.Net.Http;

namespace Bot_Application4.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
            return Task.CompletedTask;
        }
        private async Task Start1(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            if (activity.Text == "Help")
            {
                await context.PostAsync($"HELP1");// сообщение о помощи
            }
            if (activity.Text == "Старт")
            {
                await context.PostAsync($"Введите ингридиенты...");
                context.Wait(Ask1);
            }
            if (activity.Text != "Старт" && activity.Text != "Help")
            {
                //вывод списка из 10 названий
                await context.PostAsync($"Введите номер рецепта");
            }
            context.Wait(Ask1);
        }
        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {

            var activity = await result as Activity;
            if (activity.Text == "start")
            {
                await context.PostAsync($"Здравствуйте! " +
                      $"Я Бот Кулинар, ваш помощник по приготовлению блюда. Я могу предложить вам рецепты. Напишите Старт что бы начать");
                context.Wait(Start1);
            }
            if (activity.Text == "Help")
            {
                await context.PostAsync($"HELP");// сообщение о помощи
            }
            if (activity.Text == "Старт")
            {

                await context.PostAsync($"Введите ингридиенты...");
                context.Wait(Start1);

            }
            if (activity.Text != "Старт" && activity.Text != "Help" && activity.Text != "start")
            {
                await context.PostAsync($"Я не знаю такой команды");
                context.Wait(MessageReceivedAsync);
            }
        }
        private async Task Ask1(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            switch (activity.Text)
            {
                case "1":
                    await context.PostAsync($"Французский салат. Продукты (на 8 порций) Яблоки - 2 шт. Морковь свежая - 2 шт. Яйца куриные - 4 шт. Сыр твердый - 100-150 г Лук репчатый (по желанию) - 1 шт.айонез - по вкусу (100 г)");
                    await context.PostAsync($"Способ приготовления...");
                    await context.PostAsync($"Вам понравился рецепт?");
                    context.Wait(Ask2);
                    break;
                case "2":
                    await context.PostAsync($"Селёдка под шубой. ");
                    await context.PostAsync($"Способ приготовления...");
                    await context.PostAsync($"Вам понравился рецепт?");
                    context.Wait(Ask2);
                    break;
                case "3":
                    await context.PostAsync($"Суп с говядиной. ");
                    await context.PostAsync($"Способ приготовления...");
                    await context.PostAsync($"Вам понравился рецепт?");
                    context.Wait(Ask2);
                    break;
                case "4":
                    await context.PostAsync($"Запеканка. ");
                    await context.PostAsync($"Способ приготовления...");
                    await context.PostAsync($"Вам понравился рецепт?");
                    context.Wait(Ask2);
                    break;
                case "5":
                    await context.PostAsync($"Компот из сухофруктов. ");
                    await context.PostAsync($"Способ приготовления...");
                    await context.PostAsync($"Вам понравился рецепт?");
                    context.Wait(Ask2);
                    break;
                case "6":
                    await context.PostAsync($"Плов по-узбекски. ");
                    await context.PostAsync($"Способ приготовления...");
                    await context.PostAsync($"Вам понравился рецепт?");
                    context.Wait(Ask2);
                    break;
                case "7":
                    await context.PostAsync($"Макароны по-флотски. ");
                    await context.PostAsync($"Способ приготовления...");
                    await context.PostAsync($"Вам понравился рецепт?");
                    context.Wait(Ask2);
                    break;
                case "8":
                    await context.PostAsync($"Рис с мясом. ");
                    await context.PostAsync($"Способ приготовления...");
                    await context.PostAsync($"Вам понравился рецепт?");
                    context.Wait(Ask2);
                    break;
                case "9":
                    await context.PostAsync($"Очпочмак. ");
                    await context.PostAsync($"Способ приготовления...");
                    await context.PostAsync($"Вам понравился рецепт?");
                    context.Wait(Ask2);
                    break;
                case "10":
                    await context.PostAsync($"Мантый. ");
                    await context.PostAsync($"Способ приготовления...");
                    await context.PostAsync($"Вам понравился рецепт?");
                    context.Wait(Ask2);
                    break;
                case "Help":
                    await context.PostAsync($"HELP");
                    context.Wait(Ask1);
                    break;
                case "Старт":
                    await context.PostAsync($"Введите ингридиенты...");
                    context.Wait(Start1);
                    break;
                default:
                    await context.PostAsync($"Ошибка. Вы ввели неправильно!");
                    await context.PostAsync($"Введите номер рецепта или введите Старт что бы начать заново");
                    context.Wait(Ask1);
                    break;
            }
        }
        private async Task Ask2(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            switch (activity.Text)
            {
                case "Да":
                    await context.PostAsync($"Вы можете ввести Старт что бы найти что-то ещё");
                    context.Wait(Hop);
                    break;
                case "Нет":
                    await context.PostAsync($"Выберите один из оставшихся");
                    context.Wait(Ask1);
                    break;
            }
        }
        private async Task Hop(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            if (activity.Text == "Старт")
            {
                await context.PostAsync($"Введите ингридиенты...");
                context.Wait(Start1);
            }
            else
            {
                context.Wait(Hop);
                await context.PostAsync($"Введите Старт для продожения");
            }
        }
    }
}
            //string str = Parser.GetPage(site, message);

            //HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(site);
            //HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

            //using (StreamReader stream = new StreamReader(
            //     resp.GetResponseStream(), Encoding.GetEncoding(1251)))
            //{
            //    str = stream.ReadToEnd();
            //}
            //Regex reg = new Regex(@"http://www.povarenok.ru/recipes/show/[0-9]+/");
            //Regex article = new Regex(@"/[0-9]+/");
            //Regex reciept = new Regex(@"<td valign=""top"" style=""padding: 0px 0px 0px 6px;"">[^\>\<]+</td>");
            //Regex NameRecieptReg = new Regex(@"<a href=""http://www.povarenok.ru/recipes/show/[0-9]+/"">[^\>\<]+</a>");
            //Match art;
            //foreach (Match match in reg.Matches(str))
            //{
            //    art = article.Match(match.ToString());
            //    break;
            //}
            //art = article.Match(reg.Match(str).ToString());//парсим ответ с полученными рецептами, находим номер статьи, чтобы получить принт версию
            //site = "http://www.povarenok.ru/recipes/print" + art.ToString();
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
            string [,] res = Parser.ParseListReciepts(message.Text, message);
            for (int i = 0; i < 10; i++)
            {
                context.PostAsync(res[0, i]);
                Thread.Sleep(100);
            }
            message = await argument;
            if (Regex.Match(message.Text, @"[\d]+").ToString() != "")
            {
                int kek = Convert.ToInt32(message.Text);
                context.PostAsync(res[0, kek]);
                string result = Parser.ParseIngredient(res[1, kek]);
                context.PostAsync(res[0, kek]);
                result = Parser.ParseReciept(res[1, kek]);
                context.PostAsync(result);
            }
            context.Wait(MessageReceivedAsync);
        }
    }
}
