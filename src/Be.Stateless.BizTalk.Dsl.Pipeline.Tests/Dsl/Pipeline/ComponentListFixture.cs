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

using System;
using Be.Stateless.BizTalk.Component;
using FluentAssertions;
using Microsoft.BizTalk.Component;
using Xunit;
using static FluentAssertions.FluentActions;

namespace Be.Stateless.BizTalk.Dsl.Pipeline
{
	public class ComponentListFixture
	{
		[Fact]
		public void AddComponentToCompatibleStage()
		{
			var list = new ComponentList(new(StageCategory.Decoder.Id, PolicyFile.BTSReceivePolicy.Value));
			Invoking(() => list.Add(new FailedMessageRoutingEnablerComponent())).Should().NotThrow();
		}

		[Fact]
		public void AddComponentToIncompatibleStageThrows()
		{
			var list = new ComponentList(new(StageCategory.Decoder.Id, PolicyFile.BTSReceivePolicy.Value));
			Invoking(() => list.Add(new PartyRes())).Should()
				.Throw<ArgumentException>()
				.WithMessage("Party resolution is made for any of the PartyResolver stages and is not compatible with a Decoder stage.*");
		}

		[Fact]
		public void FetchComponentFromComponentList()
		{
			var component = new FailedMessageRoutingEnablerComponent();
			var list = new ComponentList(new(StageCategory.Decoder.Id, PolicyFile.BTSReceivePolicy.Value)) {
				component
			};

			list.Component<FailedMessageRoutingEnablerComponent>().Should().BeSameAs(component);
		}

		[Fact]
		public void FetchUnregisteredComponentFromComponentListThrows()
		{
			var list = new ComponentList(new(StageCategory.Decoder.Id, PolicyFile.BTSReceivePolicy.Value)) {
				new FailedMessageRoutingEnablerComponent()
			};

			Invoking(() => list.Component<PartyRes>()).Should()
				.Throw<InvalidOperationException>()
				.WithMessage("Stage 'Decoder' has no 'PartyRes' component.");
		}
	}
}
