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

namespace Be.Stateless.BizTalk.Dsl.Pipeline
{
	public class SendPipelineStageList : StageList, ISendPipelineStageList
	{
		internal SendPipelineStageList()
		{
			PreAssemble = Add(new Stage(StageCategory.Any.Id, PolicyFile.BTSTransmitPolicy.Value));
			Assemble = Add(new Stage(StageCategory.AssemblingSerializer.Id, PolicyFile.BTSTransmitPolicy.Value));
			Encode = Add(new Stage(StageCategory.Encoder.Id, PolicyFile.BTSTransmitPolicy.Value));
		}

		#region ISendPipelineStageList Members

		public IStage PreAssemble { get; }

		public IStage Assemble { get; }

		public IStage Encode { get; }

		#endregion
	}
}
