using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Linq;
using System.Web.Http;
using System.Net.Http;

namespace Bot_Application1.Dialogs
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
