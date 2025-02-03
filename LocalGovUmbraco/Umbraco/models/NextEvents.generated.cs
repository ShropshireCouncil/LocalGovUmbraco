//------------------------------------------------------------------------------
// <auto-generated>
//   This code was generated by a tool.
//
//    Umbraco.ModelsBuilder.Embedded v13.6.0+b9837ac
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
	/// <summary>Upcoming Events</summary>
	[PublishedModel("nextEvents")]
	public partial class NextEvents : PublishedElementModel, IBlockStyles
	{
		// helpers
#pragma warning disable 0109 // new is redundant
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.6.0+b9837ac")]
		public new const string ModelTypeAlias = "nextEvents";
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.6.0+b9837ac")]
		public new const PublishedItemType ModelItemType = PublishedItemType.Content;
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.6.0+b9837ac")]
		[return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
		public new static IPublishedContentType GetModelContentType(IPublishedSnapshotAccessor publishedSnapshotAccessor)
			=> PublishedModelUtility.GetModelContentType(publishedSnapshotAccessor, ModelItemType, ModelTypeAlias);
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.6.0+b9837ac")]
		[return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
		public static IPublishedPropertyType GetModelPropertyType<TValue>(IPublishedSnapshotAccessor publishedSnapshotAccessor, Expression<Func<NextEvents, TValue>> selector)
			=> PublishedModelUtility.GetModelPropertyType(GetModelContentType(publishedSnapshotAccessor), selector);
#pragma warning restore 0109

		private IPublishedValueFallback _publishedValueFallback;

		// ctor
		public NextEvents(IPublishedElement content, IPublishedValueFallback publishedValueFallback)
			: base(content, publishedValueFallback)
		{
			_publishedValueFallback = publishedValueFallback;
		}

		// properties

		///<summary>
		/// Archive: The archive to pull from
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.6.0+b9837ac")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("archive")]
		public virtual global::Umbraco.Cms.Core.Models.PublishedContent.IPublishedContent Archive => this.Value<global::Umbraco.Cms.Core.Models.PublishedContent.IPublishedContent>(_publishedValueFallback, "archive");

		///<summary>
		/// Archive label: Override the archive link text
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.6.0+b9837ac")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("archiveLabel")]
		public virtual string ArchiveLabel => this.Value<string>(_publishedValueFallback, "archiveLabel");

		///<summary>
		/// Display count: The number of articles to display
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.6.0+b9837ac")]
		[ImplementPropertyType("count")]
		public virtual int Count => this.Value<int>(_publishedValueFallback, "count");

		///<summary>
		/// Heading
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.6.0+b9837ac")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("heading")]
		public virtual string Heading => this.Value<string>(_publishedValueFallback, "heading");

		///<summary>
		/// Show archive link: Show a link back to the archive
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.6.0+b9837ac")]
		[ImplementPropertyType("showArchiveLink")]
		public virtual bool ShowArchiveLink => this.Value<bool>(_publishedValueFallback, "showArchiveLink");

		///<summary>
		/// Show Date
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.6.0+b9837ac")]
		[ImplementPropertyType("showDate")]
		public virtual bool ShowDate => this.Value<bool>(_publishedValueFallback, "showDate");

		///<summary>
		/// Show Excerpts: Show the article excerpt in the teaser
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.6.0+b9837ac")]
		[ImplementPropertyType("showExcerpts")]
		public virtual bool ShowExcerpts => this.Value<bool>(_publishedValueFallback, "showExcerpts");

		///<summary>
		/// Show Images: Show the article thumbnail in the teaser
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.6.0+b9837ac")]
		[ImplementPropertyType("showImages")]
		public virtual bool ShowImages => this.Value<bool>(_publishedValueFallback, "showImages");

		///<summary>
		/// Show "Read more" link
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.6.0+b9837ac")]
		[ImplementPropertyType("showReadMore")]
		public virtual bool ShowReadMore => this.Value<bool>(_publishedValueFallback, "showReadMore");

		///<summary>
		/// Custom CSS Classes
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.6.0+b9837ac")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("classes")]
		public virtual string Classes => global::Umbraco.Cms.Web.Common.PublishedModels.BlockStyles.GetClasses(this, _publishedValueFallback);

		///<summary>
		/// Horizontal Alignment
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.6.0+b9837ac")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("horizontalAlignment")]
		public virtual string HorizontalAlignment => global::Umbraco.Cms.Web.Common.PublishedModels.BlockStyles.GetHorizontalAlignment(this, _publishedValueFallback);

		///<summary>
		/// Vertical Alignment
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.6.0+b9837ac")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("verticalAlignment")]
		public virtual string VerticalAlignment => global::Umbraco.Cms.Web.Common.PublishedModels.BlockStyles.GetVerticalAlignment(this, _publishedValueFallback);
	}
}
