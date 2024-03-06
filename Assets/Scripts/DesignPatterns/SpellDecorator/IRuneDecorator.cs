using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IRuneDecorator : IEffectRune
{
    protected IEffectRune wrappee;

    public IRuneDecorator(IEffectRune wrappee)
    {
        this.wrappee = wrappee;
    }
}
