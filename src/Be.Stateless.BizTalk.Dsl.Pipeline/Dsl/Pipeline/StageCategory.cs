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
using Microsoft.BizTalk.PipelineOM;
using Stages = Microsoft.BizTalk.PipelineOM.Stage;

namespace Be.Stateless.BizTalk.Dsl.Pipeline
{
	public sealed class StageCategory : IEquatable<StageCategory>
	{
		#region Operators

		public static bool operator ==(StageCategory left, StageCategory right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(StageCategory left, StageCategory right)
		{
			return !Equals(left, right);
		}

		#endregion

		public static StageCategory Any => _stages[Stages.Any];

		public static StageCategory AssemblingSerializer => _stages[Stages.AssemblingSerializer];

		public static StageCategory Decoder => _stages[Stages.Decoder];

		public static StageCategory DisassemblingParser => _stages[Stages.DisassemblingParser];

		public static StageCategory Encoder => _stages[Stages.Encoder];

		public static StageCategory PartyResolver => _stages[Stages.PartyResolver];

		public static StageCategory Validator => _stages[Stages.Validator];

		public static bool IsKnownCategoryId(Guid categoryId)
		{
			return _stages.ContainsKey(categoryId);
		}

		public static StageCategory FromKnownCategoryId(Guid categoryId)
		{
			return _stages[categoryId];
		}

		internal StageCategory(string name, Guid id, ExecutionMode executionMode = ExecutionMode.all)
		{
			ExecutionMode = executionMode;
			Id = id;
			Name = name;
		}

		#region IEquatable<StageCategory> Members

		public bool Equals(StageCategory other)
		{
			return other is not null && (ReferenceEquals(this, other) || other.Id.Equals(Id));
		}

		#endregion

		#region Base Class Member Overrides

		public override bool Equals(object obj)
		{
			return obj is not null && (ReferenceEquals(this, obj) || obj is StageCategory category && Equals(category));
		}

		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}

		#endregion

		[SuppressMessage("ReSharper", "MemberCanBePrivate.Global", Justification = "Follow actual pipeline stage structure.")]
		[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global", Justification = "Follow actual pipeline stage structure.")]
		public ExecutionMode ExecutionMode { get; }

		public Guid Id { get; }

		public string Name { get; }

		public bool IsCompatibleWith(IEnumerable<StageCategory> stageCategories)
		{
			return stageCategories.Any(sc => sc == Any || sc == this);
		}

		private static readonly IDictionary<Guid, StageCategory> _stages = new Dictionary<Guid, StageCategory> {
			{ Stages.Any, new StageCategory("Any", Stages.Any) },
			{ Stages.AssemblingSerializer, new StageCategory("AssemblingSerializer", Stages.AssemblingSerializer) },
			{ Stages.Decoder, new StageCategory("Decoder", Stages.Decoder) },
			{ Stages.DisassemblingParser, new StageCategory("DisassemblingParser", Stages.DisassemblingParser, ExecutionMode.firstRecognized) },
			{ Stages.Encoder, new StageCategory("Encoder", Stages.Encoder) },
			{ Stages.PartyResolver, new StageCategory("PartyResolver", Stages.PartyResolver) },
			{ Stages.Validator, new StageCategory("Validator", Stages.Validator) }
		};
	}
}
