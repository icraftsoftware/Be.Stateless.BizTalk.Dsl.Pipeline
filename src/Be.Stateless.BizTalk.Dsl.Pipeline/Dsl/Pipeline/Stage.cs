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

using System;
using System.Linq;
using Microsoft.BizTalk.Component.Interop;
using Microsoft.BizTalk.PipelineEditor.PolicyFile;
using PipelinePolicy = Microsoft.BizTalk.PipelineEditor.PolicyFile.Document;
using StagePolicy = Microsoft.BizTalk.PipelineEditor.PolicyFile.Stage;

namespace Be.Stateless.BizTalk.Dsl.Pipeline
{
	public class Stage : IStage, IVisitable<IPipelineVisitor>
	{
		internal Stage(Guid categoryId, PipelinePolicy pipelinePolicy)
		{
			Category = StageCategory.FromKnownCategoryId(categoryId);
			StagePolicy = pipelinePolicy.Stages.Cast<StagePolicy>().Single(s => new Guid(s.StageIdGuid) == Category.Id);
			Components = new ComponentList(this);
		}

		#region IStage Members

		public StageCategory Category { get; }

		public IComponentList Components { get; }

		public StagePolicy StagePolicy { get; }

		public IStage AddComponent<T>(T component) where T : IBaseComponent, IComponentUI, IPersistPropertyBag
		{
			// see Microsoft.BizTalk.PipelineEditor.PipelineCompiler::ValidateStage, Microsoft.BizTalk.PipelineOM, Version=3.0.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
			EnsureUniqueComponent(component);
			Components.Add(component);
			return this;
		}

		public IStage AddComponent<T>(Action<T> componentConfigurator) where T : IBaseComponent, IComponentUI, IPersistPropertyBag, new()
		{
			if (componentConfigurator == null) throw new ArgumentNullException(nameof(componentConfigurator));
			var component = new T();
			componentConfigurator(component);
			Components.Add(component);
			return this;
		}

		public T Component<T>() where T : IBaseComponent, IPersistPropertyBag
		{
			return Components.Component<T>();
		}

		public IConfigurableComponent<T, IStage> ComponentAt<T>(int index) where T : IBaseComponent, IPersistPropertyBag
		{
			return new ConfigurableComponent<T, IStage>((ConfigurableComponent<T, IComponentList>) Components.ComponentAt<T>(index), this);
		}

		public IStage Component<T>(Action<T> componentConfigurator) where T : IBaseComponent, IPersistPropertyBag
		{
			Components.Component(componentConfigurator);
			return this;
		}

		public IStage FirstComponent<T>(Action<T> componentConfigurator) where T : IBaseComponent, IPersistPropertyBag
		{
			Components.FirstComponent(componentConfigurator);
			return this;
		}

		public IStage SecondComponent<T>(Action<T> componentConfigurator) where T : IBaseComponent, IPersistPropertyBag
		{
			Components.SecondComponent(componentConfigurator);
			return this;
		}

		public IStage ThirdComponent<T>(Action<T> componentConfigurator) where T : IBaseComponent, IPersistPropertyBag
		{
			Components.ThirdComponent(componentConfigurator);
			return this;
		}

		public IStage FourthComponent<T>(Action<T> componentConfigurator) where T : IBaseComponent, IPersistPropertyBag
		{
			Components.FourthComponent(componentConfigurator);
			return this;
		}

		public IStage FifthComponent<T>(Action<T> componentConfigurator) where T : IBaseComponent, IPersistPropertyBag
		{
			Components.FifthComponent(componentConfigurator);
			return this;
		}

		#endregion

		#region IVisitable<IPipelineVisitor> Members

		void IVisitable<IPipelineVisitor>.Accept(IPipelineVisitor visitor)
		{
			// see Microsoft.BizTalk.PipelineEditor.PipelineCompiler::ValidateStage, Microsoft.BizTalk.PipelineOM, Version=3.0.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
			EnsureAtLeastComponent();
			EnsureAtMostComponent();
			visitor.VisitStage(this);
			((IVisitable<IPipelineVisitor>) Components).Accept(visitor);
		}

		#endregion

		private void EnsureAtLeastComponent()
		{
			// see Microsoft.BizTalk.PipelineEditor.PipelineCompiler::ValidateStage, Microsoft.BizTalk.PipelineOM, Version=3.0.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
			var minOccurs = StagePolicy.MinOccurs;
			if (Components.Count < minOccurs) throw new ArgumentException($"Stage '{Category.Name}' should contain at least {minOccurs} components.");
		}

		private void EnsureAtMostComponent()
		{
			// see Microsoft.BizTalk.PipelineEditor.PipelineCompiler::ValidateStage, Microsoft.BizTalk.PipelineOM, Version=3.0.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
			var maxOccurs = StagePolicy.MaxOccurs < 0 ? byte.MaxValue : StagePolicy.MaxOccurs;
			if (Components.Count > maxOccurs) throw new ArgumentException($"Stage '{Category.Name}' should contain at most {maxOccurs} components.");
		}

		private void EnsureUniqueComponent<T>(T component) where T : IBaseComponent, IPersistPropertyBag
		{
			// see Microsoft.BizTalk.PipelineEditor.PipelineCompiler::ValidateStage, Microsoft.BizTalk.PipelineOM, Version=3.0.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
			if (StagePolicy.ExecutionMethod == ExecMethod.All && Components.Contains<T>())
				throw new ArgumentException($"Stage '{Category.Name}' has multiple '{component.GetType().FullName}' components.");
		}
	}
}
