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
	/// <summary>Homepage</summary>
	[PublishedModel("indexPage")]
	public partial class IndexPage : Main, IBodyContent
	{
		// helpers
#pragma warning disable 0109 // new is redundant
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.8.0+a486d5d")]
		public new const string ModelTypeAlias = "indexPage";
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.8.0+a486d5d")]
		public new const PublishedItemType ModelItemType = PublishedItemType.Content;
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.8.0+a486d5d")]
		[return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
		public new static IPublishedContentType GetModelContentType(IPublishedSnapshotAccessor publishedSnapshotAccessor)
			=> PublishedModelUtility.GetModelContentType(publishedSnapshotAccessor, ModelItemType, ModelTypeAlias);
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.8.0+a486d5d")]
		[return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
		public static IPublishedPropertyType GetModelPropertyType<TValue>(IPublishedSnapshotAccessor publishedSnapshotAccessor, Expression<Func<IndexPage, TValue>> selector)
			=> PublishedModelUtility.GetModelPropertyType(GetModelContentType(publishedSnapshotAccessor), selector);
#pragma warning restore 0109

		private IPublishedValueFallback _publishedValueFallback;

		// ctor
		public IndexPage(IPublishedContent content, IPublishedValueFallback publishedValueFallback)
			: base(content, publishedValueFallback)
		{
			_publishedValueFallback = publishedValueFallback;
		}

		// properties

		///<summary>
		/// Beta: Display/Hide the "Beta" phase banner
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.8.0+a486d5d")]
		[ImplementPropertyType("beta")]
		public virtual bool Beta => this.Value<bool>(_publishedValueFallback, "beta");

		///<summary>
		/// Copyright holder
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.8.0+a486d5d")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("copyright")]
		public virtual string Copyright => this.Value<string>(_publishedValueFallback, "copyright");

		///<summary>
		/// Favicon
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.8.0+a486d5d")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("favicon")]
		public virtual global::Umbraco.Cms.Core.Models.MediaWithCrops Favicon => this.Value<global::Umbraco.Cms.Core.Models.MediaWithCrops>(_publishedValueFallback, "favicon");

		///<summary>
		/// Feedback Link
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.8.0+a486d5d")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("feedback")]
		public virtual global::Umbraco.Cms.Core.Models.Link Feedback => this.Value<global::Umbraco.Cms.Core.Models.Link>(_publishedValueFallback, "feedback");

		///<summary>
		/// Links
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.8.0+a486d5d")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("footerLinks")]
		public virtual global::System.Collections.Generic.IEnumerable<global::Umbraco.Cms.Core.Models.Link> FooterLinks => this.Value<global::System.Collections.Generic.IEnumerable<global::Umbraco.Cms.Core.Models.Link>>(_publishedValueFallback, "footerLinks");

		///<summary>
		/// Global Meta Tags
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.8.0+a486d5d")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("globalMetaTags")]
		public virtual global::System.Collections.Generic.Dictionary<string, global:: System.String> GlobalMetaTags => this.Value<global::System.Collections.Generic.Dictionary<string, global:: System.String>>(_publishedValueFallback, "globalMetaTags");

		///<summary>
		/// Google Analytics: If you have a Google Analytics property, enter it here.
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.8.0+a486d5d")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("googleAnalytics")]
		public virtual string GoogleAnalytics => this.Value<string>(_publishedValueFallback, "googleAnalytics");

		///<summary>
		/// Logo
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.8.0+a486d5d")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("logo")]
		public virtual global::Umbraco.Cms.Core.Models.MediaWithCrops Logo => this.Value<global::Umbraco.Cms.Core.Models.MediaWithCrops>(_publishedValueFallback, "logo");

		///<summary>
		/// Show Search Box
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.8.0+a486d5d")]
		[ImplementPropertyType("showSearch")]
		public virtual bool ShowSearch => this.Value<bool>(_publishedValueFallback, "showSearch");

		///<summary>
		/// Site Information: Supporting text for the footer, such as licence information.
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.8.0+a486d5d")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("siteInformation")]
		public virtual global::Umbraco.Cms.Core.Strings.IHtmlEncodedString SiteInformation => this.Value<global::Umbraco.Cms.Core.Strings.IHtmlEncodedString>(_publishedValueFallback, "siteInformation");

		///<summary>
		/// Site Name
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.8.0+a486d5d")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("siteName")]
		public virtual string SiteName => this.Value<string>(_publishedValueFallback, "siteName");

		///<summary>
		/// Body
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.8.0+a486d5d")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("body")]
		public virtual global::Umbraco.Cms.Core.Models.Blocks.BlockGridModel Body => global::Umbraco.Cms.Web.Common.PublishedModels.BodyContent.GetBody(this, _publishedValueFallback);
	}
}
