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
using FluentAssertions;
using Xunit;
using Stages = Microsoft.BizTalk.PipelineOM.Stage;

namespace Be.Stateless.BizTalk.Dsl.Pipeline
{
	public class StageCategoryFixture
	{
		[Fact]
		public void CategoryEqualityIsReflexive()
		{
			StageCategory.Decoder.Should().Be(StageCategory.Decoder);
		}

		[Fact]
		public void CategoryNameIsIrrelevant()
		{
			var id = Guid.NewGuid();
			var c1 = new StageCategory("c1", id);
			var c2 = new StageCategory("c2", id);
			c1.Should().Be(c2);
		}

		[Fact]
		public void CategoryWithDifferentIdAreNotEqual()
		{
			var c1 = new StageCategory("category", Guid.NewGuid());
			var c2 = new StageCategory("category", Guid.NewGuid());
			c1.Should().NotBe(c2);
		}

		[Fact]
		public void CategoryWithSameIdAreEqual()
		{
			var id = Guid.NewGuid();
			var c1 = new StageCategory("category", id);
			var c2 = new StageCategory("category", id);
			c1.Should().Be(c2);
		}

		[Fact]
		public void IsCompatibleWithAny()
		{
			StageCategory.Decoder.IsCompatibleWith(new[] { StageCategory.Encoder, StageCategory.Any }).Should().BeTrue();
		}

		[Fact]
		public void IsCompatibleWithItself()
		{
			StageCategory.Encoder.IsCompatibleWith(new[] { StageCategory.Any, StageCategory.Encoder }).Should().BeTrue();
		}

		[Fact]
		public void IsNotCompatibleWith()
		{
			StageCategory.Encoder.IsCompatibleWith(new[] { StageCategory.Decoder, StageCategory.PartyResolver }).Should().BeFalse();
		}

		[Fact]
		public void SanityCheck()
		{
			StageCategory.Any.Id.Should().Be(Stages.Any);
			StageCategory.AssemblingSerializer.Id.Should().Be(Stages.AssemblingSerializer);
			StageCategory.Decoder.Id.Should().Be(Stages.Decoder);
			StageCategory.DisassemblingParser.Id.Should().Be(Stages.DisassemblingParser);
			StageCategory.Encoder.Id.Should().Be(Stages.Encoder);
			StageCategory.PartyResolver.Id.Should().Be(Stages.PartyResolver);
			StageCategory.Validator.Id.Should().Be(Stages.Validator);
		}
	}
}
