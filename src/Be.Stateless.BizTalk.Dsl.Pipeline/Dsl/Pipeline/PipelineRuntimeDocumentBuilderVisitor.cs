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

using System.Diagnostics.CodeAnalysis;
using Be.Stateless.BizTalk.Dsl.Pipeline.Extensions;
using Microsoft.BizTalk.PipelineEditor.PipelineFile;
using StageDocument = Microsoft.BizTalk.PipelineEditor.PipelineFile.Stage;

namespace Be.Stateless.BizTalk.Dsl.Pipeline
{
	public class PipelineRuntimeDocumentBuilderVisitor : PipelineDesignerDocumentBuilderVisitor
	{
		#region Base Class Member Overrides

		[SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Done by corresponding base class' Visit method.")]
		protected override ComponentInfo CreateComponentInfo(IPipelineComponentDescriptor componentDescriptor)
		{
			var componentInfo = base.CreateComponentInfo(componentDescriptor);
			componentInfo.QualifiedNameOrClassId = componentDescriptor.AssemblyQualifiedName;
			return componentInfo;
		}

		[SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Done by corresponding base class' Visit method.")]
		protected override Document CreatePipelineDocument<T>(Pipeline<T> pipeline)
		{
			var pipelinePolicy = pipeline.GetPolicyFileDocument();
			var pipelineDocument = base.CreatePipelineDocument(pipeline);
			pipelineDocument.FriendlyName = pipelinePolicy.FriendlyName;
			pipelineDocument.CategoryId = pipelinePolicy.CategoryId.ToString();
			return pipelineDocument;
		}

		[SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Done by corresponding base class' Visit method.")]
		protected override StageDocument CreateStageDocument(IStage stage)
		{
			var stageDocument = base.CreateStageDocument(stage);
			stageDocument.PolicyFileStage = stage.StagePolicy;
			return stageDocument;
		}

		#endregion
	}
}
