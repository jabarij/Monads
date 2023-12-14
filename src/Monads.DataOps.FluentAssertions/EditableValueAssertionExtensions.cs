﻿namespace Monads.DataOps.FluentAssertions;

public static class EditableValueAssertionExtensions
{
    public static EditableValueAssertions<T> Should<T>(this EditableValue<T> actualValue) => new(actualValue);
    public static NullableEditableValueAssertions<T> Should<T>(this EditableValue<T>? actualValue) => new(actualValue);
}