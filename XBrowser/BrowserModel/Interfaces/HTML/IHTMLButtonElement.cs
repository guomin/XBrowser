using System;
namespace XBrowserProject.HtmlDom
{
    /**
     * Push button. See the BUTTON element definition in Html 4.01.
     * <p>See also the <a href='http://www.w3.org/TR/2003/REC-DOM-Level-2-Html-20030109'>Document Object Model (DOM) Level 2 Html Specification</a>.
     */
    public interface IHTMLButtonElement : IHTMLElement
    {
        /**
         * A single character access key to give access to the form control. See 
         * the accesskey attribute definition in Html 4.01.
         */
        string accessKey { get; set; }

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
         * Form control or object name when submitted with a form. See the name 
         * attribute definition in Html 4.01.
         */
        string name { get; set; }

        /**
         * Index that represents the element's position in the tabbing order. See 
         * the tabindex attribute definition in Html 4.01.
         */
        int tabIndex { get; set; }

        /**
         * The type of button (all lower case). See the type attribute definition 
         * in Html 4.01.
         */
        string type { get; }

        /**
         * The current form control value. See the value attribute definition in 
         * Html 4.01.
         */
        string value { get; set; }
    }
}
