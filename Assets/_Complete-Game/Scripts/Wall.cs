using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;

namespace Completed
{
    public class Wall : MonoBehaviour
    {
        [FormerlySerializedAs("chopSound1")] public AudioClip _chopSound1; //1 of 2 audio clips that play when the wall is attacked by the player.
        [FormerlySerializedAs("chopSound2")] public AudioClip _chopSound2; //2 of 2 audio clips that play when the wall is attacked by the player.
        [FormerlySerializedAs("dmgSprite")] public Sprite _dmgSprite; //Alternate sprite to display after Wall has been attacked by player.
        [FormerlySerializedAs("hp")] public int _hp = 3; //hit points for the wall.


        private SpriteRenderer _spriteRenderer; //Store a component reference to the attached SpriteRenderer.


        private void Awake()
        {
            //Get a component reference to the SpriteRenderer.
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }


        //DamageWall is called when the player attacks a wall.
        public void DamageWall(int loss)
        {
            //Call the RandomizeSfx function of SoundManager to play one of two chop sounds.
            SoundManager.Instance.RandomizeSfx(_chopSound1, _chopSound2);

            //Set spriteRenderer to the damaged wall sprite.
            _spriteRenderer.sprite = _dmgSprite;

            //Subtract loss from hit point total.
            _hp -= loss;

            //If hit points are less than or equal to zero:
            if (_hp <= 0)
                //Disable the gameObject.
                gameObject.SetActive(false);
        }
    }
}