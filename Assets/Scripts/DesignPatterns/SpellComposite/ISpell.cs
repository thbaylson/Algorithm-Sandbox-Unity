using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ISpell
{
    public abstract void Cast();
    public abstract void Hit();
    public abstract int GetManaCost();
}
