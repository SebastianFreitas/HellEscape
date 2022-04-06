internal class BulletStats
{
    internal float fireDamage;
    internal float coldDamage;
    internal float poisonDamage;

    internal float physicalDamage;

    internal float critMulti;

    internal BulletStats(float fire, float cold, float poison, float phys, float crit)
    {
        fireDamage = fire;
        coldDamage = cold;
        poisonDamage = poison;
        physicalDamage = phys;
        critMulti = crit;
    }

    internal float GetDamage()
    {
        return physicalDamage + coldDamage + fireDamage;
    }
}