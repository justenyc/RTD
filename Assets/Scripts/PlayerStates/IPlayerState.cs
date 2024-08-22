using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Player
{
    public interface IPlayerState
    {
        public void StateStart();
        public void StateUpdate();
        public void StateFixedUpdate();
    }
}