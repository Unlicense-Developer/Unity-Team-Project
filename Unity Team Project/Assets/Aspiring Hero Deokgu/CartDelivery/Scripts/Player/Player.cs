using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CartDelivery
{
    public class Player : MonoBehaviour
    {
        // 타이틀창에서는 게임 실행안되게 ==> bool에 GamePlay 조건 추가 하여 플레이일때만 버튼눌리게 
        public bool gamePlaying = false;

        public bool playerMoveSign = true;

        [SerializeField]
        Transform horseCartTransform; //레이 위치(말과 마차사이)
        public Transform cameraTransform; //카메라 위치
        float speed, inputH, inputV;
        public float stopSpeed = 0f;
        public float moveSpeed = 5f;//말 이동속도
        public float runSpeed = 1.5f;//말 달리는속도
        public float jumpForce = 15f;//말 점프높이
        public bool isJumping;
        public LayerMask floor;
        public float groundHeight = 1f;
        [SerializeField]
        Rigidbody horseRigd; //말 리지드바디

        [SerializeField]
        Animator horseAni;//말 애니
        //애니
        void Start()
        {
            AniDefult();//말 기본애니(Idle)
            speed = moveSpeed;
            playerMoveSign = true;
            // isJumping = false;
        }
        void Move()
        {
            //SoundManager.instance.PlaySoundHorseWalkDefault();
            //이동값
            float inputH = Input.GetAxisRaw("Horizontal"); //A,D키 누르면 (왼,오)
            float inputV = Input.GetAxisRaw("Vertical"); //W,S키 누르면 (앞,뒤)

            Vector3 velocity = new Vector3(inputH, 0, inputV);

            velocity = cameraTransform.TransformDirection(velocity);

            velocity *= moveSpeed;
            #region W,A,S,D 입력에 애니상태

            if (inputV >= 1) //1(앞)
            {
                AniFrontWalk();
            }
            else if (inputV < 0)//-1(뒤)
            {
                AniBackWalk();
            }

            else if (inputH < 0) //왼(-1)
            {
                LeftRotation();
                AniLeftWalk();

            }
            else if (inputH >= 1) //오(1)
            {
                RightRotation();
                AniRightWalk();
            }

            else if (inputV == 0 || inputH == 0)
            {
                AniDefult(); //기본(0)
            }

            #endregion

            #region 땅에서 떨어지는 속도
            //떨어지는 속도
            float fallSpeed = horseRigd.velocity.y;

            velocity.y = fallSpeed; //떨어지는속도 초기화
            horseRigd.velocity = velocity;
            #endregion
        }





        void RightRotation()//오른쪽 회전
        {
            inputH = inputH * speed * Time.deltaTime;
            this.transform.Rotate(Vector3.up * inputH);
        }
        void LeftRotation()//왼쪽 회전
        {
            inputH = inputH * speed * Time.deltaTime;
            this.transform.Rotate(Vector3.up * -inputH);
        }
        void SpeedUp()
        {
            if (Input.GetKey(KeyCode.LeftShift))//Shift키+W키(앞으로) 누르면
            {
                SoundManager.instance.PlaySoundHorseRun(); //달리는 소리켜지게
                Vector3 pos = new Vector3(0, 0, 1);
                pos = cameraTransform.TransformDirection(pos);
                transform.position = transform.position + pos * (runSpeed * Time.deltaTime);//이동속도 높이기
                AniRun();
            }
            else
            {
                SoundManager.instance.PlaySoundHorseWalkDefault(); //사운드 기본셋팅으로 리셋(사운드속도 기본,클립 null(없음))
            }
        }


        void SpeedDown()
        {

            if (Input.GetKey(KeyCode.Space) || gamePlaying == false)
            {
                playerMoveSign = false;
                if (playerMoveSign == false)
                {
                    Vector3 pos = new Vector3(0, 0, 0);
                    pos = cameraTransform.TransformDirection(pos);
                    transform.position = transform.position + pos;
                    AniDefult();
                }
            }
            else
            {
                playerMoveSign = true;
                if (playerMoveSign == true)
                {
                    return;
                }
            }
        }



        #region 말 애니 상태 종류
        void AniBackWalk()
        {
            horseAni.SetInteger("Motion", -1);//BackwardWalk // 뒤_걷기
        }
        void AniDefult()
        {
            horseAni.SetInteger("Motion", 0);//Idle // 기본동작
        }
        void AniFrontWalk()
        {
            horseAni.SetInteger("Motion", 1);//ForwardWalk // 앞_걷기
        }
        void AniRun()
        {
            horseAni.SetInteger("Motion", 2);//Run // 뛰기

        }
        void AniLeftWalk()
        {
            horseAni.SetInteger("Motion", 3);//LeftWalk // 왼_걷기
        }
        void AniRightWalk()
        {
            horseAni.SetInteger("Motion", 4);//RightWalk //오_걷기
        }
        #endregion

        public void PlayerControlSign() //Start 버튼 눌렀을때만 이동
        {
            if (gamePlaying == true && playerMoveSign == true)
            {
                Move();
                SpeedUp();
            }
        }
        void Update()
        {

        }
        #region 말 걷는 소리 SoundManager에서 가져옴
        void OnWalkSoundOne()
        {
            SoundManager.instance.PlaySoundHorseWalkOne();
        }
        void OnWalkSoundTwo()
        {
            SoundManager.instance.PlaySoundHorseWalkTwo();
        }
        void OnWalkSoundThree()
        {
            SoundManager.instance.PlaySoundHorseWalkThree();
        }
        void OnWalkSoundFour()
        {
            SoundManager.instance.PlaySoundHorseWalkFour();
        }
        #endregion

        void FixedUpdate()
        {
            PlayerControlSign();
            SpeedDown();
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("River"))
                UIManager.instance.OnGameOver();

            else if (other.gameObject.CompareTag("GameClearZone"))
                UIManager.instance.OnGameClear();

        }



    }











}

