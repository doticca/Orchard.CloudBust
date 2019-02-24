'use strict';

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

/**
 * Foundation perfect scrollbar integration by Samuel Moncarey.
 * Licensed under MIT Open Source
 */

!function ($) {

    /**
     * PerfectScrollbar module.
     * @module foundation.perfectScrollbar
     */

    var PerfectScrollbar = function () {
        /**
         * Creates a new instance of a PerfectScrollbar.
         * @class
         * @fires PerfectScrollbar#init
         * @param {jQuery} element - jQuery object to attach a perfect-scrollbar to.
         * @param {Object} options - object to extend the default configuration.
         */
        function PerfectScrollbar(element, options) {
            _classCallCheck(this, PerfectScrollbar);

            this.$element = element;
            this.options = $.extend({}, PerfectScrollbar.defaults, this.$element.data(), options);

            this._init();

            Foundation.registerPlugin(this, 'PerfectScrollbar');
        }

        /**
         * Initializes the perfect-scrollbar by setting the creating the tip element, adding it's text, setting private variables and setting attributes on the anchor.
         * @private
         */


        _createClass(PerfectScrollbar, [{
            key: '_init',
            value: function _init() {
                this.$element.perfectScrollbar(this.options);
                this._events();
            }

            /**
             * adds event listeners for the perfect-scrollbar and its anchor
             * @private
             */

        }, {
            key: '_events',
            value: function _events() {
                $(window).on('load', this.load.bind(this));
            }
        }, {
            key: 'load',
            value: function load() {
                if (this.options.initialYPosition !== 'top') {
                    if (!isNaN(this.options.initialYPosition)) {
                        this.$element.scrollTop(this.options.initialYPosition);
                    }
                    if (this.options.initialYPosition == 'bottom') {
                        this.$element.scrollTop(this.$element[0].scrollHeight);
                    }
                }

                if (this.options.initialXPosition !== 'left') {
                    if (!isNaN(this.options.initialXPosition)) {
                        this.$element.scrollLeft(this.options.initialXPosition);
                    }
                    if (this.options.initialXPosition == 'right') {
                        this.$element.scrollLeft(this.$element[0].scrollWidth);
                    }
                }
            }
        }, {
            key: 'scrollToElement',
            value: function scrollToElement($element) {
                var _this = this;
                if ($.contains(this.$element[0], $element[0])) {
                    var containerDims = Foundation.Box.GetDimensions(this.$element);
                    var targetDims = Foundation.Box.GetDimensions($element);

                    if (targetDims.offset.top < containerDims.offset.top) {
                        var scrollTop = targetDims.offset.top - containerDims.offset.top + this.$element.scrollTop();
                        this._scrollAnimation(scrollTop);
                    }

                    if (targetDims.offset.top + targetDims.height > containerDims.offset.top + containerDims.height) {
                        var _scrollTop = targetDims.offset.top - (containerDims.height - targetDims.height) - containerDims.offset.top + this.$element.scrollTop();
                        this._scrollAnimation(_scrollTop);
                    }
                } else console.warn('Element not in container.');
            }
        }, {
            key: '_scrollAnimation',
            value: function _scrollAnimation(scrollTop) {
                var _this = this;
                this.$element.finish().animate({
                    scrollTop: scrollTop
                }, {
                    duration: _this.options.animateScrollTo ? _this.options.animationSpeed : 0
                });
            }

            /**
             * Destroys an instance of perfect-scrollbar, removes template element from the view.
             * @function
             */

        }, {
            key: 'destroy',
            value: function destroy() {
                this.$element.perfectScrollbar('destroy');
                Foundation.unregisterPlugin(this);
            }
        }], [{
            key: 'defaults',
            get: function get() {
                return {
                    handlers: ['click-rail', 'drag-scrollbar', 'keyboard', 'wheel', 'touch'],
                    wheelSpeed: 1,
                    wheelPropagation: false,
                    swipePropagation: true,
                    minScrollbarLength: null,
                    useBothWheelAxes: false,
                    suppressScrollX: false,
                    suppressScrollY: false,
                    scrollXMarginOffset: 0,
                    scrollYMarginOffset: 0,
                    initialYPosition: 'top',
                    initialXPosition: 'left',
                    stopPropagationOnClick: true,
                    theme: 'foundation',
                    animateScrollTo: false,
                    animationSpeed: 200
                };
            }
        }]);

        return PerfectScrollbar;
    }();

    // Window exports


    Foundation.plugin(PerfectScrollbar, 'PerfectScrollbar');
}(jQuery);