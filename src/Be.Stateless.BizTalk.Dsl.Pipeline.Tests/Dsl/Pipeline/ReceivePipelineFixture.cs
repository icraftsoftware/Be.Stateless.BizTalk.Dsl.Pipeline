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
using Be.Stateless.BizTalk.Component;
using Be.Stateless.BizTalk.Dsl.Pipeline.Extensions;
using FluentAssertions;
using Microsoft.BizTalk.Component;
using Xunit;
using static Be.Stateless.DelegateFactory;

namespace Be.Stateless.BizTalk.Dsl.Pipeline
{
	public class ReceivePipelineFixture
	{
		[Fact]
		public void ReceivePipelineDslGrammarVarianceEquivalence()
		{
			var pipelineDocument1 = new XmlReceiveVariant1().GetPipelineDesignerDocumentSerializer();
			var pipelineDocument2 = new XmlReceiveVariant2().GetPipelineDesignerDocumentSerializer();

			pipelineDocument1.Serialize().Should().Be(pipelineDocument2.Serialize());
		}

		[Fact]
		public void SerializeThrowsWhenComponentNotFound()
		{
			Action(() => new XmlReceiveVariant1().SecondDisassembler<XmlDasmComp>(null)).Should().Throw<ArgumentOutOfRangeException>();
		}

		private class XmlReceiveVariant1 : ReceivePipeline
		{
			public XmlReceiveVariant1()
			{
				Description = "XML receive micro pipeline.";
				Version = new Version(1, 0);
				Stages.Decode
					.AddComponent(new FailedMessageRoutingEnablerComponent { SuppressRoutingFailureReport = false })
					.AddComponent(new MicroPipelineComponent { Enabled = true });
				Stages.Disassemble
					.AddComponent(new XmlDasmComp());
			}
		}

		private class XmlReceiveVariant2 : ReceivePipeline
		{
			public XmlReceiveVariant2()
			{
				Description = "XML receive micro pipeline.";
				Version = new Version(1, 0);
				Decoders
					.Add(new FailedMessageRoutingEnablerComponent { SuppressRoutingFailureReport = false })
					.Add(new MicroPipelineComponent { Enabled = true });
				Disassemblers
					.Add(new XmlDasmComp());
			}
		}
	}
}
