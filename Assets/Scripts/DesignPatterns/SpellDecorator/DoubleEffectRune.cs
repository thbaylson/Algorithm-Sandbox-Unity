using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleEffectRune : IRuneDecorator
{
    private int manaCost = 11;

    public DoubleEffectRune(IEffectRune wrappee) : base(wrappee)
    {
        this.wrappee = wrappee;
    }

    public override void Cast()
    {
        wrappee.Cast();
    }
    
    public override void Hit()
    {
        wrappee.Hit();
        wrappee.Hit();
    }

    public override int GetManaCost()
    {
        return manaCost + wrappee.GetManaCost();
    }
}
