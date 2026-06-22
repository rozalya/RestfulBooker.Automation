using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using RestfulBooker.Core;

namespace RestfulBooker.Tests
{
    public class BookingAssertion : ReferenceTypeAssertions<object, BookingAssertion>
    {
        private readonly BookingGetResponse _flatData;
        private readonly BookingCreateResponse _wrappedData;

        public BookingAssertion(BookingGetResponse instance, AssertionChain chain)
         : base(instance, chain) { _flatData = instance; }

        // Overload 2: Handle Wrapped Data
        public BookingAssertion(BookingCreateResponse instance, AssertionChain chain)
            : base(instance, chain) { _wrappedData = instance; }


        protected override string Identifier => "booking";

        /// <summary>
        /// Serves as a high-level entry point to trigger automated property validation.
        /// </summary>
        /// <param name="because"></param>
        /// <param name="becauseArgs"></param>
        /// <returns></returns>
        public AndConstraint<BookingAssertion> BeValid(string because = "", params object[] becauseArgs)
        {
            var dataToValidate = (object)_flatData ?? _wrappedData?.booking;

            // Run the automatic check
            ValidateProperties(dataToValidate);
            return new AndConstraint<BookingAssertion>(this);
        }

        /// <summary>
        /// Performs a detailed, field-by-field comparison 
        /// between the actual booking data and the expected object
        /// </summary>
        /// <param name="expected"></param>
        public void Match(BookingRequests expected)
        {
            // 1. Normalize the actual data
            string actualFirst = _flatData?.firstname ?? _wrappedData?.booking.firstname;
            string actualLast = _flatData?.lastname ?? _wrappedData?.booking.lastname;
            int actualPrice = (int)(_flatData?.totalprice ?? _wrappedData?.booking.totalprice);

            // 2. Perform the comparison
            actualFirst.Should().Be(expected.firstname);
            actualLast.Should().Be(expected.lastname);
            actualPrice.Should().Be(expected.totalprice);
        }

        private void ValidateProperties(object data)
        {
            if (data == null) throw new Exception("The object is null!");
            var properties = data.GetType().GetProperties();

            foreach (var prop in properties)
            {
                var value = prop.GetValue(data);               
                if (value == null || value.Equals(GetDefaultValue(prop.PropertyType)))
                {
                    throw new Exception($"Property '{prop.Name}' is empty or null!");
                }
            }
        }
        private object GetDefaultValue(Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }
    }
}
