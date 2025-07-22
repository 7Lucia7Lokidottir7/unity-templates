namespace PG.HealthSystem
{
    public interface IDeath
    {
        public void OnDeath();
        public event System.Action dead;
    }
}
