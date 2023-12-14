namespace Monads.DataOps.Json;

static class Constants
{
    public const string ValuePropertyName = "value";
    public const string StatePropertyName = "state";

    public const string Editable_UpdateState = nameof(EditableValue<object>.Update);
    public const string Editable_NoActionState = nameof(EditableValue<object>.NoAction);

    public const string Clearable_SetState = nameof(ClearableValue<object>.Set);
    public const string Clearable_ClearState = nameof(ClearableValue<object>.Clear);
    public const string Clearable_NoActionState = nameof(ClearableValue<object>.NoAction);
}