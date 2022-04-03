internal class BulletStats
{
    internal int fireDamage;
    internal int coldDamage;
    internal int poisonDamage;

    internal int physicalDamage;

    internal float critMulti;

    internal BulletStats(int fire, int cold, int poison, int phys, float crit)
    {
        fireDamage = fire;
        coldDamage = cold;
        poisonDamage = poison;
        physicalDamage = phys;
        critMulti = crit;
    }

    internal float GetDamage()
    {
        return physicalDamage + coldDamage;
    }
}