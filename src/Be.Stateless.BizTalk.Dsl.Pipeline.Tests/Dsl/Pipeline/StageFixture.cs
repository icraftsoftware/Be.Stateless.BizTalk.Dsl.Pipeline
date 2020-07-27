#region Copyright & License

// Copyright © 2012 - 2020 François Chabot
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Be.Stateless.BizTalk.Component;
using FluentAssertions;
using Moq;
using Xunit;
using static Be.Stateless.DelegateFactory;

namespace Be.Stateless.BizTalk.Dsl.Pipeline
{
	public class StageFixture
	{
		[Fact]
		[SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
		public void CanOnlyCreateStageForKnownCategory()
		{
			Action(() => new Stage(Guid.NewGuid())).Should().Throw<KeyNotFoundException>();
		}

		[Fact]
		public void FetchComponentFromStage()
		{
			var component = new FailedMessageRoutingEnablerComponent();

			var stage = new Stage(StageCategory.Decoder.Id);
			stage.AddComponent(component);

			stage.Component<FailedMessageRoutingEnablerComponent>().Should().BeSameAs(component);
		}

		[Fact]
		[SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global")]
		public void WalkVisitorAccordingToPrescribedPath()
		{
			var failedMessageRoutingEnablerComponent = new FailedMessageRoutingEnablerComponent();
			var microPipelineComponent = new MicroPipelineComponent();

			var sut = new Stage(StageCategory.Any.Id);
			sut.AddComponent(failedMessageRoutingEnablerComponent)
				.AddComponent(microPipelineComponent);

			var visitor = new Mock<IPipelineVisitor>(MockBehavior.Strict);
			var sequence = new MockSequence();
			visitor.InSequence(sequence).Setup(v => v.VisitStage(sut));
			visitor.InSequence(sequence).Setup(
				v => v.VisitComponent(
					It.Is<PipelineComponentDescriptor<FailedMessageRoutingEnablerComponent>>(
						c => ReferenceEquals((FailedMessageRoutingEnablerComponent) c, failedMessageRoutingEnablerComponent)))
			);
			visitor.InSequence(sequence).Setup(
				v => v.VisitComponent(
					It.Is<PipelineComponentDescriptor<MicroPipelineComponent>>(
						c => ReferenceEquals((MicroPipelineComponent) c, microPipelineComponent)))
			);

			((IVisitable<IPipelineVisitor>) sut).Accept(visitor.Object);

			visitor.Verify();
		}
	}
}
