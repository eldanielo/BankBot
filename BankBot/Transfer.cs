using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Builder.FormFlow;

namespace BankBot
{
    [Serializable]
    public sealed class Transfer

    {

        public static IForm<Transfer> BuildForm()
        {


            return new FormBuilder<Transfer>(true)
                    .Message("Um Ihren Fall bestmöglich behandeln zu können geben Sie uns bitte folgende Informationen")
                    .Build();
        }
        public string recipient { get; set; }
        public DateTime TransferDate { get; set; }

      
    }
}