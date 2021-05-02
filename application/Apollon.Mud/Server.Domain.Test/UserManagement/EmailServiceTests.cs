using System;
using System.Threading.Tasks;
using Apollon.Mud.Server.Domain.Implementations.UserManagement;
using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Kernel;
using FluentEmail.Core;
using NSubstitute;
using Xunit;

namespace Apollon.Mud.Server.Domain.Test.UserManagement
{
    public class EmailServiceTests
    {
        //ToDo Tests
        private IFixture _Fixture;

        public EmailServiceTests()
        {
            _Fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
        }

        /*[Fact]
        public async Task SendMail_Succeed()
        {
            //ToDo strange error from resharper
            var email = "noreply@apollon-dungeons.de";
            var subject = _Fixture.Create<string>();
            var message = _Fixture.Create<string>();
            var fluentMailFactory = Substitute.For<IFluentEmailFactory>();

            var emailService = new EmailService(fluentMailFactory); 
            await emailService.SendEmail(email, message, subject);

            fluentMailFactory.Received().Create();
        }*/
    }
}