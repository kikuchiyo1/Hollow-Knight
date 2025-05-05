using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerAudioManager : MonoBehaviour
{
    public AudioSource RUNSource;
    public AudioSource JUMPTWOSource;
    public AudioSource JUMPSource;
    public AudioSource FALLSource;
    public AudioSource LANDSource;
    public AudioSource ATTACKSource;
    public AudioSource DASHSource;

    public void Run()
    {
        RUNSource.Play();
    }

    public void Jump()
    {
        JUMPSource.Play();
    }

    public void JumpTwo()
    {
        JUMPTWOSource.Play();
    }


    public void Land()
    {
        LANDSource.Play();
    }
    public void Attack()
    {
        ATTACKSource.Play();
    }


    public void StopAttack()
    {
        ATTACKSource.Stop();
    }
    public void StopRun()
    {
        RUNSource.Stop();
    }

    public void StopJump()
    {
        JUMPSource.Stop();
    }
    public void StopJumpTwo()
    {
        JUMPTWOSource.Stop();
    }

    public void StopLand()
    {
        LANDSource.Stop();
    }

    public void Falling()
    {
        if (!FALLSource.isPlaying) FALLSource.Play();
    }

    public void StopFalling()
    {
        FALLSource.Stop();
    }

    public void Dash()
    {
        DASHSource.Play();
    }

    public void StopDash()
    {
        DASHSource.Stop();
    }

}
