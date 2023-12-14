using System;
using AutoFixture;

namespace Monads.TestAbstractions;

public class TestFixture
{
    public TestFixture()
    {
        // Perform global test setup here
        var fixture = new Fixture();
        CustomizeFixture(fixture);
        Fixture = fixture;
    }

    public IFixture Fixture { get; }

    private void CustomizeFixture(Fixture fixture)
    {
        // Customize common fixture setup here
    }

    private bool _isDisposed;
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    protected virtual void Dispose(bool disposing)
    {
        if (_isDisposed) return;
        if (disposing)
        {
            Teardown();
            // Dispose managed resources
        }

        _isDisposed = true;
    }

    private void Teardown()
    {
        // Perform global test teardown here
    }
}