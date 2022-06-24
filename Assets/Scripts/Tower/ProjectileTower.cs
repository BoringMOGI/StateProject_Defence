using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTower : Tower
{
    [Header("Projectile")]
    public Bullet bulletPrefab; // ź.
    public float bulletSpeed;   // ź��.

    protected override void OnAttack(Enemy target)
    {
        Bullet bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity); // �Ѿ� ������ ����.
        bullet.Setup(target, bulletSpeed, attackPower);     // �Ѿ� ����.
    }
}
