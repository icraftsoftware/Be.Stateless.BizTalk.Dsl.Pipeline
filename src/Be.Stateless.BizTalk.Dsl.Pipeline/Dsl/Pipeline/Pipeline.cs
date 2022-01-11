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

using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Be.Stateless.BizTalk.Dsl.Pipeline
{
	public abstract class Pipeline<T> : IFluentInterface, IVisitable<IPipelineVisitor>
		where T : IPipelineStageList
	{
		static Pipeline()
		{
			if (!typeof(IReceivePipelineStageList).IsAssignableFrom(typeof(T)) && !typeof(ISendPipelineStageList).IsAssignableFrom(typeof(T)))
				throw new ArgumentException(
					$"A pipeline does not support {typeof(T).Name} as a stage container because it does not derive from either IReceivePipelineStageList or ISendPipelineStageList.");
		}

		protected Pipeline(T stages)
		{
			Stages = stages;
			Version = new(1, 0);
			VersionDependentGuid = Guid.NewGuid();
		}

		#region IFluentInterface Members

		[SuppressMessage("ReSharper", "BaseObjectEqualsIsObjectEquals")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		[SuppressMessage("ReSharper", "BaseObjectGetHashCodeCallInGetHashCode")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		public override string ToString()
		{
			return base.ToString();
		}

		#endregion

		#region IVisitable<IPipelineVisitor> Members

		T1 IVisitable<IPipelineVisitor>.Accept<T1>(T1 visitor)
		{
			visitor.VisitPipeline(this);
			return ((IVisitable<IPipelineVisitor>) Stages).Accept(visitor);
		}

		#endregion

		public string Description { get; protected set; }

		public T Stages { get; }

		public Version Version { get; protected set; }

		public Guid VersionDependentGuid { get; set; }
	}
}
