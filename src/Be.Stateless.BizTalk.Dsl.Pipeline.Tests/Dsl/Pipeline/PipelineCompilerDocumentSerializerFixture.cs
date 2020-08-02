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

using System.Reflection;
using System.Xml.Linq;
using Be.Stateless.BizTalk.Dsl.Pipeline.Dummies;
using Be.Stateless.BizTalk.Dsl.Pipeline.Extensions;
using Be.Stateless.Resources;
using FluentAssertions;
using Xunit;

namespace Be.Stateless.BizTalk.Dsl.Pipeline
{
	public class PipelineCompilerDocumentSerializerFixture
	{
		[Fact]
		public void SerializeMicroPipeline()
		{
			var pipelineDocument = new XmlMicroPipeline().GetPipelineCompilerDocumentSerializer();
			XDocument.Parse(pipelineDocument.Serialize()).Should().BeEquivalentTo(
				ResourceManager.Load(
					Assembly.GetExecutingAssembly(),
					$"{GetType().Namespace}.Resources.XmlMicroPipelineCompilerDocument.xml",
					XDocument.Load));
		}

		[Fact]
		public void SerializeRegularPipeline()
		{
			var pipelineDocument = new XmlRegularPipeline().GetPipelineCompilerDocumentSerializer();
			XDocument.Parse(pipelineDocument.Serialize()).Should().BeEquivalentTo(
				ResourceManager.Load(
					Assembly.GetExecutingAssembly(),
					$"{GetType().Namespace}.Resources.XmlRegularPipelineCompilerDocument.xml",
					XDocument.Load));
		}
	}
}
