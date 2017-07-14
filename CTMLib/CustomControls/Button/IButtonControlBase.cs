namespace CTMCustomControlLib.CustomControls.Button
{
    public interface IButtonControl : ISizeProperty, IColorProperty
    {
        string Text { get; set; }
        bool IsLinkBtn { get; set; }
        string MaterialIcon { get; set; }
        bool IsSubmitBtn { get; set; }
    }
}