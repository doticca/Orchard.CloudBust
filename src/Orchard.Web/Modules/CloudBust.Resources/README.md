CloudBust.Resources
===================

The CloudBust.Resources module contains al the js and styles for various components required for CloudBust applications.

DropZone
========

DropzoneJS is an open source library that provides drag’n’drop file uploads with image previews.

A common location for Dropzone.js and related script libraries for Orchard Project.

This module defines a script manifest for Dropzone Library with name "Dropzone".<br>
You can include Dropzone script inside your Razor views using:<br>
Script.Require("Dropzone")<br>

Dropzone module will automatically insert your Dropzone.js script in every page.<br>
You can disable this bevavior and include dropzone on demand (using Script.Require("Dropzone") inside your theme/view) by unchecking Auto Enable at dropzone module settings.<br>

http://www.dropzonejs.com

Emmet
=====

Emmet is a plugin for many popular text editors which greatly improves HTML & CSS workflow.

A common location for Emmet and related script libraries for Orchard Project.

http://emmet.io/

Underscore
==========


Underscore is a JavaScript library that provides a whole mess of useful functional programming helpers without extending any built-in objects.

A common location for Underscore.js and related script libraries for Orchard Project.

This module defines a script manifest for Underscore Library with name "Underscore".<br>
You can include Underscore script inside your Razor views using:<br>
Script.Require("Underscore")<br>

Underscore module will automatically insert your Underscore.js script in every page.<br>
You can disable this bevavior and include underscore on demand (using Script.Require("Underscore) inside your theme/view) by unchecking Auto Enable at underscore module settings.<br>

http://underscorejs.org

GoogleMaps
==========

Create engaging web and mobile applications using Google Maps' powerful mapping platform including driving directions, Street View imagery and more

A common location for GoogleMaps.js and related script libraries for Orchard Project.

This module defines a script manifest for GoogleMaps Library with name "GoogleMaps".<br>
You can include GoogleMaps script inside your Razor views using:<br>
Script.Require("GoogleMaps")<br>

GoogleMaps module will automatically insert your GoogleMaps.js script in every page.<br>
You can disable this bevavior and include googlemaps on demand (using Script.Require("GoogleMaps") inside your theme/view) by unchecking Auto Enable at googlemaps module settings.<br>

https://developers.google.com/maps/documentation/javascript/

Highlight
=========

Syntax highlighting for the Web

A common location for Highlight.js and related script libraries for Orchard Project.

This module defines a script manifest for Highlight Library with name "Highlight".<br>
You can include Highlight script inside your Razor views using:<br>
Script.Require("Highlight")<br>
Style.Require("Highlight_default")<br>
Replace _default with the theme of your choise default,xcode,foundation,dark,etc.<br>

Highlight module will automatically insert your Highlight.js script in every page.<br>
You can disable this bevavior and include highlight on demand (using Script.Require("Highlight") inside your theme/view) by unchecking Auto Enable at highlight module settings.<br>

You can also select the full version of Highlight to automatically load on every page.<br>

https://highlightjs.org/

Slick
=====

the last carousel you'll ever need

A common location for Slick.js and related script libraries for Orchard Project.

This module defines a script manifest for Slick Library with name "Slick".<br>
You can include Slick script and styles inside your Razor views using:<br>
Script.Require("Slick")<br>
Style.Require("Slick")<br>

Slick module will automatically insert your Slick.js script in every page.<br>
You can disable this bevavior and include slick on demand (using Script.Require("Slick") inside your theme/view) by unchecking Auto Enable at slick module settings.<br>

http://kenwheeler.github.io/slick/

Niceselect
==========

A lightweight jQuery plugin that replaces native select elements with customizable dropdowns.

A common location for Niceselect.js and related script libraries for Orchard Project.

This module defines a script manifest for Niceselect Library with name "Niceselect".<br>
You can include Niceselect script inside your Razor views using:<br>
Script.Require("Niceselect")<br>

Niceselect module will automatically insert your Niceselect.js script in every page.<br>
You can disable this bevavior and include niceselect on demand (using Script.Require("Niceselect") inside your theme/view) by unchecking Auto Enable at niceselect module settings.<br>

http://hernansartorio.com/jquery-nice-select/

Masonry
=======

Cascading grid layout library

A common location for Masonry.js and related script libraries for Orchard Project.

This module defines a script manifest for Masonry Library with name "Masonry".<br>
You can include Masonry script inside your Razor views using:<br>
Script.Require("Masonry")<br>

Masonry module will automatically insert your Masonry.js script in every page.<br>
You can disable this bevavior and include masonry on demand (using Script.Require("Masonry) inside your theme/view) by unchecking Auto Enable at masonry module settings.<br>

http://masonry.desandro.com/

Magnific (popup)
================

Magnific Popup is a responsive lightbox & dialog script with focus on performance and providing best experience for user with any device

A common location for Magnific.js and related script libraries for Orchard Project.

This module defines a script manifest for Magnific Library with name "Magnific".<br>
You can include Magnific script and styles inside your Razor views using:<br>
Script.Require("Magnific")<br>
Style.Require("Magnific")<br>

Magnific module will automatically insert your Magnific.js script in every page.<br>
You can disable this bevavior and include magnific on demand (using Script.Require("Magnific) inside your theme/view) by unchecking Auto Enable at magnific module settings.<br>

http://dimsemenov.com/plugins/magnific-popup/

ImagesLoaded
=============

Detect when images have been loaded.

A common location for imagesloaded.js and related script libraries for Orchard Project.

This module defines a script manifest for imagesloaded Library with name "imagesLoaded".<br>
You can include imagesloaded script inside your Razor views using:<br>
Script.Require("imagesLoaded")<br>

imagesLoaded module will automatically insert your imagesloaded.js script in every page.<br>
You can disable this bevavior and include imagesloaded on demand (using Script.Require("imagesLoaded) inside your theme/view) by unchecking Auto Enable at imagesloaded module settings.<br>

http://imagesloaded.desandro.com/

Ace
===

The Ace module for Orchard enables Html, Css and Javascript editing of Body items using the Ace Editor (http://ace.c9.io/).

Features

The Css part enables Css snippets for any content. The module automatically attaches Css parts to Pages and Html Widgets.

The Js part enables Javascript snippets for any content. The module automatically attaches Js parts to Pages and Html Widgets.

The Ace module introduces various flavors of Body editing:
- ace: the body editor will switch to Ace editor for advanced HTML editing. Enable this flavor if you prefer editing with pure Html and you don't like TinyMCE's auto formatting.
- tabbedace: the body editor will combine Html, Css and Javascript parts into a Tabbed interface. Editor is Ace for all parts.
- tabbed: the body editor will combine Html, Css and Javascript parts into a Tabbed interface. Editor is TinyMCE for Html and Ace for Css and Javascript.

Quick Setup

Enable the module and change the flavor of any Body Part you like to ace, tabbed and tabbedace instead of html.

The module automatically adds Css and Javacript parts to Pages and Html Widgets.

https://ace.c9.io

CookieCuttr
===========

A tailorable jQuery plugin to deal with the EU Cookie Law.

CookieCuttr for Orchard <br />

Quick Setup<br />

Enable the CookieCuttr Module.<br />
Go to Widgets and select Default Layer.<br />
Press the 'Add' button to any zone you like. (Footer is ok)<br />
Select CookieCuttr Widget from the List of widgets.<br />
Enter a title for the widget. e.g. Cookies, and uncheck the tickbox to hide the title.<br />
Ready, you are ok with the EU cookie Law.<br />

Available Options<br />

<strong>cookieAnalytics</strong> - if you are just using a simple analytics package you can set this to true, it displays a simple default message with no privacy policy link - this is set to true by default.

<strong>cookieDeclineButton</strong> - if you’d like a decline button to (ironically) write a cookie into the browser then set this to true.

<strong>cookieAcceptButton</strong> - set this to true to hide the accept button, its set to false by default

<strong>cookieResetButton</strong> - if you’d like a reset button to delete the accept or decline cookies then set this to true.

<strong>cookiePolicyLink</strong> - if applicable, enter the link to your privacy policy in here - this is as soon as cookieAnalytics is set to false;

<strong>cookieMessage</strong> - edit the message you want to appear in the cookie bar, remember to keep the {{cookiePolicyLink}} variable in tact so it inserts your privacy policy link.

<strong>cookieAnalyticsMessage</strong> - edit the message you want to appear, this is the default message.

<strong>cookieWhatAreTheyLink</strong> - edit the link for the 'What are Cookies' link.

<strong>cookieWhatAreLinkText</strong> - you can change the text of the "What are Cookies" link shown on Google Analytics message.

<strong>cookieNotificationLocationBottom</strong> - this is false by default, change it to true and the cookie notification bar will show at the bottom of the page instead, please note this will remain at the top for mobile and iOS devices and Internet Explorer 6.

<strong>cookieAcceptButtonText</strong> - you can change the text of the green accept button.

<strong>cookieDeclineButtonText</strong> - you can change the text of the red decline button.

<strong>cookieResetButtonText</strong> - you can change the text of the orange reset button.

<strong>cookiePolicyPage</strong> - set this to true to display the message you want to appear on your privacy or cookie policy page.

<strong>cookiePolicyPageMessage</strong> - edit the message you want to appear on your policy page.

<strong>cookieDiscreetLink</strong> - false by default, set to true to enable.

<strong>cookieDiscreetLinkText</strong> - edit the text you want to appear on the discreet option.

<strong>cookieDiscreetPosition</strong> - set to topleft by default, you can also set topright, bottomleft, bottomright.

FontAwesome
===========

The iconic font and CSS toolkit

A common location for FontAwesome css and font files.

This module defines a style manifest for FontAwesome Library with name "FontAwesome".<br>
You can include FontAwesome styles inside your Razor views using:<br>
Style.Require("FontAwesome")<br>

FontAwesome module will automatically insert your fontawesome stylesheet in every page.<br>
You can disable this bevavior and include fontawesome on demand (using Style.Require("FontAwesome") inside your theme/view) by unchecking Auto Enable at fontawesome module settings.<br>

http://fontawesome.io/

ElegantIcon
===========

The Elegant Icon Font

A common location for elegant icon css and font files.

This module defines a style manifest for ElegantIcon Library with name "ElegantIcon".<br>
You can include ElegantIcon styles inside your Razor views using:<br>
Style.Require("ElegantIcon")<br>

ElegantIcon module will automatically insert your elegant icon stylesheet in every page.<br>
You can disable this bevavior and include elegant icon on demand (using Style.Require("ElegantIcon") inside your theme/view) by unchecking Auto Enable at elegant icon module settings.<br>

https://github.com/josephnle/elegant-icons

Bootstrap
=========

Bootstrap is the most popular HTML, CSS, and JS framework for developing responsive, mobile first projects on the web.

A common location for Bootstrap css, js and font files.

This module defines a style manifest for Bootstrap css Library with name "Bootstrap".<br>
This module defines a javascript manifest for Bootstrap js Library with name "Bootstrap".<br>
You can include FontAwesome styles and js inside your Razor views using:<br>
Script.Require("Bootstrap")<br>
Style.Require("Bootstrap")<br>

Bootstrap module will automatically insert your bootstrap stylesheet and js files in every page.<br>
You can disable this bevavior and include bootstrap on demand (using Style.Require("Bootstrap") and Script.Require("Bootstrap") inside your theme/view) by unchecking Auto Enable at bootstrap module settings.<br>

http://getbootstrap.com/

OwlCarousel
===========

Touch enabled jQuery plugin that lets you create a beautiful responsive carousel slider.

A common location for owlcarousel and related script libraries for Orchard Project.

This module defines a script manifest for OwlCarousel Library with name "OwlCarousel".<br>
You can include OwlCarousel script and styles inside your Razor views using:<br>
Script.Require("OwlCarousel")<br>
Style.Require("OwlCarousel")<br>

OwlCarousel module will automatically insert your OwlCarousel script and stylesheet in every page.<br>
You can disable this bevavior and include owlcarousel on demand (using Script.Require("OwlCarousel") and Style.Require("OwlCarousel") inside your theme/view) by unchecking Auto Enable at owlcarousel module settings.<br>

http://kenwheeler.github.io/slick/

Notify.js
=========

A simple, versatile notification library.

A common location for Notify.js for Orchard Project.

This module defines a script manifest for Notify Library with name "Notify".<br>
You can include Notify script inside your Razor views using:<br>
Script.Require("Notify")<br>

Notify module will automatically insert your Notify script in every page.<br>
You can disable this bevavior and include Notify on demand (using Script.Require("Notify") inside your theme/view) by unchecking Auto Enable at Notify module settings.<br>

https://notifyjs.com/