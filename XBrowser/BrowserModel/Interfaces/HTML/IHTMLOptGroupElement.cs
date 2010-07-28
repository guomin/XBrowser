﻿using System;
namespace XBrowserProject.HtmlDom
{
    public interface IHTMLOptGroupElement : IHTMLElement
    {
        /**
         * The control is unavailable in this context. See the disabled attribute 
         * definition in Html 4.01.
         */
        bool disabled { get; set; }

        /**
         * Assigns a label to this option group. See the label attribute definition
         *  in Html 4.01.
         */
        string label { get; set; }
    }
}