(function($) {
    ace.require("ace/lib/fixoldbrowsers");

    ace.require("ace/ext/spellcheck");
    ace.require("ace/multi_select")

    var config = ace.require("ace/config");
    config.init();

    var dom = ace.require("ace/lib/dom");
    var net = ace.require("ace/lib/net");
    var lang = ace.require("ace/lib/lang");
    var useragent = ace.require("ace/lib/useragent");

    var event = ace.require("ace/lib/event");
    var theme = ace.require("ace/theme/textmate");
    var EditSession = ace.require("ace/edit_session").EditSession;
    var UndoManager = ace.require("ace/undomanager").UndoManager;

    var HashHandler = ace.require("ace/keyboard/hash_handler").HashHandler;

    var Renderer = ace.require("ace/virtual_renderer").VirtualRenderer;
    var Editor = ace.require("ace/editor").Editor;

    var whitespace = ace.require("ace/ext/whitespace");
    ace.require("ace/ext/language_tools");
    var ElasticTabstopsLite = ace.require("ace/ext/elastic_tabstops_lite").ElasticTabstopsLite;

    var workerModule = ace.require("ace/worker/worker_client");
    if (location.href.indexOf("noworker") !== -1) {
        workerModule.WorkerClient = workerModule.UIWorkerClient;
    }

    var Emmet = ace.require("ace/ext/emmet");
    Emmet.setCore(window.emmet);
    var snippetManager = ace.require("ace/snippets").snippetManager;
})(jQuery);