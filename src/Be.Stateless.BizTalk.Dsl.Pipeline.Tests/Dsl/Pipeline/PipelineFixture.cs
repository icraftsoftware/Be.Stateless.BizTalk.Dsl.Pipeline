#region Copyright & License

// Copyright © 2012 - 2021 François Chabot
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
using System.Diagnostics.CodeAnalysis;
using Be.Stateless.BizTalk.Dummies;
using FluentAssertions;
using Moq;
using Xunit;
using static FluentAssertions.FluentActions;

namespace Be.Stateless.BizTalk.Dsl.Pipeline
{
	public class PipelineFixture
	{
		[Fact]
		[SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
		public void CanOnlyCreateIReceiveOrISendPipelineStageListBasedPipelines()
		{
			Invoking(() => new InvalidPipeline()).Should()
				.Throw<Exception>().WithInnerException<ArgumentException>()
				.WithMessage(
					"A pipeline does not support IPipelineStageList as a stage container because it does not derive from either IReceivePipelineStageList or ISendPipelineStageList.");
		}

		[Fact]
		public void WalkVisitorAccordingToPrescribedPathForReceivePipeline()
		{
			var sut = new ReceivePipelineImpl();

			var visitor = new Mock<IPipelineVisitor>(MockBehavior.Strict);
			var sequence = new MockSequence();
			visitor.InSequence(sequence).Setup(v => v.VisitPipeline(sut));
			visitor.InSequence(sequence).Setup(v => v.VisitStage(sut.Stages.Decode));
			visitor.InSequence(sequence).Setup(v => v.VisitStage(sut.Stages.Disassemble));
			visitor.InSequence(sequence).Setup(v => v.VisitStage(sut.Stages.Validate));
			visitor.InSequence(sequence).Setup(v => v.VisitStage(sut.Stages.ResolveParty));

			((IVisitable<IPipelineVisitor>) sut).Accept(visitor.Object);

			visitor.Verify();
		}

		[Fact]
		public void WalkVisitorAccordingToPrescribedPathForSendPipeline()
		{
			var sut = new SendPipelineImpl();

			var visitor = new Mock<IPipelineVisitor>(MockBehavior.Strict);
			var sequence = new MockSequence();
			visitor.InSequence(sequence).Setup(v => v.VisitPipeline(sut));
			visitor.InSequence(sequence).Setup(v => v.VisitStage(sut.Stages.PreAssemble));
			visitor.InSequence(sequence).Setup(v => v.VisitStage(sut.Stages.Assemble));
			visitor.InSequence(sequence).Setup(v => v.VisitStage(sut.Stages.Encode));

			((IVisitable<IPipelineVisitor>) sut).Accept(visitor.Object);

			visitor.Verify();
		}

		private class InvalidPipeline : Pipeline<IPipelineStageList>
		{
			public InvalidPipeline() : base(new ReceivePipelineStageList()) { }
		}
	}
}
