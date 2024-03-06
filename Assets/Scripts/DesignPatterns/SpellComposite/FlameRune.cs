using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameRune : IEffectRune
{
    private int manaCost = 3;
    private int damage = 4;

    public override void Cast()
    {
        //
    }

    public override void Hit()
    {
        // Deal damage
        Debug.Log($"The spell deals {damage} fire damage");
    }

    public override int GetManaCost()
    {
        return manaCost;
    }
}
