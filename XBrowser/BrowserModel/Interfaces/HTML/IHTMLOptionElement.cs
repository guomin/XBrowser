using System;
namespace XBrowserProject.HtmlDom
{
    internal interface IHTMLOptionElement : IHTMLElement
    {
        /**
         * Represents the value of the Html selected attribute. The value of this 
         * attribute does not change if the state of the corresponding form 
         * control, in an interactive user agent, changes. See the selected 
         * attribute definition in Html 4.01.
         * @version DOM Level 2
         */
        bool defaultSelected { get; set; }

        /**
         * The control is unavailable in this context. See the disabled attribute 
         * definition in Html 4.01.
         */
        bool disabled { get; set; }

        /**
         * Returns the <code>FORM</code> element containing this control. Returns 
         * <code>null</code> if this control is not within the context of a 
         * form. 
         */
        IHTMLFormElement form { get; }

        /**
         * The index of this <code>OPTION</code> in its parent <code>SELECT</code>
         * , starting from 0.
         * @version DOM Level 2
         */
        int index { get; }

        /**
         * Option label for use in hierarchical menus. See the label attribute 
         * definition in Html 4.01.
         */
        string label { get; set; }

        /**
         * Represents the current state of the corresponding form control, in an 
         * interactive user agent. Changing this attribute changes the state of 
         * the form control, but does not change the value of the Html selected 
         * attribute of the element.
         */
        bool selected { get; set; }

        /**
         * The text contained within the option element. 
         */
        string text { get; }

        /**
         * The current form control value. See the value attribute definition in 
         * Html 4.01.
         */
        string value { get; set; }
    }
}
