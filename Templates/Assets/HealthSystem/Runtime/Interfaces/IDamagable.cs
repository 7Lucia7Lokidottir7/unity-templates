namespace PG.HealthSystem
{
    public interface IDamagable
    {
        public event System.Action<float> damaged;
        public void OnDamage(float damage, bool ignoreDamage = false);
    }
}
