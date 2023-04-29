using UnityEngine;
using System.Collections;
using UnityEngine.UI; //Allows us to use UI.
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Completed
{
    //Player inherits from MovingObject, our base class for objects that can move, Enemy also inherits from this.
    public class Player : MovingObject
    {
        [SerializeField] private float _restartLevelDelay = 1f; //Delay time in seconds to restart level.
        [SerializeField] private int _pointsPerFood = 10; //Number of points to add to player food points when picking up a food object.
        [SerializeField] private int _pointsPerSoda = 20; //Number of points to add to player food points when picking up a soda object.
        [SerializeField] private int _wallDamage = 1; //How much damage a player does to a wall when chopping it.
        [SerializeField] private Text _foodText; //UI Text to display current player food total.
        [SerializeField] private AudioClip _moveSound1; //1 of 2 Audio clips to play when player moves.
        [SerializeField] private AudioClip _moveSound2; //2 of 2 Audio clips to play when player moves.
        [SerializeField] private AudioClip _eatSound1; //1 of 2 Audio clips to play when player collects a food object.
        [SerializeField] private AudioClip _eatSound2; //2 of 2 Audio clips to play when player collects a food object.
        [SerializeField] private AudioClip _drinkSound1; //1 of 2 Audio clips to play when player collects a soda object.
        [SerializeField] private AudioClip _drinkSound2; //2 of 2 Audio clips to play when player collects a soda object.

        [FormerlySerializedAs("gameOverSound")]
        public AudioClip _gameOverSound; //Audio clip to play when player dies.

        private IGetInput _input;
        private ICharacterAnimations _characterAnimations;

        public void SetInput(IGetInput input)
        {
            _input = input;
        }
        
        //LoseFood is called when an enemy attacks the player.
        //It takes a parameter loss which specifies how many points to lose.
        public void LoseFood(int loss)
        {
            //Set the trigger for the player animator to transition to the playerHit animation.
            _characterAnimations.SetPlayerHit();

            //Subtract lost food points from the players total.
            FoodManager.Instance.CurrentFood.Remove(loss);

            //Update the food display with the new total.
            DrawFood(-loss, FoodManager.Instance.CurrentFood.Amount);

            //Check to see if game has ended.
            CheckIfGameOver();
        }
        
        //Start overrides the Start function of MovingObject
        protected override void Start()
        {
            //Get a component reference to the Player's animator component
            _characterAnimations = new PlayerAnimations(GetComponent<Animator>());

            //Set the foodText to reflect the current player food total.
            DrawCurrentFood();

            //Call the Start function of the MovingObject base class.
            base.Start();
        }

        private void Update()
        {
            if (!GameManager.Instance._playersTurn) return;

            var input = _input.GetInput();
            var horizontal = input.x;
            var vertical = input.y;

            if (horizontal != 0) vertical = 0;
            
            if (horizontal != 0 || vertical != 0)
                AttemptMove<Wall>(horizontal, vertical);
        }

        //AttemptMove overrides the AttemptMove function in the base class MovingObject
        //AttemptMove takes a generic parameter T which for Player will be of the type Wall, it also takes integers for x and y direction to move in.
        protected override void AttemptMove<T>(int xDir, int yDir)
        {
            //Every time player moves, subtract from food points total.
            FoodManager.Instance.CurrentFood.Remove(1);

            //Update food text display to reflect current score.
            DrawCurrentFood();

            //Call the AttemptMove method of the base class, passing in the component T (in this case Wall) and x and y direction to move.
            base.AttemptMove<T>(xDir, yDir);

            //Hit allows us to reference the result of the Linecast done in Move.
            RaycastHit2D hit;

            //If Move returns true, meaning Player was able to move into an empty space.
            if (Move(xDir, yDir, out hit))
                //Call RandomizeSfx of SoundManager to play the move sound, passing in two audio clips to choose from.
                SoundManager.Instance.RandomizeSfx(_moveSound1, _moveSound2);

            //Since the player has moved and lost food points, check if the game has ended.
            CheckIfGameOver();

            //Set the playersTurn boolean of GameManager to false now that players turn is over.
            GameManager.Instance._playersTurn = false;
        }

        private void DrawCurrentFood()
        {
            _foodText.text = "Food: " + FoodManager.Instance.CurrentFood.Amount;
        }


        //OnCantMove overrides the abstract function OnCantMove in MovingObject.
        //It takes a generic parameter T which in the case of Player is a Wall which the player can attack and destroy.
        protected override void OnCantMove<T>(T component)
        {
            //Set hitWall to equal the component passed in as a parameter.
            var hitWall = component as Wall;

            //Call the DamageWall function of the Wall we are hitting.
            hitWall.DamageWall(_wallDamage);

            //Set the attack trigger of the player's animation controller in order to play the player's attack animation.
            _characterAnimations.SetAttack();
        }


        //OnTriggerEnter2D is sent when another object enters a trigger collider attached to this object (2D physics only).
        private void OnTriggerEnter2D(Collider2D other)
        {
            //Check if the tag of the trigger collided with is Exit.
            if (other.tag == "Exit")
            {
                //Invoke the Restart function to start the next level with a delay of restartLevelDelay (default 1 second).
                Invoke("Restart", _restartLevelDelay);

                //Disable the player object since level is over.
                enabled = false;
            }

            //Check if the tag of the trigger collided with is Food.
            else if (other.tag == "Food")
            {
                //Add pointsPerFood to the players current food total.
                FoodManager.Instance.CurrentFood.Add(_pointsPerFood);

                //Update foodText to represent current total and notify player that they gained points
                DrawFood(_pointsPerFood, FoodManager.Instance.CurrentFood.Amount);

                //Call the RandomizeSfx function of SoundManager and pass in two eating sounds to choose between to play the eating sound effect.
                SoundManager.Instance.RandomizeSfx(_eatSound1, _eatSound2);

                //Disable the food object the player collided with.
                other.gameObject.SetActive(false);
            }

            //Check if the tag of the trigger collided with is Soda.
            else if (other.tag == "Soda")
            {
                //Add pointsPerSoda to players food points total
                FoodManager.Instance.CurrentFood.Add(_pointsPerSoda);

                //Update foodText to represent current total and notify player that they gained points
                DrawFood(_pointsPerSoda, FoodManager.Instance.CurrentFood.Amount);


                //Call the RandomizeSfx function of SoundManager and pass in two drinking sounds to choose between to play the drinking sound effect.
                SoundManager.Instance.RandomizeSfx(_drinkSound1, _drinkSound2);

                //Disable the soda object the player collided with.
                other.gameObject.SetActive(false);
            }
        }

        private void DrawFood(int amountAdded, int current)
        {
            if (amountAdded < 0)
            {
                _foodText.text += "-";
            }
            else
            {
                _foodText.text += "+";
            }
            
            _foodText.text += (amountAdded + " Food: " + current);
        }


        //Restart reloads the scene when called.
        private void Restart()
        {
            //Load the last scene loaded, in this case Main, the only scene in the game. And we load it in "Single" mode so it replace the existing one
            //and not load all the scene object in the current scene.
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
        }


        //CheckIfGameOver checks if the player is out of food points and if so, ends the game.
        private void CheckIfGameOver()
        {
            //Check if food point total is less than or equal to zero.
            if (FoodManager.Instance.CurrentFood.Amount <= 0)
            {
                //Call the PlaySingle function of SoundManager and pass it the gameOverSound as the audio clip to play.
                SoundManager.Instance.PlaySingle(_gameOverSound);

                //Stop the background music.
                SoundManager.Instance._musicSource.Stop();

                //Call the GameOver function of GameManager.
                GameManager.Instance.GameOver();
            }
        }
    }
}