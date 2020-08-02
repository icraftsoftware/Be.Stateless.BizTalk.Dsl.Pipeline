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
using Microsoft.BizTalk.Component.Interop;
using StagePolicy = Microsoft.BizTalk.PipelineEditor.PolicyFile.Stage;

namespace Be.Stateless.BizTalk.Dsl.Pipeline
{
	[SuppressMessage("ReSharper", "UnusedMethodReturnValue.Global", Justification = "Public DSL API.")]
	public interface IStage : IFluentInterface
	{
		StageCategory Category { get; }

		IComponentList Components { get; }

		StagePolicy StagePolicy { get; }

		IStage AddComponent<T>(T component) where T : IBaseComponent, IComponentUI, IPersistPropertyBag;

		[SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "Public DSL API.")]
		IStage AddComponent<T>(Action<T> componentConfigurator) where T : IBaseComponent, IComponentUI, IPersistPropertyBag, new();

		T Component<T>() where T : IBaseComponent, IPersistPropertyBag;

		IConfigurableComponent<T, IStage> ComponentAt<T>(int index) where T : IBaseComponent, IPersistPropertyBag;

		IStage Component<T>(Action<T> componentConfigurator) where T : IBaseComponent, IPersistPropertyBag;

		IStage FirstComponent<T>(Action<T> componentConfigurator) where T : IBaseComponent, IPersistPropertyBag;

		IStage SecondComponent<T>(Action<T> componentConfigurator) where T : IBaseComponent, IPersistPropertyBag;

		IStage ThirdComponent<T>(Action<T> componentConfigurator) where T : IBaseComponent, IPersistPropertyBag;

		IStage FourthComponent<T>(Action<T> componentConfigurator) where T : IBaseComponent, IPersistPropertyBag;

		IStage FifthComponent<T>(Action<T> componentConfigurator) where T : IBaseComponent, IPersistPropertyBag;
	}
}
