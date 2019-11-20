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

namespace Tangram.Core
{
    public class WebMessageServer
    {
        private readonly WebSocketServer websocket = new WebSocketServer("/tg");
        public WebMessageServer()
        {
            var server = new WebServer(5555);
            server.WithModule(websocket);
            //server.WithModule(new CorsModule("/api"));
            server.WithStaticFolder("/static", Path.Combine(Directory.GetCurrentDirectory(), "static"), false);
            server.WithCors();
            server.WithWebApi("/post", ctx => { ctx.RegisterController<WebApi>(); });
            server.RunAsync();
            server.Start();
        }
        public Task SendToAsync(string payload, string name)
        {
            return this.websocket.SendToAsync(payload, name);
        }  /// <summary>
           /// Defines a very simple chat server
           /// </summary>
        class WebSocketServer : WebSocketModule
        {
            private Dictionary<string, IWebSocketContext> clients = new Dictionary<string, IWebSocketContext>();
            public WebSocketServer(string urlPath)
                : base(urlPath, true)
            {
                // placeholder
            }
            protected override Task OnMessageReceivedAsync(IWebSocketContext context, byte[] buffer, IWebSocketReceiveResult result)
            {
                var data = Encoding.GetString(buffer);
                var message = JsonConvert.DeserializeObject<WebMessage>(data);

                switch (message.type)
                {
                    case "show":
                        ScreenManager.External.Invoke(message.from, "show", message.data.Select(m => m.ToString()).ToArray());
                        break;
                    case "hide":
                        ScreenManager.External.Invoke(message.from, "hide");
                        break;
                    case "close":
                        ScreenManager.External.Invoke(message.from, "close");
                        break;
                    case "refresh":
                        ScreenManager.External.Invoke(message.from, "refresh", message.data.Select(m => m.ToString()).ToArray());
                        break;
                    case "position":
                        ScreenManager.External.Invoke(message.from, "position", message.data.Select(m => m.ToString()).ToArray());
                        break;
                    case "size":
                        ScreenManager.External.Invoke(message.from, "size", message.data.Select(m => m.ToString()).ToArray());
                        break;
                    case "mode":
                        ScreenManager.External.Invoke(message.from, "mode", message.data.Select(m => m.ToString()).ToArray());
                        break;
                    case "exec":
                        var script = message.data.GetString(0);
                        ScreenManager.External.Exec(message.from, script);
                        break;
                    case "set":
                        var key = message.data.GetString(0);
                        var value = message.data.GetString(1);
                        var timeout = message.data.GetInt(2);
                        if (timeout == 0)
                        {
                            CacheHelper.Set(key, value);
                        }
                        else
                        {
                            CacheHelper.Set(key, value, timeout);
                        }
                        break;
                    default:
                        break;
                }
                return Task.Run(() => { });
            }

            protected override Task OnClientConnectedAsync(IWebSocketContext context)
            {
                var title = StringUtil.GetQueryString(context.RequestUri.Query, "title");
                if (this.clients.ContainsKey(title))
                {
                    this.clients[title] = context;
                }
                else
                {
                    this.clients.Add(title, context);
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
        class WebApi : WebApiController
        {
            [Route(HttpVerbs.Post, "/open")]
            public async Task OpenForm()
            {
                var message = HttpContext.GetRequestDataAsync<WebMessage>().Result;
                string url = message.data.GetString(0);
                string features = message.data.GetString(1);

                var form = ScreenManager.External.Open(url, features);
                await HttpContext.SendStringAsync(form.Text, "application/json", Encoding.UTF8);
            }
            [Route(HttpVerbs.Post, "/get")]
            public async Task GetCache()
            {
                var message = HttpContext.GetRequestDataAsync<WebMessage>().Result;
                var data = CacheHelper.Get(message.data.GetString(0));
                if (data != null)
                {
                    await HttpContext.SendStringAsync(data, "application/json", Encoding.UTF8);
                }
            }
        }
    }
}