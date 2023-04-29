namespace Completed
{
    public class FoodManager : MonoBehaviourSingleton<FoodManager>
    {
        private FoodManager()
        {
            CurrentFood = new Food(GameManager.Instance.Config.PlayerFoodPoints);
        }

        public Food CurrentFood { get; private set; }
    }
}