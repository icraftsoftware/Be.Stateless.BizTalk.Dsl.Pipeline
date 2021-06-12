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

using System.IO;
using Be.Stateless.BizTalk.Dummies;
using Be.Stateless.BizTalk.Management;
using FluentAssertions;
using Microsoft.BizTalk.PipelineEditor.PolicyFile;
using Xunit;

namespace Be.Stateless.BizTalk.Dsl.Pipeline.Extensions
{
	public class PolicyFileExtensionsFixture
	{
		[Fact]
		public void GetReceivePolicyFileName()
		{
			new ReceivePipelineImpl().GetPolicyFileName().Should().Be("BTSReceivePolicy.xml");
		}

		[Fact]
		public void GetTransmitPolicyFileName()
		{
			new SendPipelineImpl().GetPolicyFileName().Should().Be("BTSTransmitPolicy.xml");
		}

		[SkippableFact]
		public void LoadReceivePolicyFileDocument()
		{
			Skip.IfNot(BizTalkInstallation.IsInstalled && Directory.Exists(BizTalkInstallation.DeveloperToolsPath), "BizTalk Server is not installed.");
			PolicyFileExtensions.LoadPolicyFileDocument("BTSReceivePolicy.xml").Should().BeOfType<Document>().And.NotBeNull();
		}

		[Fact]
		public void LoadReceivePolicyResourceDocument()
		{
			PolicyFileExtensions.LoadPolicyResourceDocument("BTSReceivePolicy.xml").Should().BeOfType<Document>().And.NotBeNull();
		}

		[SkippableFact]
		public void LoadTransmitPolicyFileDocument()
		{
			Skip.IfNot(BizTalkInstallation.IsInstalled && Directory.Exists(BizTalkInstallation.DeveloperToolsPath), "BizTalk Server is not installed.");
			PolicyFileExtensions.LoadPolicyFileDocument("BTSTransmitPolicy.xml").Should().BeOfType<Document>().And.NotBeNull();
		}

		[Fact]
		public void LoadTransmitPolicyResourceDocument()
		{
			PolicyFileExtensions.LoadPolicyResourceDocument("BTSTransmitPolicy.xml").Should().BeOfType<Document>().And.NotBeNull();
		}
	}
}
