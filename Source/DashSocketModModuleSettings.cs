namespace Celeste.Mod.DashSocketMod;

public class DashSocketModModuleSettings : EverestModuleSettings {

    [SettingNumberInput(allowNegatives: false, maxLength: 5)]
    public int ListenerPort { get; set; } = 25678;
}