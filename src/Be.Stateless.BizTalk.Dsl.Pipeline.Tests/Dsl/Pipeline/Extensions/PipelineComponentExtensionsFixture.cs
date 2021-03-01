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
using Be.Stateless.BizTalk.Component;
using Be.Stateless.BizTalk.Dummies;
using FluentAssertions;
using Moq;
using Xunit;
using static FluentAssertions.FluentActions;

namespace Be.Stateless.BizTalk.Dsl.Pipeline.Extensions
{
	public class PipelineComponentExtensionsFixture
	{
		[Fact]
		[SuppressMessage("ReSharper", "StringLiteralTypo")]
		public void ReflectCategoryFromNonPipelineComponentThrows()
		{
			var component = new Mock<PipelineComponent>();
			Invoking(() => component.Object.GetStageCategories()).Should()
				.Throw<ArgumentException>()
				.WithMessage(
					"PipelineComponentProxy is not categorized as a pipeline component. "
					+ "Apply the ComponentCategoryAttribute with a category of Microsoft.BizTalk.Component.Interop.CategoryTypes.CATID_PipelineComponent.");
		}

		[Fact]
		public void ReflectCategoryFromPipelineComponent()
		{
			var component = new FailedMessageRoutingEnablerComponent();
			component.GetStageCategories().Should().Contain(StageCategory.Any);
		}

		[Fact]
		public void ReflectCategoryFromPipelineComponentWithMultipleStageCategoriesDoesNotThrow()
		{
			var component = new MultipleStageComponent();
			Invoking(() => component.GetStageCategories()).Should().NotThrow();
		}

		[Fact]
		public void ReflectCategoryFromPipelineComponentWithoutStageCategoryThrows()
		{
			var component = new NoStageComponent();
			Invoking(() => component.GetStageCategories()).Should()
				.Throw<ArgumentException>()
				.WithMessage(
					"NoStageComponent has not been associated with a pipeline stage category. "
					+ "Apply the ComponentCategoryAttribute with one of the stage categories available through Microsoft.BizTalk.Component.Interop.CategoryTypes.");
		}
	}
}
