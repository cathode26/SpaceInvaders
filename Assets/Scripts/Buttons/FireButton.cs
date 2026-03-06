using deVoid.Utils;
using UnityEngine;

namespace SpaceInvaders
{
    public class FireButton : MonoBehaviour
    {
        public void OnPressed()
        {
            Signals.Get<Project.Input.OnHandleShootSignal>().Dispatch();
        }
    }
}