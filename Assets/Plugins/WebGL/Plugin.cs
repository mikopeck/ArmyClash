using System.Runtime.InteropServices;
public static class Plugin
{
    [DllImport("__Internal")]
    public static extern void PassText(string text);

    [DllImport("__Internal")]
    public static extern void SetFastData(string text);

    [DllImport("__Internal")]
    public static extern void GetFastData();

    [DllImport("__Internal")]
    public static extern void SetSlowData(string text);

    [DllImport("__Internal")]
    public static extern void GetSlowData();

    [DllImport("__Internal")]
    public static extern void GetKeys();

    [DllImport("__Internal")]
    public static extern void InitMySky();

    [DllImport("__Internal")]
    public static extern void ReloadGame();

    [DllImport("__Internal")]
    public static extern void AddContentRecord(string text);
}
