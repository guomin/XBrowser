namespace AxeFrog.Net.Parser
{
	interface IElementPositioningRule
	{
		/// <summary>
		/// The tag name for the element
		/// </summary>
		string TagName { get; }
		/// <summary>
		/// If the element is present in the wrong area, it will be removed and appended to the correct area
		/// </summary>
		DocumentArea Area { get; }
		/// <summary>
		/// These are the tags that the element is allowed to be a child of
		/// </summary>
		string[] AllowedParentTags { get; }
		/// <summary>
		/// If the element exists as a child of a tag that it is not permitted to be a child of, this is the parent tag to create in its place and then move the element inside.
		/// </summary>
		string CreateParentTag { get; }
		/// <summary>
		/// If null, the tag can have both text and non-text children. If true, it can only have text children and if false, it cannot have text children.
		/// </summary>
		bool? TextChildren { get; }
		/// <summary>
		/// Prevent a tag existing in another instance of the same tag
		/// </summary>
		bool AllowNesting { get; set; }
		/// <summary>
		/// e.g. A paragraph tag cannot be nested in another paragraph tag, but it can be if there is a table tag in between
		/// </summary>
		string[] NestingCheckBreakingTags { get; set; }
	}
}