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
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Be.Stateless.BizTalk.Component;
using FluentAssertions;
using Microsoft.BizTalk.PipelineEditor;
using Microsoft.BizTalk.PipelineEditor.PipelineFile;
using Xunit;

namespace Be.Stateless.BizTalk.Dsl.Pipeline
{
	public class PipelineDesignerDocumentBuilderVisitorFixture
	{
		[Fact]
		[SuppressMessage("ReSharper", "CoVariantArrayConversion")]
		public void CreateComponentInfo()
		{
			var componentDescriptor = new PipelineComponentDescriptor<FailedMessageRoutingEnablerComponent>(new FailedMessageRoutingEnablerComponent());

			var visitor = new Visitor();
			var componentInfo = visitor.CreateComponentInfo(componentDescriptor);

			var expectedProperties = new[] {
				new PropertyContents("Enabled", true),
				new PropertyContents("EnableFailedMessageRouting", true),
				new PropertyContents("SuppressRoutingFailureReport", true)
			};

			componentInfo.QualifiedNameOrClassId.Should().Be(typeof(FailedMessageRoutingEnablerComponent).FullName);
			componentInfo.ComponentName.Should().Be(nameof(FailedMessageRoutingEnablerComponent));

			componentInfo.ComponentProperties
				.Cast<PropertyContents>()
				.Should().BeEquivalentTo(expectedProperties);
		}

		[Fact]
		public void CreatePipelineDocument()
		{
			var pipeline = new ReceivePipelineImpl();

			var visitor = new Visitor();
			var pipelineDocument = visitor.CreatePipelineDocument(pipeline);

			pipelineDocument.PolicyFilePath.Should().Be("BTSReceivePolicy.xml");
			pipelineDocument.Description.Should().Be("A receive pipeline.");
			pipelineDocument.MajorVersion.Should().Be(5);
			pipelineDocument.MinorVersion.Should().Be(6);
		}

		[Fact]
		public void CreateStageDocument()
		{
			var stage = new Stage(StageCategory.Decoder.Id);

			var visitor = new Visitor();
			var stageDocument = visitor.CreateStageDocument(stage);

			stageDocument.CategoryId.Should().Be(StageCategory.Decoder.Id);
		}

		private class ReceivePipelineImpl : ReceivePipeline
		{
			public ReceivePipelineImpl()
			{
				Description = "A receive pipeline.";
				Version = new Version(5, 6);
			}
		}

		private class Visitor : PipelineDesignerDocumentBuilderVisitor
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
