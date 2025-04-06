using System;
using UnityEngine;

public class EnemyBullet : Bullet
{
    [SerializeField] private SoundSO fireAudio;
    [SerializeField] private GameEventChannelSO soundChannelSo;

    public void PlaySound()
    {
        if (soundChannelSo != null)
        {
            var evt = SoundEvents.PlaySfxEvent;

            evt.clipData = fireAudio;
            soundChannelSo.RaiseEvent(evt);
        }
        
    }
}
