using System;

namespace Monads.DataOps.Json
{
    internal class ClearableValueBuilder<T>
    {
        public T Value { get; set; }
        public string State { get; set; }

        public bool TryBuild(out ClearableValue<T> result)
        {
            T value = Value;
            string state = State;
            if (string.IsNullOrEmpty(state) || string.Equals(state, Constants.Clearable_NoActionState, StringComparison.OrdinalIgnoreCase))
            {
                result = ClearableValue<T>.NoAction();
                return true;
            }

            if (string.Equals(state, Constants.Clearable_ClearState, StringComparison.OrdinalIgnoreCase))
            {
                result = ClearableValue<T>.Clear();
                return true;
            }

            if (string.Equals(state, Constants.Clearable_SetState, StringComparison.OrdinalIgnoreCase))
            {
                result = ClearableValue<T>.Set(value);
                return true;
            }

            result = default(ClearableValue<T>);
            return false;
        }
    }
}
