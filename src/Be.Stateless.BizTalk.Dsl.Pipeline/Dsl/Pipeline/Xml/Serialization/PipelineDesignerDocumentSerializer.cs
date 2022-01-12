﻿#region Copyright & License

// Copyright © 2012 - 2022 François Chabot
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
using Microsoft.BizTalk.PipelineEditor.PipelineFile;

namespace Be.Stateless.BizTalk.Dsl.Pipeline.Xml.Serialization
{
	public class PipelineDesignerDocumentSerializer : PipelineSerializer
	{
		internal PipelineDesignerDocumentSerializer(IVisitable<IPipelineVisitor> pipeline) : base(pipeline) { }

		#region Base Class Member Overrides

		protected override Document CreatePipelineDocument()
		{
			return Pipeline.Accept(new PipelineDesignerDocumentBuilderVisitor()).Document;
		}

		protected override XmlSerializer CreateXmlSerializer()
		{
			return CachingXmlSerializerFactory.Create(typeof(Document));
		}

		#endregion
	}
}
