(function ($) {
    function removeHash() {
        window.history.pushState("", document.title, window.location.pathname);
    }
    window.onhashchange = function () {
        var disabled = !(location.hash || location.href.slice(-1) == "#");
        if (!disabled) removeHash();
    }
})(jQuery);