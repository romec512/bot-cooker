using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Linq;
using System.Web.Http;
using System.Net.Http;
using System.Threading;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using AdaptiveCards;

namespace Bot_Application1.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        private string[,] res;
        private int kek;
        private int j = 0;
        private List<string> reciepts;
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }
        private Activity NewActivity(string _type, string _title, string _value, string _text)
        {
            CardAction action = new CardAction()
            {
                Type = _type,
                Title = _title,
                Value = _value
            };
            HeroCard card = new HeroCard();
            card.Buttons.Add(action);
            Activity activity = MessagesController.act.CreateReply("");
            Attachment attachment = card.ToAttachment();
            activity.Attachments.Add(attachment);
            activity.Text = _text;
            return activity;

        }
        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)//нажатие на кнопку start в telegram
        {
            var message = await argument;
            if(message.Text.ToLower() == "старт")
            {
                await context.PostAsync("Введите ингредиенты или название блюда.");
                context.Wait(Poisk);
            }
            else if(message.Text.ToLower() == "помощь")
            {
                await context.PostAsync("Введите \"Старт\", чтобы начать, далее следуйте инструкциям! Удачного пользования!");
                context.Wait(MessageReceivedAsync);
            }
            else if(message.Text.ToLower() == "/start")
            {
                //await context.PostAsync("Вас приветствует бот-кулинар! Введите \"старт\" для начала, а далее следуйте инструкциям! Вас приветствует бот-кулинар!");
                Activity act = this.NewActivity("postBack", "Начать", "старт","Вас приветствует бот-кулинар!Нажмите \"старт\" для начала, а далее следуйте инструкциям! Вас приветствует бот-кулинар!");
                await context.PostAsync(act);
                context.Wait(MessageReceivedAsync);
            }
            else
            {
                await context.PostAsync("Прощу прощения, я вас не понял! Введите \"Старт\", чтобы начать сначала!");
                context.Wait(MessageReceivedAsync);
            }
        }

        private async Task Poisk(IDialogContext context, IAwaitable<IMessageActivity> argument)//Поиск (ввод ингридиентов) и вывод списка - Парсинг
        {//Здесь парсится ингридиенты и вывод списка.
            var message = await argument;
            res = Parser.ParseListReciepts(message.Text, message);
            if (res[0, 1] == null)
            {
                await context.PostAsync("К сожалению, по вашему запросу ничего не найдено. Попробуйте ввести ингредиенты заново.");
                context.Wait(Poisk);
            }
            else
            {
                //this.Show(context);
                //context.PostAsync(this.NewActivity("postBack", res[0, 1], "0", "kiki"));
                HeroCard card = new HeroCard();
                for (int i = 0; i < 10; i++)
                {
                    if (res[0, i] == null)
                        break;
                    CardAction action = new CardAction()
                    {
                        Type = "postBack",
                        Title = res[0, i],
                        Value = "" + i
                    };
                    card.Buttons.Add(action);
                }
                Activity act = MessagesController.act.CreateReply("");
                Attachment attacment = card.ToAttachment();
                act.Attachments.Add(attacment);
                //act.AttachmentLayout = "carousel";
                act.Text = "Выберите рецепт";
                await context.PostAsync(act);
                context.Wait(Eating);// Передача номера рецепта
            }
        }

        private async void Show(IDialogContext context)
        {
            HeroCard card = new HeroCard();
            for(int i = 0; i < 1; i++)
            {
                if (res[0, i] == null)
                    break;
                CardAction action = new CardAction()
                {
                    Type = "postBack",
                    Title = res[0, i],
                    Value = i
                };
                card.Buttons.Add(action);
            }
            Attachment attacment = card.ToAttachment();
            Activity act = MessagesController.act.CreateReply("");
            act.Attachments.Add(attacment);
            //act.AttachmentLayout = "carousel";
            act.Text = "Выберите рецепт";
            await context.PostAsync(act);
        }
        private async Task Eating(IDialogContext context, IAwaitable<IMessageActivity> argument)//Вывод приготовления блюда - Парсинг. И вопрос нравится или нет блюдо
        {
            var message = await argument;
            if (Regex.Match(message.Text, @"[\d]+").ToString() != "")
            {
                kek = Convert.ToInt32(message.Text);
                Thread.Sleep(300);
                await context.PostAsync(res[0, kek]);
                string result = Parser.ParseIngredient(res[1, kek]);
                string time = Parser.ParseTime(res[1, kek]);
                Thread.Sleep(300);
                await context.PostAsync(result);
                Thread.Sleep(300);
                await context.PostAsync(time);
                Thread.Sleep(300);
                HeroCard card = new HeroCard();
                card.Buttons.Add(new CardAction()
                {
                    Type = "postBack",
                    Title = "Да",
                    Value = "Да"                     
                });
                card.Buttons.Add(new CardAction()
                {
                    Type = "postBack",
                    Title = "Нет",
                    Value = "Нет"
                });
                Attachment attachment = card.ToAttachment();
                Activity mes = MessagesController.act.CreateReply("") ;
                mes.Attachments.Add(attachment);
                mes.Text = "Желаете продолжить с этим рецептом?";
                Thread.Sleep(300);
                await context.PostAsync(mes);
                Thread.Sleep(300);
                context.Wait(Like);
            }
            else if(message.Text.ToLower() == "старт")
            {
                await context.PostAsync("Введите ингредиенты или название блюда.");
                context.Wait(Poisk);
            }
            else
            {
                await context.PostAsync($"Ошибка! Такого числа нет в списке. Выберите один из рецептов. Введите число от 1 до 10:");
                context.Wait(Eating);
            }

        }
        //Здесь добавить вывод рецепта.
        private async Task Like(IDialogContext context, IAwaitable<IMessageActivity> argument)//Обработка нравится или нет. Запрос выбора рецепта из того же списка.
        {
            reciepts = new List<string>();
            var message = await argument;
            if (message.Text.ToLower() == "да")
            {
                reciepts = Parser.ParseReciept(res[1, kek]);
                //HeroCard card = new HeroCard();
                //card.Buttons.Add(new CardAction()
                //{
                //    Type = "postBack",
                //    Title = "Дальше",
                //    Value = "Да"
                //});
                //Attachment attachment = card.ToAttachment();
                //Activity mes = MessagesController.act.CreateReply("");
                //mes.Text = reciepts[j];
                //mes.Attachments.Add(attachment);
                Activity mes = this.NewActivity("postBack", "Далее", "да", reciepts[j]);
                Thread.Sleep(300);
                await context.PostAsync(mes);
                context.Wait(Further);
            }
            else if (message.Text.ToLower() == "нет")
            {
                this.Show(context);
                context.Wait(Eating);
            }
            else if(message.Text.ToLower() == "старт")
            {
                await context.PostAsync("Введите ингредиенты или название блюда.");
                context.Wait(Poisk);
            }
            else
            {
                await context.PostAsync($"Ошибка! Вам понравилось блюдо? Введите Да или Нет:");
                context.Wait(Like);
            }
        }
        private async Task Further(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;
            if ((j < reciepts.Count - 1) && (message.Text.ToLower() == "да"))
            {
                j++;
                //Thread.Sleep(300);
                //await context.PostAsync(reciepts[j]);
                //Thread.Sleep(300);
                //HeroCard card = new HeroCard();
                //card.Buttons.Add(new CardAction()
                //{
                //    Type = "postBack",
                //    Title = "Дальше",
                //    Value = "Да"
                //});
                //Attachment attachment = card.ToAttachment();
                //Activity mes = MessagesController.act.CreateReply("");
                //mes.Text = "";
                //mes.Attachments.Add(attachment);
                Activity mes = this.NewActivity("postBack", "Далее", "да", reciepts[j]);
                Thread.Sleep(300);
                await context.PostAsync(mes);
                Thread.Sleep(300);
                context.Wait(Further);
            }
            else if(message.Text.ToLower() == "нет")
            {
                await context.PostAsync(reciepts[j]);
                await context.PostAsync("Вы закончили этот шаг?");
                Thread.Sleep(300);
                context.Wait(Further);
            }
            else if (j == reciepts.Count - 1)
            {
                HeroCard card = new HeroCard();
                card.Buttons.Add(new CardAction()
                {
                    Type = "postBack",
                    Title = "Начать сначала",
                    Value = "старт"
                });
                Attachment attachment = card.ToAttachment();
                Activity mes = MessagesController.act.CreateReply("");
                mes.Text = "Ваше блюдо готово!Приятного аппетита. Жду вас снова! Для начала нажмите на кнопку.";
                mes.Attachments.Add(attachment);
                Thread.Sleep(300);
                await context.PostAsync(mes);
                Thread.Sleep(300);
                context.Wait(MessageReceivedAsync);
            }
            else if (message.Text.ToLower() == "старт")
            {
                await context.PostAsync("Введите ингредиенты или название блюда.");
                context.Wait(Poisk);
            }
        }
        //private async Task Ask2(IDialogContext context, IAwaitable<IMessageActivity> argument)//Запрос на ввод ингридиентов, отправляется ввод в метод Poisk
        //{
        //    var message = await argument;
        //    if (message.Text.ToLower() == "да")
        //    {
        //        await context.PostAsync($"Для поиска рецепта введите ингридиенты. Например: огурец, помидор...");
        //        context.Wait(Poisk);
        //    }
        //    else if (message.Text.ToLower() == "нет")
        //    {
        //        await context.PostAsync("Спасибо за то, что воспользовались нашим ботом.");
        //        await context.PostAsync("Для начала работы введите слово Старт:");
        //        context.Wait(Start1);
        //    }
        //    else
        //    {
        //        await context.PostAsync($"Ошибка! Хотите выбрать другой рецепт? Введите: Да, Нет");
        //        context.Wait(Ask2);
        //    }
        //}
    }
}
