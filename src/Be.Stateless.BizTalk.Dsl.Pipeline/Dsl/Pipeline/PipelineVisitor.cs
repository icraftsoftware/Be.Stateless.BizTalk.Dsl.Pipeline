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
using Microsoft.BizTalk.PipelineEditor.PipelineFile;

namespace Be.Stateless.BizTalk.Dsl.Pipeline
{
	public abstract class PipelineVisitor : IPipelineVisitor
	{
		#region IPipelineVisitor Members

		public virtual void VisitPipeline<T>(Pipeline<T> pipeline) where T : IPipelineStageList
		{
			if (pipeline == null) throw new ArgumentNullException(nameof(pipeline));
			Document = CreatePipelineDocument(pipeline);
		}

		public virtual void VisitStage(IStage stage)
		{
			if (stage == null) throw new ArgumentNullException(nameof(stage));
			_stageDocument = CreateStageDocument(stage);
			Document.Stages.Add(_stageDocument);
		}

		public virtual void VisitComponent(IPipelineComponentDescriptor componentDescriptor)
		{
			if (componentDescriptor == null) throw new ArgumentNullException(nameof(componentDescriptor));
			var componentInfo = CreateComponentInfo(componentDescriptor);
			_stageDocument.Components.Add(componentInfo);
		}

		#endregion

		public Document Document { get; private set; }

		protected abstract ComponentInfo CreateComponentInfo(IPipelineComponentDescriptor componentDescriptor);

		protected abstract Document CreatePipelineDocument<T>(Pipeline<T> pipeline) where T : IPipelineStageList;

		protected abstract Microsoft.BizTalk.PipelineEditor.PipelineFile.Stage CreateStageDocument(IStage stage);

		private Microsoft.BizTalk.PipelineEditor.PipelineFile.Stage _stageDocument;
	}
}
