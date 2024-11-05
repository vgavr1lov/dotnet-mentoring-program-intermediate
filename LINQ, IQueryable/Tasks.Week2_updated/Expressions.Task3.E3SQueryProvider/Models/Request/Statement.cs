using Newtonsoft.Json;
using System.Collections.Generic;

namespace Expressions.Task3.E3SQueryProvider.Models.Request
{
   [JsonObject]
   public class Statement
   {
      [JsonProperty("query")]
      public string Query { get; set; }

   }
}
