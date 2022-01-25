﻿#region Copyright & License

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

using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.BizTalk.PipelineEditor.PipelineFile;

namespace Be.Stateless.BizTalk.Dsl.Pipeline.Xml.Serialization
{
	public abstract class PipelineSerializer : IDslSerializer
	{
		protected PipelineSerializer(IVisitable<IPipelineVisitor> pipeline)
		{
			Pipeline = pipeline;
		}

		#region IDslSerializer Members

		public string Serialize()
		{
			using (var writer = new StringWriter())
			using (var xmlTextWriter = new XmlTextWriter(writer))
			{
				Serialize(xmlTextWriter);
				return writer.ToString();
			}
		}

		public void Save(string filePath)
		{
			using (var file = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.Read))
			{
				Write(file);
			}
		}

		public void Write(Stream stream)
		{
			using (var xmlTextWriter = new XmlTextWriter(stream, Encoding.Unicode))
			{
				Serialize(xmlTextWriter);
			}
		}

		#endregion

		protected IVisitable<IPipelineVisitor> Pipeline { get; }

		private void Serialize(XmlTextWriter xmlTextWriter)
		{
			var pipelineDocument = CreatePipelineDocument();
			xmlTextWriter.Formatting = Formatting.Indented;
			xmlTextWriter.QuoteChar = '\'';
			var xmlSerializer = CreateXmlSerializer();
			xmlSerializer.Serialize(xmlTextWriter, pipelineDocument);
		}

		protected abstract Document CreatePipelineDocument();

		protected abstract XmlSerializer CreateXmlSerializer();
	}
}
