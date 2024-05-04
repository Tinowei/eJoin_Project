namespace Web.ViewModels.HostViewModel
{
    public class ThemeSelectViewModel
    {
        public List<Themes> Theme { get; set; }
    }

    public class Themes
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
    }
}
