using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoF_BGMManager : MonoBehaviour
{
    [Range(0.1f, 2.0f), SerializeField]
    private float switchDamp = 1.0f;

    [SerializeField]
    private AudioSource _audioSrcA;
    private float _audioSrcMaxVolumeA;
    [SerializeField]
    private AudioSource _audioSrcB;
    private float _audioSrcMaxVolumeB;

    [SerializeField]
    private AudioClip clip_round01;
    [SerializeField]
    private AudioClip clip_round02;

    private Hashtable m_bgmDatabase;


    public enum EStatus
    {
        none,
        round01,
        round02,
    }
    private EStatus m_status;

    void Start()
    {
        Debug.Assert(_audioSrcA != null);
        _audioSrcA.Stop();
        _audioSrcA.playOnAwake = false;
        _audioSrcA.loop = true;
        _audioSrcMaxVolumeA = _audioSrcA.volume;
        _audioSrcA.volume = 0.0f;
        _audioSrcA.clip = clip_round01;
        

        Debug.Assert(_audioSrcB != null);
        _audioSrcB.Stop();
        _audioSrcB.playOnAwake = false;
        _audioSrcB.loop = true;
        _audioSrcMaxVolumeB = _audioSrcB.volume;
        _audioSrcB.volume = 0.0f;
        _audioSrcB.clip = clip_round02;

        m_bgmDatabase = new Hashtable();
        m_bgmDatabase.Add(EStatus.round01, clip_round01);
        m_bgmDatabase.Add(EStatus.round02, clip_round02);

        m_status = EStatus.none;
    }


    public void ChangeStatus(EStatus status)
    {
        if (status != m_status)
        {
            m_status = status;

            switch (m_status)
            {
                case EStatus.round01:
                    _audioSrcA.volume = 0.0f;
                    _audioSrcA.Play();
                    break;

                case EStatus.round02:
                    _audioSrcB.volume = 0.0f;
                    _audioSrcB.Play();
                    break;
            }
        }
    }

    private void Update()
    {
        switch (m_status)
        {
            case EStatus.round01:
                // fade in
                if (_audioSrcA.volume <= _audioSrcMaxVolumeA)
                {
                    _audioSrcA.volume += Time.deltaTime / switchDamp;
                }
                break;

            case EStatus.round02:
                // cross fade
                if (_audioSrcA.volume >= 0.0f)
                {
                    _audioSrcA.volume -= Time.deltaTime / switchDamp;
                }
                else
                {
                    _audioSrcA.Stop();
                }

                if (_audioSrcB.volume <= _audioSrcMaxVolumeB)
                {
                    _audioSrcB.volume += Time.deltaTime / switchDamp;
                }
                break;
        }

    }

}
