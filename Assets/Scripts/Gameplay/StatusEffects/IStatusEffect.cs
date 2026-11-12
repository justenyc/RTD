using UnityEngine;

public interface IStatusEffect
{
    public void Apply(GameObject target) { }
    public void Apply(Renderer target) { }
    public void InitVfx(GameObject target) { }
}
