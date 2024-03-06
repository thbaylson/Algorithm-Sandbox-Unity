using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseRune : ISpell
{
    public override void Cast()
    {
        Debug.Log("Base Cast");
    }

    public override void Hit()
    {
        Debug.Log("Base Hit");
    }

    public override int GetManaCost()
    {
        Debug.Log("Base GetManaCost");
        return 0;
    }
}
