namespace SharpCanvas.Host.Browser
{
    public delegate void OnSaveFileHandler(byte[] data);

    public interface ILocation
    {
        event OnSaveFileHandler OnSaveFile;
        object hash { get; set; }
        object host { get; set; }
        object hostname { get; set; }
        string href { get; set; }
        object pathname { get; set; }
        object port { get; set; }
        object protocol { get; set; }
        object search { get; set; }
    }
}