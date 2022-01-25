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

using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.BizTalk.Component.Interop;

namespace Be.Stateless.BizTalk.Dsl.Pipeline
{
	[SuppressMessage("ReSharper", "UnusedMethodReturnValue.Global", Justification = "Public DSL API.")]
	public interface IComponentList : IFluentInterface
	{
		int Count { get; }

		// IComponentUI support is required in compliance with Microsoft.BizTalk.PipelineEditor.PipelineCompiler::ValidateStage, Microsoft.BizTalk.PipelineOM, Version=3.0.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
		IComponentList Add<T>(T component) where T : IBaseComponent, IComponentUI, IPersistPropertyBag;

		bool Contains<T>() where T : IBaseComponent, IPersistPropertyBag;

		T Component<T>() where T : IBaseComponent, IPersistPropertyBag;

		IConfigurableComponent<T, IComponentList> ComponentAt<T>(int index) where T : IBaseComponent, IPersistPropertyBag;

		IComponentList Component<T>(Action<T> componentConfigurator) where T : IBaseComponent, IPersistPropertyBag;

		IComponentList FirstComponent<T>(Action<T> componentConfigurator) where T : IBaseComponent, IPersistPropertyBag;

		IComponentList SecondComponent<T>(Action<T> componentConfigurator) where T : IBaseComponent, IPersistPropertyBag;

		IComponentList ThirdComponent<T>(Action<T> componentConfigurator) where T : IBaseComponent, IPersistPropertyBag;

		IComponentList FourthComponent<T>(Action<T> componentConfigurator) where T : IBaseComponent, IPersistPropertyBag;

		IComponentList FifthComponent<T>(Action<T> componentConfigurator) where T : IBaseComponent, IPersistPropertyBag;
	}
}
