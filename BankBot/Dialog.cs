using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

using System.Configuration;
using BankBot;
using Microsoft.Bot.Builder.Luis.Models;

namespace BankBot
{
    [Serializable]
    public class Dialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }

        const string entity_Recipient = "recipient";
        const string entity_Set = "builtin.datetime.set";
        const string entity_Date = "builtin.datetime.date";
        const string entity_Money = "builtin.money";

        const string intent_transfer = "transfer";
        const string intent_transfer_order = "transfer_order";
        const string intent_hi = "hi";

        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<Message> argument)
        {
            var message = await argument;
           
            string resp = "";
            BankBot.LUISModel model = await GetEntityFromLUIS(message.Text);
          
            if (model.intents.Count() > 0)
            {
                switch (model.intents[0].intent)
                {
                    case intent_hi:

                        await context.PostAsync("Hallo! Versuch mal: Where is the nearest police station?");
                        await context.PostAsync("Oder: Can I park here?");
                        await context.PostAsync("Oder: I was robbed!");
                        context.Wait(MessageReceivedAsync);
                        break;
                    case intent_transfer:
                     
                        Transfer transfer = new Transfer();
                        List<EntityRecommendation> entities = new List<EntityRecommendation>();

                        if (model.entities.Count() > 0)
                        {
                           //get recipient
                            Entity entity = model.entities.FirstOrDefault(e => e.type == entity_Recipient);
                            if (entity != null) {
                                entities.Add(new EntityRecommendation(null, entity.entity, entity.type, entity.startIndex, entity.endIndex, entity.score, null));
                            }
                            //get date
                            Entity date = model.entities.FirstOrDefault(e => e.type == entity_Date);
                            if (date != null)
                            {
                                entities.Add(new EntityRecommendation(null, date.entity, "TransferDate", date.startIndex, date.endIndex, date.score, null));
                            }
                            //get money
                            Entity money = model.entities.FirstOrDefault(e => e.type == entity_Money);
                            if (money != null)
                            {
                                entities.Add(new EntityRecommendation(null, money.entity, "amount", money.startIndex, money.endIndex, money.score, null));
                            }
                        }
                        IFormDialog<Transfer> tmp = MakeRootDialog(transfer, entities: entities);
                        context.Call(tmp, TransferComplete);
                        break;
                    case "transfer_order":
                      
                     
                        break;
                  
                    default:
                        PostAndWait(context, "Das habe ich leider nicht verstanden");
                        break;
                }
            }
            else
            {
                PostAndWait(context, "Das habe ich leider nicht verstanden");
            }

        }

        private async void PostAndWait(IDialogContext context, string resp)
        {

            await context.PostAsync(resp);

            context.Wait(MessageReceivedAsync);
        }
        private async void PostAndWait(IDialogContext context, Message resp)
        {

            await context.PostAsync(resp);

            context.Wait(MessageReceivedAsync);
        }
        
        private async Task TransferComplete(IDialogContext context, IAwaitable<Transfer> result)
        {
            await context.PostAsync("TAN was sent. Enter here: ");
            context.Wait(MessageReceivedAsync);
        }

        internal static IFormDialog<Transfer> MakeRootDialog(Transfer transfer, List<EntityRecommendation> entities)
        {
            return new FormDialog<Transfer>(new Transfer(), Transfer.BuildForm , options: FormOptions.PromptInStart, entities: entities );
        }

        private static async Task<LUISModel> GetEntityFromLUIS(string Query)
        {
            Query = Uri.EscapeDataString(Query);
            LUISModel Data = new LUISModel();
            using (HttpClient client = new HttpClient())
            {
                string RequestURI = "https://api.projectoxford.ai/luis/v1/application?id=fe37f798-e736-47bd-86d1-a20e7ffe19a4&subscription-key=40a189ba4f8a4824b4e3371ca059a82b&q=" + Query;
                HttpResponseMessage msg = await client.GetAsync(RequestURI);

                if (msg.IsSuccessStatusCode)
                {
                    var JsonDataResponse = await msg.Content.ReadAsStringAsync();
                    Data = JsonConvert.DeserializeObject<LUISModel>(JsonDataResponse);
                }
            }
            return Data;
        }
    }
}