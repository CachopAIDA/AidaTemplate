using AidaTemplate.Api.UseCases;
using FluentAssertions;
using NUnit.Framework;

namespace AidaTemplate.Tests.Unit {
    public class GetHelloCommandHandlerShould {

        [Test]
        public void get_hello_message() {
            var getHelloCommandHandler = new GetSampleCommandHandler();

            var message = getHelloCommandHandler.Handle();

            message.Should().Be("Hello World!!");
        }

    }
}