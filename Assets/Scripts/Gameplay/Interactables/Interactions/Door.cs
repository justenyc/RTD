using Player;
using UnityEngine;
using DG.Tweening;

public class Door : Interactable
{
    [SerializeField] bool isOpen;
    [SerializeField] bool npcAble;
    [SerializeField] float turnTime = 1;
    [SerializeField] Transform[] transformsToTurn;
    [SerializeField] Vector3[] openPositions;
    [SerializeField] Vector3[] closePositions;

    DG.Tweening.Sequence doorSequence;

    public override void Interact(PlayerController playerController)
    {
        OpenDoor();
    }

    public void OpenDoor()
    {
        isOpen = !isOpen;

        OpenDoor(isOpen);
    }

    public void OpenDoor(bool b, bool isPlayer = true)
    {
        if ((!npcAble && !isPlayer) || transformsToTurn.Length < 1) return;

        doorSequence = DOTween.Sequence();
        
        if(doorSequence.IsPlaying())
        {
            doorSequence.Kill();
        }

        for (int ii = 0; ii < transformsToTurn.Length; ii++)
        {
            //transformsToTurn[ii].localRotation = b ? Quaternion.Euler(openPositions[ii]) : Quaternion.Euler(closePositions[ii]);
            Vector3 goal = b ? openPositions[ii] : closePositions[ii];
            doorSequence.Join(transformsToTurn[ii].DORotate(goal, turnTime));
        }

        doorSequence.OnComplete(() => doorSequence.Kill());
    }
}
