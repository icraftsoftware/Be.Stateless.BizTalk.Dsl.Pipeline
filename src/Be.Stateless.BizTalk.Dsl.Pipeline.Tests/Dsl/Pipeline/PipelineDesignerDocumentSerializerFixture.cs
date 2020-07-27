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
using System.IO;
using System.Reflection;
using System.Xml.Linq;
using Be.Stateless.BizTalk.Component;
using Be.Stateless.BizTalk.ContextProperties;
using Be.Stateless.BizTalk.Dsl.Pipeline.Extensions;
using Be.Stateless.BizTalk.MicroComponent;
using Be.Stateless.BizTalk.Schema.Annotation;
using Be.Stateless.Resources;
using FluentAssertions;
using Microsoft.BizTalk.Component;
using Xunit;

namespace Be.Stateless.BizTalk.Dsl.Pipeline
{
	[SuppressMessage("ReSharper", "RedundantArgumentDefaultValue")]
	public class PipelineDesignerDocumentSerializerFixture
	{
		[Fact]
		public void SerializeMicroPipeline()
		{
			var pipelineDocument = new XmlMicroPipeline().GetPipelineDesignerDocumentSerializer();
			XDocument.Parse(pipelineDocument.Serialize()).Should().BeEquivalentTo(
				XDocument.Parse(
					ResourceManager.Load(
						Assembly.GetExecutingAssembly(),
						"Be.Stateless.BizTalk.Dsl.Pipeline.Data.XmlMicroPipelineDocument.xml",
						s => new StreamReader(s).ReadToEnd())));
		}

		[Fact]
		public void SerializeRegularPipeline()
		{
			var pipelineDocument = new XmlRegularPipeline().GetPipelineDesignerDocumentSerializer();
			XDocument.Parse(pipelineDocument.Serialize()).Should().BeEquivalentTo(
				XDocument.Parse(
					ResourceManager.Load(
						Assembly.GetExecutingAssembly(),
						"Be.Stateless.BizTalk.Dsl.Pipeline.Data.XmlRegularPipelineDocument.xml",
						s => new StreamReader(s).ReadToEnd())));
		}

		private class XmlRegularPipeline : ReceivePipeline
		{
			public XmlRegularPipeline()
			{
				Description = "XML receive regular pipeline.";
				Version = new Version(1, 0);
				Stages.Decode
					.AddComponent(
						new FailedMessageRoutingEnablerComponent {
							SuppressRoutingFailureReport = false
						});
				Stages.Disassemble
					.AddComponent(new XmlDasmComp());
			}
		}

		private class XmlMicroPipeline : ReceivePipeline
		{
			public XmlMicroPipeline()
			{
				Description = "XML receive micro pipeline.";
				Version = new Version(1, 0);
				Stages.Decode
					.AddComponent(
						new FailedMessageRoutingEnablerComponent {
							SuppressRoutingFailureReport = false
						})
					.AddComponent(
						new MicroPipelineComponent {
							Enabled = true,
							Components = new[] {
								new ContextPropertyExtractor {
									Extractors = new[] {
										new XPathExtractor(BizTalkFactoryProperties.SenderName.QName, "/letter/*/from", ExtractionMode.Promote),
										new XPathExtractor(BizTalkFactoryProperties.EnvironmentTag.QName, "/letter/*/paragraph", ExtractionMode.Write)
									}
								}
							}
						});
				Stages.Disassemble
					.AddComponent(new XmlDasmComp());
				Stages.Validate
					.AddComponent(new MicroPipelineComponent { Enabled = true });
			}
		}
	}
}
