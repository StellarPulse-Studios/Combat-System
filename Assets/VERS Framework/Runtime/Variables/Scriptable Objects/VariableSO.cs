using UnityEngine;

namespace VERS
{
    public abstract class VariableSO<T> : ScriptableObject
    {
        public T Value;
    }
}
