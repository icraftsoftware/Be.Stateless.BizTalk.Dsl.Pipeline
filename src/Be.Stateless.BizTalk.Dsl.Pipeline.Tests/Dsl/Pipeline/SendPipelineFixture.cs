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
	public class SendPipelineFixture
	{
		[Fact]
		public void SendPipelineDslGrammarVarianceEquivalence()
		{
			var pipelineDocument1 = new XmlTransmitVariant1().GetPipelineDesignerDocumentSerializer();
			var pipelineDocument2 = new XmlTransmitVariant2().GetPipelineDesignerDocumentSerializer();

			pipelineDocument1.Serialize().Should().Be(pipelineDocument2.Serialize());
		}

		[Fact]
		public void SerializeThrowsWhenComponentNotFound()
		{
			Action(() => new XmlTransmitVariant1().SecondAssembler<XmlDasmComp>(null)).Should().Throw<ArgumentOutOfRangeException>();
		}

		private class XmlTransmitVariant1 : SendPipeline
		{
			public XmlTransmitVariant1()
			{
				Description = "XML send micro pipeline.";
				Version = new Version(1, 0);
				Stages.PreAssemble
					.AddComponent(new FailedMessageRoutingEnablerComponent { SuppressRoutingFailureReport = false })
					.AddComponent(new MicroPipelineComponent { Enabled = true });
				Stages.Assemble
					.AddComponent(new XmlAsmComp());
			}
		}

		private class XmlTransmitVariant2 : SendPipeline
		{
			public XmlTransmitVariant2()
			{
				Description = "XML send micro pipeline.";
				Version = new Version(1, 0);
				PreAssemblers
					.Add(new FailedMessageRoutingEnablerComponent { SuppressRoutingFailureReport = false })
					.Add(new MicroPipelineComponent { Enabled = true });
				Assemblers
					.Add(new XmlAsmComp());
			}
		}
	}
}
