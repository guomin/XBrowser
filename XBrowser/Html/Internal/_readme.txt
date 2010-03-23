The Html.Internal namespace contains objects that are designed to map directly
to the W3C DOM specification. These objects are what is passed to and
interacted with by the JavaScript on a given page.

Each Dom{Node} object is a thin wrapper around an XBrowser{Node} and should
interact with the rest of the DOM using that object wherever appropriate. No
values, event hooks etc should be dealt with directly if possible, rather the
containing XBrowser{Node} should be called. It will return values required and
bubble up events as needed to the surface.

In a nutshell, a Dom{Node} instance is meant to be merely an interface with
which an XBrowser{Node} object can interact with the page whilst appearing to
be a native object conforming to the W3C DOM specification.