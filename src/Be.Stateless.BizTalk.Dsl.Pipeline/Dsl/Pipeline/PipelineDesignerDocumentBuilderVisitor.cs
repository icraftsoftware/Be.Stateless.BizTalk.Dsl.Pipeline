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
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Be.Stateless.BizTalk.Dsl.Pipeline.Extensions;
using Be.Stateless.Linq.Extensions;
using Microsoft.BizTalk.PipelineEditor;
using Microsoft.BizTalk.PipelineEditor.PipelineFile;
using StageDocument = Microsoft.BizTalk.PipelineEditor.PipelineFile.Stage;

namespace Be.Stateless.BizTalk.Dsl.Pipeline
{
	public class PipelineDesignerDocumentBuilderVisitor : IPipelineVisitor
	{
		#region IPipelineVisitor Members

		public void VisitPipeline<T>(Pipeline<T> pipeline) where T : IPipelineStageList
		{
			if (pipeline == null) throw new ArgumentNullException(nameof(pipeline));
			Document = CreatePipelineDocument(pipeline);
		}

		public void VisitStage(IStage stage)
		{
			if (stage == null) throw new ArgumentNullException(nameof(stage));
			_stageDocument = CreateStageDocument(stage);
			Document.Stages.Add(_stageDocument);
		}

		public void VisitComponent(IPipelineComponentDescriptor componentDescriptor)
		{
			if (componentDescriptor == null) throw new ArgumentNullException(nameof(componentDescriptor));
			var componentInfo = CreateComponentInfo(componentDescriptor);
			_stageDocument.Components.Add(componentInfo);
		}

		#endregion

		public Document Document { get; private set; }

		[SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Done by corresponding Visit method.")]
		protected virtual ComponentInfo CreateComponentInfo(IPipelineComponentDescriptor componentDescriptor)
		{
			var componentInfo = new ComponentInfo {
				QualifiedNameOrClassId = componentDescriptor.FullName,
				ComponentName = componentDescriptor.Name,
				Description = componentDescriptor.Description,
				Version = componentDescriptor.Version,
				CachedDisplayName = componentDescriptor.Name,
				CachedIsManaged = true
			};
			var bag = new PropertyBag();
			componentDescriptor.Save(bag, false, false);
			bag.Properties.Cast<PropertyContents>().ForEach(property => componentInfo.ComponentProperties.Add(property));
			return componentInfo;
		}

		[SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Done by corresponding Visit method.")]
		protected virtual Document CreatePipelineDocument<T>(Pipeline<T> pipeline) where T : IPipelineStageList
		{
			return new Document {
				PolicyFilePath = pipeline.GetPolicyFileName(),
				Description = pipeline.Description,
				MajorVersion = pipeline.Version.Major,
				MinorVersion = pipeline.Version.Minor
			};
		}

		[SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Done by corresponding Visit method.")]
		protected virtual StageDocument CreateStageDocument(IStage stage)
		{
			return new StageDocument { CategoryId = stage.Category.Id };
		}

		private StageDocument _stageDocument;
	}
}
