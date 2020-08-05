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
using System.CodeDom;
using System.Diagnostics.CodeAnalysis;
using Be.Stateless.BizTalk.Dsl.Pipeline.Extensions;
using Be.Stateless.BizTalk.Dsl.Pipeline.Xml.Serialization;

namespace Be.Stateless.BizTalk.Dsl.Pipeline.CodeDom
{
	public static class PipelineExtensions
	{
		[SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "Public DSL API.")]
		public static CodeCompileUnit ConvertToCodeCompileUnit(this Type pipelineType)
		{
			return pipelineType.AsReceivePipeline()?.ConvertToCodeCompileUnit()
				?? pipelineType.AsSendPipeline()?.ConvertToCodeCompileUnit()
				?? throw new InvalidOperationException("Pipeline instantiation failure.");
		}

		public static CodeCompileUnit ConvertToCodeCompileUnit<T>(this Pipeline<T> pipeline) where T : IPipelineStageList
		{
			if (pipeline == null) throw new ArgumentNullException(nameof(pipeline));

			//see Microsoft.BizTalk.PipelineEditor.PipelineCompiler::GenerateCompilerOutput, Microsoft.BizTalk.PipelineOM, Version=3.0.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
			var @namespace = new CodeNamespace(pipeline.GetType().Namespace);
			var @class = @namespace.AddPipelineClass(pipeline);
			@class.AddConstructor(pipeline.Stages);
			@class.AddXmlContentProperty(pipeline.GetPipelineRuntimeDocumentSerializer().Serialize());
			@class.AddVersionDependentGuidProperty(pipeline.VersionDependentGuid);
			return new CodeCompileUnit { Namespaces = { @namespace } };
		}
	}
}
