using System;

namespace VERS
{
    [Serializable]
    public class FloatReference : Reference<float, FloatVariableSO>
    {
        public FloatReference() { }

        public FloatReference(float value) : base(value) { }

        public static implicit operator FloatReference(float value)
        {
            return new FloatReference(value);
        }
    }
}
