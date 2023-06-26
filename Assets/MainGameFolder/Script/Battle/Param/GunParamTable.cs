using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GunParam", menuName = "CreateParamData/CreateGunParam")]
public class GunParamTable : ScriptableObject
{
    public List<GunSelection> GunList = new List<GunSelection>();
}

[System.Serializable]
public class GunSelection
{
    public enum Level
    {
        Common = 0,
        Rare = 1,
        Unique = 2,
    }
    public enum ShootMode { AUTO, SEMIAUTO }
    [SerializeField] Level level;
    [SerializeField] ShootMode shootMode;
    [SerializeField] bool Scope;
    [Range(0.01f, 2.0f)]
    [SerializeField] float shotRate = 0.13f;
    [Range(2, 100)]
    [SerializeField] int ammo = 25;
    [Range(10, 300)]
    [SerializeField] int fullAmmo = 100;
    [Range(0.1f, 500000f)]
    [SerializeField] float bulletSpeed;
    [Range(78, 240)]
    [SerializeField] int headDamage = 160;
    [Range(32, 150)]
    [SerializeField] int bodyDamage = 40;
    [Range(16, 101)]
    [SerializeField] int legDamage = 20;
    [Range(0f, 1.0f)]
    [SerializeField] float distanceAttenuation = 0;
    [Range(0f, 1.0f)]
    [SerializeField] float stopRecoil;
    [Range(15f, 60f)]
    [SerializeField] float maxZoomFOV;

    public Level getLevel()
    {
        return level;
    }
    public ShootMode getShootMode()
    {
        return shootMode;
    }
    public bool getScope()
    {
        return Scope;
    }
    public float getShotRate()
    {
        return shotRate;
    }
    public int getAmmo()
    {
        return ammo;
    }
    public int getFullAmmo()
    {
        return fullAmmo;
    }
    public float getBulletSpeed()
    {
        return bulletSpeed;
    }
    public int getHeadDamage()
    {
        return headDamage;
    }
    public int getBodyDamage()
    {
        return bodyDamage;
    }
    public int getLegDamage()
    {
        return legDamage;
    }
    public float getDistanceAttenuation()
    {
        return distanceAttenuation;
    }
    public float getStopRecoil()
    {
        return stopRecoil;
    }
    public float getMaxZoomFOV()
    {
        return maxZoomFOV;
    }
}