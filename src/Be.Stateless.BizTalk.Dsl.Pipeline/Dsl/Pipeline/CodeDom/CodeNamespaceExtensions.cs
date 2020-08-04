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
using System.CodeDom;
using System.Reflection;

namespace Be.Stateless.BizTalk.Dsl.Pipeline.CodeDom
{
	internal static class CodeNamespaceExtensions
	{
		internal static void ImportNamespace<T>(this CodeNamespace @namespace)
		{
			if (@namespace == null) throw new ArgumentNullException(nameof(@namespace));
			@namespace.ImportNamespace(typeof(T));
		}

		internal static void ImportNamespace(this CodeNamespace @namespace, Type type)
		{
			if (@namespace == null) throw new ArgumentNullException(nameof(@namespace));
			if (type == null) throw new ArgumentNullException(nameof(type));
			@namespace.Imports.Add(new CodeNamespaceImport(type.Namespace));
		}

		internal static CodeTypeDeclaration AddPipelineClass<T>(this CodeNamespace @namespace, Pipeline<T> pipeline) where T : IPipelineStageList
		{
			if (@namespace == null) throw new ArgumentNullException(nameof(@namespace));
			if (pipeline == null) throw new ArgumentNullException(nameof(pipeline));
			var @class = new CodeTypeDeclaration {
				IsClass = true,
				// TODO ?? TypeAttributes.Serializable ??
				TypeAttributes = TypeAttributes.Public | TypeAttributes.Sealed | TypeAttributes.Serializable,
				Name = pipeline.GetType().Name,
				BaseTypes = {
					new CodeTypeReference(
						pipeline is ReceivePipeline
							? typeof(Microsoft.BizTalk.PipelineOM.ReceivePipeline)
							: typeof(Microsoft.BizTalk.PipelineOM.SendPipeline))
				}
			};
			@namespace.Types.Add(@class);
			return @class;
		}
	}
}
