public interface IAnimalStatus
{
    void DecreaseHydration();
    void IncreaseHydration();
    void DecreaseFullness();
    void ModifyHuntingRadius(float multiplier);
    bool IsPredator();
}
