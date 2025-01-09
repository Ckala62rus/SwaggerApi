using Microsoft.AspNetCore.Mvc;
using PusherServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Architecture.Helpers
{
    public class Channel
    {
        public static async Task<IActionResult> Trigger(object data, string channelName, string eventName)
        {
            var options = new PusherOptions
            {
                Cluster = "eu", // cluster
                Encrypted = true
            };

            var pusher = new Pusher(
              "1615798", // id
              "748638b96ea26212abeb", // key
              "d2146c9dd41e9d62d315", // secret
              options
            );

            var result = await pusher.TriggerAsync(
              channelName,
              eventName,
              data
            );

            return new OkObjectResult(data);
        }
    }
}
