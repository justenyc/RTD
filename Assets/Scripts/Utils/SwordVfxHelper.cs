using UnityEngine;

public class SwordVfxHelper : MonoBehaviour
{
    [SerializeField] GameObject swordObject;
    [SerializeField] GameObject particleObject;

    public void SwordAppear()
    {
        swordObject.SetActive(true);
        particleObject.SetActive(true);
    }

    public void SwordDisappear()
    {
        if(swordObject.activeSelf)
        {
            swordObject.SetActive(false);
            particleObject.SetActive(true);
        }
    }
}
