﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Be.Stateless.BizTalk.Dummies
{
	
	
	public sealed class XmlRegularPipeline : Microsoft.BizTalk.PipelineOM.ReceivePipeline
	{
		
		public XmlRegularPipeline()
		{
			Microsoft.BizTalk.PipelineOM.Stage stage = this.AddStage(Microsoft.BizTalk.PipelineOM.Stage.Decoder, Microsoft.BizTalk.PipelineOM.ExecutionMode.all);
			Microsoft.BizTalk.Component.Interop.IBaseComponent comp0 = Microsoft.BizTalk.PipelineOM.PipelineManager.CreateComponent("Be.Stateless.BizTalk.Component.FailedMessageRoutingEnablerComponent, Be.Stateless.BizTalk.Pipeline.Components, Version=2.0.0.0, Culture=neutral, PublicKeyToken=3707daa0b119fc14");
			if (comp0.GetType().IsInstanceOfType(typeof(Microsoft.BizTalk.Component.Interop.IPersistPropertyBag)))
			{
				((Microsoft.BizTalk.Component.Interop.IPersistPropertyBag)(comp0)).Load(new Microsoft.BizTalk.PipelineEditor.PropertyBag(new System.Collections.ArrayList(new Microsoft.BizTalk.PipelineEditor.PropertyContents[] {
									new Microsoft.BizTalk.PipelineEditor.PropertyContents("Enabled", true),
									new Microsoft.BizTalk.PipelineEditor.PropertyContents("EnableFailedMessageRouting", true),
									new Microsoft.BizTalk.PipelineEditor.PropertyContents("SuppressRoutingFailureReport", false)})), 0);
			}
			this.AddComponent(stage, comp0);
			stage = this.AddStage(Microsoft.BizTalk.PipelineOM.Stage.DisassemblingParser, Microsoft.BizTalk.PipelineOM.ExecutionMode.firstRecognized);
			Microsoft.BizTalk.Component.Interop.IBaseComponent comp1 = Microsoft.BizTalk.PipelineOM.PipelineManager.CreateComponent("Microsoft.BizTalk.Component.XmlDasmComp, Microsoft.BizTalk.Pipeline.Components, Version=3.0.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35");
			if (comp1.GetType().IsInstanceOfType(typeof(Microsoft.BizTalk.Component.Interop.IPersistPropertyBag)))
			{
				((Microsoft.BizTalk.Component.Interop.IPersistPropertyBag)(comp1)).Load(new Microsoft.BizTalk.PipelineEditor.PropertyBag(new System.Collections.ArrayList(new Microsoft.BizTalk.PipelineEditor.PropertyContents[] {
									new Microsoft.BizTalk.PipelineEditor.PropertyContents("EnvelopeSpecNames", ""),
									new Microsoft.BizTalk.PipelineEditor.PropertyContents("EnvelopeSpecTargetNamespaces", ""),
									new Microsoft.BizTalk.PipelineEditor.PropertyContents("DocumentSpecNames", ""),
									new Microsoft.BizTalk.PipelineEditor.PropertyContents("DocumentSpecTargetNamespaces", ""),
									new Microsoft.BizTalk.PipelineEditor.PropertyContents("AllowUnrecognizedMessage", false),
									new Microsoft.BizTalk.PipelineEditor.PropertyContents("ValidateDocument", false),
									new Microsoft.BizTalk.PipelineEditor.PropertyContents("RecoverableInterchangeProcessing", false),
									new Microsoft.BizTalk.PipelineEditor.PropertyContents("HiddenProperties", "EnvelopeSpecTargetNamespaces,DocumentSpecTargetNamespaces"),
									new Microsoft.BizTalk.PipelineEditor.PropertyContents("DtdProcessing", "")})), 0);
			}
			this.AddComponent(stage, comp1);
		}
		
		public override string XmlContent
		{
			get
			{
				return _strPipeline;
			}
		}
		
		private const string _strPipeline = @"$$PipelineRuntimeDocument$$";
		
		public override System.Guid VersionDependentGuid
		{
			get
			{
				return new System.Guid(_versionDependentGuid);
			}
		}
		
		private const string _versionDependentGuid = "55a6e50d-1750-4ccd-8995-e5151b049a01";
	}
}