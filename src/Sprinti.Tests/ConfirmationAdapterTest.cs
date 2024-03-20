using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
using Sprinti.Api.Button;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprinti.Tests
{
    public class ConfirmationAdapterTest
    {
        [Fact]
        public async Task StartAsync_ReturnsExpectedResponseBody()
        {
            var ConfirmationAdapter = new ConfirmationAdapter();

            var cancellationTokenSource = new CancellationTokenSource();
            string responseBody = await ConfirmationAdapter.StartAsync(cancellationTokenSource.Token);

            Assert.NotNull(responseBody);
            Console.WriteLine($"Response Body: {responseBody}");
        }
    }
}
