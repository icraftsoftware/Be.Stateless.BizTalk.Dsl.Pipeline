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
using Microsoft.BizTalk.Component;
using Microsoft.BizTalk.PipelineEditor.PolicyFile;
using Moq;
using Xunit;
using PipelinePolicy = Microsoft.BizTalk.PipelineEditor.PolicyFile.Document;
using static Be.Stateless.DelegateFactory;

namespace Be.Stateless.BizTalk.Dsl.Pipeline
{
	public class StageFixture
	{
		[Fact]
		[SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
		public void CanOnlyCreateStageForKnownCategory()
		{
			Action(() => new Stage(Guid.NewGuid(), PolicyFile.BTSReceivePolicy.Value)).Should().Throw<KeyNotFoundException>();
		}

		[Fact]
		public void EnsureAtLeastComponentThrows()
		{
			var pipelinePolicy = new PipelinePolicy {
				Stages = {
					new Microsoft.BizTalk.PipelineEditor.PolicyFile.Stage {
						StageIdGuid = StageCategory.Decoder.Id.ToString(),
						MinOccurs = 1
					}
				}
			};
			var stage = new Stage(StageCategory.Decoder.Id, pipelinePolicy);

			Action(() => stage.As<IVisitable<IPipelineVisitor>>().Accept(new Mock<IPipelineVisitor>().Object))
				.Should().Throw<ArgumentException>()
				.WithMessage("Stage 'Decoder' should contain at least 1 components.");
		}

		[Fact]
		public void EnsureAtMostComponentThrows()
		{
			var pipelinePolicy = new PipelinePolicy {
				Stages = {
					new Microsoft.BizTalk.PipelineEditor.PolicyFile.Stage {
						StageIdGuid = StageCategory.AssemblingSerializer.Id.ToString(),
						ExecutionMethod = ExecMethod.FirstMatch,
						MaxOccurs = 1
					}
				}
			};
			var stage = new Stage(StageCategory.AssemblingSerializer.Id, pipelinePolicy);
			var component = new XmlAsmComp();
			stage.AddComponent(component);
			stage.AddComponent(component);

			Action(() => stage.As<IVisitable<IPipelineVisitor>>().Accept(new Mock<IPipelineVisitor>().Object))
				.Should().Throw<ArgumentException>()
				.WithMessage("Stage 'AssemblingSerializer' should contain at most 1 components.");
		}

		[Fact]
		public void EnsureUniqueComponentSucceedsIfStageExecutionMethodIsNotAll()
		{
			var stage = new Stage(StageCategory.DisassemblingParser.Id, PolicyFile.BTSReceivePolicy.Value);
			var component = new XmlDasmComp();

			stage.AddComponent(component);

			Action(() => stage.AddComponent(component)).Should().NotThrow();
		}

		[Fact]
		public void EnsureUniqueComponentThrowsIfStageExecutionMethodIsAll()
		{
			var stage = new Stage(StageCategory.Decoder.Id, PolicyFile.BTSReceivePolicy.Value);
			var component = new FailedMessageRoutingEnablerComponent();

			stage.AddComponent(component);

			Action(() => stage.AddComponent(component))
				.Should().Throw<ArgumentException>()
				.WithMessage($"Stage 'Decoder' has multiple '{component.GetType().FullName}' components.");
		}

		[Fact]
		public void FetchComponentFromStage()
		{
			var stage = new Stage(StageCategory.Decoder.Id, PolicyFile.BTSReceivePolicy.Value);
			var component = new FailedMessageRoutingEnablerComponent();

			stage.AddComponent(component);

			stage.Component<FailedMessageRoutingEnablerComponent>().Should().BeSameAs(component);
		}

		[Fact]
		[SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global")]
		public void WalkVisitorAccordingToPrescribedPath()
		{
			var failedMessageRoutingEnablerComponent = new FailedMessageRoutingEnablerComponent();
			var microPipelineComponent = new MicroPipelineComponent();

			var sut = new Stage(StageCategory.Any.Id, PolicyFile.BTSTransmitPolicy.Value);
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
