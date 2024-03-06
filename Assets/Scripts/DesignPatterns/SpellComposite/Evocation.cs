using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Evocation : ISpell
{
    private IEffectRune _effectRune;
    private IShapeRune _shapeRune;
    private List<ISpell> _spells;

    public Evocation(IEffectRune effect, IShapeRune shape, List<ISpell> spells = null)
    {
        //IEffectRune effect, IShapeRune shape, ITargeterRune targeter
        _effectRune = effect;
        _shapeRune = shape;

        if (spells == null )
        {
            spells = new List<ISpell>();
        }
        spells.Add(_effectRune);
        spells.Add(_shapeRune);
        _spells = spells;
    }

    public void AddSpell(ISpell spell)
    {
        _spells.Add(spell);
    }

    public void AddSpells(List<ISpell> spells)
    {
        _spells.AddRange(spells);
    }

    public override void Cast()
    {
        _spells.ForEach(spell => { spell.Cast(); });
    }

    public override void Hit()
    {
        _spells.ForEach(spell => { spell.Hit(); });
    }

    public override int GetManaCost()
    {
        return _spells.Sum(spell => spell.GetManaCost());
    }
}
