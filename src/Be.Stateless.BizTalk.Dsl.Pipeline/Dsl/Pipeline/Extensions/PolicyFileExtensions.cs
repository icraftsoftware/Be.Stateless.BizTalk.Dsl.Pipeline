#region Copyright & License

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

using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Be.Stateless.BizTalk.Management;
using Be.Stateless.Reflection;
using Be.Stateless.Resources;
using Be.Stateless.Xml;
using Be.Stateless.Xml.Serialization;
using Microsoft.BizTalk.PipelineEditor.PolicyFile;

namespace Be.Stateless.BizTalk.Dsl.Pipeline.Extensions
{
	internal static class PolicyFileExtensions
	{
		internal static string GetPolicyFileName(this IVisitable<IPipelineVisitor> pipeline)
		{
			return pipeline is ReceivePipeline ? nameof(PolicyFile.BTSReceivePolicy) + ".xml" : nameof(PolicyFile.BTSTransmitPolicy) + ".xml";
		}

		internal static Document GetPolicyFileDocument(this IVisitable<IPipelineVisitor> pipeline)
		{
			return pipeline is ReceivePipeline ? PolicyFile.BTSReceivePolicy.Value : PolicyFile.BTSTransmitPolicy.Value;
		}

		internal static Document LoadPolicyDocument(string name)
		{
			// fallback on embedded resources to prevent build failure on build agent without BTS installed
			return BizTalkInstallation.IsInstalled
				? LoadPolicyFileDocument(name)
				: LoadPolicyResourceDocument(name);
		}

		internal static Document LoadPolicyFileDocument(string name)
		{
			// see Microsoft.BizTalk.PipelineEditor.PipelineFile.Document::Load, Microsoft.BizTalk.PipelineOM, Version=3.0.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
			var path = Path.Combine(BizTalkInstallation.DeveloperToolsPath, "Pipeline Policy Files", name);
			return (Document) Reflector.InvokeMethod(typeof(Document), "Load", path);
		}

		internal static Document LoadPolicyResourceDocument(string name)
		{
			// see Microsoft.BizTalk.PipelineEditor.PolicyFile.Document::Load, Microsoft.BizTalk.PipelineOM, Version=3.0.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
			var xmlSchemas = (XmlSchemas) Reflector.GetProperty<Document>("SchemasForObjectModel");
			using (var stream = ResourceManager.Load(Assembly.GetExecutingAssembly(), $"{typeof(IStage).Namespace}.Resources.{name}"))
			using (var xmlReader = XmlReader.Create(stream, ValidatingXmlReaderSettings.Create(XmlSchemaContentProcessing.Strict, xmlSchemas.ToArray())))
			{
				var xmlSerializer = CachingXmlSerializerFactory.Create(typeof(Document));
				return (Document) xmlSerializer.Deserialize(xmlReader);
			}
		}
	}
}
