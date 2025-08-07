namespace PG.HungerSystem
{
    public interface IEating
    {
        public event System.Action<float> eated;
        public void Eat(float eatValue);
    }
}
