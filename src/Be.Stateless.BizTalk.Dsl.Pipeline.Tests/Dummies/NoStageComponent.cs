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
using Be.Stateless.BizTalk.Component;
using Microsoft.BizTalk.Component.Interop;
using Microsoft.BizTalk.Message.Interop;

namespace Be.Stateless.BizTalk.Dummies
{
	[ComponentCategory(CategoryTypes.CATID_PipelineComponent)]
	internal class NoStageComponent : PipelineComponent
	{
		#region Base Class Member Overrides

		public override string Description => throw new NotSupportedException();

		protected override IBaseMessage ExecuteCore(IPipelineContext pipelineContext, IBaseMessage message)
		{
			throw new NotSupportedException();
		}

		public override void GetClassID(out Guid classId)
		{
			throw new NotSupportedException();
		}

		protected override void Load(IPropertyBag propertyBag)
		{
			throw new NotSupportedException();
		}

		protected override void Save(IPropertyBag propertyBag)
		{
			throw new NotSupportedException();
		}

		#endregion
	}
}
