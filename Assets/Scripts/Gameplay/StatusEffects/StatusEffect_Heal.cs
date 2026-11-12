using UnityEngine;

public class StatusEffect_Heal : MonoBehaviour, IStatusEffect
{
    [SerializeField] ParticleSystem _particleSystem;

    [Header("Debug")]
    [SerializeField] Status statusToAffect;

    public void Apply(Renderer target) 
    {
        if (target == null || transform.parent == null)
        {
            Destroy(this.gameObject);
            Logger.LogError($"Failed to find <color=yellow>target</color> for {this.name} effect!");
        }

        InitVfx(target);
    }

    void InitVfx(Renderer target)
    {
        Renderer r = target.transform.GetComponentInAll<Renderer>();
        var shape = _particleSystem.shape;

        if (r is SkinnedMeshRenderer s)
        {
            shape.shapeType = ParticleSystemShapeType.SkinnedMeshRenderer;
            shape.meshShapeType = ParticleSystemMeshShapeType.Vertex;
            shape.skinnedMeshRenderer = s;
            _particleSystem.Play();
            return;
        }

        shape.shapeType = ParticleSystemShapeType.MeshRenderer;
        shape.meshShapeType = ParticleSystemMeshShapeType.Triangle;
        shape.meshRenderer = r as MeshRenderer;
        _particleSystem.Play();
    }
}
