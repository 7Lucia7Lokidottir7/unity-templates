namespace PG.HungerSystem
{
    public interface IHungry
    {
        public event System.Action<float> hungred;
        public void Hungry(float hungryValue);
    }
}
