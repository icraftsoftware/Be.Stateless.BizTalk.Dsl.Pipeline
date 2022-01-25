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

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Be.Stateless.BizTalk.Dsl.Pipeline.Extensions;
using Be.Stateless.Linq.Extensions;
using Microsoft.BizTalk.Component.Interop;

namespace Be.Stateless.BizTalk.Dsl.Pipeline
{
	public class ComponentList : List<IPipelineComponentDescriptor>, IComponentList, IVisitable<IPipelineVisitor>
	{
		internal ComponentList(Stage stage)
		{
			Stage = stage;
		}

		#region IComponentList Members

		IComponentList IComponentList.Add<T>(T component)
		{
			component.EnsureIsPipelineComponent();
			component.EnsureIsCompatibleWith(Stage.Category);
			base.Add(new PipelineComponentDescriptor<T>(component));
			return this;
		}

		public bool Contains<T>() where T : IBaseComponent, IPersistPropertyBag
		{
			return this.OfType<PipelineComponentDescriptor<T>>().Any();
		}

		public T Component<T>() where T : IBaseComponent, IPersistPropertyBag
		{
			return this.OfType<PipelineComponentDescriptor<T>>().SingleOrDefault()
				?? throw new InvalidOperationException($"Stage '{Stage.Category.Name}' has no '{typeof(T).Name}' component.");
		}

		public IConfigurableComponent<T, IComponentList> ComponentAt<T>(int index) where T : IBaseComponent, IPersistPropertyBag
		{
			return new ConfigurableComponent<T, IComponentList>(
				(PipelineComponentDescriptor<T>) this.ElementAtOrDefault(index)
				?? throw new InvalidOperationException($"Stage '{Stage.Category.Name}' has no '{typeof(T).Name}' component."),
				this);
		}

		public IComponentList Component<T>(Action<T> componentConfigurator) where T : IBaseComponent, IPersistPropertyBag
		{
			if (componentConfigurator == null) throw new ArgumentNullException(nameof(componentConfigurator));
			var component = Component<T>();
			componentConfigurator(component);
			return this;
		}

		public IComponentList FirstComponent<T>(Action<T> componentConfigurator) where T : IBaseComponent, IPersistPropertyBag
		{
			ComponentOfTypeAt<T>(0).Configure(componentConfigurator);
			return this;
		}

		public IComponentList SecondComponent<T>(Action<T> componentConfigurator) where T : IBaseComponent, IPersistPropertyBag
		{
			ComponentOfTypeAt<T>(1).Configure(componentConfigurator);
			return this;
		}

		public IComponentList ThirdComponent<T>(Action<T> componentConfigurator) where T : IBaseComponent, IPersistPropertyBag
		{
			ComponentOfTypeAt<T>(2).Configure(componentConfigurator);
			return this;
		}

		public IComponentList FourthComponent<T>(Action<T> componentConfigurator) where T : IBaseComponent, IPersistPropertyBag
		{
			ComponentOfTypeAt<T>(3).Configure(componentConfigurator);
			return this;
		}

		public IComponentList FifthComponent<T>(Action<T> componentConfigurator) where T : IBaseComponent, IPersistPropertyBag
		{
			ComponentOfTypeAt<T>(4).Configure(componentConfigurator);
			return this;
		}

		#endregion

		#region IVisitable<IPipelineVisitor> Members

		T IVisitable<IPipelineVisitor>.Accept<T>(T visitor)
		{
			this.Cast<IVisitable<IPipelineVisitor>>().ForEach(component => component.Accept(visitor));
			return visitor;
		}

		#endregion

		private Stage Stage { get; }

		[SuppressMessage("ReSharper", "UnusedMethodReturnValue.Global", Justification = "Public DSL API.")]
		public IComponentList Add<T>(T component) where T : IBaseComponent, IComponentUI, IPersistPropertyBag
		{
			return ((IComponentList) this).Add(component);
		}

		private IConfigurableComponent<T, IComponentList> ComponentOfTypeAt<T>(int index) where T : IBaseComponent, IPersistPropertyBag
		{
			return new ConfigurableComponent<T, IComponentList>(
				this.OfType<PipelineComponentDescriptor<T>>().ElementAtOrDefault(index)
				?? throw new InvalidOperationException($"Stage '{Stage.Category.Name}' has no '{typeof(T).Name}' component."),
				this);
		}
	}
}
