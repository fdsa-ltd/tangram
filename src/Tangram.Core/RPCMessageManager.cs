using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using Tangram.Core;
using Newtonsoft.Json;
using EmbedIO.WebSockets;
using EmbedIO.WebApi;
using EmbedIO.Routing;
using EmbedIO;
using System.IO;
using EmbedIO.Cors;

namespace Tangram.Core.Event
{
    public class RPCMessageManager
    {

        static RPCMessageManager singlton;


        public static RPCMessageManager External
        {
            get
            {
                if (singlton == null)
                {
                    singlton = new RPCMessageManager();
                }
                return singlton;
            }
        }



        private readonly WebSocketServer websocket = new WebSocketServer("/tg");
        private RPCMessageManager()
        {
            var server = new WebServer(5555);
            server.WithModule(websocket);
            //server.WithModule(new CorsModule("/api"));
            server.WithStaticFolder("/", Path.Combine(Directory.GetCurrentDirectory(), "static"), false);
            server.WithCors();
            //server.WithWebApi("/post", ctx => { ctx.RegisterController<WebApi>(); });
            server.RunAsync();
            server.Start();
        }
        public void RegisterCallback(GlobalEventCallback eventCallback)
        {
            this.websocket.RegisterCallback(eventCallback);
        }
        public string Send(string to, MessageType type, params object[] data)
        {
            var from = DateTime.Now.Ticks.ToString();
            WebMessage message = new WebMessage()
            {
                from = from,
                to = to,
                type = type.ToString(),
                data = data,
            };
            this.websocket.SendToAsync(message.ToString(), to).Wait();

            return string.Empty;

        }
        /// <summary>
        /// Defines a very simple chat server
        /// </summary>
        class WebSocketServer : WebSocketModule
        {
            GlobalEventCallback eventCallback;
            public void RegisterCallback(GlobalEventCallback eventCallback)
            {
                this.eventCallback = eventCallback;
            }
            private Dictionary<string, IWebSocketContext> clients = new Dictionary<string, IWebSocketContext>();
            public WebSocketServer(string urlPath)
                : base(urlPath, true)
            {
                // placeholder
            }
            protected override Task OnMessageReceivedAsync(IWebSocketContext context, byte[] buffer, IWebSocketReceiveResult result)
            {
                if (this.eventCallback != null)
                {
                    var data = Encoding.GetString(buffer);
                    var message = WebMessage.Parse(data);

                    var gm = new GlobalMessage()
                    {
                        From = message.from,
                        To = message.to,
                        Type = Enum.Parse<GlobalMessageType>(message.type, true),
                        Data = message.data,
                    };
                    this.eventCallback(gm);
                }
                return Task.Run(() =>
                {

                });
            }
            object locker = new object();
            protected override Task OnClientConnectedAsync(IWebSocketContext context)
            {
                var title = StringUtil.GetQueryString(context.RequestUri.Query, "title");
                lock (this.locker)
                {
                    if (this.clients.ContainsKey(title))
                    {
                        this.clients[title] = context;
                    }
                    else
                    {
                        this.clients.Add(title, context);
                    }
                }

                return base.OnClientConnectedAsync(context);
            }

            protected override Task OnClientDisconnectedAsync(IWebSocketContext context)
            {
                var title = StringUtil.GetQueryString(context.RequestUri.Query, "title");
                this.clients.Remove(title);
                return SendToOthersAsync(context, title + " lefts");
            }
            public Task SendToAsync(string payload, string name)
            {
                if (this.clients.ContainsKey(name))
                {
                    return SendAsync(this.clients[name], payload);
                }
                return null;
            }
            private Task SendToOthersAsync(IWebSocketContext context, string payload)
                => BroadcastAsync(payload, c => c != context);
        }
        //class WebApi : WebApiController
        //{
        //    [Route(HttpVerbs.Post, "/open")]
        //    public async Task OpenForm()
        //    {
        //        var message = HttpContext.GetRequestDataAsync<WebMessage>().Result;
        //        string url = message.data.GetString(0);
        //        string features = message.data.GetString(1);
        //        var form = ScreenManager.External.Open(url, features);
        //        await HttpContext.SendStringAsync(form.Text, "application/json", Encoding.UTF8);
        //    }
        //    [Route(HttpVerbs.Post, "/get")]
        //    public async Task GetCache()W
        //    {
        //        var message = HttpContext.GetRequestDataAsync<WebMessage>().Result;
        //        var data = CacheHelper.Get(message.data.GetString(0));
        //        if (data != null)
        //        {
        //            await HttpContext.SendStringAsync(data, "application/json", Encoding.UTF8);
        //        }
        //    }
        //}
    }
}