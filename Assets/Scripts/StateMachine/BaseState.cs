using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.StateMachine
{
    public abstract class BaseState<T>
    {
        public abstract void OnEnterState(T controller);
        public abstract void Update(T controller);
        public virtual void FixedUpdate(T controller) { }

        public abstract void OnExitState(T controller);
    }
}
