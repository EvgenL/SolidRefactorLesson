using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;

namespace Completed
{
    public class SoundManager : MonoBehaviourSingleton<SoundManager>
    {
        [FormerlySerializedAs("efxSource")] public AudioSource _efxSource; //Drag a reference to the audio source which will play the sound effects.
        [FormerlySerializedAs("musicSource")] public AudioSource _musicSource; //Drag a reference to the audio source which will play the music.
        [FormerlySerializedAs("lowPitchRange")] public float _lowPitchRange = .95f; //The lowest a sound effect will be randomly pitched.
        [FormerlySerializedAs("highPitchRange")] public float _highPitchRange = 1.05f; //The highest a sound effect will be randomly pitched.


        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }


        //Used to play single sound clips.
        public void PlaySingle(AudioClip clip)
        {
            //Set the clip of our efxSource audio source to the clip passed in as a parameter.
            _efxSource.clip = clip;

            //Play the clip.
            _efxSource.Play();
        }


        //RandomizeSfx chooses randomly between various audio clips and slightly changes their pitch.
        public void RandomizeSfx(params AudioClip[] clips)
        {
            //Generate a random number between 0 and the length of our array of clips passed in.
            var randomIndex = Random.Range(0, clips.Length);

            //Choose a random pitch to play back our clip at between our high and low pitch ranges.
            var randomPitch = Random.Range(_lowPitchRange, _highPitchRange);

            //Set the pitch of the audio source to the randomly chosen pitch.
            _efxSource.pitch = randomPitch;

            //Set the clip to the clip at our randomly chosen index.
            _efxSource.clip = clips[randomIndex];

            //Play the clip.
            _efxSource.Play();
        }
    }
}