using ArchUnitNET.Fluent;
using ArchUnitNET.xUnit;
using TeckShop.Core.Events;

namespace Catalog.Arch.UnitTests.Domain
{
    public class DomainTests : ArchUnitBaseTest
    {
        [Fact]
        public void DomainEvents_Should_BeSealed()
        {
            ArchRuleDefinition
                .Classes()
                .That()
                .ImplementInterface(typeof(IDomainEvent))
                .Should()
                .BeSealed()
                .Check(Architecture);
        }
        [Fact]
        public void DomainEvents_Should_HaveDomainEventPrefix()
        {
            ArchRuleDefinition
                .Classes()
                .That()
                .ImplementInterface(typeof(IDomainEvent))
                .Should()
                .HaveNameEndingWith("DomainEvent")
                .Check(Architecture);
        }
    }
}
