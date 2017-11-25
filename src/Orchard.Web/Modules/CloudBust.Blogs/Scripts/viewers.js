(function ($) {
    $.fn.extend({
        helpfullySetTheViewers: function () {
            var __cookieScope = "__viewers";
            var __cookiePath = document.location.pathname;

            return $(this).each(function () {
                var ajaxRequest;
                var _this = $(this);

                var _contentId = _this.find("#contentId");
                if (_contentId.length != 1) {
                    return _this;
                }

                var id = _contentId.val() + '';

                var _contentType = _this.find("#contentType");
                if (_contentType.length != 1) {
                    return _this;
                }
                var type = _contentType.val() + '';
                var urlJson;
                switch (type) {
                    case "BlogPost":
                        urlJson = virtualDirectory + 'v1/blogpost/' + id + '/views?$format=json';
                        break;
                }

                var _viewers_count = _this.find('span.viewers-count');
                var _viewers_impressions = _this.find('span.viewers-total');
                var _viewers_personal = _this.find('span.viewers-current');
                //if (_viewers_count.length != 1) {
                //    return _this;
                //}

                var syncWithServer = function () {
                    ajaxRequest = $.getJSON(urlJson, function (data) {
                        _viewers_count = _this.find('span.viewers-count');
                        if (_viewers_count.length != 0)
                            _viewers_count.html(data.Count + '  unique views').show();
                        _viewers_impressions = _this.find('span.viewers-total');

                        if (_viewers_impressions.length != 0) {
                            _viewers_impressions.html('Total impressions: ' + data.Result).show();
                        }
                        _viewers_personal = _this.find('span.viewers-current');
                        if (_viewers_personal.length != 0) {
                            _viewers_personal.html("you've seen this " + data.Vote + " times").show();
                        }
                        $.orchard.cookie(__cookieScope, 1, { expires: $.orchard.__cookieExpiration, path: __cookiePath });
                    });
                }
                
                window.setTimeout(syncWithServer, 500);

                return _this;
            });
        }
    });
    $(function () {
        $(".viewers-rating").helpfullySetTheViewers();
    });
})(jQuery);