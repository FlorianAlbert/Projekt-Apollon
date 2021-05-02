using System;
using AutoFixture;
using AutoFixture.AutoNSubstitute;
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

        [Fact]
        public void SendMail_Succeed()
        {
            var fluentMailFactory = Substitute.For<IFluentEmailFactory>();
        }
    }
}