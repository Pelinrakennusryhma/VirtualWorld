using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGun : Weapon
{

    public LaserGunProjectile ProjectilePrefab;

    public List<LaserGunProjectile> Projectiles;

    public int CurrentProjectileIndex;

    public float CoolDown;
    public float CoolDownLength = 0.4f;

    public GameObject FireTarget;

    public Animator Animator;

    public const string ShootString = "Shoot";

    public void Awake()
    {
        CoolDownLength = 0.4f;
        TypeOfWeapon = WeaponType.LaserGun;

        Animator = GetComponent<Animator>();

        // Create object pool

        Projectiles = new List<LaserGunProjectile>();

        for (int i = 0; i < 30; i++)
        {
            LaserGunProjectile projectile = Instantiate(ProjectilePrefab);
            projectile.Init(this);
            Projectiles.Add(projectile);
        }

        CurrentProjectileIndex = 0;
    }

    public void SetFireTarget(GameObject fireTarget)
    {
        FireTarget = fireTarget;
        //Debug.Log("Setting fire target");
    }

    public override void OnFire1Down()
    {
        if (CoolDown > 0)
        {
            //Debug.Log("Tried to fire laser gun, but cooldown has not yet been cleared");
            return;
        }

        base.OnFire1Down();
        CoolDown = CoolDownLength;

        //Debug.Log("Current projectile index is " + CurrentProjectileIndex + " projectiles count is " + Projectiles.Count);


        Projectiles[CurrentProjectileIndex].OnLaunch(transform.position, FireTarget.transform.position - transform.position);

        CurrentProjectileIndex++;


        if (CurrentProjectileIndex >= Projectiles.Count)
        {
            CurrentProjectileIndex = 0;
        }

        Animator.SetTrigger(ShootString);

        //Debug.LogWarning("Fire 1 called on laser gun " + Time.time);
    }

    private void Update()
    {
        CoolDown -= Time.deltaTime;
    }

    public void ReturnObjectToPool(LaserGunProjectile projectile)
    {
        //Debug.Log("Here should be functionality for returning object to pool. If needed");
    }

    public void MoveProjectilesToScene(UnityEngine.SceneManagement.Scene scene)
    {
        for (int i =0; i < Projectiles.Count; i++)
        {
            UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(Projectiles[i].gameObject, scene);
        }
    }

}
