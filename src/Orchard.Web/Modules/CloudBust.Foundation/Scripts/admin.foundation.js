$(function () {
    var $content = $('fieldset:has(input[type="hidden"])').filter(function () {
        if ($(this).find('> input[type="hidden"]').length)
            return $(this).children().length === 1;
        else
            return false;
    }).hide();
    
    var $content = $('.manage').filter(function () {
        return $(this).children().length === 0;
    }).hide();

    $('ul li:has(ul.menuItems)').addClass('has_sub');

    $('.actions:first').addClass('manage');
    $('html.orchard-modules .actions:first').removeClass('manage');

    $('.message.message-Information').addClass('callout').addClass('success').slideDown();
    $('.message.message-Warning').addClass('callout').addClass('warning').slideDown();
    $('.delete-button button').addClass('button').addClass('alert');
    if ($('.manage:first').next().is('form')) {
        var form = $('.manage:first').next();
        var element = $('.manage:first').detach();
        form.prepend(element);        
    }
    $('.manage').show();
    $('.actions').show();
});
$(document).ready(function () {
    // tabs
    var container = $('.edit-item-content');
    var items = [];
    if (container.length) {

        container.find('div.tabbedcontent').each(function () {
            var $this = $(this);
            items.push(this);
            $this.detach();
        });
        $('.edit-item-content').wrapInner('<div class="tabs-panel is-active" id="tab-1"></div>');
        $('.edit-item-content').wrapInner('<div class="tabs-content" data-tabs-content="admin-tabs"></div>');
        $('.edit-item-content').prepend('<ul class="tabs" data-tabs id="admin-tabs"><li class="tabs-title is-active"><a href="#tab-1" aria-selected="true">Generic</a></li></ul>');
        var $tabs = $('.edit-item-content ul.tabs');
        var $tabscontent = $('.edit-item-content div.tabs-content');

        var arrayLength = items.length;
        if (arrayLength > 0) {
            for (var i = 0; i < arrayLength; i++) {
                var content = items[i];
                var t = $(content).data('title');
                var tabno = i + 2;
                var tabname = 'tab-' + tabno;
                $tabs.append('<li class="tabs-title"><a href="#' + tabname + '">' + t + '</a></li>');
                $tabscontent.append('<div class="tabs-panel" id="' + tabname + '"></div>');
                var $divcontent = $tabscontent.find('div#' + tabname);
                $divcontent.append($(content));
            }
        }
    }
    $(document).foundation();
  
    // sidebar
    var stickySidebar = $('#adminmenuwrap.sticky');
    if (stickySidebar.length > 0) {
        var stickyHeight = stickySidebar.height(),
            sidebarTop = stickySidebar.offset().top,
            contentHeight = $('body').height(),
            unstuck = false;
    }
    $(window).scroll(function () {
        if (stickySidebar.length > 0) {
            var scrollTop = $(window).scrollTop();
            if (sidebarTop < scrollTop) {
                var currentHeight = scrollTop - sidebarTop + contentHeight;
                if (currentHeight < stickyHeight)
                    stickySidebar.css('top', -(scrollTop - sidebarTop));
                else {
                    stickySidebar.css('top', -(stickyHeight - contentHeight));
                }
            }
            else {
                stickySidebar.css('top', '0');
            }
        }
    });
    $(window).resize(function () {
        if (stickySidebar.length > 0) {
            stickyHeight = stickySidebar.height();
            contentHeight = $('body').height();
        }
    });
    $('#adminmenuwrap.sticky').on('sticky.zf.unstuckfrom:bottom', function () {
        unstuck = true;
        stickySidebar.css('top', -(stickyHeight - contentHeight));
    });
    $('#adminmenuwrap.sticky').on('sticky.zf.stuckto:top', function () {
        if (unstuck) {
            unstuck = false;
            stickySidebar.css('top', -(stickyHeight - contentHeight));
        }
    });

    // tabs
    setTimeout(function () {
        $('.edit-item-content').show();
        $('html.settings #content').show();
    }, 50);
});