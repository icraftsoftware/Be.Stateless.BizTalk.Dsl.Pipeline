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
using System.Diagnostics.CodeAnalysis;
using Be.Stateless.BizTalk.Dsl.Pipeline;
using Be.Stateless.BizTalk.Dsl.Pipeline.Xml.Serialization;

namespace Be.Stateless.BizTalk.Unit.Dsl.Pipeline.Interpreters
{
	[SuppressMessage("ReSharper", "UnusedType.Global", Justification = "Public API")]
	public class SendPipelineInterpreter<T> : Microsoft.BizTalk.PipelineOM.SendPipeline
		where T : SendPipeline, new()
	{
		static SendPipelineInterpreter()
		{
			_pipelineDefinition = new();
		}

		#region Base Class Member Overrides

		public override Guid VersionDependentGuid
		{
			get
			{
				var versionDependentGuid = _pipelineDefinition.VersionDependentGuid;
				if (versionDependentGuid == Guid.Empty) throw new InvalidOperationException("VersionDependentGuid must be initialized when used by a pipeline interpreter.");
				return versionDependentGuid;
			}
		}

		public override string XmlContent => _pipelineDefinition.GetPipelineRuntimeDocumentSerializer().Serialize();

		#endregion

		private static readonly T _pipelineDefinition;
	}
}
