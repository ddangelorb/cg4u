namespace CG4U.Donate.ClientApp.Med.Services.Root
{
    public class RootEntity<T> where T : RootEntity<T>
    {
        public int Id { get; set; }
        public int Active { get; set; }
    }
}
