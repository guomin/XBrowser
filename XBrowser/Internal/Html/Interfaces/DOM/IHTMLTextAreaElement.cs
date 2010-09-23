namespace XBrowserProject.Internal.Html.Interfaces.DOM
{
    internal interface IHTMLTextAreaElement : IHTMLElement
    {
        /**
         * A single character access key to give access to the form control. See 
         * the accesskey attribute definition in Html 4.01.
         */
        string accessKey { get; set; }

        /**
         * Removes keyboard focus from this element.
         */
        void blur();

        /**
         * Width of control (in characters). See the cols attribute definition in 
         * Html 4.01.
         */
        int cols { get; set; }

        /**
         * Represents the contents of the element. The value of this attribute 
         * does not change if the contents of the corresponding form control, in 
         * an interactive user agent, changes.
         * @version DOM Level 2
         */
        string defaultValue { get; set; }

        /**
         * The control is unavailable in this context. See the disabled attribute 
         * definition in Html 4.01.
         */
        bool disabled { get; set; }

        /**
         * Gives keyboard focus to this element.
         */
        void focus();

        /**
         * Returns the <code>FORM</code> element containing this control. Returns 
         * <code>null</code> if this control is not within the context of a 
         * form. 
         */
        IHTMLFormElement form { get; }

        /**
         * Form control or object name when submitted with a form. See the name 
         * attribute definition in Html 4.01.
         */
        string name { get; set; }

        /**
         * This control is read-only. See the readonly attribute definition in 
         * Html 4.01.
         */
        bool readOnly { get; set; }

        /**
         * Number of text rows. See the rows attribute definition in Html 4.01.
         */
        int rows { get; set; }

        /**
         * Select the contents of the <code>TEXTAREA</code>.
         */
        void select();

        /**
         * Index that represents the element's position in the tabbing order. See 
         * the tabindex attribute definition in Html 4.01.
         */
        int tabIndex { get; set; }

        /**
         * The type of this form control. This the string "textarea".
         */
        string type { get; }

        /**
         * Represents the current contents of the corresponding form control, in 
         * an interactive user agent. Changing this attribute changes the 
         * contents of the form control, but does not change the contents of the 
         * element. If the entirety of the data can not fit into a single 
         * <code>DOMString</code>, the implementation may truncate the data.
         */
        string value { get; set; }
    }
}
