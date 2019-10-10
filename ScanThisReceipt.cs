using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DB.Hackathon
{
    public static class ScanThisReceipt
    {
        [FunctionName("ScanThisReceipt")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var client = new HttpClient();
            var subscriptionKey= "0999134924c6480aa94df1ccd0c4568e§";
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
            var queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);
            HttpResponseMessage response;

            // Request headers
            var uri = "https://uksouth.api.cognitive.microsoft.com/vision/v1.0/ocr?language=unk&detectOrientation=true†";
            HttpRequestMessage requestMessage=new HttpRequestMessage(HttpMethod.Post,uri);
            requestMessage.Content=new StringContent("{\"url\":\"https://res.cloudinary.com/skillsmatter/image/upload/c_fill,w_300,h_300,g_north_west/v1548242609/gao7av1dbcjfhup11qyd.png\"}", Encoding.UTF8, "application/json");
            //https://res.cloudinary.com/skillsmatter/image/upload/c_fill,w_300,h_300,g_north_west/v1548242609/gao7av1dbcjfhup11qyd.png
            //https://products2.imgix.drizly.com/ci-hendricks-gin-7775cb59d3b3e755.jpeg?auto=format%2Ccompress&fm=jpeg&q=20
            response=await client.PostAsync(uri,requestMessage.Content);
            var responseResult=response.Content.ReadAsStringAsync().Result;
            JObject objectJson=JObject.Parse(responseResult);
            objectGuess=(string)objectJson["regions"][0]["lines"][0]["words"][0]["text"];
            return (ActionResult)new OkObjectResult(objectGuess);
        }
    }
}
