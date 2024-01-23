using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GrillingMeatGame
{
    public class MeatGrill : MonoBehaviour
    {
        int meatCount = 0;
        [SerializeField]
        bool haveMeat = false;
        private void Start()
        {
            haveMeat = false;
        }
        void MeatSoundOnOffSign()
        {
            if (haveMeat == true)
            {
                GameManager.instance.SoundMeatPlay(); //고기 소리 재생 
            }
            else if (haveMeat == false)
            {
                GameManager.instance.SoundMeatStop();//고기 소리 재생멈춤 
            }
        }



        private void OnTriggerEnter2D(Collider2D other)
        {

            if (other.gameObject.CompareTag("MEAT")) //고기가 닿으면 
            {
                haveMeat = true;
                meatCount += 1;
                MeatSoundOnOffSign();
            }

        }
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("MEAT")) //고기가 닿으면 
            {
                meatCount -= 1;
                if(meatCount ==0)
                {
                haveMeat = false;
                MeatSoundOnOffSign();

                }
            }
        }
    }
}







