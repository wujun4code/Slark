using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LeanCloud;
using Slark.Core;
using Slark.Core.Utils;

namespace TheMessage
{
    public class TMClient : SlarkClient
    {
        public TMClient()
        {

        }

        public EventHandler<TMJsonResponse> OnResponse { get; set; }

        public AVUser User { get; set; }

        public Task<TMJsonResponse> SendAsync(TMJsonRequest request, bool ignoreAck = false, int timeLimitForDefaultResult = 3000)
        {
            try
            {
                var jsonObj = TMEncoding.Instance.Serialize(request);
                var json = TMEncoding.Instance.Deserialize(jsonObj) as Dictionary<string, object>;
                var tcs = new TaskCompletionSource<TMJsonResponse>();
                var qId = StringRandom.RandomHexString(8);
                json["si"] = qId;

                var jsonStringText = TMEncoding.Instance.Serialize(json);

                EventHandler<TMJsonResponse> onResponse = null;

                onResponse = (sender, response) =>
                {
                    if (response.CommandId == qId)
                    {
                        if (!ignoreAck || response.Results != null)
                        {
                            OnResponse -= onResponse;
                            if (tcs.Task.IsCanceled || tcs.Task.IsCompleted)
                            {
                                return;
                            }
                            tcs.SetResult(response);
                        }
                    }
                };

                this.SendAsync(jsonStringText).Wait();
                OnResponse += onResponse;
                tcs.SetDefaultResult(new TMJsonResponse(), timeLimitForDefaultResult, () =>
                {
                    OnResponse -= onResponse;
                });
                return tcs.Task;
            }
            catch (TaskCanceledException tce)
            {
                return Task.FromResult(new TMJsonResponse());
            }

        }
    }
}
