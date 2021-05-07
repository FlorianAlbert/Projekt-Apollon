using System;
using System.Collections.Generic;
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
        private readonly IFixture _Fixture;

        public EmailServiceTests()
        {
            _Fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
        }

        [Fact]
        public async Task SendMail_NothingNull_Succeed()
        {
            var userEmail = _Fixture.Create<string>();
            var subject = _Fixture.Create<string>();
            var message = _Fixture.Create<string>();
            var fluentMailFactory = Substitute.For<IFluentEmailFactory>();

            var emailService = new EmailService(fluentMailFactory); 
            await emailService.SendEmail(userEmail, message, subject);

            await fluentMailFactory
                .Create()
                .Received()
                .To(userEmail)
                .Subject(subject)
                .Body(message)
                .SendAsync();
        }

        [Theory]
        [InlineData(null, "","")]
        [InlineData("noreply@apollon-dungeons.de", null, "")]
        [InlineData("noreply@apollon-dungeons.de", "", null)]
        public async Task SendMail_SomethingNull_Fails(string userEmail, string message, string subject)
        {
            var fluentMailFactory = Substitute.For<IFluentEmailFactory>();

            var emailService = new EmailService(fluentMailFactory);
            await emailService.SendEmail(userEmail, message, subject);

            await fluentMailFactory
                .Create()
                .DidNotReceive()
                .To(userEmail)
                .Subject(subject)
                .Body(message)
                .SendAsync();
        }

        [Theory]
        [InlineData(false, null, "")]
        [InlineData(false, "", null)]
        [InlineData(true, "", "")]
        public async Task BroadcastEmail_ParameterNull_Fails(bool userEmailsNull, string message, string subject)
        {
            var email = _Fixture.Create<string>();
            var userEmails = userEmailsNull ? null : new List<string>{ email };
            var fluentMailFactory = Substitute.For<IFluentEmailFactory>();

            var emailService = new EmailService(fluentMailFactory);
            await emailService.BroadcastEmail(userEmails, message, subject);

            await fluentMailFactory
                .Create()
                .DidNotReceive()
                .To(email)
                .Subject(subject)
                .Body(message)
                .SendAsync();
        }

        [Fact]
        public async Task BroadcastEmail_OneUserEmailNull_SucceedsTwice()
        {
            var email = _Fixture.Create<string>();
            var userEmail = _Fixture.Create<string>();

            var userEmails = new List<string> { email, userEmail, null };

            var subject = _Fixture.Create<string>();
            var message = _Fixture.Create<string>();
            
            var fluentMailFactory = Substitute.For<IFluentEmailFactory>();

            var emailService = new EmailService(fluentMailFactory);
            await emailService.BroadcastEmail(userEmails, message, subject);

            await fluentMailFactory
                .Create()
                .DidNotReceive()
                .To((string) null)
                .Subject(subject)
                .Body(message)
                .SendAsync();

            await fluentMailFactory
                .Create()
                .Received()
                .To(userEmail)
                .Subject(subject)
                .Body(message)
                .SendAsync();

            await fluentMailFactory
                .Create()
                .Received()
                .To(email)
                .Subject(subject)
                .Body(message)
                .SendAsync();
        }

        [Fact]
        public async Task BroadcastEmail_NothingNull_Succeed()
        {
            var email = _Fixture.Create<string>();
            var userEmail = _Fixture.Create<string>();

            var userEmails = new List<string> { email, userEmail };

            var subject = _Fixture.Create<string>();
            var message = _Fixture.Create<string>();

            var fluentMailFactory = Substitute.For<IFluentEmailFactory>();

            var emailService = new EmailService(fluentMailFactory);
            await emailService.BroadcastEmail(userEmails, message, subject);

            await fluentMailFactory
                .Create()
                .Received()
                .To(userEmail)
                .Subject(subject)
                .Body(message)
                .SendAsync();

            await fluentMailFactory
                .Create()
                .Received()
                .To(email)
                .Subject(subject)
                .Body(message)
                .SendAsync();
        }
    }
}