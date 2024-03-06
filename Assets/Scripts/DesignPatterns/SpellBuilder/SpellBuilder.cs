using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBuilder : MonoBehaviour
{
    public List<ISpell> spells;
    public ISpell spell;

    // Start is called before the first frame update
    void Start()
    {
        spell = new Evocation(new DoubleEffectRune(new FlameRune()), new BallRune());
        Debug.Log($"You cast the spell for {spell.GetManaCost()} mana.");
        spell.Cast();

        // This feels kinda gross
        var evoSpell = spell as Evocation;
        evoSpell.Hit();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
