using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;

namespace MyLineBot.Controllers
{
   public class CallbackController : ApiController
  {
      public async Task<HttpResponseMessage> Post()
      {
        var contentString = await Request.Content.ReadAsStringAsync();
 
        dynamic contentObj = JsonConvert.DeserializeObject(contentString);
        var result = contentObj.result[0];

        var client = new HttpClient();
        try
        {
          client.DefaultRequestHeaders
            .Add("X-Line-ChannelID", "");
          client.DefaultRequestHeaders
            .Add("X-Line-ChannelSecret", "");
          client.DefaultRequestHeaders
            .Add("X-Line-Trusted-User-With-ACL", "");
                
               
          var res = await client.PostAsJsonAsync("https://trialbot-api.line.me/v1/events",
              new {
                to = new[] { result.content.from },
                toChannel = "1383378250",
                eventType = "138311608800106203",
                content = new {
                  contentType = 1,
                  toType = 1,
                  text = string.Format("「{0}」って言ったのか？ちょっと何言っているのかわからない", result.content.text)
                }
              });
          
          System.Diagnostics.Debug.WriteLine(await res.Content.ReadAsStringAsync());
          return new HttpResponseMessage(System.Net.HttpStatusCode.OK);
       }
       catch (Exception e)
       {
         System.Diagnostics.Debug.WriteLine(e);
         return new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
       }
    }
  }
}
