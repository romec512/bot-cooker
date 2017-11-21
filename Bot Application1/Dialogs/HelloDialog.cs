using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Bot_Application1.Dialogs
{
    [Serializable]
    public class HelloDialog: IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.PostAsync("Привет! Я бот, который поможет тебе приготовить блюдо! Введи \"Cтарт\" для начала, а дальше следуй инструкциям!");
            return Task.CompletedTask;
        }
    }
}