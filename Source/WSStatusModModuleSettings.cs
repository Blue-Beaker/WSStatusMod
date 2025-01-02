namespace Celeste.Mod.WSStatusMod;

public class WSStatusModModuleSettings : EverestModuleSettings {

    [SettingNumberInput(allowNegatives: false, maxLength: 5)]
    public int ListenerPort { get; set; } = 25678;
}