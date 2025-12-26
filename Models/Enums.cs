namespace MKTL.WPF.Models
{
    public enum AppType
    {
        Game,
        DLC,
        Tool
    }

    public enum GenerationMode
    {
        Goldberg,
        Coldloader
    }

    public enum PatcherStatus
    {
        Idle,
        Downloading,
        Extracting,
        Finished,
        Error
    }
}