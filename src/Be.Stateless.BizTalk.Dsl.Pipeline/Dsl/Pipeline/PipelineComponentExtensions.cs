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
using System.Linq;
using Microsoft.BizTalk.Component.Interop;

namespace Be.Stateless.BizTalk.Dsl.Pipeline
{
	internal static class PipelineComponentExtensions
	{
		internal static StageCategory[] GetStageCategories<T>(this T component) where T : IBaseComponent, IPersistPropertyBag
		{
			component.EnsureIsPipelineComponent();

			var stageCategories = component.GetType().GetCustomAttributes(typeof(ComponentCategoryAttribute), false)
				.Cast<ComponentCategoryAttribute>()
				.Where(a => StageCategory.IsKnownCategoryId(a.Category))
				.Select(a => StageCategory.FromKnownCategoryId(a.Category))
				.ToArray();

			if (!stageCategories.Any())
				throw new ArgumentException(
					$"{component.GetType().Name} has not been associated with a pipeline stage category. Apply the ComponentCategoryAttribute with one of the stage categories available through {typeof(CategoryTypes).FullName}.");
			return stageCategories;
		}

		internal static void EnsureIsCompatibleWith<T>(this T component, StageCategory containingStageCategory) where T : IBaseComponent, IPersistPropertyBag
		{
			var componentCategories = component.GetStageCategories();
			if (!containingStageCategory.IsCompatibleWith(componentCategories))
				throw new ArgumentException(
					$"{component.Name} is made for any of the {componentCategories.Aggregate(string.Empty, (names, sc) => names + ", " + sc.Name, names => names.Substring(2))} stages and is not compatible with a {containingStageCategory.Name} stage.",
					nameof(component));
		}

		[SuppressMessage("ReSharper", "StringLiteralTypo")]
		internal static void EnsureIsPipelineComponent<T>(this T component) where T : IBaseComponent, IPersistPropertyBag
		{
			if (!component.IsPipelineComponent())
				throw new ArgumentException(
					$"{component.GetType().Name} is not categorized as a pipeline component. Apply the ComponentCategoryAttribute with a category of {typeof(CategoryTypes).FullName}.CATID_PipelineComponent.");
		}

		private static bool IsPipelineComponent<T>(this T component) where T : IBaseComponent, IPersistPropertyBag
		{
			return component.GetType().GetCustomAttributes(typeof(ComponentCategoryAttribute), false)
				.Cast<ComponentCategoryAttribute>()
				.Any(a => new Guid(CategoryTypes.CATID_PipelineComponent) == a.Category);
		}
	}
}
