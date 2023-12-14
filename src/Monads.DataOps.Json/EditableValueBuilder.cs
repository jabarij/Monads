using System;

namespace Monads.DataOps.Json;

internal class EditableValueBuilder<T>
{
    public T Value { get; set; }
    public string State { get; set; }

    public bool TryBuild(out EditableValue<T> result)
    {
        T value = Value;
        string state = State;
        if (string.IsNullOrEmpty(state) || string.Equals(state, Constants.Editable_NoActionState, StringComparison.OrdinalIgnoreCase))
        {
            result = EditableValue<T>.NoAction();
            return true;
        }

        if (string.Equals(state, Constants.Editable_UpdateState, StringComparison.OrdinalIgnoreCase))
        {
            result = EditableValue<T>.Update(value);
            return true;
        }

        result = default;
        return false;
    }
}