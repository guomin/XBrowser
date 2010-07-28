using System;
namespace XBrowserProject.HtmlDom
{
    /**
     * The <code>FORM</code> element encompasses behavior similar to a collection 
     * and an element. It provides direct access to the contained form controls 
     * as well as the attributes of the form element. See the FORM element 
     * definition in Html 4.01.
     * <p>See also the <a href='http://www.w3.org/TR/2003/REC-DOM-Level-2-Html-20030109'>Document Object Model (DOM) Level 2 Html Specification</a>.
     */
    public interface IHTMLFormElement : IHTMLElement
    {

        /**
         * List of character sets supported by the server. See the accept-charset 
         * attribute definition in Html 4.01.
         */
        string acceptCharset { get; set; }

        /**
         * Server-side form handler. See the action attribute definition in Html 
         * 4.01.
         */
        string action { get; set; }

        /**
         * Returns a collection of all form control elements in the form. 
         */
        IHTMLCollection elements { get; }

        /**
         * The content type of the submitted form, generally 
         * "application/x-www-form-urlencoded". See the enctype attribute 
         * definition in Html 4.01. The onsubmit even handler is not guaranteed 
         * to be triggered when invoking this method. The behavior is 
         * inconsistent for historical reasons and authors should not rely on a 
         * particular one. 
         */
        string enctype { get; set; }

        /**
         * The number of form controls in the form.
         */
        int length { get; }

        /**
         * HTTP method [<a href='http://www.ietf.org/rfc/rfc2616.txt'>IETF RFC 2616</a>] used to submit form. See the method attribute definition 
         * in Html 4.01.
         */
        string method { get; set; }

        /**
         * Names the form. 
         */
        string name { get; set; }

        /**
         * Restores a form element's default values. It performs the same action 
         * as a reset button.
         */
        void reset();

        /**
         * Submits the form. It performs the same action as a submit button.
         */
        void submit();

        /**
         * Frame to render the resource in. See the target attribute definition in 
         * Html 4.01.
         */
        string target { get; set; }
    }
}
