using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class BlueEffect
{

    internal enum ContactType
    {
        Preserve,
        Pursue
    }

    internal enum PursueEffects
    {
        Traps,
        SlowPlayer,
        EnemyWaits
    }

    internal enum PreserveEffects
    {
        Traps,
        SlowPlayer,
        EnemyWaits
    }

    internal string[] pursueEffects;

    internal static BlueEffect GetNewBlueEffect(ContactType contact)
    {
        switch (contact)
        {
            case ContactType.Preserve:
                PreserveEffects type = (PreserveEffects)Random.Range(0, System.Enum.GetValues(typeof(PreserveEffects)).Length);

                break;

            case ContactType.Pursue:
                PursueEffects a = (PursueEffects)Random.Range(0, System.Enum.GetValues(typeof(PursueEffects)).Length);
                break;
        }

        return new BlueEffect();
    }
}