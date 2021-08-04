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

using System.Xml.Serialization;
using Be.Stateless.Xml.Serialization;
using Be.Stateless.Xml.Serialization.Extensions;
using Microsoft.BizTalk.PipelineEditor.PipelineFile;
using PolicyFileStage = Microsoft.BizTalk.PipelineEditor.PipelineFile.Stage;

namespace Be.Stateless.BizTalk.Dsl.Pipeline.Xml.Serialization
{
	public class PipelineRuntimeDocumentSerializer : PipelineSerializer
	{
		internal PipelineRuntimeDocumentSerializer(IVisitable<IPipelineVisitor> pipeline) : base(pipeline) { }

		#region Base Class Member Overrides

		protected override Document CreatePipelineDocument()
		{
			var visitor = new PipelineRuntimeDocumentBuilderVisitor();
			Pipeline.Accept(visitor);
			return visitor.Document;
		}

		protected override XmlSerializer CreateXmlSerializer()
		{
			var overrides = new XmlAttributeOverrides();
			overrides.Ignore<Document>(d => d.PolicyFilePath);
			overrides.Add<Document>(d => d.CategoryId, new() { XmlElements = { new XmlElementAttribute(nameof(Document.CategoryId), typeof(string)) } });
			overrides.Add<Document>(d => d.FriendlyName, new() { XmlElements = { new XmlElementAttribute(nameof(Document.FriendlyName), typeof(string)) } });
			overrides.Ignore<PolicyFileStage>(s => s.CategoryId);
			overrides.Add<PolicyFileStage>(
				s => s.PolicyFileStage,
				new() { XmlElements = { new XmlElementAttribute(nameof(PolicyFileStage.PolicyFileStage), typeof(Microsoft.BizTalk.PipelineEditor.PolicyFile.Stage)) } });
			return CachingXmlSerializerFactory.Create(typeof(Document), overrides);
		}

		#endregion
	}
}
