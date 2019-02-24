$(document).foundation();


$('form.closemodal').submit(function (event) {
    var $form = $(this)
    event.preventDefault();

    var action = $form.attr('data-action');
    var object = $form.attr('data-object');
    var id = $form.attr('data-objectid');

    var parentform = "#" + object + '-' + action + '-' + id;

    setTimeout(function () {
        var $formtosubmit = window.parent.$(parentform);
        $formtosubmit.submit();
    }, 500);
    setTimeout(function () {
        window.parent.$('#modal').iziModal('close');
    }, 100);
});