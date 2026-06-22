using FluentAssertions.Execution;
using RestfulBooker.Core;

namespace RestfulBooker.Tests
{
    public static class AssertionExtensions
    {
        public static BookingAssertion Should(this BookingGetResponse instance)
        {
            return new BookingAssertion(instance, AssertionChain.GetOrCreate());
        }

        public static BookingAssertion Should(this BookingCreateResponse instance)
        {
            return new BookingAssertion(instance, AssertionChain.GetOrCreate());
        }
    }
}
