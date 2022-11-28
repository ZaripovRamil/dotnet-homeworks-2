using System;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Hw6;
using Xunit;

namespace Hw6Tests
{
    public class BasicTests : IClassFixture<CustomWebApplicationFactory<App.Startup>>
    {
        private readonly CustomWebApplicationFactory<App.Startup> _factory;
        private const decimal Epsilon = 0.001m;

        public BasicTests(CustomWebApplicationFactory<App.Startup> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData(15, 5, "Plus", 20, HttpStatusCode.OK)]
        [InlineData(15, 5, "Minus", 10, HttpStatusCode.OK)]
        [InlineData(15, 5, "Multiply", 75, HttpStatusCode.OK)]
        [InlineData(15, 5, "Divide", 3, HttpStatusCode.OK)]
        public async Task TestAllOperationsInt(int value1, int value2, string operation, int expectedValue,
            HttpStatusCode statusCode) =>
            await RunTest(value1.ToString(), value2.ToString(), operation, expectedValue.ToString(), statusCode);

        [Theory]
        [InlineData(15.6, 5.6, "Plus", 21.2, HttpStatusCode.OK)]
        [InlineData(15.6, 5.6, "Minus", 10, HttpStatusCode.OK)]
        [InlineData(15.6, 5.6, "Multiply", 87.36, HttpStatusCode.OK)]
        [InlineData(15.6, 5.6, "Divide", 2.7857, HttpStatusCode.OK)]
        public async Task TestAllOperationsDouble(double value1, double value2, string operation,
            double expectedValue, HttpStatusCode statusCode) =>
            await RunTest(value1.ToString(CultureInfo.InvariantCulture), value2.ToString(CultureInfo.InvariantCulture),
                operation, expectedValue.ToString(CultureInfo.InvariantCulture), statusCode);

        [Theory]
        [InlineData(15.6, 5.6, "Plus", 21.2, HttpStatusCode.OK)]
        [InlineData(15.6, 5.6, "Minus", 10, HttpStatusCode.OK)]
        [InlineData(15.6, 5.6, "Multiply", 87.36, HttpStatusCode.OK)]
        [InlineData(15.6, 5.6, "Divide", 2.7857, HttpStatusCode.OK)]
        public async Task TestAllOperationsDecimal(decimal value1, decimal value2, string operation,
            decimal expectedValue, HttpStatusCode statusCode) =>
            await RunTest(value1.ToString(CultureInfo.InvariantCulture), value2.ToString(CultureInfo.InvariantCulture),
                operation, expectedValue.ToString(CultureInfo.InvariantCulture), statusCode);

        [Theory]
        [InlineData("biba", "5.6", "Plus", "Could not parse value 'biba'", HttpStatusCode.BadRequest)]
        [InlineData("15.6", "boba", "Plus", "Could not parse value 'boba'", HttpStatusCode.BadRequest)]
        [InlineData("biba", "boba", "Plus", "Could not parse value 'biba'", HttpStatusCode.BadRequest)]
        [InlineData("biba", "5.6", "Minus", "Could not parse value 'biba'", HttpStatusCode.BadRequest)]
        [InlineData("15.6", "boba", "Minus", "Could not parse value 'boba'", HttpStatusCode.BadRequest)]
        [InlineData("biba", "boba", "Minus", "Could not parse value 'biba'", HttpStatusCode.BadRequest)]
        [InlineData("biba", "5.6", "Multiply", "Could not parse value 'biba'", HttpStatusCode.BadRequest)]
        [InlineData("15.6", "boba", "Multiply", "Could not parse value 'boba'", HttpStatusCode.BadRequest)]
        [InlineData("biba", "boba", "Multiply", "Could not parse value 'biba'", HttpStatusCode.BadRequest)]
        [InlineData("biba", "5.6", "Divide", "Could not parse value 'biba'", HttpStatusCode.BadRequest)]
        [InlineData("15.6", "boba", "Divide", "Could not parse value 'boba'", HttpStatusCode.BadRequest)]
        [InlineData("biba", "boba", "Divide", "Could not parse value 'biba'", HttpStatusCode.BadRequest)]
        [InlineData("15.6.6", "5.6", "Divide", "Could not parse value '15.6.6'", HttpStatusCode.BadRequest)]
        public async Task TestAllOperationsIncorrectValues(string value1, string value2, string operation,
            string expectedValue, HttpStatusCode statusCode) =>
            await RunTest(value1, value2, operation, expectedValue, statusCode);

        [Theory]
        [InlineData("15.6", "5.6", "@", "Could not parse value '@'", HttpStatusCode.BadRequest)]
        [InlineData("15.6", "5.6", "$", "Could not parse value '$'", HttpStatusCode.BadRequest)]
        [InlineData("15.6", "5.6", "^", "Could not parse value '^'", HttpStatusCode.BadRequest)]
        public async Task TestIncorrectOperation(string value1, string value2, string operation,
            string expectedValue, HttpStatusCode statusCode) =>
            await RunTest(value1, value2, operation, expectedValue, statusCode);

        [Fact]
        public async Task TestParserDividingByZero() =>
            await RunTest("15.6", "0", "Divide", "DivideByZero", HttpStatusCode.OK, true);

        [Theory]
        [InlineData("15.6", "5.6", "+", "wrongName", "value2", "operation", "Could not parse value1",
            HttpStatusCode.BadRequest)]
        [InlineData("15.6", "5.6", "-", "value1", "wrongName", "operation", "Could not parse value2",
            HttpStatusCode.BadRequest)]
        [InlineData("15.6", "5.6", "*", "value1", "value2", "wrongName", "Could not parse operation",
            HttpStatusCode.BadRequest)]
        public async Task TestIncorrectValueNames(string value1, string value2, string operation, string value1Name,
            string value2Name, string opName,
            string expectedValue, HttpStatusCode statusCode) =>
            await RunTestWithCustomUrl(value1, value2, operation, value1Name,opName,value2Name,expectedValue, statusCode);
    

    private async Task RunTest(string value1, string value2, string operation, string expectedValueOrError,
            HttpStatusCode statusCode, bool isDividingByZero = false)
        {
            // arrange
            var url = $"/calculate?value1={value1}&operation={operation}&value2={value2}";

            // act
            var response = await GetResponse(url);

            // assert
            await AssertResponse(response, statusCode,expectedValueOrError,isDividingByZero);
        }


        private async Task RunTestWithCustomUrl(string value1, string value2, string operation,
            string value1Name, string opName, string value2Name, string expectedValueOrError,
            HttpStatusCode statusCode, bool isDividingByZero = false)
        {
            // arrange
            var url = $"/calculate?{value1Name}={value1}&{opName}={operation}&{value2Name}={value2}";

            // act
            var response = await GetResponse(url);

            // assert
            await AssertResponse(response, statusCode, expectedValueOrError, isDividingByZero);
        }

        private async Task<HttpResponseMessage> GetResponse(string url)
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync(url);
            return response;
        }
        private static async Task AssertResponse(HttpResponseMessage response, HttpStatusCode statusCode, string expectedValueOrError, bool isDividingByZero)
        {
            var result = await response.Content.ReadAsStringAsync();
            Assert.True(response.StatusCode == statusCode);
            if (statusCode == HttpStatusCode.OK && !isDividingByZero)
                Assert.True(Math.Abs(decimal.Parse(expectedValueOrError, CultureInfo.InvariantCulture) -
                                     decimal.Parse(result, CultureInfo.CurrentCulture)) < Epsilon);
            else
                Assert.Contains(expectedValueOrError, result);
        }
    }
}