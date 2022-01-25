﻿#region Copyright & License

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

namespace Be.Stateless.BizTalk.Dsl.Pipeline
{
	public class ReceivePipelineStageList : StageList, IReceivePipelineStageList
	{
		internal ReceivePipelineStageList()
		{
			Decode = Add(new(StageCategory.Decoder.Id, PolicyFile.BTSReceivePolicy.Value));
			Disassemble = Add(new(StageCategory.DisassemblingParser.Id, PolicyFile.BTSReceivePolicy.Value));
			Validate = Add(new(StageCategory.Validator.Id, PolicyFile.BTSReceivePolicy.Value));
			ResolveParty = Add(new(StageCategory.PartyResolver.Id, PolicyFile.BTSReceivePolicy.Value));
		}

		#region IReceivePipelineStageList Members

		public IStage Decode { get; }

		public IStage Disassemble { get; }

		public IStage Validate { get; }

		public IStage ResolveParty { get; }

		#endregion
	}
}
