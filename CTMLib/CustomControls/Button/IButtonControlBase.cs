namespace CTMLib.CustomControls.Button
{
    public interface IButtonControlBase : ISizeOptions, IColorOptions
    {
        string Text { get; set; }
        bool IsLinkBtn { get; set; }
        string MaterialIcon { get; set; }
        bool IsSubmitBtn { get; set; }
    }
}