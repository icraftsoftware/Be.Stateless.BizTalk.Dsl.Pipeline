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

using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Be.Stateless.BizTalk.Component;
using Be.Stateless.BizTalk.Dsl.Pipeline.Dummies;
using Be.Stateless.BizTalk.Dsl.Pipeline.Extensions;
using FluentAssertions;
using Microsoft.BizTalk.PipelineEditor;
using Microsoft.BizTalk.PipelineEditor.PipelineFile;
using Xunit;

namespace Be.Stateless.BizTalk.Dsl.Pipeline
{
	public class PipelineRuntimeDocumentBuilderVisitorFixture
	{
		[Fact]
		[SuppressMessage("ReSharper", "CoVariantArrayConversion")]
		public void CreateComponentInfo()
		{
			var componentDescriptor = new PipelineComponentDescriptor<FailedMessageRoutingEnablerComponent>(new FailedMessageRoutingEnablerComponent());

			var visitor = new VisitorSpy();
			var componentInfo = visitor.CreateComponentInfo(componentDescriptor);

			var expectedProperties = new[] {
				new PropertyContents("Enabled", true),
				new PropertyContents("EnableFailedMessageRouting", true),
				new PropertyContents("SuppressRoutingFailureReport", true)
			};

			componentInfo.QualifiedNameOrClassId.Should().Be(typeof(FailedMessageRoutingEnablerComponent).AssemblyQualifiedName);
			componentInfo.ComponentName.Should().Be(nameof(FailedMessageRoutingEnablerComponent));

			componentInfo.ComponentProperties
				.Cast<PropertyContents>()
				.Should().BeEquivalentTo(expectedProperties);
		}

		[Fact]
		public void CreatePipelineDocument()
		{
			var pipeline = new ReceivePipelineImpl();

			var visitor = new VisitorSpy();
			var pipelineDocument = visitor.CreatePipelineDocument(pipeline);

			pipelineDocument.PolicyFilePath.Should().Be(pipeline.GetPolicyFileName());
			pipelineDocument.Description.Should().Be("A receive pipeline.");
			pipelineDocument.MajorVersion.Should().Be(5);
			pipelineDocument.MinorVersion.Should().Be(6);
			pipelineDocument.FriendlyName.Should().Be(PolicyFile.BTSReceivePolicy.Value.FriendlyName);
			pipelineDocument.CategoryId.Should().Be(PolicyFile.BTSReceivePolicy.Value.CategoryId.ToString());
		}

		[Fact]
		public void CreateReceiveDecoderStageDocument()
		{
			var stage = new Stage(StageCategory.Decoder.Id, PolicyFile.BTSReceivePolicy.Value);

			var visitor = new VisitorSpy();
			var stageDocument = visitor.CreateStageDocument(stage);

			stageDocument.CategoryId.Should().Be(StageCategory.Decoder.Id);
			stageDocument.PolicyFileStage.Should().BeEquivalentTo(PolicyFile.BTSReceivePolicy.Value.Stages[0]);
		}

		[Fact]
		public void CreateReceiveDisassemblerStageDocument()
		{
			var stage = new Stage(StageCategory.DisassemblingParser.Id, PolicyFile.BTSReceivePolicy.Value);

			var visitor = new VisitorSpy();
			var stageDocument = visitor.CreateStageDocument(stage);

			stageDocument.CategoryId.Should().Be(StageCategory.DisassemblingParser.Id);
			stageDocument.PolicyFileStage.Should().BeEquivalentTo(PolicyFile.BTSReceivePolicy.Value.Stages[1]);
		}

		[Fact]
		public void CreateReceivePartyResolverStageDocument()
		{
			var stage = new Stage(StageCategory.PartyResolver.Id, PolicyFile.BTSReceivePolicy.Value);

			var visitor = new VisitorSpy();
			var stageDocument = visitor.CreateStageDocument(stage);

			stageDocument.CategoryId.Should().Be(StageCategory.PartyResolver.Id);
			stageDocument.PolicyFileStage.Should().BeEquivalentTo(PolicyFile.BTSReceivePolicy.Value.Stages[3]);
		}

		[Fact]
		public void CreateReceiveValidatorStageDocument()
		{
			var stage = new Stage(StageCategory.Validator.Id, PolicyFile.BTSReceivePolicy.Value);

			var visitor = new VisitorSpy();
			var stageDocument = visitor.CreateStageDocument(stage);

			stageDocument.CategoryId.Should().Be(StageCategory.Validator.Id);
			stageDocument.PolicyFileStage.Should().BeEquivalentTo(PolicyFile.BTSReceivePolicy.Value.Stages[2]);
		}

		[Fact]
		public void CreateSendAssemblerStageDocument()
		{
			var stage = new Stage(StageCategory.AssemblingSerializer.Id, PolicyFile.BTSTransmitPolicy.Value);

			var visitor = new VisitorSpy();
			var stageDocument = visitor.CreateStageDocument(stage);

			stageDocument.CategoryId.Should().Be(StageCategory.AssemblingSerializer.Id);
			stageDocument.PolicyFileStage.Should().BeEquivalentTo(PolicyFile.BTSTransmitPolicy.Value.Stages[1]);
		}

		[Fact]
		public void CreateSendEncoderStageDocument()
		{
			var stage = new Stage(StageCategory.Encoder.Id, PolicyFile.BTSTransmitPolicy.Value);

			var visitor = new VisitorSpy();
			var stageDocument = visitor.CreateStageDocument(stage);

			stageDocument.CategoryId.Should().Be(StageCategory.Encoder.Id);
			stageDocument.PolicyFileStage.Should().BeEquivalentTo(PolicyFile.BTSTransmitPolicy.Value.Stages[2]);
		}

		[Fact]
		public void CreateSendPreAssemblerStageDocument()
		{
			var stage = new Stage(StageCategory.Any.Id, PolicyFile.BTSTransmitPolicy.Value);

			var visitor = new VisitorSpy();
			var stageDocument = visitor.CreateStageDocument(stage);

			stageDocument.CategoryId.Should().Be(StageCategory.Any.Id);
			stageDocument.PolicyFileStage.Should().BeEquivalentTo(PolicyFile.BTSTransmitPolicy.Value.Stages[0]);
		}

		private class VisitorSpy : PipelineRuntimeDocumentBuilderVisitor
		{
			public new ComponentInfo CreateComponentInfo(IPipelineComponentDescriptor componentDescriptor)
			{
				return base.CreateComponentInfo(componentDescriptor);
			}

			public new Document CreatePipelineDocument<T>(Pipeline<T> pipeline) where T : IPipelineStageList
			{
				return base.CreatePipelineDocument(pipeline);
			}

			public new Microsoft.BizTalk.PipelineEditor.PipelineFile.Stage CreateStageDocument(IStage stage)
			{
				return base.CreateStageDocument(stage);
			}
		}
	}
}
