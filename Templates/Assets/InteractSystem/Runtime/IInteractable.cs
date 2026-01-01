namespace PG.InteractSystem
{
    public interface IInteractable
    {
        public void OnInteract();
        public event System.Action interacted;
    }
}
