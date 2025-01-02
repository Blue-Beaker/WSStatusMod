
using System;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace Celeste.Mod.DashSocketMod;

public class WSServer {
    public static ClientWebSocket socket;
    public static async Task open(){
        try
        {
            Uri uri = new("ws://localhost:"+DashSocketModModule.Settings.ListenerPort);
            socket = new();
            await socket.ConnectAsync(uri, default);
        }
        catch (System.Exception e)
        {
            Logger.Error("DashSocketMod","When opening: "+e.ToString());
        }
    }
    public static async void send(string value){
        try
        {
            if(socket==null || (socket.State!=WebSocketState.Open && socket.State!=WebSocketState.Connecting)){
                await open();
            }
            if(socket.State==WebSocketState.Open){
                byte[] bytes = Encoding.UTF8.GetBytes(value);
                await socket.SendAsync(bytes,WebSocketMessageType.Text,true,default);
            }
        }
        catch (System.Exception e)
        {
            Logger.Error("DashSocketMod","When sending: "+e.ToString());
            await socket.CloseAsync(WebSocketCloseStatus.EndpointUnavailable,"Closed",default);
            socket=null;
        }
    }
    public static void close(){
        try
        {
            socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Client closed", default);
            socket=null;
        }
        catch (System.Exception e)
        {
            Logger.Error("DashSocketMod","When closing: "+e.ToString());
        }
    }
}