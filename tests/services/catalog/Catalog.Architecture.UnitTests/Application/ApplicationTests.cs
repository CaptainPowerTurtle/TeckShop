using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArchUnitNET.Fluent;
using ArchUnitNET.xUnit;
using Catalog.Arch.UnitTests;
using TeckShop.Core.CQRS;
using TeckShop.Core.Events;

namespace Catalog.Arch.UnitTests.Application
{
    public class ApplicationTests : ArchUnitBaseTest
    {
        [Fact]
        public void CommandHandler_ShouldHave_NameEndingWith_CommandHandler()
        {
            ArchRuleDefinition
                .Classes()
                .That()
                .ImplementInterface(typeof(ICommandHandler<,>))
                .Or()
                .ImplementInterface(typeof(ICommandHandler<>))
                .Should()
                .HaveNameEndingWith("CommandHandler")
                .Check(Architecture);
        }
        [Fact]
        public void CommandHandler_Should_NotBePublic()
        {
            ArchRuleDefinition
                .Classes()
                .That()
                .ImplementInterface(typeof(ICommandHandler<,>))
                .Or()
                .ImplementInterface(typeof(ICommandHandler<>))
                .Should()
                .NotBePublic()
                .Check(Architecture);
        }
        [Fact]
        public void QueryHandler_ShouldHave_NameEndingWith_QueryHandler()
        {
            ArchRuleDefinition
                .Classes()
                .That()
                .ImplementInterface(typeof(IQueryHandler<,>))
                .Should()
                .HaveNameEndingWith("QueryHandler")
                .Check(Architecture);
        }
        [Fact]
        public void QueryHandler_Should_NotBePublic()
        {
            ArchRuleDefinition
                .Classes()
                .That()
                .ImplementInterface(typeof(IQueryHandler<,>))
                .Should()
                .NotBePublic()
                .Check(Architecture);
        }
    }
}
