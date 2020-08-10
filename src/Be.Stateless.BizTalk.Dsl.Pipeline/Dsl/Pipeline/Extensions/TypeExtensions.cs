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
using System.Reflection;

namespace Be.Stateless.BizTalk.Dsl.Pipeline.Extensions
{
	internal static class TypeExtensions
	{
		internal static ReceivePipeline AsReceivePipeline(this Type type)
		{
			if (type == null) throw new ArgumentNullException(nameof(type));
			if (type.IsReceivePipelineDefinition()) return (ReceivePipeline) Activator.CreateInstance(type);
			if (type.IsSendPipelineDefinition()) return null;
			throw new ArgumentException(BuildMessage(type));
		}

		internal static SendPipeline AsSendPipeline(this Type type)
		{
			if (type == null) throw new ArgumentNullException(nameof(type));
			if (type.IsReceivePipelineDefinition()) return null;
			if (type.IsSendPipelineDefinition()) return (SendPipeline) Activator.CreateInstance(type);
			throw new ArgumentException(BuildMessage(type));
		}

		internal static bool IsPipelineDefinition(this Type type)
		{
			return type.IsReceivePipelineDefinition() || type.IsSendPipelineDefinition();
		}

		private static bool IsReceivePipelineDefinition(this Type type)
		{
			return type.IsConcrete() && _dslBaseReceivePipelineType.IsAssignableFrom(type.GetTypeInfo());
		}

		private static bool IsSendPipelineDefinition(this Type type)
		{
			return type.IsConcrete() && _dslBaseSendPipelineType.IsAssignableFrom(type.GetTypeInfo());
		}

		internal static bool IsConcrete(this Type type)
		{
			return !type.GetTypeInfo().IsAbstract && !type.GetTypeInfo().IsGenericTypeDefinition;
		}

		private static string BuildMessage(Type type)
		{
			return $"'{type.FullName}' does not derive from neither '{typeof(ReceivePipeline).FullName}' nor '{typeof(SendPipeline).FullName}'.";
		}

		private static readonly TypeInfo _dslBaseReceivePipelineType = typeof(ReceivePipeline).GetTypeInfo();
		private static readonly TypeInfo _dslBaseSendPipelineType = typeof(SendPipeline).GetTypeInfo();
	}
}
