using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostParticles : MonoBehaviour
{
    public ParticleSystem LeftMotorParticles;
    public ParticleSystem RightMotorParticles;
    public ParticleSystem LeftMotor2Particles;
    public ParticleSystem RightMotor2Particles;

    public void Awake()
    {
        LeftMotorParticles.Stop(true);
        RightMotorParticles.Stop(true);
        LeftMotorParticles.gameObject.SetActive(false);
        RightMotorParticles.gameObject.SetActive(false);
        LeftMotor2Particles.Stop(true);
        RightMotor2Particles.Stop(true);
        LeftMotor2Particles.gameObject.SetActive(false);
        RightMotor2Particles.gameObject.SetActive(false);
    }

    public void Boost()
    {
        //GameObject left = GameObject.Instantiate(LeftMotorParticles.gameObject,
        //                                 LeftMotorParticles.transform.position,
        //                                 LeftMotorParticles.transform.rotation, null);

        //ParticleSystem leftParticles = left.GetComponent<ParticleSystem>();
        //leftParticles.Play(true);

        //GameObject right = GameObject.Instantiate(RightMotorParticles.gameObject,
        //                                          RightMotorParticles.transform.position,
        //                                          RightMotorParticles.transform.rotation,
        //                                          null);
        //ParticleSystem rightParticles = right.GetComponent<ParticleSystem>();
        //rightParticles.Play(true);

        LeftMotorParticles.gameObject.SetActive(true);
        RightMotorParticles.gameObject.SetActive(true);
        LeftMotorParticles.Stop(true);
        RightMotorParticles.Stop(true);
        LeftMotorParticles.Play(true);
        RightMotorParticles.Play(true);

        LeftMotor2Particles.gameObject.SetActive(true);
        RightMotor2Particles.gameObject.SetActive(true);
        LeftMotor2Particles.Stop(true);
        RightMotor2Particles.Stop(true);
        LeftMotor2Particles.Play(true);
        RightMotor2Particles.Play(true);
    }
}
