﻿#region Copyright & License

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
using System.CodeDom.Compiler;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Be.Stateless.BizTalk.Dsl.Pipeline.Dummies;
using Be.Stateless.IO.Extensions;
using Be.Stateless.Resources;
using FluentAssertions;
using Microsoft.CSharp;
using Xunit;

namespace Be.Stateless.BizTalk.Dsl.Pipeline.CodeDom
{
	public class PipelineExtensionsFixture
	{
		[Fact]
		[SuppressMessage("ReSharper", "CommentTypo")]
		public void CodeCompileUnitCanBeCompiled()
		{
			using (var provider = new CSharpCodeProvider())
			{
				var results = provider.CompileAssemblyFromDom(
					new CompilerParameters(
						new[] {
							// Microsoft.BizTalk.Pipeline
							typeof(Microsoft.BizTalk.Component.Interop.IBaseComponent).Assembly.Location,
							// Microsoft.BizTalk.Pipeline.Components
							typeof(Microsoft.BizTalk.Component.XmlAsmComp).Assembly.Location,
							// Microsoft.BizTalk.PipelineOM
							typeof(Microsoft.BizTalk.PipelineEditor.PropertyContents).Assembly.Location,
							// Microsoft.XLANGs.BaseTypes
							typeof(Microsoft.XLANGs.BaseTypes.PipelineBase).Assembly.Location
						}),
					new XmlRegularPipeline().ConvertToCodeCompileUnit());
				results.Errors.Should().BeEmpty();
				results.Errors.HasErrors.Should().BeFalse();
			}
		}

		[Fact]
		public void ConvertToCodeCompileUnit()
		{
			var builder = new StringBuilder();
			using (var provider = new CSharpCodeProvider())
			using (var writer = new StringWriter(builder))
			{
				provider.GenerateCodeFromCompileUnit(
					new XmlRegularPipeline { VersionDependentGuid = Guid.Parse("55a6e50d-1750-4ccd-8995-e5151b049a01") }.ConvertToCodeCompileUnit(),
					writer,
					new CodeGeneratorOptions { BracingStyle = "C", IndentString = "\t", VerbatimOrder = true });
			}

			// be resilient to runtime version in CodeDom heading comment
			Regex.Replace(builder.ToString(), @"(//\s+)Runtime Version:\d\.\d\.\d+\.\d+", @"$1Runtime Version:4.0.30319.42000", RegexOptions.Multiline)
				.Should().Be(
					ResourceManager.Load(
						Assembly.GetExecutingAssembly(),
						"Be.Stateless.BizTalk.Dsl.Pipeline.Resources.XmlRegularPipeline.btp.cs",
						s => s.ReadToEnd()));
		}
	}
}
