using UnityEngine;

public class SwordVfxHelper : MonoBehaviour
{
    [SerializeField] GameObject swordObject;
    [SerializeField] GameObject particleObject;
    [SerializeField] ParticleSystem particleObjectSystem;

    public void SwordAppear()
    {
        swordObject.SetActive(true);
        particleObject.SetActive(true);
        particleObjectSystem.Stop();
        particleObjectSystem.Play();
    }

    public void SwordDisappear()
    {
        if(swordObject.activeSelf)
        {
            swordObject.SetActive(false);
            particleObject.SetActive(true);
            particleObjectSystem.Stop();
            particleObjectSystem.Play();
        }
    }
}
