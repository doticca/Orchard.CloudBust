!function ($) {

    $(function () {

        var $window = $(window)
        var $body = $(document.body)

        var navHeight = $('.navbar').outerHeight(true) + 10

        $body.scrollspy({
            target: '.bs-sidebar',
            offset: navHeight
        })

        $window.on('load', function () {
            var url = window.location.pathname;
            $('#mnu_howto').removeClass('active');
            $('#mnu_getstarted').removeClass('active');
            $('.navbar-brand').removeClass('active');
            if (url === '/setup/howto') {
                $('#mnu_howto').addClass('active');
            }
            else if (url === '/setup/index') {
                $('#mnu_getstarted').addClass('active');
            }
            else {
                $(document.body).addClass("grapto-home");
                //$('#mnu_getstarted').addClass('active');
            }
            $body.scrollspy('refresh')
        })

        $('.grapto-container [href=#]').click(function (e) {
            e.preventDefault()
        })

        // back to top
        setTimeout(function () {
            var $sideBar = $('.bs-sidebar')

            $sideBar.affix({
                offset: {
                    top: function () {
                        var offsetTop = $sideBar.offset().top
                        var sideBarMargin = parseInt($sideBar.children(0).css('margin-top'), 10)
                        var navOuterHeight = $('.bs-docs-nav').height()

                        return (this.top = offsetTop - navOuterHeight - sideBarMargin)
                    }
                , bottom: function () {
                    return (this.bottom = $('.bs-footer').outerHeight(true))
                }
                }
            })
        }, 100)

        setTimeout(function () {
            $('.bs-top').affix()
        }, 100)

    })

}(window.jQuery)
