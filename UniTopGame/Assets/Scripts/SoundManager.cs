using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// BGM ����
public enum BGMType { None,Title,InGame,InBoss}
// SE ����
public enum SEType { GameClear,GameOver,Shoot}

public class SoundManager : MonoBehaviour
{
    public AudioClip bgmInTitle;    //BGM(Ÿ��Ʋ)
    public AudioClip bgmInGame;     //BGM(������)
    public AudioClip bgmInBoss;     //BGM(������)
    public AudioClip meGameClear;   //SE(����Ŭ����)
    public AudioClip meGameOver;    //SE(���ӿ���)
    public AudioClip setShoot;      //SE(Ȱ���)

    // ù SoundManager�� ������ static ����
    public static SoundManager soundManager;

    // ���� ��� ���� BGM
    public static BGMType playingBGM = BGMType.None;

    private void Awake()    //start���� ����
    {
        // BGM ���
        if(soundManager == null)
        {
            // static ������ �ڱ��ڽ��� ����
            soundManager = this;

            // Scene�� �̵��ص� ������Ʈ�� �ı����� ����
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // ������ ���ԵǾ� �ִٸ� ��� �ı�
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

    // BGM ����
    public void StopBGM()
    {
        GetComponent<AudioSource>().Stop();
        playingBGM = BGMType.None;
    }

    // SE ���
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
