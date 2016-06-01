using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.FormFlow.Advanced;

namespace BankBot
{
    [Serializable]
    public sealed class Transfer

    {

        public static IForm<Transfer> BuildForm()
        {

    
            return new FormBuilder<Transfer>(true)
                      .Field(new FieldReflector<Transfer>(nameof(recipient))
                            .SetActive((state) => state.recipient == null))
                             .Field(new FieldReflector<Transfer>(nameof(amount))
                            .SetActive((state) => state.amount == null))
                             .Field(new FieldReflector<Transfer>(nameof(date))
                            .SetActive((state) => state.date == null))
                             .Field(new FieldReflector<Transfer>(nameof(schedule))
                            .SetActive((state) =>false))
                .Confirm("Do you want to transfer {amount} to {recipient} on {?{date}} {schedule}?" )                
                .Field(nameof(recipient))
                .Field(nameof(date))
                .Field(nameof(amount))
                .Field(nameof(schedule))
                    .Build();

        }

        
        [Prompt("Who is the recipient. (Person in your contact List or IBAN)")]
        public string recipient { get; set; }
        [Prompt("When do you want to transfer the money)")]
        public DateTime date { get; set; }
        [Prompt("Please specify an amount, including currency eg. 100€")]
        public string amount { get; set; }
        [Prompt("Do you want to repeat this transaction? eg. every month")]
        public string schedule { get; set; }
      
    }
}