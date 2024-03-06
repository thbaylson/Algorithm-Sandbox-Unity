using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallRune : IShapeRune
{
    private int manaCost = 1;
    private GameObject shape;

    public BallRune()
    {
        //
    }

    public override void Cast()
    {
        //
    }

    public override void OnInstantiate()
    {
        // Instantiate
        Debug.Log("The spell takes the shape of a sphere");
    }

    public override void Hit()
    {
        // OnHit animation logic
    }

    public override int GetManaCost()
    {
        return manaCost;
    }
}
