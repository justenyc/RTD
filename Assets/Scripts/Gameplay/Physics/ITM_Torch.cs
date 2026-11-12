using UnityEngine;

public class ITM_Torch : MonoBehaviour
{
    [SerializeField] GameObject torchProjPrefab;
    [SerializeField] GameObject rootGO;

    private void OnParticleSystemStopped()
    {
        Logger.LogMessage("Particle System stopped");
        Item_Projectile itmp = Instantiate(torchProjPrefab, rootGO.transform.position, rootGO.transform.rotation).GetComponent<Item_Projectile>();
        itmp.InitProtocol(DB_SO.instance.itemSO.GetItem("Torch"), vfxOnStart: false);
        itmp.GetComponent<TorqueApplicator>().enabled = false;

        rootGO.SetActive(false);
        gameObject.SetActive(false);
    }
}