
using System;
using System.IO;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace Celeste.Mod.DashSocketMod;

public class WSClient {
    public static ClientWebSocket socket;
    public static async Task Open(){
        try
        {
            Uri uri = new("ws://localhost:"+DashSocketModModule.Settings.ListenerPort);
            socket = new();
            await socket.ConnectAsync(uri, default);
        }
        catch (System.Exception e)
        {
            Logger.Error("DashSocketMod","When opening: "+e.ToString());
            await Close();
        }
    }
    public static async void Send(string value){
        try
        {
            if(socket==null){
                await WSClient.Open();
            }
            if(socket!=null && socket.State==WebSocketState.Open){
                byte[] bytes = Encoding.UTF8.GetBytes(value);
                await socket.SendAsync(bytes,WebSocketMessageType.Text,true,default);
            }
        }
        catch (System.Exception e)
        {
            Logger.Error("DashSocketMod","When sending: "+e.ToString());
            await Close();
        }
    }
    public static async Task Close(){
        try
        {
            await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Client closed", default);
            socket.Dispose();
            socket=null;
        }
        catch (System.Exception e)
        {
            Logger.Error("DashSocketMod","When closing: "+e.ToString());
            socket.Dispose();
            socket=null;
        }
    }
}