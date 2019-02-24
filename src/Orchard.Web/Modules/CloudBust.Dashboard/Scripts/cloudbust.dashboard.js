(function ($, window, document, undefined) {
    "use strict";
    var pluginName = "cbdashboard",
        defaults = {
            virtualDirectory: '/',
            homepath: '/',
            errormessage: '',
            systemerrormessage: '',
            usermessage: '',
            notifyelement: '',
            notifyposition: 'right middle',
            ishomepage: false,
            isloginpage: false,
            modal: '',
            scrollPosition: 0,
            editid: 0
        };
    function Plugin(element, options) {
        this.element = element;
        this.notifyelement = $('');
        this.modal = $('');
        this.settings = $.extend({}, defaults, options);
        this._defaults = defaults;
        this._name = pluginName;
        this.init();
    }

    $.extend(Plugin.prototype, {
        options: function (option, val) {
            this.settings[option] = val;
            this._vars_init();
        },

        _vars_init: function () {
            this.settings.virtualDirectory = $('html').data('virtual');
            this.settings.homepath = $('html').data('homepath');
            this.settings.errormessage = $('html').data('errormessage');
            this.settings.scrollPosition = $('html').data('scrollposition');
            this.settings.usermessage = $('.message.message-Information').text();
            this.settings.systemerrormessage = $('.message.message-Error').text();
            this.settings.editid = $('[data-editid]').data('editid');
            if(typeof(this.settings.editid) === 'undefined') this.settings.editid = 0;

            if (this.settings.notifyelement.length > 0)
                this.notifyelement = $(this.element).find(this.settings.notifyelement);

            this._initialize_notify();
            this.show_errors();
        },

        init: function () {
            this._vars_init();
            this._initialize_links();
            this._intialize_forms();
            this._initialize_search();
            this._initialize_accordion();
            this._initialize_tooltips();
            this._initialize_spinners();
            this._initialize_realtimeupdates();
            this._initialize_configurators();
            this._initialize_upload();
        },

        readImage: function (file, $preview, data) {
            var reader = new FileReader();
            //            $thumbnail = $preview.find('.mediathumbnail');
            reader.onloadend = (function (ui) {
                //                var $image = ui.find('img.baseimage');
                return function (e) {
                    $image.load(function () {
                        //                        $image.loadgo({
                        //                        });
                        data.submit();
                    }).attr('src', useBlob ? window.URL.createObjectURL(file) : reader.result);
                };
            })($thumbnail);

            reader.readAsDataURL(file);
        },

        _initialize_upload: function () {
            var _this = this;

            if ($('#fileupload').length === 0) return;
            var form = $('#__AjaxAntiForgeryForm');
            var token = $('input[name="__RequestVerificationToken"]', form).val();
            
            $('#fileupload').fileupload({
                url: _this.settings.virtualDirectory + '/v1/m/upload',
                autoUpload: true,
                formData: {
                    folderPath: '',
                    type: '',
                    __RequestVerificationToken: token
                },
                done: function (e, data) {
                    console.log(data.result);
                },
                progressall: function (e, data) {
                    var progress = Math.floor(data.loaded / data.total * 100, 10);
                    //$('#progress .bar').css('width', progress + '%');
                },
                add: function (e, data) {
                    var self = $(this);

                    if (!data.files.length) {
                        return;
                    }

                    var filesLength = data.files.length;
                    for (var i = 0; i < filesLength; i++) {
                        var file = data.files[i];

                        if ((/\.(png|jpeg|jpg|gif)$/i).test(file.name)) {
                        } else {
                            errors += file.name + " Unsupported Image extension\n";
                        }
                    }
                    data.submit();
                },
                // Callback for upload progress events:
                progress: function (e, data) {
                    if (data.context) {
                        var progress = Math.floor(data.loaded / data.total * 100);
                    }
                },
                pasteZone: window.parent.document,
                paste: function (e, data) {
                    $.each(data.files, function (index, file) {
                        //console.log('Pasted file type: ' + file.type);
                    });
                    return true;
                },
                done: function (e, data) {
                    var result = data.result[0];

                    if (result.error) {
                        console.log('error: ' + result.error);
                        return;
                    }
                    location.reload(true);
                },
                fail: function (e, data) {
                    data.context.status('error');
                }
            });
        },

        _updateURL: function (key, val) {
            var url = window.location.href;
            var reExp = new RegExp("[\?|\&]" + key + "=[0-9a-zA-Z\_\+\-\|\.\,\;]*");

            if (reExp.test(url)) {
                var reExp = new RegExp("[\?&]" + key + "=([^&#]*)");
                var delimiter = reExp.exec(url)[0].charAt(0);
                url = url.replace(reExp, delimiter + key + "=" + val);
            } else {
                var newParam = key + "=" + val;
                if (url.indexOf('?') < 0) { url += '?'; }

                if (url.indexOf('#') > -1) {
                    var urlparts = url.split('#');
                    url = urlparts[0] + "&" + newParam + (urlparts[1] ? "#" + urlparts[1] : '');
                } else {
                    url += "&" + newParam;
                }
            }
            window.history.pushState(null, document.title, url);
        },

        _updateExplicitURL: function (key, val, url) {
            var reExp = new RegExp("[\?|\&]" + key + "=[0-9a-zA-Z\_\+\-\|\.\,\;]*");

            if (reExp.test(url)) {
                var reExp = new RegExp("[\?&]" + key + "=([^&#]*)");
                var delimiter = reExp.exec(url)[0].charAt(0);
                url = url.replace(reExp, delimiter + key + "=" + val);
            } else {
                var newParam = key + "=" + val;
                if (url.indexOf('?') < 0) { url += '?'; }

                if (url.indexOf('#') > -1) {
                    var urlparts = url.split('#');
                    url = urlparts[0] + "&" + newParam + (urlparts[1] ? "#" + urlparts[1] : '');
                } else {
                    url += "&" + newParam;
                }
            }
            return url;
        },

        _initialize_realtimeupdates: function () {
            var _this = this;

            if (_this.settings.scrollPosition > 0) {
                $(window).scrollTop(_this.settings.scrollPosition);
                _this.settings.scrollPosition = 0;
            }

            var lazyScroll = _.debounce(function () {
                var tempScrollTop = $(window).scrollTop();
                $("[data-update-fromjs='scrollposition']").val(tempScrollTop);

                $("[data-realtime-scrollposition]").each(function () {
                    var url = $(this).attr("href");
                    if (typeof (url) != 'undefined' && url.length > 0) {
                        url = decodeURIComponent(url);
                        url = _this._updateExplicitURL('scrollposition', tempScrollTop, url);
                        var queryparameter = url.indexOf("?ReturnUrl=");
                        if (queryparameter > 0) {
                            var querystring = url.slice(url.indexOf('?ReturnUrl=') + 8);
                            url = url.substring(0, queryparameter);
                            url = url += "?ReturnUrl=" + encodeURIComponent(querystring);
                        }

                        $(this).attr("href", url);
                    }
                });
            }, 150);

            $(window).scroll(lazyScroll);

            $(".tabs").on('change.zf.tabs', function (e) {
                var $source = $(this);
                var source = this;
                var $target = $("[data-update-from='" + $source.attr('id') + "']");
                if ($target.length > 0) {
                    $target.each(function () {
                        var i = $source.find('.is-active').index();

                        _this._updateURL('tabid', i);
                        $(this).val(i);
                    });
                }
            });

            $("[data-updater]").on('change', function (event) {
                var $source = $(this);
                var source = this;
                var $target = $("[data-update-from='" + $source.attr('id') + "']");
                if ($target.length > 0) {
                    $target.each(function () {
                        if ($source.data('updater') === 'val') {
                            if ($(this).is('[src]')) {
                                var url = _this.settings.virtualDirectory + '/v1/m(' + $source.val() + ')';
                                $(this).attr("src", url);
                            }
                            else
                                $(this).html($source.val());
                        }
                        if ($source.data('updater') === 'checked') {
                            var checked = source.checked;
                            var field = $(this).data('update-class')
                            if ($(this).data('update-not') === "")
                                checked = !checked;
                            if (checked)
                                $(this).addClass(field);
                            else
                                $(this).removeClass(field);
                        }
                        if ($source.data('updater') === 'option') {
                            var trigger = $target.data('update-trigger');
                            var value = parseInt($source.val());
                            var checked = trigger === value;
                            var field = $(this).data('update-class')
                            if ($(this).data('update-not') === "")
                                checked = !checked;

                            if (checked)
                                $(this).addClass(field);
                            else
                                $(this).removeClass(field);
                        }
                    });
                }
            });

            $("[data-action='select']").unbind("click").click(function (e) {
                e.preventDefault();
                var $this = $(this);
                var $target = $($this.data('target'));
                var value = $this.data('value');
                if ($target.length > 0) {
                    $target.val(value);
                    _this._after_action($this);
                    $target.change();
                }
            });

            $("[data-validate='select']").on('change', function () {                
                var $this = $(this);
                var value = parseInt($this.val());

                var action = $this.data('validate-action');
                if(action === 'link')
                {
                    var url = $('option:selected', this).attr('link');
                    window.location = url;
                    return;                      
                }

                var targets = $this.data('validate-target').split(';');
                $.each(targets, function (index, item) {
                    var $target = $(this);

                    if ($target.length > 0) {
                        if (value > 0) {
                            $target.removeAttr('disabled');
                        }
                        else {
                            $target.attr('disabled', 'disabled');
                        }
                    }
                });
            });

            $("select[data-ajax-target]").on('change', function () {
                var $this = $(this);
                var value = parseInt($this.val());
                var url = $('option:selected', this).attr('post');
                var $actiontarget = $($this.data('ajax-action-target'));
                var action = $this.data('ajax-action');
                if (action === 'link') {
                    url = $('option:selected', this).attr('link');
                    window.location = url;
                    return;
                }
                var targets = $this.data('ajax-target').split(';');

                if (typeof (targets) != 'undefined') {
                    var form = $('#__AjaxAntiForgeryForm');
                    var token = $('input[name="__RequestVerificationToken"]', form).val();
                    var jqxhr = $.post(url, { '__RequestVerificationToken': token }, function () {
                        if ($actiontarget.length > 0) {
                            if (action === 'hide') {
                                $actiontarget.hide();
                            }
                            if (action === 'show') {
                                $actiontarget.show();
                            }
                        }
                    })
                        .done(function (returndata) {
                            if (returndata.ok) {
                                $.each(targets, function (index, item) {
                                    var $target = $(this);

                                    if ($target.data('updater') === 'checked') {
                                        var target = $target[0];
                                        var checked = returndata.data.value;
                                        if ($target.data('update-not') === "")
                                            checked = !checked;

                                        target.checked = checked;
                                        $target.trigger("change");
                                    }
                                    else {
                                        $target.val(returndata.data[Object.keys(returndata.data)[index]]);
                                    }
                                });
                                _this._after_action($this, returndata.data);
                            }
                        });
                }
            });

            $('[data-ajax-post]').unbind("click").click(function (e) {
                e.preventDefault();

                var $this = $(this);

                var $actiontarget = $($this.data('ajax-action-target'));

                var action = $this.data('ajax-action');

                var url = $this.attr('href');
                if (typeof (url) === "undefined")
                    url = $this.data('href');
                if (typeof (url) === "undefined") return;

                var form = $('#__AjaxAntiForgeryForm');
                if (form.length === 0) return;

                var token = $('input[name="__RequestVerificationToken"]', form).val();
                if (typeof (token) === "undefined") return;

                var fields = { '__RequestVerificationToken': token };

                var field;
                var data = $this.data();

                for (var i in data) {
                    if (i.match("^ajaxField")) {
                        var $target = $(data[i]);
                        if ($target.is('select')) {
                            var obj = {};
                            var value = $target.val();
                            field = i.substring(9, i.length).toLowerCase();
                            obj[field] = value;
                            fields = $.extend({}, fields, obj);
                        }
                        if ($target.is('input')) {
                            var obj = {};
                            var value = $target.val();
                            if ($target[0].type === "number") {
                                value = parseFloat(value);
                            }
                            if ($target[0].type === "checkbox") {
                                value = $target[0].checked;
                            }
                            field = i.substring(9, i.length).toLowerCase();
                            obj[field] = value;
                            fields = $.extend({}, fields, obj);
                        }
                    }
                }

                if ($actiontarget.length > 0) {
                    if (action === 'hide') {
                        $actiontarget.hide();
                    }
                    if (action === 'show') {
                        $actiontarget.show();
                    }
                }

                $.ajax({
                    type: "POST",
                    data: fields,
                    url: url,
                    success: function (returndata) {
                        if (returndata.ok) {
                            if (returndata.data !== null && returndata.data.msg !== null) {
                                iziToast.info({
                                    title: 'Post',
                                    message: returndata.data.msg,
                                    timeout: 3000,
                                });
                            }
                            if ($this.is('input')) {
                                if ($this[0].type === "checkbox") {
                                    $this[0].checked = returndata.data.value;
                                    $this.trigger("change");
                                }
                            }

                            _this._after_action($this, returndata.data);
                        }
                        else {
                            if (returndata.data !== null && returndata.data.msg !== null) {
                                iziToast.error({
                                    title: 'Post',
                                    message: returndata.data.msg,
                                    timeout: 3000,
                                });
                            }
                        }
                    }
                });
            })
        },

        _set_culture: function (culture, redirect) {
            var _this = this;
            var settings = {
                "async": true,
                "url": "/v1/culture/update/" + culture,
                "method": "GET",
                "headers": {}
            }
            $.ajax(settings)
                .done(function(response) {
                    window.location = redirect;
                });
        },

        _update_ajaxFields($this, ajaxdata) {
            var _this = this;

            var data = $this.data();

            for (var i in data) {
                if (i.match("^ajaxFieldUpdate")) {
                    var $target = $(data[i]);
                    var field = i.substring(15, i.length).toLowerCase();

                    if ($target[0].type === "checkbox") {
                        $target[0].checked = ajaxdata[field];
                    }
                    else{
                        $target.val(ajaxdata[field]);
                    }
                }
            }
        },
        
        _after_action($this, data) {
            var _this = this;

            _this._update_ajaxFields($this, data);

            var afteraction = $this.data('after-action');
            var target = $this.data('after-action-target')
            var $target = $(target);
            if ($target.length > 0) {
                $target.each(function () {
                    if (afteraction == 'reload') {
                        setTimeout(function () {
                            window.location.reload();
                        }, 1000);
                        return;
                    }
                    else if (afteraction === 'empty') {
                        $target.empty();
                        var original = "[data-target='" + target + "']";
                        $(original).each(function () {
                            $(this).show();
                        });
                    }
                    else if (afteraction === 'hide') {
                        $target.hide();
                    }
                    else if (afteraction === 'show') {
                        $target.show();
                    }
                    else if (afteraction == 'append') {
                        $target.append(data.div);
                    }
                    _this._initialize_realtimeupdates();
                });
            }
            else {
                if (afteraction == 'reload') {
                    setTimeout(function () {
                        window.location.reload();
                    }, 1000);
                }
            }
        },

        _initialize_spinners: function () {
            var _this = this;
            $("div[data-action='add']").unbind("click").click(function (event) {
                event.preventDefault();
                var $target = $('#' + $(this).data('target'));
                var productid = $target.data('object-id');
                var step = $target.data('step'); if (step === 0) step = 1;
                var minimum = $target.data('min'); if (minimum < 0) minimum = 0;
                var maximum = $target.data('max'); if (maximum < 1) maximum = 99999;

                var num = +$target.val() + step;
                if (num < minimum) num = minimum;
                if (num > maximum) {
                    num = maximum;
                    iziToast.warning({
                        title: 'Maximum Amount',
                        message: 'You have reached the maximum amount available for this product in a single Quote',
                    });
                }
                $("input[type=number][data-action='update'][data-object-type='product'][data-object-id='" + productid + "']").val(num);
                $target.trigger('change');
            });
            $("div[data-action='remove']").unbind("click").click(function (event) {
                event.preventDefault();
                var $target = $('#' + $(this).data('target'));
                var productid = $target.data('object-id');
                var step = $target.data('step'); if (step === 0) step = 1;
                var minimum = $target.data('min'); if (minimum < 0) minimum = 0;
                var maximum = $target.data('max'); if (maximum < 1) maximum = 99999;

                var num = +$target.val() - step;
                if (num < minimum) num = minimum;
                if (num > maximum) num = maximum;
                $("input[type=number][data-action='update'][data-object-type='product'][data-object-id='" + productid + "']").val(num);
                $target.trigger('change');
            });
        },

        _initialize_accordion: function () {
            var ACCORDION_KEY = 'docs-accordion-expandall';
            var expandAccordion = function ($a) {
                $a.parent('.accordion').find('.accordion-item, .accordion-content').addClass('is-active');
                $a.text('Collapse');
                $a.data('expandAll', false);
                if (localStorage) {
                    localStorage.setItem(ACCORDION_KEY, 'true');
                }
            };
            var contractAccordion = function ($a) {
                $a.parent('.accordion').find('.accordion-item, .accordion-content').removeClass('is-active');
                $a.text('Expand');
                $a.data('expandAll', true);
                if (localStorage) {
                    localStorage.setItem(ACCORDION_KEY, 'false');
                }
            };
            $('[data-expand-all]').on('click', function () {
                var $a = $(this);
                if ($a.data().expandAll === true) {
                    expandAccordion($a);
                } else {
                    contractAccordion($a);
                }
            });
            if (localStorage.getItem(ACCORDION_KEY) === 'true') {
                expandAccordion($('[data-expand-all]'));
            } else {
                $('[data-expand-all]').text('Expand');
            }
        },

        _initialize_tooltips: function () {
            var TOOLTIPS_KEY = 'docs-tooltips-hideall';
            var hideTooltips = function ($a) {
                $a.addClass('hidden');
                $('#tooltips-revealhide').text('Show all tooltips').css('opacity', '1');
                if (localStorage) {
                    localStorage.setItem(TOOLTIPS_KEY, 'true');
                }
            };
            var showTooltips = function ($a) {
                $a.removeClass('hidden');
                $('#tooltips-revealhide').text('Hide all tooltips').css('opacity', '1');
                if (localStorage) {
                    localStorage.setItem(TOOLTIPS_KEY, 'false');
                }
            };
            $('.training-callout').on('click', function (e) {
                e.preventDefault();
                hideTooltips($('.training-callout'));
            });
            $('#tooltips-revealhide').on('click', function (e) {
                e.preventDefault();
                if (localStorage.getItem(TOOLTIPS_KEY) === 'true') {
                    showTooltips($('.training-callout'));
                } else {
                    hideTooltips($('.training-callout'));
                }
            });

            if (localStorage.getItem(TOOLTIPS_KEY) === 'true') {
                hideTooltips($('.training-callout'));
            } else {
                showTooltips($('.training-callout'));
            }
        },

        _initialize_search: function () {
            var source = {
                limit: 10,
                source: function (query, sync, async) {
                    query = query.toLowerCase();
                    $.getJSON("/cb/data/search", function (data, status) {
                        async(data.filter(function (elem, i, arr) {
                            var name = elem.Name.toLowerCase();
                            var terms = [name, name.replace("-", "")].concat(elem.OType || []);
                            for (var j in terms)
                                if (terms[j].indexOf(query) > -1)
                                    return true;
                            return false;
                        }));
                    });
                },
                display: function (item) {
                    return item.name;
                },
                templates: {
                    notFound: function (query) {
                        return '<div class="tt-empty">No results for "' + query.query + '".</div>';
                    },
                    suggestion: function (item) {
                        return '<div><span class="name">' + item.Name + '<span class="meta">' + item.OType + '</span></span> <span class="desc">' + item.Description + "</span></div>";
                    }
                }
            }

            $('[data-docs-search]').typeahead({
                highlight: false
            }, source).on('typeahead:select', function (e, sel) {
                window.location.href = sel.link;
            });
            if (!navigator.userAgent.match(/(iP(hone|ad|od)|Android)/)) {
                $('[data-docs-search]').focus();
            }
        },

        _initialize_configurators: function () {
            var _this = this;
            $("a[data-action='culture']").unbind("click").click(function (event) {
                event.preventDefault();

                var $this = $(this);
                var redirect = $this.data('redirect');
                var culture = $this.data('culture');

                _this._set_culture(culture, redirect);
            });
        },

        show_errors: function () {
            if (this.settings.errormessage.length > 0) {
                var res = this.settings.errormessage.split("::");
                for (var i in res) {
                    if (res[i].length > 0) {
                        iziToast.error({
                            title: 'Error',
                            message: res[i]
                        });
                    }
                }
            }

            if (this.settings.usermessage.length > 0) {
                iziToast.info({
                    title: 'Info',
                    message: this.settings.usermessage,
                });
            }

            if (this.settings.systemerrormessage.length > 0) {
                iziToast.error({
                    title: 'System Error',
                    message: this.settings.systemerrormessage
                });
            }
        },

        _initialize_notify: function () {
            this.settings.modal = $('#modal');
            this.settings.modal.iziModal({
                history: false,
                iframe: true,
                headerColor: '#2AC1CF',
                fullscreen: true,
                zindex: 100000
            });

            iziToast.settings({
                timeout: 10000,
                resetOnHover: true,
                transitionIn: 'flipInX',
                transitionOut: 'flipOutX'
            });

            $("form").on("forminvalid.zf.abide", function (e) {
                iziToast.error({
                    title: 'Form validation',
                    message: 'please check your input',
                });
            });

            $("form .validation-summary-errors ul li").each(function () {
                iziToast.error({
                    title: 'Form validation',
                    message: $(this).html(),
                });
            });
        },

        _initialize_links: function () {
            var _this = this;
            
            if(_this.settings.editid === 0)
            {
                    $('.admineditor').hide();
            }
            else
            {
                    $('.admineditor').show();
                    $('.admineditor').click(function(){
                        var link = '/Admin/Contents/Edit/' + _this.settings.editid + ' #main';
                        $('.content-control').load(link, function(){

                        });
                    });
            }

            $(".pagination").removeClass("pager");
            $(".pagination li span.current").removeClass("current").parent().addClass("current");
            $(".pagination-next-link").removeClass("pagination-next-link").parent().removeClass("last").addClass("pagination-next");
            $(".pagination-previous-link").removeClass("pagination-previous-link").parent().removeClass("first").addClass("pagination-previous");
            $(".pagination-disabled-next-link").removeClass("pagination-disabled-next-link").parent().addClass("pagination-next").addClass("disabled");
            $(".pagination-disabled-previous-link").removeClass("pagination-disabled-previous-link").parent().addClass("pagination-previous").addClass("disabled");

            $('.homelink').click(function () {
                document.location.href = _this.settings.homepath;
            });

            $('.site-logo').click(function (e) {
                e.preventDefault();
                document.location.href = _this.settings.homepath;
            });

            $(".modaltrigger").click(function (e) {
                e.preventDefault();
                var $this = $(this);
                var url = $this.attr('href');
                var title = $this.attr('data-title');
                var subtitle = $this.attr('data-subtitle');
                if (title.length > 0)
                    _this.settings.modal.iziModal('setTitle', title);
                if (subtitle.length > 0)
                    _this.settings.modal.iziModal('setSubtitle', subtitle);
                _this.settings.modal.iziModal('open', {
                    currentTarget: e.currentTarget
                });
            });

            $("[data-action='expand']").click(function (e) {
                if ($(e.target).is('td'))
                    e.preventDefault();
                else
                    return;
                var $this = $(this);

                var expandedrow = $this.data('target');
                var $target = $('#' + expandedrow);
                if ($target.length > 0) {
                    if ($target.hasClass('is-expanded')) {
                        $target.removeClass('is-expanded');
                        return;
                    }
                    $this.parents("[data-expandable-container]").find('[data-expandable-content]').each(function () {
                        $(this).removeClass('is-expanded');
                    });

                    $target.addClass('is-expanded');
                }
            });

            $("[data-action='reveal']").click(function (e) {
                e.preventDefault();
                var $this = $(this);
                var revealedrow = $this.data('target');
                var $target = $('#' + revealedrow);
                if ($target.length > 0) {
                    $this.hide();
                    $target.addClass('is-revealed');
                }
            });

            $("[data-action='content']").click(function (e) {
                e.preventDefault();
                var $this = $(this);
                var $target = $($this.data('target'));
                var url = $this.attr('href');
                if ($target.length > 0) {
                    $this.hide();
                    $target.load(url, function () {     
                        $(document).foundation();                       
                        _this._initialize_realtimeupdates();
                    });
                }
            });
            
            $('#fileupload > input').click(function (event) {
                event.stopPropagation();
            });
        },

        _sort_table: function (container) {
            var table, rows, switching, i, x, y, shouldSwitch, hasdetails;
            table = document.getElementById(container);
            switching = true;
            hasdetails = false;
            while (switching) {
                switching = false;
                rows = table.querySelectorAll("#" + container + ">tbody>tr");
                for (i = 0; i < (rows.length - 1); i++) {
                    shouldSwitch = false;
                    x = parseInt(rows[i].dataset.position);
                    if (rows[i].dataset.action === "expand") {
                        if (i + 2 > rows.length - 1)
                            break;
                        y = parseInt(rows[i + 2].dataset.position);
                        hasdetails = true;
                    }
                    else {
                        y = parseInt(rows[i + 1].dataset.position);
                        hasdetails = false;
                    }
                    if (x > y) {
                        shouldSwitch = true;
                        break;
                    }
                }
                if (shouldSwitch) {
                    if (hasdetails) {
                        rows[i].parentNode.insertBefore(rows[i + 2], rows[i]);
                        rows[i].parentNode.insertBefore(rows[i + 3], rows[i]);
                    }
                    else
                        rows[i].parentNode.insertBefore(rows[i + 1], rows[i]);
                    switching = true;
                }
            }
        },

        _intialize_forms: function () {
            var _this = this;
            $("[data-action='post']").click(function (e) {
                e.preventDefault();
                var $this = $(this);
                var url = $this.data('action-url');
                var form = $('#__AjaxAntiForgeryForm');
                var token = $('input[name="__RequestVerificationToken"]', form).val();

                $.ajax({
                    type: "POST",
                    url: url,
                    data: {
                        __RequestVerificationToken: token
                    },
                    success: function (returndata) {
                        if (returndata.ok) {
                            var postaction = $this.data('post-action');
                            var posttarget = $this.data('post-action-target');
                            if (typeof postaction !== 'undefined' && typeof posttarget !== 'undefined') {
                                if (postaction === 'reorder') {
                                    var $container = $('#' + posttarget);
                                    if ($container.length > 0) {
                                        returndata.data.forEach(function (item, index) {
                                            var $row = $('#' + posttarget + '>tbody [data-id=' + item.Id + ']');
                                            if ($row.length > 0) {
                                                $row.attr("data-position", item.Position);
                                            }
                                        });
                                        _this._sort_table(posttarget);

                                        $('#' + posttarget + '>tbody>[data-id]').each(function (i, val) {
                                            var $row = $(val);
                                            $row.find('[data-direction]').each(function (i, val) {
                                                var $arrow = $(val);
                                                var direction = parseInt($arrow.attr('data-direction'));
                                                var position = parseInt($row.attr('data-position'));

                                                if (direction === 1) {
                                                    if (position > 0) {
                                                        $arrow.removeClass('hidden');
                                                    }
                                                    else {
                                                        $arrow.addClass('hidden');
                                                    }
                                                }
                                                else if (direction === 0) {
                                                    var rowCount = $('#' + posttarget + '>tbody>[data-id]').length;
                                                    if (position < rowCount - 1) {
                                                        $arrow.removeClass('hidden');
                                                    }
                                                    else {
                                                        $arrow.addClass('hidden');
                                                    }
                                                }
                                            });
                                        });
                                    }
                                }
                            }
                        }
                    }
                });
            });

            $("form[data-action='delete']").submit(function (event) {
                event.preventDefault();
                var $form = $(this);
                iziToast.question({
                    rtl: false,
                    layout: 1,
                    drag: false,
                    timeout: false,
                    close: false,
                    overlay: true,
                    toastOnce: true,
                    id: 'question',
                    title: 'Delete',
                    message: 'Do you really want to continue?',
                    position: 'center',
                    inputs: [],
                    buttons: [
                        ['<button><b>Confirm</b></button>', function (instance, toast, button, e, inputs) {
                            instance.hide({ transitionOut: 'fadeOut' }, toast, 'button');
                            setTimeout(function () {
                                $form.unbind("submit");
                                $form.submit();
                            }, 500);
                        }, true], // true to focus
                        ['<button>NO</button>', function (instance, toast, button, e) {
                            instance.hide({ transitionOut: 'fadeOut' }, toast, 'button');
                        }]
                    ],
                    onClosing: function (instance, toast, closedBy) {
                        // console.info('Closing | closedBy: ' + closedBy);
                    },
                    onClosed: function (instance, toast, closedBy) {
                        //console.info('Closed | closedBy: ' + closedBy);
                    }
                });
            });

            $("form[data-action='remove']").submit(function (event) {
                event.preventDefault();
                var $form = $(this);
                var itemdescription = $form.data('item');
                iziToast.question({
                    rtl: false,
                    layout: 1,
                    drag: false,
                    timeout: false,
                    close: false,
                    overlay: true,
                    toastOnce: true,
                    id: 'question',
                    title: 'Remove',
                    message: 'Do you really want to remove ' + itemdescription + '?',
                    position: 'center',
                    inputs: [],
                    buttons: [
                        ['<button><b>Confirm</b></button>', function (instance, toast, button, e, inputs) {
                            instance.hide({ transitionOut: 'fadeOut' }, toast, 'button');
                            setTimeout(function () {
                                $form.unbind("submit");
                                $form.submit();
                            }, 500);
                        }, true], // true to focus
                        ['<button>NO</button>', function (instance, toast, button, e) {
                            instance.hide({ transitionOut: 'fadeOut' }, toast, 'button');
                        }]
                    ],
                    onClosing: function (instance, toast, closedBy) {
                        // console.info('Closing | closedBy: ' + closedBy);
                    },
                    onClosed: function (instance, toast, closedBy) {
                        //console.info('Closed | closedBy: ' + closedBy);
                    }
                });
            });

            $("form[data-action='revoke']").submit(function (event) {
                event.preventDefault();
                var $form = $(this);
                iziToast.question({
                    rtl: false,
                    layout: 1,
                    drag: false,
                    timeout: false,
                    close: false,
                    overlay: true,
                    toastOnce: true,
                    id: 'question',
                    title: 'Revoke',
                    message: 'Do you really want to revoke rights?',
                    position: 'center',
                    inputs: [],
                    buttons: [
                        ['<button><b>Confirm</b></button>', function (instance, toast, button, e, inputs) {
                            instance.hide({ transitionOut: 'fadeOut' }, toast, 'button');
                            setTimeout(function () {
                                $form.unbind("submit");
                                $form.submit();
                            }, 500);
                        }, true], // true to focus
                        ['<button>NO</button>', function (instance, toast, button, e) {
                            instance.hide({ transitionOut: 'fadeOut' }, toast, 'button');
                        }]
                    ],
                    onClosing: function (instance, toast, closedBy) {
                        // console.info('Closing | closedBy: ' + closedBy);
                    },
                    onClosed: function (instance, toast, closedBy) {
                        //console.info('Closed | closedBy: ' + closedBy);
                    }
                });
            });

            var $dtBox = $("#dtBox");
            if ($dtBox.length > 0) {
                $dtBox.DateTimePicker({
                    dateSeparator: "/",
                    dateFormat: "dd/MM/yyyy"
                });
            }
        },
    });

    $.fn[pluginName] = function (options) {
        var args = $.makeArray(arguments),
            after = args.slice(1);

        return this.each(function () {
            if (!$.data(this, "plugin_" + pluginName)) {
                $.data(this, "plugin_" + pluginName,
                    new Plugin(this, options));
            }
            else {
                if ($.isFunction(Plugin.prototype[options])) {
                    $.data(this, 'plugin_' + pluginName)[options].apply($.data(this, 'plugin_' + pluginName), after);
                }
            }
        });
    };
})(jQuery, window, document);
$(function () { $(document.body).cbdashboard(); });