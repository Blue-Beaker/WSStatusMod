using System;
using System.Net.WebSockets;

namespace Celeste.Mod.WSStatusMod;

public class WSStatusModModule : EverestModule {
    public static WSStatusModModule Instance { get; private set; }

    public override Type SettingsType => typeof(WSStatusModModuleSettings);
    public static WSStatusModModuleSettings Settings => (WSStatusModModuleSettings) Instance._Settings;

    public override Type SessionType => typeof(WSStatusModModuleSession);
    public static WSStatusModModuleSession Session => (WSStatusModModuleSession) Instance._Session;

    public override Type SaveDataType => typeof(WSStatusModModuleSaveData);
    public static WSStatusModModuleSaveData SaveData => (WSStatusModModuleSaveData) Instance._SaveData;

    public WSStatusModModule() {
        Instance = this;
#if DEBUG
        // debug builds use verbose logging
        Logger.SetLogLevel(nameof(WSStatusModModule), LogLevel.Verbose);
#else
        // release builds use info logging to reduce spam in log files
        Logger.SetLogLevel(nameof(WSStatusModModule), LogLevel.Info);
#endif
    }

    public override void Load() {

        // On.Celeste.Player.RefillDash += Player_RefillDash;
        // On.Celeste.Player.DashBegin += Player_DashBegin;
        On.Celeste.Player.Update += Player_Update;
        WSClient.Open();
    }

    public override void Unload() {
        
        // On.Celeste.Player.RefillDash -= Player_RefillDash;
        // On.Celeste.Player.DashBegin -= Player_DashBegin;
        On.Celeste.Player.Update -= Player_Update;
        WSClient.Close();
    }

    // private static void Player_DashBegin(On.Celeste.Player.orig_DashBegin orig, Player self) {
    //             UpdateDashes(self.Dashes);
    //             // We can call the original method at any point in the hook.
    //             orig(self);
    //         }
    // private static bool Player_RefillDash(On.Celeste.Player.orig_RefillDash orig, Player self) {
    //             UpdateDashes(self.Dashes);
    //             // We can call the original method at any point in the hook.
    //             return orig(self);
    //         }
    private static void Player_Update(On.Celeste.Player.orig_Update orig, Player self) {
                UpdateDashes(self.Dashes);
                // We can call the original method at any point in the hook.
                orig(self);
            }

    static int last_dashes = 0;
    private static void UpdateDashes(int dashes){
        if(dashes==last_dashes){
            return;
        }
        last_dashes=dashes;
        Logger.Log(LogLevel.Debug,"Dashes: ", ""+dashes);
        WSClient.Send("dash="+dashes);
    }

}