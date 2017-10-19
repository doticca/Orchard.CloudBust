ace.define('ace/theme/grapto', ['require', 'exports', 'module', 'ace/lib/dom'], function (require, exports, module) {


    exports.isDark = false;
    exports.cssClass = "ace-grapto";
    exports.cssText = ' .ace-grapto .ace_gutter {border-right: 1px solid #7C8891;color: #2baddb;background-color: #f4f4f4;}\
                        .ace-grapto .ace_print-margin {width: 1px;background: #ebebeb;}\
                        .ace-grapto .ace_scroller {background-color: white;-webkit-box-shadow: inset 1px 0px 6px rgba(255, 255, 255, 0.6);;-moz-box-shadow: inset 1px 0px 6px rgba(255, 255, 255, 0.6);;-o-box-shadow: inset 1px 0px 6px rgba(255, 255, 255, 0.6);box-shadow: inset 1px 0px 6px rgba(255, 255, 255, 0.6);;}\
                        .ace-grapto .ace_fold {background-color: rgb(60, 76, 114);}\
                        .ace-grapto .ace_text-layer {}.ace-grapto .ace_cursor {border-left: 2px solid black;}\
                        .ace-grapto .ace_storage, .ace-grapto .ace_keyword, .ace-grapto .ace_variable {color: #0000ff;}\
                        .css.ace-grapto .ace_variable {color: #800000;}\
                        .css.ace-grapto .ace_keyword {color: #800000;}\
                        .ace-grapto .ace_keyword.ace_operator{color:black;}\
                        .ace-grapto .ace_constant {color: #800000;}\
                        .ace-grapto .ace_support.ace_constant, .ace-grapto .ace_constant.ace_numeric, .css.ace-grapto .ace_keyword, .ace-grapto .ace_support.ace_function, .css.ace-grapto .ace_string {color: #0000ff;}\
                        .ace-grapto .ace_support.ace_type {color: #ff0000;}\
                        .ace-grapto .ace_constant.ace_buildin {color: rgb(88, 72, 246);}\
                        .ace-grapto .ace_constant.ace_library {color: rgb(6, 150, 14);}\
                        .ace-grapto .ace_function {color: black;}\
                        .ace-grapto .ace_string {color: #a31515;}\
                        .ace-grapto .ace_comment {color: rgb(63, 127, 95);}\
                        .ace-grapto .ace_comment.ace_doc {color: rgb(63, 95, 191);}\
                        .ace-grapto .ace_comment.ace_doc.ace_tag {color: rgb(127, 159, 191);}\
                        .ace-grapto .ace_constant.ace_numeric {}\
                        .ace-grapto .ace_tag {color: rgb(63, 127, 127);}\
                        .ace-grapto .ace_type {color: #0000ff;}\
                        .ace-grapto .ace_xml-pe {color: rgb(104, 104, 91);}\
                        .ace-grapto .ace_marker-layer .ace_selection {background: rgb(181, 213, 255);}\
                        .ace-grapto .ace_marker-layer .ace_bracket {margin: -1px 0 0 -1px;border: 1px solid rgb(192, 192, 192);}\
                        .ace-grapto .ace_meta.ace_tag {color:#0000ff;}\
                        .ace-grapto .ace_entity.ace_other.ace_attribute-name {color:rgb(127, 0, 127);}\
                        .ace-grapto .ace_marker-layer .ace_step {background: rgb(255, 255, 0);}\
                        .ace-grapto .ace_marker-layer .ace_active-line {border: 2px solid #eaeaf2; background:none; margin-top: -2px;}\
                        .ace-grapto .ace_marker-layer .ace_selected-word {border: 1px solid rgb(181, 213, 255);}\
                        .ace-grapto .ace_indent-guide {background: url(data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAACCAYAAACZgbYnAAAAEklEQVQImWOQlJT8z1BeXv4fAA2KA6+h9Z+2AAAAAElFTkSuQmCC) right repeat-y}';

    var dom = require("../lib/dom");
    dom.importCssString(exports.cssText, exports.cssClass);
});
