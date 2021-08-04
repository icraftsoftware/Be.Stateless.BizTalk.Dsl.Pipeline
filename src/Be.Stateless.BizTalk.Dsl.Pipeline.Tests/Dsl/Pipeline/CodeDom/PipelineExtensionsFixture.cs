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
using System.CodeDom.Compiler;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Be.Stateless.BizTalk.Component;
using Be.Stateless.BizTalk.Dsl.Pipeline.Xml.Serialization;
using Be.Stateless.BizTalk.Dummies;
using Be.Stateless.IO.Extensions;
using Be.Stateless.Resources;
using FluentAssertions;
using Microsoft.BizTalk.Component;
using Microsoft.CSharp;
using Xunit;

namespace Be.Stateless.BizTalk.Dsl.Pipeline.CodeDom
{
	public class PipelineExtensionsFixture
	{
		[Fact]
		public void FFReceiveCodeCompileUnitCompiles()
		{
			using (var provider = new CSharpCodeProvider())
			{
				var results = provider.CompileAssemblyFromDom(CompilerParameters, new FFReceive().ConvertToPipelineRuntimeCodeCompileUnit());
				results.Errors.HasErrors.Should().BeFalse();
				results.Errors.Cast<object>().Should().BeEmpty();
			}
		}

		[Fact]
		public void FFTransmitCodeCompileUnitCompiles()
		{
			using (var provider = new CSharpCodeProvider())
			{
				var results = provider.CompileAssemblyFromDom(CompilerParameters, new FFTransmit().ConvertToPipelineRuntimeCodeCompileUnit());
				results.Errors.HasErrors.Should().BeFalse();
				results.Errors.Cast<object>().Should().BeEmpty();
			}
		}

		[Fact]
		public void PassThruReceiveCodeCompileUnitCompiles()
		{
			using (var provider = new CSharpCodeProvider())
			{
				var results = provider.CompileAssemblyFromDom(CompilerParameters, new PassThruReceive().ConvertToPipelineRuntimeCodeCompileUnit());
				results.Errors.HasErrors.Should().BeFalse();
				results.Errors.Cast<object>().Should().BeEmpty();
			}
		}

		[Fact]
		public void PassThruTransmitCodeCompileUnitCompiles()
		{
			using (var provider = new CSharpCodeProvider())
			{
				var results = provider.CompileAssemblyFromDom(CompilerParameters, new PassThruTransmit().ConvertToPipelineRuntimeCodeCompileUnit());
				results.Errors.HasErrors.Should().BeFalse();
				results.Errors.Cast<object>().Should().BeEmpty();
			}
		}

		[Fact]
		public void XmlMicroPipelineCodeCompileUnitCompiles()
		{
			using (var provider = new CSharpCodeProvider())
			{
				var results = provider.CompileAssemblyFromDom(CompilerParameters, new XmlMicroPipeline().ConvertToPipelineRuntimeCodeCompileUnit());
				results.Errors.HasErrors.Should().BeFalse();
				results.Errors.Cast<object>().Should().BeEmpty();
			}
		}

		[Fact]
		public void XmlMicroPipelineConvertsToCodeCompileUnit()
		{
			var pipeline = new XmlMicroPipeline { VersionDependentGuid = Guid.Parse("55a6e50d-1750-4ccd-8995-e5151b049a01") };

			var builder = new StringBuilder();
			using (var provider = new CSharpCodeProvider())
			using (var writer = new StringWriter(builder))
			{
				provider.GenerateCodeFromCompileUnit(
					pipeline.ConvertToPipelineRuntimeCodeCompileUnit(),
					writer,
					new() { BracingStyle = "C", IndentString = "\t", VerbatimOrder = true });
			}

			// be resilient to runtime version in CodeDom heading comment
			Regex.Replace(builder.ToString(), @"(//\s+)Runtime Version:\d\.\d\.\d+\.\d+", @"$1Runtime Version:4.0.30319.42000", RegexOptions.Multiline)
				.Should().Be(
					ResourceManager.Load(
						Assembly.GetExecutingAssembly(),
						"Be.Stateless.BizTalk.Resources.XmlMicroPipeline.btp.cs",
						// be resilient to XML Schema and Instance namespace declaration order by reusing pipeline runtime document as generated by the build server
						s => s.ReadToEnd()).Replace("$$PipelineRuntimeDocument$$", pipeline.GetPipelineRuntimeDocumentSerializer().Serialize()));
		}

		[Fact]
		public void XmlReceiveCodeCompileUnitCompiles()
		{
			using (var provider = new CSharpCodeProvider())
			{
				var results = provider.CompileAssemblyFromDom(CompilerParameters, new XmlReceive().ConvertToPipelineRuntimeCodeCompileUnit());
				results.Errors.HasErrors.Should().BeFalse();
				results.Errors.Cast<object>().Should().BeEmpty();
			}
		}

		[Fact]
		public void XmlRegularPipelineCodeCompileUnitCompiles()
		{
			using (var provider = new CSharpCodeProvider())
			{
				var results = provider.CompileAssemblyFromDom(CompilerParameters, new XmlRegularPipeline().ConvertToPipelineRuntimeCodeCompileUnit());
				results.Errors.HasErrors.Should().BeFalse();
				results.Errors.Cast<object>().Should().BeEmpty();
			}
		}

		[Fact]
		public void XmlRegularPipelineConvertsToCodeCompileUnit()
		{
			var pipeline = new XmlRegularPipeline { VersionDependentGuid = Guid.Parse("55a6e50d-1750-4ccd-8995-e5151b049a01") };

			var builder = new StringBuilder();
			using (var provider = new CSharpCodeProvider())
			using (var writer = new StringWriter(builder))
			{
				provider.GenerateCodeFromCompileUnit(
					pipeline.ConvertToPipelineRuntimeCodeCompileUnit(),
					writer,
					new() { BracingStyle = "C", IndentString = "\t", VerbatimOrder = true });
			}

			// be resilient to runtime version in CodeDom heading comment
			Regex.Replace(builder.ToString(), @"(//\s+)Runtime Version:\d\.\d\.\d+\.\d+", @"$1Runtime Version:4.0.30319.42000", RegexOptions.Multiline)
				.Should().Be(
					ResourceManager.Load(
						Assembly.GetExecutingAssembly(),
						"Be.Stateless.BizTalk.Resources.XmlRegularPipeline.btp.cs",
						// be resilient to XML Schema and Instance namespace declaration order by reusing pipeline runtime document as generated by the build server
						s => s.ReadToEnd()).Replace("$$PipelineRuntimeDocument$$", pipeline.GetPipelineRuntimeDocumentSerializer().Serialize()));
		}

		[Fact]
		public void XmlTransmitCodeCompileUnitCompiles()
		{
			using (var provider = new CSharpCodeProvider())
			{
				var results = provider.CompileAssemblyFromDom(CompilerParameters, new XmlTransmit().ConvertToPipelineRuntimeCodeCompileUnit());
				results.Errors.HasErrors.Should().BeFalse();
				results.Errors.Cast<object>().Should().BeEmpty();
			}
		}

		private class FFReceive : ReceivePipeline
		{
			public FFReceive()
			{
				Description = "Flat-File receive micro-pipeline.";
				Version = new(1, 0);
				Stages.Decode
					.AddComponent(new FailedMessageRoutingEnablerComponent())
					.AddComponent(new MicroPipelineComponent { Enabled = true });
				Stages.Disassemble
					.AddComponent(
						new FFDasmComp {
							DocumentSpecName = new(typeof(Microsoft.XLANGs.BaseTypes.Any).AssemblyQualifiedName),
							ValidateDocumentStructure = true
						});
				Stages.Validate
					.AddComponent(new MicroPipelineComponent { Enabled = true });
			}
		}

		private class FFTransmit : SendPipeline
		{
			public FFTransmit()
			{
				Description = "Flat-File send micro-pipeline.";
				Version = new(1, 0);
				Stages.PreAssemble
					.AddComponent(new FailedMessageRoutingEnablerComponent())
					.AddComponent(new MicroPipelineComponent { Enabled = true });
				Stages.Assemble
					.AddComponent(new FFAsmComp());
				Stages.Encode
					.AddComponent(new MicroPipelineComponent { Enabled = true });
			}
		}

		private class PassThruReceive : ReceivePipeline
		{
			public PassThruReceive()
			{
				Description = "Pass-through receive micro-pipeline.";
				Version = new(1, 0);
				Stages.Decode
					.AddComponent(new FailedMessageRoutingEnablerComponent())
					.AddComponent(new MicroPipelineComponent { Enabled = true });
			}
		}

		private class PassThruTransmit : SendPipeline
		{
			public PassThruTransmit()
			{
				Description = "Pass-through send micro-pipeline.";
				Version = new(1, 0);
				Stages.PreAssemble
					.AddComponent(new FailedMessageRoutingEnablerComponent())
					.AddComponent(new MicroPipelineComponent { Enabled = true });
			}
		}

		private class XmlReceive : ReceivePipeline
		{
			public XmlReceive()
			{
				Description = "XML receive micro-pipeline.";
				Version = new(1, 0);
				Stages.Decode
					.AddComponent(new FailedMessageRoutingEnablerComponent())
					.AddComponent(new MicroPipelineComponent { Enabled = true });
				Stages.Disassemble
					.AddComponent(new XmlDasmComp());
				Stages.Validate
					.AddComponent(new MicroPipelineComponent { Enabled = true });
			}
		}

		private class XmlTransmit : SendPipeline
		{
			public XmlTransmit()
			{
				Description = "XML send micro-pipeline.";
				Version = new(1, 0);
				Stages.PreAssemble
					.AddComponent(new FailedMessageRoutingEnablerComponent())
					.AddComponent(new MicroPipelineComponent { Enabled = true });
				Stages.Assemble
					.AddComponent(new XmlAsmComp());
				Stages.Encode
					.AddComponent(new MicroPipelineComponent { Enabled = true });
			}
		}

		[SuppressMessage("ReSharper", "CommentTypo")]
		private CompilerParameters CompilerParameters => new(
			new[] {
				// System.dll
				typeof(System.ComponentModel.ICustomTypeDescriptor).Assembly.Location,
				// Microsoft.BizTalk.Pipeline.dll
				typeof(Microsoft.BizTalk.Component.Interop.IBaseComponent).Assembly.Location,
				// Microsoft.BizTalk.Pipeline.Components
				typeof(XmlAsmComp).Assembly.Location,
				// Microsoft.BizTalk.PipelineOM.dll
				typeof(Microsoft.BizTalk.PipelineEditor.PropertyContents).Assembly.Location,
				// Microsoft.XLANGs.BaseTypes.dll
				typeof(Microsoft.XLANGs.BaseTypes.PipelineBase).Assembly.Location
			});
	}
}
