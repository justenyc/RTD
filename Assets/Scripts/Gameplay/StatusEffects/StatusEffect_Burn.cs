using UnityEngine;

public class StatusEffect_Burn : MonoBehaviour, IStatusEffect
{
    [Header("Properties")]
    [SerializeField] float lifeTime = 10;
    [SerializeField] float power = 1;
    [SerializeField] int damageTickRate = 0;
    [SerializeField] int spreadRate = 5;
    [SerializeField] float spreadRadius = 1;
    [SerializeField] float spreadChance = 50;

    [Header("References")]
    [SerializeField] ParticleSystem fireParticles;

    [Header("Debug")]
    [SerializeField] float lifeTick = 0;
    [SerializeField] int damageTick = 0;
    [SerializeField] float spreadTick = 0;

    [SerializeField] Status statusToAffect;
    [SerializeField] Destructable destructableToAffect;

    public void Apply(GameObject target)
    {
        if(target == null || transform.parent == null)
        {
            Destroy(this.gameObject);
        }

        statusToAffect = target.GetComponent<Status>();
        destructableToAffect = target.GetComponent<Destructable>();
        if(destructableToAffect != null)
        {
            destructableToAffect.OverrideBurnTime(ref lifeTime);
        }

        InitVfx(target);
    }

    void InitVfx(GameObject target)
    {
        Renderer r = target.GetComponent<Renderer>();
        var shape = fireParticles.shape;

        if (r is SkinnedMeshRenderer s)
        {
            shape.shapeType = ParticleSystemShapeType.SkinnedMeshRenderer;
            shape.meshShapeType = ParticleSystemMeshShapeType.Vertex;
            shape.skinnedMeshRenderer = s;
            return;
        }

        shape.shapeType = ParticleSystemShapeType.MeshRenderer;
        shape.meshShapeType = ParticleSystemMeshShapeType.Triangle;
        shape.meshRenderer = r as MeshRenderer;

        fireParticles.Play();
    }

    void FixedUpdate()
    {
        lifeTick += Time.fixedDeltaTime;
        damageTick++;
        spreadTick += Time.fixedDeltaTime;

        if(lifeTick >= lifeTime)
        {
            if (destructableToAffect != null)
            {
                Destroy(destructableToAffect.gameObject);
            }
            this.enabled = false;
            fireParticles.Stop();
            Destroy(this.gameObject, 5);
            return;
        }

        if(damageTick >= damageTickRate)
        {
            damageTick = 0;
            statusToAffect?.TakeDamage(power);
        }

        if(spreadTick >= spreadRate)
        {
            spreadTick = 0;
            SpreadFire();
        }
    }

    public void SpreadFire()
    {
        float spreadCheck = Random.Range(0, 100);
        if(spreadCheck > spreadChance)
        {
            return;
        }

        Collider[] cols = Physics.OverlapSphere(transform.position, spreadRadius);
        foreach(Collider col in cols)
        {
            Logger.LogMessage($"Found another burn effect on {col.name}: {col.transform.GetComponentInChildren<StatusEffect_Burn>() != null}");
            if (col.transform.GetComponentInChildren<StatusEffect_Burn>() != null)
            {
                return;
            }

            Status s = col.transform.GetComponent<Status>();
            Destructable d = col.transform.GetComponent<Destructable>();

            if (s != null || d != null)
            {
                DB_SO.instance.statusEffectsSO.ApplyEffect(Status_Effects.StatusEffect.Burn, col.gameObject);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        //Gizmos.color = Color.yellow;
        //Gizmos.DrawSphere(transform.position, spreadRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, spreadRadius);
    }
}
