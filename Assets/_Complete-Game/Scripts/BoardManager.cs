using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Serialization; //Allows us to use Lists.
using Random = UnityEngine.Random; //Tells Random to use the Unity Engine random number generator.

namespace Completed

{
    public class BoardManager : MonoBehaviour
    {
        // Using Serializable allows us to embed a class with sub properties in the inspector.
        [Serializable]
        public class Count
        {
            [FormerlySerializedAs("minimum")] public int _minimum; //Minimum value for our Count class.
            [FormerlySerializedAs("maximum")] public int _maximum; //Maximum value for our Count class.


            //Assignment constructor.
            public Count(int min, int max)
            {
                _minimum = min;
                _maximum = max;
            }
        }


        [FormerlySerializedAs("columns")] public int _columns = 8; //Number of columns in our game board.
        [FormerlySerializedAs("rows")] public int _rows = 8; //Number of rows in our game board.
        [FormerlySerializedAs("wallCount")] public BoardManager.Count _wallCount = new(5, 9); //Lower and upper limit for our random number of walls per level.
        [FormerlySerializedAs("foodCount")] public BoardManager.Count _foodCount = new(1, 5); //Lower and upper limit for our random number of food items per level.
        [FormerlySerializedAs("exit")] public GameObject _exit; //Prefab to spawn for exit.
        [FormerlySerializedAs("floorTiles")] public GameObject[] _floorTiles; //Array of floor prefabs.
        [FormerlySerializedAs("wallTiles")] public GameObject[] _wallTiles; //Array of wall prefabs.
        [FormerlySerializedAs("foodTiles")] public GameObject[] _foodTiles; //Array of food prefabs.
        [FormerlySerializedAs("enemyTiles")] public GameObject[] _enemyTiles; //Array of enemy prefabs.
        [FormerlySerializedAs("outerWallTiles")] public GameObject[] _outerWallTiles; //Array of outer tile prefabs.

        private Transform _boardHolder; //A variable to store a reference to the transform of our Board object.
        private List<Vector3> _gridPositions = new(); //A list of possible locations to place tiles.


        //Clears our list gridPositions and prepares it to generate a new board.
        private void InitialiseList()
        {
            //Clear our list gridPositions.
            _gridPositions.Clear();

            //Loop through x axis (columns).
            for (var x = 1; x < _columns - 1; x++)
                //Within each column, loop through y axis (rows).
            for (var y = 1; y < _rows - 1; y++)
                //At each index add a new Vector3 to our list with the x and y coordinates of that position.
                _gridPositions.Add(new Vector3(x, y, 0f));
        }


        private void BoardSetup()
        {
            _boardHolder = new GameObject("Board").transform;

            for (var x = -1; x < _columns + 1; x++)
            for (var y = -1; y < _rows + 1; y++)
            {
                // var toInstantiate = CollectionExtensions.GetRandomElement(_floorTiles);
                var toInstantiate = _floorTiles.GetRandomElement();

                if (x == -1 || x == _columns || y == -1 || y == _rows)
                    toInstantiate = _outerWallTiles.GetRandomElement();;

                var instance =
                    Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

                instance.transform.SetParent(_boardHolder);
            }
        }


        //RandomPosition returns a random position from our list gridPositions.
        private Vector3 RandomPosition()
        {
            //Declare an integer randomIndex, set it's value to a random number between 0 and the count of items in our List gridPositions.
            var randomIndex = Random.Range(0, _gridPositions.Count);

            //Declare a variable of type Vector3 called randomPosition, set it's value to the entry at randomIndex from our List gridPositions.
            var randomPosition = _gridPositions[randomIndex];

            //Remove the entry at randomIndex from the list so that it can't be re-used.
            _gridPositions.RemoveAt(randomIndex);

            //Return the randomly selected Vector3 position.
            return randomPosition;
        }


        //LayoutObjectAtRandom accepts an array of game objects to choose from along with a minimum and maximum range for the number of objects to create.
        private void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
        {
            //Choose a random number of objects to instantiate within the minimum and maximum limits
            var objectCount = Random.Range(minimum, maximum + 1);

            //Instantiate objects until the randomly chosen limit objectCount is reached
            for (var i = 0; i < objectCount; i++)
            {
                //Choose a position for randomPosition by getting a random position from our list of available Vector3s stored in gridPosition
                var randomPosition = RandomPosition();

                //Choose a random tile from tileArray and assign it to tileChoice
                var tileChoice = tileArray.GetRandomElement();

                //Instantiate tileChoice at the position returned by RandomPosition with no change in rotation
                Instantiate(tileChoice, randomPosition, Quaternion.identity);
            }
        }


        //SetupScene initializes our level and calls the previous functions to lay out the game board
        public void SetupScene(int level)
        {
            //Creates the outer walls and floor.
            BoardSetup();

            //Reset our list of gridpositions.
            InitialiseList();

            //Instantiate a random number of wall tiles based on minimum and maximum, at randomized positions.
            LayoutObjectAtRandom(_wallTiles, _wallCount._minimum, _wallCount._maximum);

            //Instantiate a random number of food tiles based on minimum and maximum, at randomized positions.
            LayoutObjectAtRandom(_foodTiles, _foodCount._minimum, _foodCount._maximum);

            //Determine number of enemies based on current level number, based on a logarithmic progression
            var enemyCount = (int)Mathf.Log(level, 2f);

            //Instantiate a random number of enemies based on minimum and maximum, at randomized positions.
            LayoutObjectAtRandom(_enemyTiles, enemyCount, enemyCount);

            //Instantiate the exit tile in the upper right hand corner of our game board
            Instantiate(_exit, new Vector3(_columns - 1, _rows - 1, 0f), Quaternion.identity);
        }
    }
}