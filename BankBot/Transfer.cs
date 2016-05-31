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
                    .Build();
        }
        public string recipient { get; set; }
        public DateTime TransferDate { get; set; }

        public string amount { get; set; }

      
    }
}