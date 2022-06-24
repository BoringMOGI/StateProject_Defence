using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTower : Tower
{
    [Header("Projectile")]
    public Bullet bulletPrefab; // Åº.
    public float bulletSpeed;   // Åº¼Ó.

    protected override void OnAttack(Enemy target)
    {
        Bullet bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity); // ÃÑ¾Ë ÇÁ¸®ÆÕ »ý¼º.
        bullet.Setup(target, bulletSpeed, attackPower);     // ÃÑ¾Ë ¼¼ÆÃ.
    }
}
