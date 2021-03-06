**NOTE**: XBrowser development is cancelled. Instead, see [SimpleBrowser](http://github.com/axefrog/SimpleBrowser), which is a very mature and capable browser automation engine that I developed, and is now under continuing development by [Teun](/Teun). Whilst it doesn't support JavaScript, it provides all of the functionality required to get around odd AJAX/Form post scenarios that would normally be invoked by JavaScript operations. It also includes extensive detailed HTML logging so you can really easily debug your automation script, as well as ASPX doPostBack link "clicking" functionality.

XBrowser 
========

*A fully-managed "headless" web browser for .Net*

This project is still in the early stages and is not yet usable, but here are some of our goals:

* Full W3C HTML5 compliance
* "Headless" - whilst rendering pages is plausible at a later stage, the browser aims to make scriptable web browsing possible from .Net code without any top-heavy external dependencies (other than this assembly)
* No external dependencies - one single fully-managed assembly
* Support for all modern browser technologies such as HTML5, JavaScript, SVG, Canvas, etc. (not plugins such as Flash)
* Friendly query engine that uses jQuery (SizzleCS) syntax for element extraction

While waiting for XBrowser, check out its predecessor [SimpleBrowser](http://github.com/axefrog/SimpleBrowser), which, if you don't need JavaScript support, is an excellent lightweight browser automation engine.