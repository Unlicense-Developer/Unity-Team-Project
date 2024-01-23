using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CartDelivery
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager instance;
        public AudioSource gameSound;

        public AudioClip horseRunSound;
        public AudioClip horseWalkSound;
        public AudioClip horseWalkTwoSound;
        public AudioClip horseWalkThreeSound;
        public AudioClip horseWalkFourSound;
        public float RunSoundPitch = 1.1f;
        public int WalkSoundPitch = 1;


        [SerializeField]
        Transform horse;
        void Awake()
        {
            if (SoundManager.instance == null)
                SoundManager.instance = this;
        }
        #region 말 발굽소리 
        public void PlaySoundHorseWalkOne()
        {
            gameSound.PlayOneShot(horseWalkSound);
        }
        public void PlaySoundHorseWalkTwo()
        {
            gameSound.PlayOneShot(horseWalkTwoSound);
        }

        public void PlaySoundHorseWalkThree()
        {
            gameSound.PlayOneShot(horseWalkThreeSound);
        }
        public void PlaySoundHorseWalkFour()
        {
            gameSound.PlayOneShot(horseWalkFourSound);
        }
        public void PlaySoundHorseRun() //달리는 소리속도 
        {
            if (gameSound.loop == false)//루프가 처음에 false일때 작동(true일때는 작동안함)
            {
                gameSound.clip = horseRunSound;
                gameSound.pitch = RunSoundPitch;
                gameSound.loop = true;//작동하면 true로 변경 
                gameSound.Play();
            }

        }
        public void PlaySoundHorseWalkDefault() //달리지 않을때 기본소리 속도 
        {
            gameSound.clip = null;
            gameSound.pitch = WalkSoundPitch;
            gameSound.loop = false;
        }
        #endregion
    }
}
