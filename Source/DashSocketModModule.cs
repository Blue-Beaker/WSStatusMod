using System;
using System.Net.WebSockets;

namespace Celeste.Mod.DashSocketMod;

public class DashSocketModModule : EverestModule {
    public static DashSocketModModule Instance { get; private set; }

    public override Type SettingsType => typeof(DashSocketModModuleSettings);
    public static DashSocketModModuleSettings Settings => (DashSocketModModuleSettings) Instance._Settings;

    public override Type SessionType => typeof(DashSocketModModuleSession);
    public static DashSocketModModuleSession Session => (DashSocketModModuleSession) Instance._Session;

    public override Type SaveDataType => typeof(DashSocketModModuleSaveData);
    public static DashSocketModModuleSaveData SaveData => (DashSocketModModuleSaveData) Instance._SaveData;

    public DashSocketModModule() {
        Instance = this;
#if DEBUG
        // debug builds use verbose logging
        Logger.SetLogLevel(nameof(DashSocketModModule), LogLevel.Verbose);
#else
        // release builds use info logging to reduce spam in log files
        Logger.SetLogLevel(nameof(DashSocketModModule), LogLevel.Info);
#endif
    }

    public override void Load() {

        On.Celeste.Player.RefillDash += Player_RefillDash;
        On.Celeste.Player.DashBegin += Player_DashBegin;
        On.Celeste.Player.DashUpdate += Player_DashUpdate;
        WSClient.Open();
    }

    public override void Unload() {
        
        On.Celeste.Player.RefillDash -= Player_RefillDash;
        On.Celeste.Player.DashBegin -= Player_DashBegin;
        On.Celeste.Player.DashUpdate -= Player_DashUpdate;
        WSClient.Close();
    }

    private static void Player_DashBegin(On.Celeste.Player.orig_DashBegin orig, Player self) {
                UpdateDashes(self.Dashes);
                // We can call the original method at any point in the hook.
                orig(self);
            }
    private static bool Player_RefillDash(On.Celeste.Player.orig_RefillDash orig, Player self) {
                UpdateDashes(self.Dashes);
                // We can call the original method at any point in the hook.
                return orig(self);
            }
    private static int Player_DashUpdate(On.Celeste.Player.orig_DashUpdate orig, Player self) {
                UpdateDashes(self.Dashes);
                // We can call the original method at any point in the hook.
                return orig(self);
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