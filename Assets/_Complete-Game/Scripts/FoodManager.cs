namespace Completed
{
    public class FoodManager
    {
        public static FoodManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new FoodManager();
                }

                return _instance;
            }
        }
        
        private static FoodManager _instance;

        private FoodManager(){}

        private Food _storedFood;

        public void Save(Food food)
        {
            _storedFood = food;
        }
        
        public Food Get()
        {
            return _storedFood;
        }
    }
}