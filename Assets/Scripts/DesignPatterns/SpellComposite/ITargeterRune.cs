using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ITargeterRune : ISpell
{
    protected ISpell wrappee;

    public ITargeterRune(ISpell wrappee)
    {
        this.wrappee = wrappee;
    }

    public override void Cast()
    {
        wrappee.Cast();
    }

    public abstract void FindTarget();
}
