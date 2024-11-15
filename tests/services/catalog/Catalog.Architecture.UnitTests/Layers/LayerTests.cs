using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArchUnitNET.Fluent;
using ArchUnitNET.xUnit;
using TeckShop.Core.Events;

namespace Catalog.Arch.UnitTests.Layers
{
    public class LayerTests : ArchUnitBaseTest
    {
        [Fact]
        public void DomainLayer_ShouldNot_HaveDependencyOn_ApplicationLayer()
        {
            ArchRuleDefinition
                .Types()
                .That()
                .Are(DomainLayer)
                .Should()
                .NotDependOnAny(ApplicationLayer)
                .Check(Architecture);
        }
        [Fact]
        public void DomainLayer_ShouldNot_HaveDependencyOn_InfrastructureLayer()
        {
            ArchRuleDefinition
                .Types()
                .That()
                .Are(DomainLayer)
                .Should()
                .NotDependOnAny(InfrastructureLayer)
                .Check(Architecture);
        }
        [Fact]
        public void ApplicationLayer_ShouldNot_HaveDependencyOn_PresentationLayer()
        {
            ArchRuleDefinition
                .Types()
                .That()
                .Are(DomainLayer)
                .Should()
                .NotDependOnAny(PresentationLayer)
                .Check(Architecture);
        }
        [Fact]
        public void ApplicationLayer_ShouldNot_HaveDependencyOn_InfrastructureLayer()
        {
            ArchRuleDefinition
                .Types()
                .That()
                .Are(ApplicationLayer)
                .Should()
                .NotDependOnAny(InfrastructureLayer)
                .Check(Architecture);
        }
        [Fact]
        public void InfrastructureLayer_ShouldNot_HaveDependencyOn_PresentationLayer()
        {
            ArchRuleDefinition
                .Types()
                .That()
                .Are(InfrastructureLayer)
                .Should()
                .NotDependOnAny(PresentationLayer)
                .Check(Architecture);
        }
    }
}
