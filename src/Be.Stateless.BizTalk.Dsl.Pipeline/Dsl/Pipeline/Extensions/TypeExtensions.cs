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

namespace Be.Stateless.BizTalk.Dsl.Pipeline.Extensions
{
	internal static class TypeExtensions
	{
		internal static ReceivePipeline AsReceivePipeline(this Type pipelineType)
		{
			if (pipelineType == null) throw new ArgumentNullException(nameof(pipelineType));
			if (typeof(ReceivePipeline).IsAssignableFrom(pipelineType)) return (ReceivePipeline) Activator.CreateInstance(pipelineType);
			if (typeof(SendPipeline).IsAssignableFrom(pipelineType)) return null;
			throw new ArgumentException(BuildMessage(pipelineType));
		}

		internal static SendPipeline AsSendPipeline(this Type pipelineType)
		{
			if (pipelineType == null) throw new ArgumentNullException(nameof(pipelineType));
			if (typeof(ReceivePipeline).IsAssignableFrom(pipelineType)) return null;
			if (typeof(SendPipeline).IsAssignableFrom(pipelineType)) return (SendPipeline) Activator.CreateInstance(pipelineType);
			throw new ArgumentException(BuildMessage(pipelineType));
		}

		private static string BuildMessage(Type pipelineType)
		{
			return $"'{pipelineType.FullName}' does not derive from neither '{typeof(ReceivePipeline).FullName}' nor '{typeof(SendPipeline).FullName}'.";
		}
	}
}
