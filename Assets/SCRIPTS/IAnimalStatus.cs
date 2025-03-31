public interface IAnimalStatus
{
    void DecreaseHydration();
    void IncreaseHydration();
    void DecreaseFullness();
    void IncreaseFullness(); 
    void ModifyHuntingRadius(float multiplier);
    bool IsPredator();
}
