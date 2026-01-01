namespace PG.InteractSystem
{
    public interface IInteractVisible
    {
        public static System.Action<bool> visibleInteracted;
        public static bool isInteractActive { get; set; }
    }
}
