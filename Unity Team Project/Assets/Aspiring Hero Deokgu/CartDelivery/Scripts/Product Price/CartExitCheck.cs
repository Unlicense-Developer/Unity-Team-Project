using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CartDelivery
{
    public class CartExitCheck : MonoBehaviour
    {
        bool check = false;
        #region 물건들 값 태그(크기별)
        const string large = "Large";
        const string mediumlarge = "MediumLarge";
        const string medium = "Medium";
        const string smallmedium = "SmallMedium";
        const string small = "Small";
        #endregion

        #region 물건들 값
        public int largePrice = 100; //대
        public int mediumlargePrice = 70; //중+대
        public int mediumPrice = 50; //중
        public int smallmediumPrice = 30; //소+중
        public int smallPrice = 10; //소
        #endregion
        //콜라이더에서 벗어나면 (TriggerExit) 점수가 깎임(1000 -(물건값))
        void Start()
        {

        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("ScoreColl") && check)
            {
                switch (this.gameObject.tag)
                {
                    case large:
                        ScoreManager.instance.PlusProductPrice(largePrice);
                        break;
                    case mediumlarge:
                        ScoreManager.instance.PlusProductPrice(mediumlargePrice);
                        break;
                    case medium:
                        ScoreManager.instance.PlusProductPrice(mediumPrice);
                        break;
                    case smallmedium:
                        ScoreManager.instance.PlusProductPrice(smallmediumPrice);
                        break;
                    case small:
                        ScoreManager.instance.PlusProductPrice(smallPrice);
                        break;
                }
                check = false;
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("ScoreColl") && !check)
            {
                switch (this.gameObject.tag)
                {
                    case large:
                        ScoreManager.instance.MinusProductPrice(largePrice);
                        break;
                    case mediumlarge:
                        ScoreManager.instance.MinusProductPrice(mediumlargePrice);
                        break;
                    case medium:
                        ScoreManager.instance.MinusProductPrice(mediumPrice);
                        break;
                    case smallmedium:
                        ScoreManager.instance.MinusProductPrice(smallmediumPrice);
                        break;
                    case small:
                        ScoreManager.instance.MinusProductPrice(smallPrice);
                        break;
                }
                check = true;
            }
        }


        void Update()
        {

        }
    }
}
