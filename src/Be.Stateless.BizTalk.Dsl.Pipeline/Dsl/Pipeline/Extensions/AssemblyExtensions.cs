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
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace Be.Stateless.BizTalk.Dsl.Pipeline.Extensions
{
	[SuppressMessage("ReSharper", "UnusedType.Global", Justification = "Public API.")]
	[SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "Public API.")]
	[SuppressMessage("ReSharper", "MemberCanBePrivate.Global", Justification = "Public API.")]
	public static class AssemblyExtensions
	{
		public static Type[] GetPipelineDefinitionTypes(this IEnumerable<string> assemblyPaths)
		{
			if (assemblyPaths == null) throw new ArgumentNullException(nameof(assemblyPaths));
			// see https://stackoverflow.com/a/1477899/1789441
			return assemblyPaths.Select(Assembly.LoadFile)
				// make sure all assemblies are loaded before proceeding with reflection
				.ToArray()
				.GetPipelineDefinitionTypes();
		}

		public static Type[] GetPipelineDefinitionTypes(this IEnumerable<Assembly> assemblies)
		{
			if (assemblies == null) throw new ArgumentNullException(nameof(assemblies));
			return assemblies
				// discard this assembly, i.e. Be.Stateless.BizTalk.Dsl.Pipeline
				.Where(a => a != Assembly.GetExecutingAssembly())
				.SelectMany(a => a.GetPipelineDefinitionTypes())
				.ToArray();
		}

		[SuppressMessage("ReSharper", "ReturnTypeCanBeEnumerable.Global")]
		public static Type[] GetPipelineDefinitionTypes(this Assembly assembly)
		{
			if (assembly == null) throw new ArgumentNullException(nameof(assembly));
			return assembly.GetTypes()
				.Where(t => t.IsConcrete())
				.Where(t => t.IsPipelineDefinition())
				.ToArray();
		}
	}
}
