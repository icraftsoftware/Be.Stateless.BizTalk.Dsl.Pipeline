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
using System.Linq;
using Microsoft.BizTalk.Component.Interop;
using Microsoft.BizTalk.PipelineEditor;

namespace Be.Stateless.BizTalk.Dsl.Pipeline
{
	/// <summary>
	/// BizTalk Server pipeline component wrapper to be used in conjunction with <see cref="IPipelineComponentDescriptor"/>.
	/// </summary>
	/// <typeparam name="T">
	/// The pipeline component <see cref="Type"/> to wrap.
	/// </typeparam>
	/// <remarks>
	/// This class is not meant to be used explicitly but only fulfills a Pipeline DSL scaffolding role.
	/// </remarks>
	internal class PipelineComponentDescriptor<T> : IPipelineComponentDescriptor
		where T : IBaseComponent, IPersistPropertyBag
	{
		#region Operators

		public static implicit operator T(PipelineComponentDescriptor<T> pipelineComponentDescriptor)
		{
			return pipelineComponentDescriptor._pipelineComponent;
		}

		#endregion

		public PipelineComponentDescriptor(T pipelineComponent)
		{
			_pipelineComponent = pipelineComponent;
		}

		#region IPipelineComponentDescriptor Members

		public string AssemblyQualifiedName => typeof(T).AssemblyQualifiedName;

		public string FullName => typeof(T).FullName;

		IPropertyBag IPipelineComponentDescriptor.Properties { get; set; }

		IEnumerable<PropertyContents> IPipelineComponentDescriptor.PropertyContents
		{
			get
			{
				var bag = new PropertyBag();
				Save(bag, false, false);
				return bag.Properties.Cast<PropertyContents>().ToArray();
			}
		}

		T1 IVisitable<IPipelineVisitor>.Accept<T1>(T1 visitor)
		{
			visitor.VisitComponent(this);
			return visitor;
		}

		public string Name => _pipelineComponent.Name;

		public string Description => _pipelineComponent.Description;

		public string Version => _pipelineComponent.Version;

		public void GetClassID(out Guid classID)
		{
			_pipelineComponent.GetClassID(out classID);
		}

		public void InitNew()
		{
			_pipelineComponent.InitNew();
		}

		public void Load(IPropertyBag propertyBag, int errorLog)
		{
			_pipelineComponent.Load(propertyBag, errorLog);
		}

		public void Save(IPropertyBag propertyBag, bool clearDirty, bool saveAllProperties)
		{
			_pipelineComponent.Save(propertyBag, clearDirty, saveAllProperties);
		}

		#endregion

		private readonly T _pipelineComponent;
	}
}
