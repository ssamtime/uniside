using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// BGM 종류
public enum BGMType { None,Title,InGame,InBoss}
// SE 종류
public enum SEType { GameClear,GameOver,Shoot}

public class SoundManager : MonoBehaviour
{
    public AudioClip bgmInTitle;    //BGM(타이틀)
    public AudioClip bgmInGame;     //BGM(게임중)
    public AudioClip bgmInBoss;     //BGM(보스전)
    public AudioClip meGameClear;   //SE(게임클리어)
    public AudioClip meGameOver;    //SE(게임오버)
    public AudioClip setShoot;      //SE(활쏘기)

    // 첫 SoundManager를 저장할 static 변수
    public static SoundManager soundManager;

    // 현재 재생 중인 BGM
    public static BGMType playingBGM = BGMType.None;

    private void Awake()    //start보다 빠름
    {
        // BGM 재생
        if(soundManager == null)
        {
            // static 변수에 자기자신을 저장
            soundManager = this;

            // Scene이 이동해도 오브젝트를 파기하지 않음
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // 정보가 삽입되어 있다면 즉시 파기
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayBGM(BGMType type)
    {
        if(type != playingBGM)
        {
            playingBGM = type;
            AudioSource audio = GetComponent<AudioSource>();
            if (type == BGMType.Title)
                audio.clip = bgmInTitle;
            else if (type == BGMType.InGame)
                audio.clip = bgmInGame;
            else if (type == BGMType.InBoss)
                audio.clip = bgmInBoss;

            audio.Play();
        }
    }

    // BGM 정지
    public void StopBGM()
    {
        GetComponent<AudioSource>().Stop();
        playingBGM = BGMType.None;
    }

    // SE 재생
    public void SEPlay(SEType type)
    {
        if (type == SEType.GameClear)
            GetComponent<AudioSource>().PlayOneShot(meGameClear);
        else if (type == SEType.GameOver)
            GetComponent<AudioSource>().PlayOneShot(meGameOver);
        else if (type == SEType.Shoot)
            GetComponent<AudioSource>().PlayOneShot(setShoot);
    }
}
