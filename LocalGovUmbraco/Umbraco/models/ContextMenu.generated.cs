//------------------------------------------------------------------------------
// <auto-generated>
//   This code was generated by a tool.
//
//    Umbraco.ModelsBuilder.Embedded v13.8.0+a486d5d
//
//   Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Linq.Expressions;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Infrastructure.ModelsBuilder;
using Umbraco.Cms.Core;
using Umbraco.Extensions;

namespace Umbraco.Cms.Web.Common.PublishedModels
{
	/// <summary>Contextual Links</summary>
	[PublishedModel("contextMenu")]
	public partial class ContextMenu : PublishedElementModel, IBlockStyles
	{
		// helpers
#pragma warning disable 0109 // new is redundant
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.8.0+a486d5d")]
		public new const string ModelTypeAlias = "contextMenu";
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.8.0+a486d5d")]
		public new const PublishedItemType ModelItemType = PublishedItemType.Content;
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.8.0+a486d5d")]
		[return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
		public new static IPublishedContentType GetModelContentType(IPublishedSnapshotAccessor publishedSnapshotAccessor)
			=> PublishedModelUtility.GetModelContentType(publishedSnapshotAccessor, ModelItemType, ModelTypeAlias);
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.8.0+a486d5d")]
		[return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
		public static IPublishedPropertyType GetModelPropertyType<TValue>(IPublishedSnapshotAccessor publishedSnapshotAccessor, Expression<Func<ContextMenu, TValue>> selector)
			=> PublishedModelUtility.GetModelPropertyType(GetModelContentType(publishedSnapshotAccessor), selector);
#pragma warning restore 0109

		private IPublishedValueFallback _publishedValueFallback;

		// ctor
		public ContextMenu(IPublishedElement content, IPublishedValueFallback publishedValueFallback)
			: base(content, publishedValueFallback)
		{
			_publishedValueFallback = publishedValueFallback;
		}

		// properties

		///<summary>
		/// Context: The root node for the menu
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.8.0+a486d5d")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("context")]
		public virtual global::Umbraco.Cms.Core.Models.PublishedContent.IPublishedContent Context => this.Value<global::Umbraco.Cms.Core.Models.PublishedContent.IPublishedContent>(_publishedValueFallback, "context");

		///<summary>
		/// Show Descendants: Expand descendants where the link is an ancestor of the current page.
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.8.0+a486d5d")]
		[ImplementPropertyType("recurse")]
		public virtual bool Recurse => this.Value<bool>(_publishedValueFallback, "recurse");

		///<summary>
		/// Show Header: Display the contextual node's name as the block header
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.8.0+a486d5d")]
		[ImplementPropertyType("showHeader")]
		public virtual bool ShowHeader => this.Value<bool>(_publishedValueFallback, "showHeader");

		///<summary>
		/// Custom CSS Classes
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.8.0+a486d5d")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("classes")]
		public virtual string Classes => global::Umbraco.Cms.Web.Common.PublishedModels.BlockStyles.GetClasses(this, _publishedValueFallback);

		///<summary>
		/// Horizontal Alignment
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.8.0+a486d5d")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("horizontalAlignment")]
		public virtual string HorizontalAlignment => global::Umbraco.Cms.Web.Common.PublishedModels.BlockStyles.GetHorizontalAlignment(this, _publishedValueFallback);

		///<summary>
		/// Vertical Alignment
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.8.0+a486d5d")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("verticalAlignment")]
		public virtual string VerticalAlignment => global::Umbraco.Cms.Web.Common.PublishedModels.BlockStyles.GetVerticalAlignment(this, _publishedValueFallback);
	}
}
