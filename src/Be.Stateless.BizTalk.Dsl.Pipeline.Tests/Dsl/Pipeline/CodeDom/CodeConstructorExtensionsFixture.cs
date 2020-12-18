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
using FluentAssertions;
using Microsoft.BizTalk.PipelineEditor.PolicyFile;
using Xunit;
using PipelinePolicy = Microsoft.BizTalk.PipelineEditor.PolicyFile.Document;
using StagePolicy = Microsoft.BizTalk.PipelineEditor.PolicyFile.Stage;
using static Be.Stateless.Unit.DelegateFactory;

namespace Be.Stateless.BizTalk.Dsl.Pipeline.CodeDom
{
	public class CodeConstructorExtensionsFixture
	{
		[Fact]
		public void AddStageThrowsForUnsupportedStageExecutionMethod()
		{
			var pipelinePolicy = new PipelinePolicy {
				Stages = { new StagePolicy { StageIdGuid = StageCategory.Any.Id.ToString(), ExecutionMethod = ExecMethod.None } }
			};
			var stage = new Stage(StageCategory.Any.Id, pipelinePolicy) {
				StagePolicy = { ExecutionMethod = ExecMethod.None }
			};

			var sut = new CodeConstructor();

			Action(() => sut.AddStage(stage))
				.Should().Throw<ArgumentOutOfRangeException>()
				.WithMessage("Stage 'Any' Execution Method is not supported; only All and FirstMatch are supported.*");
		}
	}
}
