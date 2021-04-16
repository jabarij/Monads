using AutoFixture;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Xunit;

namespace Monads.TestAbstractions
{
    public class TestsBase : IClassFixture<TestFixture>
    {
        private readonly TestFixture _testFixture;

        protected TestsBase(TestFixture testFixture)
        {
            _testFixture = testFixture;
        }

        protected IFixture Fixture => _testFixture.Fixture;

        public static IEnumerable<object[]> GetTestData(Type sourceType, string dataSourceName)
        {
            var source = sourceType.GetMember(dataSourceName, BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.InvokeMethod | BindingFlags.GetProperty | BindingFlags.GetField);
            if (source == null || !source.Any())
                throw new InvalidOperationException($"Could not find member {sourceType.FullName}.{dataSourceName}.");
            if (source.Skip(1).Any())
                throw new InvalidOperationException($"Ambiguous member {sourceType.FullName}.{dataSourceName}.");
            var member = source.Single();

            object dataSource;
            switch (member.MemberType)
            {
                case MemberTypes.Field:
                    dataSource = ((FieldInfo)member).GetValue(null);
                    break;
                case MemberTypes.Method:
                    dataSource = ((MethodInfo)member).Invoke(null, null);
                    break;
                case MemberTypes.Property:
                    dataSource = ((PropertyInfo)member).GetValue(null);
                    break;
                default:
                    throw new InvalidOperationException($"Unhandled value of {member.MemberType.GetType().FullName}.{member.MemberType} for member {sourceType.FullName}.{dataSourceName}.");
            }

            if (dataSource == null)
                return Enumerable.Empty<object[]>();

            if (dataSource is IEnumerable<ITestDataProvider> testDataProviders)
                return testDataProviders
                    .Select(e => e.GetTestParameters());
            else if (dataSource is IEnumerable enumerable)
                return enumerable
                    .Cast<object>()
                    .Select(e => new[] { e });
            else
                throw new InvalidOperationException($"Member {sourceType.FullName}.{dataSourceName} must return {typeof(IEnumerable<ITestDataProvider>).FullName} or {typeof(IEnumerable).FullName} to be a valid test data source.");
        }
    }
}
