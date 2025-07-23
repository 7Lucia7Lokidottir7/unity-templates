namespace PG.HealthSystem
{
    public interface IHealable
    {
        public event System.Action<float> healed;
        public void OnHeal(float value);
    }
}
